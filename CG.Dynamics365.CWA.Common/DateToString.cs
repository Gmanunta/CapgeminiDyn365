using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.CWA.Common
{
    public class DateToString : CodeActivity
    {
        //------ Input parameters

        [Input("Data")]
        [ArgumentRequired]
        public InArgument<DateTime> Date { get; set; }

        [Input("Formato")]
        public InArgument<string> Format { get; set; }

        //------ Output parameters

        [Output("StringaData")]
        public OutArgument<string> DataStringa { get; set; }
        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.InitiatingUserId);

            var dateToConvert = this.Date.Get(executionContext);
            var format = this.Format.Get(executionContext);

            if (context == null)
                throw new InvalidWorkflowException("Workflow context non recuperato con successo.");
            var responseText = "Error!";

            try
            {
                responseText = (string.IsNullOrWhiteSpace(format)) ? dateToConvert.ToString() : dateToConvert.ToString(format);
            }
            catch (Exception e)
            {
                throw new InvalidWorkflowException(e.Message);
            }
            finally
            {
                DataStringa.Set(executionContext, responseText);
            }

        }
    }
}
