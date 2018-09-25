using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using CG.Dynamics365.Model.LateBound;

namespace CG.Dynamics365.CWA.Common
{
    public class EvaluateWOAssociatedToCase : CodeActivity
    {
        [Input("Incident")]
        [ArgumentRequired]
        [ReferenceTarget("incident")]
        public InArgument<EntityReference> IncidentReference { get; set; }


        [Input("Substatus")]
        [ArgumentRequired]
        [ReferenceTarget("msdyn_workordersubstatus")]
        public InArgument<EntityReference> SubStatusReference { get; set; }

        [Output("Case is to close")]

        public OutArgument<bool> IsToClose { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            if (tracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            tracingService.Trace("Entered GetCustmerId.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId);
            try
            {
                
                EntityReference IncidentRef= IncidentReference.Get(executionContext);
                EntityReference SubstatusRef = SubStatusReference.Get(executionContext);
                QueryExpression queryWO = new QueryExpression("msdyn_workorder");
                queryWO.ColumnSet = new ColumnSet("msdyn_systemstatus","msdyn_substatus");
                queryWO.Criteria.AddCondition(new ConditionExpression("msdyn_systemstatus", ConditionOperator.Equal,WorkOrder.SystemStatus.Closed_Posted));
                queryWO.Criteria.AddCondition(new ConditionExpression("msdyn_substatus", ConditionOperator.NotEqual,SubstatusRef.Id));
                queryWO.Criteria.AddCondition(new ConditionExpression("msdyn_servicerequest", ConditionOperator.Equal, IncidentRef.Id));
                EntityCollection collectionWO = service.RetrieveMultiple(queryWO);
                //PRESENCE OF WORK ORDER NOT COMPLETED
                if (collectionWO.Entities.Count > 0)
                    this.IsToClose.Set(executionContext, false);
                else
                    this.IsToClose.Set(executionContext, true);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());
                throw e;
            }

        }
    }
}
