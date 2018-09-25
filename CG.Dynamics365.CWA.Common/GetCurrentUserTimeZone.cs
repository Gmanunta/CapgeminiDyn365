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
    public class GetCurrentUserTimeZone : CodeActivity
    {
        [Output("timezone")]
        public OutArgument<int> Timezone { get; set; }

        [Output("timezoneString")]
        public OutArgument<String> TimezoneString { get; set; }

        [Output("timezoneDayLight")]
        public OutArgument<int> TimezoneDaylight { get; set; }

        [Output("timezoneDayLightString")]
        public OutArgument<String> TimezoneDaylightString { get; set; }

        [Output("timezoneTotalBias")]
        public OutArgument<int> TimezoneTotalBias { get; set; }

        [Output("timezoneTotalBiasString")]
        public OutArgument<String> TimezoneTotalBiasString { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(new Guid());

                QueryExpression query = new QueryExpression("usersettings");
                query.NoLock = true;
                query.Criteria.AddCondition("systemuserid", ConditionOperator.EqualUserId);
                query.ColumnSet = new ColumnSet("localeid", "timezonebias", "timezonedaylightbias");
                query.TopCount = 1;

                EntityCollection results = service.RetrieveMultiple(query);
                Entity result = results.Entities.First();
                int timezone = (-1) * result.GetAttributeValue<int>("timezonebias");
                String timezoneString = String.Format("{0}", timezone);

                int timezoneDaylight = (-1) * result.GetAttributeValue<int>("timezonedaylightbias");
                String timezoneDaylightString = String.Format("{0}", timezoneDaylight);

                int totalBias = timezone + timezoneDaylight;
                String totalBiasString = String.Format("{0}", totalBias);

                Timezone.Set(executionContext, timezone);
                TimezoneString.Set(executionContext, timezoneString);
                TimezoneDaylight.Set(executionContext, timezoneDaylight);
                TimezoneDaylightString.Set(executionContext, timezoneDaylightString);
                TimezoneTotalBias.Set(executionContext, totalBias);
                TimezoneTotalBiasString.Set(executionContext, totalBiasString);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
