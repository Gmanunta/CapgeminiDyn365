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
    public class GetCurrentUser : CodeActivity
    {
        [Output("Current User")]
        [ReferenceTarget("systemuser")]
        public OutArgument<EntityReference> CurrentUser { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);


                EntityReference userReference = new EntityReference("systemuser", context.InitiatingUserId);

                CurrentUser.Set(executionContext, userReference);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
