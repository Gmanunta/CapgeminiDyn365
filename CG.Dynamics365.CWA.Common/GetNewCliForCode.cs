using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Dynamics365.Model.LateBound;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CG.Dynamics365.CWA.Common
{
    public class GetNewCliForCode : CodeActivity
    {
        [Input("CliForOldString")]
        [ArgumentRequired]
        public InArgument<String> CliForOldString { get; set; }

        [Output("CliForNewString")]
        public OutArgument<string> CliForNewString { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                string cliforoldstring = CliForOldString.Get(executionContext);

                if (String.IsNullOrEmpty(cliforoldstring))
                {
                    throw new InvalidPluginExecutionException("Workflow Exception - CliForOldString cannot be null");
                }
                string[] words;

                words = cliforoldstring.Split(new string[] { "-" },
                                                StringSplitOptions.RemoveEmptyEntries);

                if(words.Length != 3)
                {
                    throw new InvalidPluginExecutionException("Workflow Exception - CliForOldString is not valid");
                }

                string result = words[0] + "-" + words[2];

                CliForNewString.Set(executionContext,result);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
