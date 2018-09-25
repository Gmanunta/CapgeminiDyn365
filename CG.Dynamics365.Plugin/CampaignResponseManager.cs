using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.IO;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CG.Dynamics365.Plugin.Integration
{
    public class CampaignResponseManager
    {
        static string campaignFieldName = "regardingobjectid";
        static string customerFieldName = "customer";
        static string uniqueCustomerFieldName = "ama_uniquecustomer";
        static string partyid = "partyid";

        static string campaignResponseEntityName = "campaignresponse";
        static string customerEntityName = "account";


        public void SignTheUniqueCustomer(IOrganizationService service, Entity campaignResponse)
        {

            string error = "";

            try
            {
                error = "1";

                bool isUnique = IsCustomerUnique(service, campaignResponse);

                error += "2";

                campaignResponse.Attributes[uniqueCustomerFieldName] = isUnique;

                error += "3";

            }

            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message + "-" + error);
            }


        }



        private bool IsCustomerUnique(IOrganizationService service, Entity campaignResponse)
        {
            string error = "";

            try
            {
                if (!campaignResponse.Attributes.Contains(campaignFieldName))
                    throw new InvalidPluginExecutionException("You cannot create a campaign reponse without an associated campagin.");



                var parties = (EntityCollection)campaignResponse.Attributes[customerFieldName];

                if (parties.Entities.Count == 0)
                    return true;


                EntityReference customerId = (EntityReference)parties.Entities[0].Attributes[partyid];

                var campaignId = ((EntityReference)campaignResponse.Attributes[campaignFieldName]).Id;


                error = "pre";

                var customers = RetrieveCampaignCustomers(service, campaignId, customerId.Id);

                error += "post";

                if (customers == null)
                    return true;

                else
                    return false;

            }

            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message + " - " + error);
            }

        }

        private EntityCollection RetrieveCampaignCustomers(IOrganizationService service, Guid campaignId, Guid customerId)
        {
            QueryExpression qe = new QueryExpression(campaignResponseEntityName);
            qe.Criteria.AddCondition(new ConditionExpression(campaignFieldName, ConditionOperator.Equal, campaignId));
            qe.ColumnSet = new ColumnSet(customerFieldName);

            var results = service.RetrieveMultiple(qe);


            EntityCollection customers = new EntityCollection();
            customers.EntityName = customerEntityName;


            foreach (Entity res in results.Entities)
            {

                EntityCollection parties = (EntityCollection)res.Attributes[customerFieldName];

                if (parties.Entities.Count == 0)
                    continue;


                Entity customer = parties.Entities.First();


                EntityReference party = (EntityReference)customer.Attributes[partyid];

                Entity account = new Entity(customerEntityName);
                account.Id = party.Id;

                if (party.Id == customerId)
                    customers.Entities.Add(account);

            }


            if (customers.Entities.Count == 0)
                return null;

            else
                return results;
        }

    }
}

