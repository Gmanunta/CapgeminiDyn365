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
    public class CheckTechicianAtWork : CodeActivity
    {
        [Input("Bookable Resources Reference")] //Bookable Resource
        [ArgumentRequired]
        [ReferenceTarget("bookableresource")]
        public InArgument<EntityReference> BookableResourceReference { get; set; }
        
               
        [Output("AtWork")]
        public OutArgument<Boolean> AtWork { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            EntityReference bookableResourceReference = BookableResourceReference.Get(executionContext);
            string fetchXMLResourceTimeOff = 
            @"<fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>
	            <entity name='msdyn_timeoffrequest'>
		            <attribute name='createdon'/>
		            <attribute name='msdyn_starttime'/>
		            <attribute name='msdyn_resource'/>
		            <attribute name='msdyn_endtime'/>
		            <attribute name='msdyn_timeoffrequestid'/>
		            <order descending='true' attribute='createdon'/>
		            <filter type='and'>
			            <filter type='and'>
				            <condition attribute='msdyn_approvedby' operator='not-null'/>
				            <condition attribute='msdyn_starttime' operator='on-or-before' value='"+DateTime.Now.Year+"-"+ DateTime.Now.Month + "-"+ DateTime.Now.Day+ "'/>"+
				            "<condition attribute='msdyn_endtime' operator='on-or-after' value='" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "'/>" +
                            "<condition attribute='msdyn_resource' operator= 'eq' value = '{"+ bookableResourceReference.Id + "}' uitype = 'bookableresource' uiname = " + bookableResourceReference.Name+ "' />"+
                        "</filter>" +
		            "</filter>"+
	            "</entity>"+
            "</fetch>";
            var fetchExpression = new FetchExpression(fetchXMLResourceTimeOff);
            EntityCollection timeOffRequests = service.RetrieveMultiple(fetchExpression);

            AtWork.Set(executionContext,!(timeOffRequests != null));


        }
    }
}
