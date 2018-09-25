using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Amada.Dynamics365.Model.LateBound;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CG.Amada.Dynamics365.CWA.Common
{
    public class GetInstallationNumber : CodeActivity
    {
        [Input("Sales Organization")]
        [ArgumentRequired]
        [ReferenceTarget("esa_salesorg")]
        public InArgument<EntityReference> SalesOrganizationReference { get; set; }

        [Output("InstallationNumber")]
        public OutArgument<string> InstallationNumber { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            EntityReference salesOrganizationReference = SalesOrganizationReference.Get(executionContext);
            Entity salesOrganization = service.Retrieve(SalesOrganization.EntityName, salesOrganizationReference.Id, new ColumnSet(SalesOrganization.InstallationNumberPrefix, SalesOrganization.InstallationNumberProgressive));
            Entity salesOrganizationToUpdate = new Entity(SalesOrganization.EntityName, salesOrganization.Id);

            int currentInstallationNumber = salesOrganization.GetAttributeValue<int>(SalesOrganization.InstallationNumberProgressive);
            int nextInstallationNumber = currentInstallationNumber + 1;
            salesOrganizationToUpdate[SalesOrganization.InstallationNumberProgressive] = nextInstallationNumber;
            service.Update(salesOrganizationToUpdate);

            String installationNumberPrefix = salesOrganization.GetAttributeValue<String>(SalesOrganization.InstallationNumberPrefix);
            String installationNumber = String.Format("{0}{1:000000}", installationNumberPrefix, nextInstallationNumber);

            InstallationNumber.Set(executionContext, installationNumber);
        }
    }
}
