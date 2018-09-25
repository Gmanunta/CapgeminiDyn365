using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using CG.Amada.Dynamics365.Model.LateBound; 

namespace CG.Amada.Dynamics365.CWA.Common
{
    public class GetSpecificFunctionTeam: CodeActivity
    {//commento
        [Input("Sales Organization")]
        [ArgumentRequired]
        [ReferenceTarget("esa_salesorg")]
        public InArgument<EntityReference> SalesOrganizationReference { get; set; }

        [Input("Function Option Set Value")]
        [ArgumentRequired]
        public InArgument<int> FunctionOptionSetValue { get; set; }

        [Input("Country")]
        [ArgumentRequired]
        [ReferenceTarget("esa_country")]
        public InArgument<EntityReference> TeamCountry { get; set; }

        [Output("Team")]
        [ReferenceTarget("team")]
        public OutArgument<EntityReference> Team { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            if (tracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            tracingService.Trace("Entered GetCustmerId.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}, Primary Entity Id :{2}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId,context.PrimaryEntityId);
            try
            {

                int functionTeamValue = this.FunctionOptionSetValue.Get(executionContext);
                EntityReference salesOrganizationReference = SalesOrganizationReference.Get(executionContext);
                Entity salesOrganization = service.Retrieve(SalesOrganization.EntityName, salesOrganizationReference.Id, new ColumnSet(SalesOrganization.CompanyId));

                EntityReference companyReference = salesOrganization.GetAttributeValue<EntityReference>(SalesOrganization.CompanyId);
                Entity company = service.Retrieve(Company.EntityName, companyReference.Id, new ColumnSet(Company.TeamId));

                Entity team = service.Retrieve(CG.Amada.Dynamics365.Model.LateBound.Team.EntityName, company.GetAttributeValue<EntityReference>(Company.TeamId).Id, new ColumnSet(CG.Amada.Dynamics365.Model.LateBound.Team.BusinessUnitId));
                Guid businessunitid = team.GetAttributeValue<EntityReference>(CG.Amada.Dynamics365.Model.LateBound.Team.BusinessUnitId).Id;

                QueryExpression queryTeam = new QueryExpression(CG.Amada.Dynamics365.Model.LateBound.Team.EntityName);
                queryTeam.Criteria.AddCondition(new ConditionExpression(CG.Amada.Dynamics365.Model.LateBound.Team.BusinessUnitId, ConditionOperator.Equal, businessunitid));
                queryTeam.Criteria.AddCondition(new ConditionExpression(CG.Amada.Dynamics365.Model.LateBound.Team.TeamFunction, ConditionOperator.Equal, functionTeamValue));

                EntityReference teamCountry = TeamCountry.Get(executionContext);
                if (teamCountry != null)
                {
                    string entity1 = CG.Amada.Dynamics365.Model.LateBound.Team.EntityName;
                    string entity2 = CG.Amada.Dynamics365.Model.LateBound.Country.EntityName;
                    string relationshipEntityName = "esa_team_esa_country_manytomany";
                    LinkEntity linkEntity1 = new LinkEntity(entity1, relationshipEntityName, CG.Amada.Dynamics365.Model.LateBound.Team.Id, CG.Amada.Dynamics365.Model.LateBound.Team.Id, JoinOperator.Inner);
                    LinkEntity linkEntity2 = new LinkEntity(relationshipEntityName, entity2, Country.Id, Country.Id, JoinOperator.Inner);
                    linkEntity2.LinkCriteria = new FilterExpression();
                    linkEntity2.LinkCriteria.AddCondition(new ConditionExpression(Country.Id, ConditionOperator.Equal, teamCountry.Id));
                    linkEntity1.LinkEntities.Add(linkEntity2);
                    queryTeam.LinkEntities.Add(linkEntity1);
                }
                EntityCollection collectionTeam = service.RetrieveMultiple(queryTeam);
                if (collectionTeam.Entities.Count > 0)
                    this.Team.Set(executionContext, new EntityReference(CG.Amada.Dynamics365.Model.LateBound.Team.EntityName, collectionTeam.Entities[0].Id));
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());
                throw e;
            }

        }
    }
}
