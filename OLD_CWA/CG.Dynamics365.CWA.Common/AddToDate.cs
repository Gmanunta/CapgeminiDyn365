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
using Microsoft.Crm.Sdk.Messages;

namespace CG.Dynamics365.CWA.Common
{
    public class AddToDate : CodeActivity
    {
        [Input("Date")]
        [ArgumentRequired]
        public InArgument<DateTime> Date { get; set; }

        [Input("years")]
        public InArgument<int> Years { get; set; }

        [Input("months")]
        public InArgument<int> Months { get; set; }

        [Input("days")]
        public InArgument<int> Days { get; set; }

        [Input("hours")]
        public InArgument<int> Hours { get; set; }

        [Input("minutes")]
        public InArgument<int> Minutes { get; set; }

        [Input("seconds")]
        public InArgument<int> Seconds { get; set; }

        [Output("Modified Date")]
        public OutArgument<DateTime> ModifiedDate { get; set; }

        [Output("Modified Date String")]
        public OutArgument<String> ModifiedDateString { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            DateTime baseDate = Date.Get(executionContext);

            baseDate.AddYears(Years.Get(executionContext));
            baseDate.AddMonths(Months.Get(executionContext));
            baseDate.AddDays(Days.Get(executionContext));
            baseDate.AddHours(Hours.Get(executionContext));
            baseDate.AddMinutes(Minutes.Get(executionContext));
            baseDate.AddSeconds(Seconds.Get(executionContext));

            DateTime modifiedDate = baseDate
                    .AddYears(Years.Get(executionContext))
                    .AddMonths(Months.Get(executionContext))
                    .AddDays(Days.Get(executionContext))
                    .AddHours(Hours.Get(executionContext))
                    .AddMinutes(Minutes.Get(executionContext))
                    .AddSeconds(Seconds.Get(executionContext));

            ModifiedDate.Set(executionContext, modifiedDate);
            ModifiedDateString.Set(executionContext, modifiedDate.ToString());
        }
    }
}
