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
using CG.Dynamics365.Model.LateBound;
using CG.Dynamics365.Plugins.Integration;

namespace CG.Dynamics365.Plugins.Integration
{
    public class OpportunityProductOnCreateSetPrimarykey : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginContext pluginContext = new PluginContext(serviceProvider);

            if (!CheckPluginRegistration(pluginContext))
                throw new InvalidPluginExecutionException("Invalid Plugin Registration");

            Entity target = pluginContext.GetTarget();
            IOrganizationService serviceAdmin = pluginContext.ServiceSystem;

            OpportunityProductCreateId(target, serviceAdmin);

        }

        private void OpportunityProductCreateId(Entity target, IOrganizationService serviceAdmin)
        {
            try
            {
                EntityReference productReference = target.GetAttributeValue<EntityReference>(OpportunityProduct.ProductID);
                EntityReference OpportunityReference = target.GetAttributeValue<EntityReference>(OpportunityProduct.Opportunity);
                String productdescription = target.GetAttributeValue<String>(OpportunityProduct.ProductDescription);

                String tempresult = "";
                if (productReference == null)
                {
                    tempresult  = OpportunityReference.Id.ToString() + "_" + productdescription.Trim();
                }
                else
                {
                    tempresult = OpportunityReference.Id.ToString() + "_" + productReference.Id.ToString().Trim();
                }
                target[OpportunityProduct.ExternalKey] = tempresult;

                Entity Result = new Entity(OpportunityProduct.EntityName);

                Result.Id = target.Id;
                Result.Attributes.Add(OpportunityProduct.ExternalKey, target.GetAttributeValue<String>(OpportunityProduct.ExternalKey));
                serviceAdmin.Update(Result);

                //serviceAdmin.Create(target);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        private bool CheckPluginRegistration(PluginContext pluginContext)
        {
            return
                pluginContext.GetTarget() != null &&
                OpportunityProduct.EntityName.Equals(pluginContext.GetTarget().LogicalName, StringComparison.InvariantCultureIgnoreCase) &&
                pluginContext.MessageName.Equals("CREATE", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
