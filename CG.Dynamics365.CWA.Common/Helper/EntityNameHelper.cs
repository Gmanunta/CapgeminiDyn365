using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CG.Dynamics365.CWA.Common.Helpers
{
    public static class EntityNameHelper
    {

        private static object Synch = new object();

        private static List<KeyValuePair<int, string>> m_Entities;

        static EntityNameHelper()
        {
            m_Entities = new List<KeyValuePair<int, string>>();

            /**
            m_Entities.Add(new KeyValuePair<int, string>(1, "account"));
            */
        }


        public static void EnsureObjectTypeCodeList(IOrganizationService service)
        {


            lock (Synch)
            {
                RetrieveAllEntitiesRequest metaDataRequest = new RetrieveAllEntitiesRequest();
                RetrieveAllEntitiesResponse metaDataResponse = new RetrieveAllEntitiesResponse();

                metaDataRequest.EntityFilters = EntityFilters.Entity;


                metaDataResponse = service.Execute(metaDataRequest) as RetrieveAllEntitiesResponse;

                foreach (var item in metaDataResponse.EntityMetadata)
                {
                    var metadatas = (from a in m_Entities
                                     where a.Key == item.ObjectTypeCode.Value
                                     select a);
                    if (metadatas.Count() > 0)
                        continue;
                    else
                    {
                        var key = new KeyValuePair<int, string>(item.ObjectTypeCode.Value, item.LogicalName.ToLower());
                        m_Entities.Add(key);
                    }

                }
            }






        }

        public static string GetEntityNameByObjectTypeCode(IOrganizationService service, int objectTypeCode)
        {

            var result = (from a in m_Entities
                          where a.Key.Equals(objectTypeCode)
                          select a);


            if (result.Count() == 0)
                EnsureObjectTypeCodeList(service);

            result = (from a in m_Entities
                      where a.Key.Equals(objectTypeCode)
                      select a);


            if (result.Count() > 0)
                return result.First().Value;
            else
            {

                throw new Exception(string.Format("The object type code '{0}' doesn0t exist", objectTypeCode));

                return null;
            }




        }

        public static int GetObjectTypeCodeByEntityName(IOrganizationService service, string entityName)
        {

            var result = (from a in m_Entities
                          where a.Value.Equals(entityName, StringComparison.InvariantCultureIgnoreCase)
                          select a);


            // se non ho risultati allora mi assicuro di aver caricato la lista dei metadati
            if (result.Count() == 0)
                EnsureObjectTypeCodeList(service);

            result = (from a in m_Entities
                      where a.Value.Equals(entityName, StringComparison.InvariantCultureIgnoreCase)
                      select a);



            if (result.Count() > 0)
                return result.First().Key;
            else
            {
                throw new Exception(string.Format("The entityname '{0}' doesn0t exist", entityName));

            }




        }

    }
}
