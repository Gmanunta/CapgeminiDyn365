using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CG.Dynamics365.Plugins.Integration
{
    public static class ExtensionMethods
    {
        public static Entity Retrieve(this IOrganizationService service, EntityReference target, ColumnSet columnSet)
        {
            return service.Retrieve(target.LogicalName, target.Id, columnSet);
        }

        public static String RetrieveOptionSetLabel(this IOrganizationService service, String entityName, String attributeName, int selectedValue)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new
            RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)service.Execute(retrieveAttributeRequest);
            PicklistAttributeMetadata retrievedPicklistAttributeMetadata = (PicklistAttributeMetadata)retrieveAttributeResponse.AttributeMetadata;

            OptionMetadata[] optionList = retrievedPicklistAttributeMetadata.OptionSet.Options.ToArray();
            string selectedOptionLabel = string.Empty;
            foreach (OptionMetadata oMD in optionList)
            {
                if (oMD.Value == selectedValue)
                {
                    return oMD.Label.UserLocalizedLabel.Label;
                }
            }

            throw new Exception(String.Format("OptionSet value not found {0} - {1} - {2}", entityName, attributeName, selectedValue));
        }

        public static Entity Merge(this Entity main, Entity delta)
        {
            if (delta == null)
                return main;

            if (!main.LogicalName.Equals(delta.LogicalName))
                throw new Exception("It not possible to merge entities with different logical name");

            foreach (var attribute in delta.Attributes)
            {
                main[attribute.Key] = attribute.Value;
            }

            return main;
        }

        public static Entity Clone(this Entity main)
        {
            Entity cloneEntity = new Entity(main.LogicalName) { Id = main.Id };
            foreach (var attribute in main.Attributes)
            {
                cloneEntity[attribute.Key] = attribute.Value;
            }
            return cloneEntity;
        }
    }
}
