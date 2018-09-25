﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;

namespace CG.Dynamics365.CWA.Common.Helpers
{
    public class DynamicUrlParser
    {


        public string Url { get; set; }
        public int EntityTypeCode { get; set; }
        public Guid Id { get; set; }

        /// <summary>
        /// Parse the dynamic url in constructor
        /// </summary>
        /// <param name="url"></param>
        public DynamicUrlParser(string url)
        {
            try
            {
                Url = url;
                var uri = new Uri(url);
                int found = 0;

                string[] parameters = uri.Query.TrimStart('?').Split('&');
                foreach (string param in parameters)
                {
                    var nameValue = param.Split('=');
                    switch (nameValue[0])
                    {
                        case "etc":
                            EntityTypeCode = int.Parse(nameValue[1]);
                            found++;
                            break;
                        case "id":
                            Id = new Guid(nameValue[1]);
                            found++;
                            break;
                    }
                    if (found > 1) break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Url '{0}' is incorrectly formated for a Dynamics CRM Dynamics Url", url), ex);
            }
        }

        /// <summary>
        /// Find the Logical Name from the entity type code - this needs a reference to the Organization Service to look up metadata
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public string GetEntityLogicalName(IOrganizationService service)
        {

            string logicalname = EntityNameHelper.GetEntityNameByObjectTypeCode(service, this.EntityTypeCode);
            if (!string.IsNullOrEmpty(logicalname))
                return logicalname;


            var entityFilter = new MetadataFilterExpression(LogicalOperator.And);
            entityFilter.Conditions.Add(new MetadataConditionExpression("ObjectTypeCode ", MetadataConditionOperator.Equals, this.EntityTypeCode));
            var propertyExpression = new MetadataPropertiesExpression { AllProperties = false };
            propertyExpression.PropertyNames.Add("LogicalName");
            var entityQueryExpression = new EntityQueryExpression()
            {
                Criteria = entityFilter,
                Properties = propertyExpression
            };

            var retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest()
            {
                Query = entityQueryExpression
            };

            var response = (RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest);

            if (response.EntityMetadata.Count == 1)
            {
                return response.EntityMetadata[0].LogicalName;
            }
            return null;
        }

        public EntityReference GetEntityId(IOrganizationService service)
        {

            string logicalname = GetEntityLogicalName(service);
            EntityReference entityid = new EntityReference(logicalname, Id);
            return entityid;

        }

    }
}
