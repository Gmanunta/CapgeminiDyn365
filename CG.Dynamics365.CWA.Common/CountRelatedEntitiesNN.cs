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
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CG.Dynamics365.CWA.Common
{
    public class CountRelatedEntitiesNN : CodeActivity
    {

        [Input("Relationship Name")]
        [ArgumentRequired]
        public InArgument<String> NNRelationshipName { get; set; }

        [Output("RelatedEntitiesCount")]
        public OutArgument<String> Risultato { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);

                String relationshipName = NNRelationshipName.Get(executionContext);

                RetrieveRelationshipRequest retrieveManyToManyRequest =
                       new RetrieveRelationshipRequest { Name = relationshipName };
                RetrieveRelationshipResponse retrieveManyToManyResponse =
                    (RetrieveRelationshipResponse)service.Execute(retrieveManyToManyRequest);

                ManyToManyRelationshipMetadata metadata = (ManyToManyRelationshipMetadata)retrieveManyToManyResponse.RelationshipMetadata;

                QueryExpression query = new QueryExpression();
                query.NoLock = true;
                query.ColumnSet = new ColumnSet(false);


                LinkEntity joinEntita = new LinkEntity();

                if (metadata.Entity1LogicalName.Equals(context.PrimaryEntityName, StringComparison.InvariantCultureIgnoreCase))
                {
                    query.EntityName = metadata.Entity2LogicalName;
                    joinEntita.LinkToEntityName = relationshipName;
                    joinEntita.LinkToAttributeName = metadata.Entity2LogicalName + "id";
                    joinEntita.LinkFromAttributeName = metadata.Entity2LogicalName + "id";
                    joinEntita.LinkFromEntityName = metadata.Entity2LogicalName;

                    joinEntita.LinkCriteria.AddCondition(metadata.Entity1LogicalName + "id", ConditionOperator.Equal, context.PrimaryEntityId);
                }
                else
                {
                    query.EntityName = metadata.Entity1LogicalName;
                    joinEntita.LinkToEntityName = relationshipName;
                    joinEntita.LinkToAttributeName = metadata.Entity1LogicalName + "id";
                    joinEntita.LinkFromAttributeName = metadata.Entity1LogicalName + "id";
                    joinEntita.LinkFromEntityName = metadata.Entity1LogicalName;

                    joinEntita.LinkCriteria.AddCondition(metadata.Entity2LogicalName + "id", ConditionOperator.Equal, context.PrimaryEntityId);
                }

                query.LinkEntities.Add(joinEntita);

                EntityCollection result = service.RetrieveMultiple(query);

                Risultato.Set(executionContext, result.Entities.Count.ToString());
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}