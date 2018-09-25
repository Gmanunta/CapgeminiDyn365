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
    public class SetAccountOnRecordById : CodeActivity
    {
        
        [Input("Entity Logical Name")]
        [ArgumentRequired]
        public InArgument<string> entity { get; set; }

        [Input("Entity Primary Field Name")]
        [ArgumentRequired]
        public InArgument<string> entityId { get; set; }

        [Input("Field Name of Account on the entity")]
        [ArgumentRequired]
        public InArgument<string> accountFieldNameOnEntity { get; set; }

        [Input("Field Name of ID on the entity")]
        [ArgumentRequired]
        public InArgument<string> idFieldNameOnEntity { get; set; }
        
        [Input("Field Name of ID on the account")]
        [ArgumentRequired]
        public InArgument<string> idFieldNameOnAccount { get; set; }



        
        protected override void Execute(CodeActivityContext executionContext)
        {

            try
            {

                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);;


                string entityName = entity.Get(executionContext);
                string entityIdName = entityId.Get(executionContext); 
                string accountFieldName = accountFieldNameOnEntity.Get(executionContext);
                string idFieldName_Record = idFieldNameOnEntity.Get(executionContext);
                string idFieldName_Account = idFieldNameOnAccount.Get(executionContext);


                QueryExpression query = new QueryExpression();
                query.EntityName = entityName;
                query.NoLock = true;
                query.ColumnSet = new ColumnSet(accountFieldName, idFieldName_Record);

                query.Criteria.AddCondition(new ConditionExpression(entityIdName, ConditionOperator.Equal, context.PrimaryEntityId));


                var record = service.RetrieveMultiple(query).Entities.First(); 


                SetAccountById Acc = new SetAccountById();
                Acc.SetAccount(service, record, accountFieldName, idFieldName_Record, idFieldName_Account);  


            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
