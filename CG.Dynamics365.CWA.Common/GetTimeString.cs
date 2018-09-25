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
    public class GetTimeString : CodeActivity
    {
        [Input("Minutes")]
        [ArgumentRequired]
        public InArgument<Decimal> Minutes { get; set; }

        [Output("sTime")]
        public OutArgument<string> sTime { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                Decimal minutes = Minutes.Get(executionContext);

                string calcHours = "00";
                decimal tempHours = 0;
                decimal tempMinutes = 0;
                string calcMinutes = "00";
                string result; 

                if((int)minutes > 0 && minutes < 60)
                {
                    calcMinutes = TimeString(minutes);
                }
                else if((int)minutes > 0)
                {
                    //perform string hours 
                    tempHours = minutes / 60;
                    calcHours = TimeString(tempHours);
                    //perform string minutes times 
                    tempMinutes = tempHours * 60;
                    tempMinutes = minutes - tempMinutes;
                    calcMinutes = TimeString(tempMinutes);
                }
                result = calcHours + ":" + calcMinutes;
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
