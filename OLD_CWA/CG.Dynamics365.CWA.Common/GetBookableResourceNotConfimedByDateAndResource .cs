using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Dynamics365.Model.LateBound;
using CG.Dynamics365.Common.CRMTools;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CG.Dynamics365.CWA.Common
{
    public class GetBookableResourceNotConfimedByDateAndResource : CodeActivity
    {
        [Input("start")]
        [ArgumentRequired]
        public InArgument<DateTime> Start { get; set; }

        [Input("end")]
        [ArgumentRequired]
        public InArgument<DateTime> End { get; set; }

        [Input("resource")]
        [ArgumentRequired]
        [ReferenceTarget("bookableresource")]
        public InArgument<EntityReference> ResourceEntityReference { get; set; }

        [Output("hasbookableresourcebookingconfirmed")]
        public OutArgument<Boolean> HasBookableResourceBookingConfirmed { get; set; }

        [Output("bookableresourcebookingid")]
        [ReferenceTarget("bookableresourcebooking")]
        public OutArgument<EntityReference> BookableResourceBookingEntityReference { get; set; }


        protected override void Execute(CodeActivityContext executionContext)
        {
            string err = "";
            try
            {
                err += "Error code ";
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                //Get Input Parameter 
                err += "29";
                Guid BookableResourceBookingInputGuid = context.PrimaryEntityId;
                DateTime start = Start.Get(executionContext);
                DateTime end = End.Get(executionContext);
                EntityReference Resource = ResourceEntityReference.Get(executionContext);
                //get Data 
                err += "30";
                QueryExpression query = new QueryExpression();
                query.EntityName = BookableResourceBooking.EntityName;
                query.TopCount = 1;
                query.NoLock = true;
                query.ColumnSet = new ColumnSet(false);
                //set up contditions
                query.Criteria.AddCondition(BookableResourceBooking.OraDiInizio, ConditionOperator.LessEqual, end);
                query.Criteria.AddCondition(BookableResourceBooking.OraDiFine, ConditionOperator.GreaterEqual, start);
                query.Criteria.AddCondition(BookableResourceBooking.Resource, ConditionOperator.Equal, Resource.Id);
                query.Criteria.AddCondition(BookableResourceBooking.WorkOrder, ConditionOperator.Null);
                query.Criteria.AddCondition(BookableResourceBooking.AltraAttivita, ConditionOperator.Null);
                query.Criteria.AddCondition(BookableResourceBooking.Id, ConditionOperator.NotEqual, BookableResourceBookingInputGuid);

                //query.Criteria.AddCondition(BookableResourceBooking.StatoPrenotazione.FieldIdName,ConditionOperator.Equal ,"" )
                
                //set up ordering
                query.AddOrder(BookableResourceBooking.ModificatoIl, OrderType.Ascending);
                err += "31";
                EntityCollection results = service.RetrieveMultiple(query);
                // Set up output parameters 
                if (results.Entities.Count > 0)
                {
                    err += "32";
                    BookableResourceBookingEntityReference.Set(executionContext, results.Entities[0].ToEntityReference());
                    HasBookableResourceBookingConfirmed.Set(executionContext, true);
                }
                else
                {
                    err += "33";
                    HasBookableResourceBookingConfirmed.Set(executionContext, false);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " - " + err);
            }
        }
    }
}
