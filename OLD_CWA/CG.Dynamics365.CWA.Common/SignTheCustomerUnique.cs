using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.CWA.Common
{
    class SignTheCustomerUnique
    {
        static string campaignFieldName = "regardingobjectid";
        static string customerFieldName = "customer";
        static string uniqueCustomerFieldName = "ama_uniquecustomer";
        static string partyid = "partyid";
        static string uniqueCustomer = "ama_uniquecustomer";

        static string campaignResponseEntityName = "campaignresponse";
        static string customerEntityName = "account";


        public void SignTheUniqueCustomer(IOrganizationService service, Entity campaignResponse)
        {
            bool isUnique = IsCustomerUnique(service, campaignResponse);

            campaignResponse.Attributes[uniqueCustomerFieldName] = isUnique;

            service.Update(campaignResponse);
        }



        private bool IsCustomerUnique(IOrganizationService service, Entity campaignResponse)
        {
            if (!campaignResponse.Attributes.Contains(campaignFieldName))
                throw new InvalidPluginExecutionException("You cannot create a campaign reponse without an associated campagin.");



            var parties = (EntityCollection)campaignResponse.Attributes[customerFieldName];

            if (parties.Entities.Count == 0)
                return true;


            EntityReference customerId = (EntityReference)parties.Entities[0].Attributes[partyid];

            var campaignId = ((EntityReference)campaignResponse.Attributes[campaignFieldName]).Id;


            var customers = RetrieveCampaignCustomers(service, campaignId, customerId.Id);


            if (customers == null)
                return true;

            else
            {
                int uniqueCustomers = 0; 

                foreach(var customer in customers.Entities)
                {
                    var unique = (bool)customer.Attributes["isUnique"];
                    if (unique)
                        uniqueCustomers++; 
                }

                if (uniqueCustomers > 0)
                    return false;

                else
                    return true; 
            }


        }

        private EntityCollection RetrieveCampaignCustomers(IOrganizationService service, Guid campaignId, Guid customerId)
        {
            QueryExpression qe = new QueryExpression(campaignResponseEntityName);
            qe.Criteria.AddCondition(new ConditionExpression(campaignFieldName, ConditionOperator.Equal, campaignId));
            qe.ColumnSet = new ColumnSet(customerFieldName, uniqueCustomer);

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


                if ((bool)res.Attributes[uniqueCustomer])
                    account.Attributes.Add("isUnique", true);

                else
                    account.Attributes.Add("isUnique", false);


                if (party.Id == customerId)
                    customers.Entities.Add(account);

            }


            if (customers.Entities.Count == 0)
                return null;

            else
                return customers;
        }

    }
}

