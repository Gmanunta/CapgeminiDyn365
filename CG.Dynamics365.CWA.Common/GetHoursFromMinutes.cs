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
    public class GetHoursFromMinutes : CodeActivity
    {
        [Input("Minutes")]
        [ArgumentRequired]
        public InArgument<Decimal> Minutes { get; set; }

        [Input("Rounded")]
        [Default("3")]
        public InArgument<Int32> Rounded { get; set; }

        [Input("IsRounded")]
        [Default("true")]
        public InArgument<Boolean> IsRounded { get; set; }

        [Output("sTime")]
        public OutArgument<Decimal> sTime { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                Decimal minutes = Minutes.Get(executionContext);
                Int32 rounded = Rounded.Get(executionContext);
                Boolean isrounded  = IsRounded.Get(executionContext);
                if(isrounded == false)
                    rounded = 5;
                Decimal result = Math.Round(minutes / 60, rounded);
                result = result * 2;
                result = Math.Round(result, 0);
                result = Math.Round(result / 2, 1);
                //throw new InvalidPluginExecutionException("Forzo l'errore per vedere il risultato " + result);

                sTime.Set(executionContext,result);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }

        private static string TimeString(decimal time)
        {
            string calcTime;
            if (time < 10)
            {
                calcTime = "0" + (int)time;
            }
            else
            {
                calcTime = "" + (int)time;
            }

            return calcTime;
        }
    }
}
