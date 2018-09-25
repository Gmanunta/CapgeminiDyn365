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
    public class GetSalesOrganizationTeam : CodeActivity
    {
        [Input("Sales Organization")]
        [ArgumentRequired]
        [ReferenceTarget("esa_salesorg")]
        public InArgument<EntityReference> SalesOrganizationReference { get; set; }

        [Output("Team")]
        [ReferenceTarget("team")]
        public OutArgument<EntityReference> Team { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            EntityReference salesOrganizationReference = SalesOrganizationReference.Get(executionContext);
            Entity salesOrganization = service.Retrieve(SalesOrganization.EntityName, salesOrganizationReference.Id, new ColumnSet(SalesOrganization.CompanyId));

            EntityReference companyReference = salesOrganization.GetAttributeValue<EntityReference>(SalesOrganization.CompanyId);
            Entity company = service.Retrieve(Company.EntityName, companyReference.Id, new ColumnSet(Company.TeamId));

            EntityReference teamReference = company.GetAttributeValue<EntityReference>(Company.TeamId);

            Team.Set(executionContext, teamReference);
        }
    }
}
