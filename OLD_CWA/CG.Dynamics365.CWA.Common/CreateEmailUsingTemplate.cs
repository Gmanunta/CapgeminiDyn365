using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Dynamics365.Model.LateBound;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

namespace CG.Dynamics365.CWA.Common
{
    public class CreateEmailUsingTemplate : CodeActivity
    {
        [Input("Template")]
        [ArgumentRequired]
        [ReferenceTarget("template")]
        public InArgument<EntityReference> Template { get; set; }

        [Output("EmailCreated")]
        [ArgumentRequired]
        [ReferenceTarget("email")]
        public OutArgument<EntityReference> EmailCreated { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            EntityReference templateReference = Template.Get(executionContext);
            InstantiateTemplateRequest instTemplateReq = new InstantiateTemplateRequest
            {
                TemplateId = templateReference.Id,
                ObjectId = context.PrimaryEntityId,
                ObjectType = context.PrimaryEntityName
            };
            InstantiateTemplateResponse instTemplateResp = (InstantiateTemplateResponse)service.Execute(instTemplateReq);
            Entity emailToCreate = instTemplateResp.EntityCollection.Entities.First();
            emailToCreate[Email.Regarding] = new EntityReference(context.PrimaryEntityName, context.PrimaryEntityId);
            emailToCreate.Id = service.Create(emailToCreate);
            EmailCreated.Set(executionContext, emailToCreate.ToEntityReference());
        }
    }
}
