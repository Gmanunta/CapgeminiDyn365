using System;
using Microsoft.Xrm.Sdk;


namespace CG.Dynamics365.Plugin.Integration
{
    public class CheckIfTheCustomerIsUnique : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IExecutionContext context = (IExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);


                Entity campaignResponse = (Entity)context.InputParameters["Target"];


                CampaignResponseManager campRes = new CampaignResponseManager();
                campRes.SignTheUniqueCustomer(service, campaignResponse);


            }

            catch (Exception e)
            {
                throw new InvalidPluginExecutionException("There is the exception: " + e.Message);
            }
        }
    }
}

