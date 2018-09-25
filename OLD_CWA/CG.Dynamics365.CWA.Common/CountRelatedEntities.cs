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
    public class CountRelatedEntities : CodeActivity
    {
        //[Input("Work Order")]
        //[ReferenceTarget("msdyn_workorder")]
        //public InArgument<EntityReference> WorkOrderReference { get; set; }

        [Input("Related Entities Logical Name")]
        [ArgumentRequired]
        public InArgument<String> RelatedEntityName { get; set; }

        [Input("Related Entities Field Name")]
        [ArgumentRequired]
        public InArgument<String> RelatedFieldName { get; set; }

        [Output("RelatedEntitiesCount")]
        public OutArgument<int> RelatedEntitiesCount { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            
            try
            {

                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);

                String relatedEntityName = RelatedEntityName.Get(executionContext);
                String relatedEntityField = RelatedFieldName.Get(executionContext);

                QueryExpression query = new QueryExpression();
                query.EntityName = relatedEntityName;
                query.NoLock = true;
                query.ColumnSet = new ColumnSet(false);

                query.Criteria.AddCondition(new ConditionExpression(relatedEntityField, ConditionOperator.Equal, context.PrimaryEntityId));

                EntityCollection relatedEntities = service.RetrieveMultiple(query);

                RelatedEntitiesCount.Set(executionContext, relatedEntities.Entities.Count);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
