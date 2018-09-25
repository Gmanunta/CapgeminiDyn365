using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace CG.Dynamics365.Plugin
{
    public class EventPartecipantPreCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IExecutionContext context = (IExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            var ama_partecipantevent = (Entity)context.InputParameters["Target"];
            var campaignReference = ama_partecipantevent.GetAttributeValue<EntityReference>("ama_campaign");
            var accountReference = ama_partecipantevent.GetAttributeValue<EntityReference>("ama_customerid");
            var campaignResponseReference = ama_partecipantevent.GetAttributeValue<EntityReference>("ama_campaignresponseid");
            var contactReference = ama_partecipantevent.GetAttributeValue<EntityReference>("ama_contactid");

            CheckIfContactAlreadyInCampaignResponse(service, contactReference, campaignResponseReference);
            var count = GetPartecipantListCount(service, campaignResponseReference);

            SetPartecipantCountOnCampaignResponse(service, count + 1, campaignResponseReference);
        }

        private void SetPartecipantCountOnCampaignResponse(IOrganizationService service, int actualCount, EntityReference campaignResponseReference)
        {
            var campaignResponse = new Entity(campaignResponseReference.LogicalName, campaignResponseReference.Id);
            campaignResponse.Attributes.Add("ama_partecipant2", actualCount);
            service.Update(campaignResponse);
        }

        private int GetPartecipantListCount(IOrganizationService service, EntityReference campaignResponseReference)
        {
            var query = new QueryExpression("ama_partecipantevent")
            {
                Criteria = {
                    FilterOperator = LogicalOperator.And,
                    Conditions = {
                        new ConditionExpression("ama_campaignresponseid", ConditionOperator.Equal, campaignResponseReference),
                        new ConditionExpression("statecode", ConditionOperator.Equal, 0)
                    }
                }
            };
            var result = service.RetrieveMultiple(query);
            return result.Entities.Count;
        }

        private void CheckIfContactAlreadyInCampaignResponse(IOrganizationService service, EntityReference contactReference, EntityReference campaignResponseReference)
        {
            var query = new QueryExpression("ama_partecipantevent")
            {
                Criteria = {
                    FilterOperator = LogicalOperator.And,
                    Conditions = {
                        new ConditionExpression("ama_campaignresponseid", ConditionOperator.Equal, campaignResponseReference),
                        new ConditionExpression("ama_contactid", ConditionOperator.Equal, contactReference),
                        new ConditionExpression("statecode", ConditionOperator.Equal, 0)
                    }
                }
            };
            var result = service.RetrieveMultiple(query);
            if (result.Entities.Count > 0)
                throw new InvalidPluginExecutionException("Attenzione, si sta tentando di aggiungere un contatto già presente nell'elenco!");
        }
    }
}
