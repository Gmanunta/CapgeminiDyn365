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

namespace CG.Dynamics365.CWA.Common
{
    public class GetOwnerType : CodeActivity
    {
        [Output("Owner Type")]
        public OutArgument<String> OwnerType { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                var owneridname = "ownerid";

                if (context.PrimaryEntityName.ToLower() == "appoitment") owneridname = "ownerid";

                Entity mainEntity = service.Retrieve(context.PrimaryEntityName, context.PrimaryEntityId, new ColumnSet(owneridname));

                EntityReference ownerReference = mainEntity.GetAttributeValue<EntityReference>(owneridname);

                OwnerType.Set(executionContext, ownerReference.LogicalName.ToLower());
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
