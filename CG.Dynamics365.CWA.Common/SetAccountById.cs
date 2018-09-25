using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.CWA.Common
{
    class SetAccountById
    {


        public void SetAccount(IOrganizationService service, Entity record, string accountFieldNameOnRecord, string idFieldNameOnRecord, string idFieldNameOnAccount)
        {
            Entity account = GetAccountById(service, record, idFieldNameOnRecord, idFieldNameOnAccount);

            if (account == null)
                return;

            record.Attributes[accountFieldNameOnRecord] = new EntityReference(account.LogicalName, account.Id);

            service.Update(record);

        }


        private Entity GetAccountById(IOrganizationService service, Entity record, string idFieldNameOnRecord, string idFieldNameOnAccount)
        {
            if (!record.Attributes.Contains(idFieldNameOnRecord))
                return null;


            string code = (string)record.Attributes[idFieldNameOnRecord];


            QueryByAttribute qe = new QueryByAttribute("account");
            qe.AddAttributeValue(idFieldNameOnAccount, code);


            EntityCollection accounts = service.RetrieveMultiple(qe);


            if (accounts.Entities.Count == 0)
                return null;

            else
                return accounts.Entities.First();

        }
    }
}
