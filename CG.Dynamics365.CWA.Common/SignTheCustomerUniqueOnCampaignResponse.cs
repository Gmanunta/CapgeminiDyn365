using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.CWA.Common
{
    public class SignTheCustomerUniqueOnCampaignResponse : CodeActivity
    {



        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);


                QueryExpression query = new QueryExpression();
                query.EntityName = "campaignresponse";
                query.NoLock = true;
                query.ColumnSet = new ColumnSet(true);

                query.Criteria.AddCondition(new ConditionExpression("activityid", ConditionOperator.Equal, context.PrimaryEntityId));


                var record = service.RetrieveMultiple(query).Entities.First();


                SignTheCustomerUnique sign = new SignTheCustomerUnique();
                sign.SignTheUniqueCustomer(service, record); 

            }

            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
