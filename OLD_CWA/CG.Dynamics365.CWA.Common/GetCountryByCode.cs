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
    public class GetCountryByCode : CodeActivity
    {
        [Input("ISO3166Alpha2 Code")]
        [ArgumentRequired]
        public InArgument<String> CountryCode { get; set; }

        [Output("Owner Type")]
        [ReferenceTarget("esa_country")]
        public OutArgument<EntityReference> CountryEntity { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                string countryCode = CountryCode.Get(executionContext);


                QueryExpression query = new QueryExpression();
                query.EntityName = Country.EntityName;
                query.TopCount = 1;
                query.NoLock = true;
                query.ColumnSet = new ColumnSet(false);
                query.Criteria.AddCondition(new ConditionExpression(Country.ISOCode, ConditionOperator.Equal, countryCode));
                EntityCollection countries = service.RetrieveMultiple(query);

                if (countries.Entities.Count == 0)
                    throw new InvalidPluginExecutionException("Country not found: " + countryCode);

                CountryEntity.Set(executionContext, countries.Entities.First().ToEntityReference());
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
