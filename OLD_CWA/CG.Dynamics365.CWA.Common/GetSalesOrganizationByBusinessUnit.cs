using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Amada.Dynamics365.Model.LateBound;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CG.Amada.Dynamics365.CWA.Common
{
    public class GetSalesOrganizationByBusinessUnit : CodeActivity
    {
        [Input("Business Unit")]
        [ArgumentRequired]
        [ReferenceTarget("businessunit")]
        public InArgument<EntityReference> BusinessUnitReference { get; set; }

        [Output("Sales Organization")]
        [ReferenceTarget("esa_salesorg")]
        public OutArgument<EntityReference> SalesOrganization { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            string fetchXMLResourceBU = "";
            String error = "a";
            try {
               
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                error += "b";
                EntityReference tempBusinessUnitReference = BusinessUnitReference.Get(executionContext);
                fetchXMLResourceBU =
                    @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='esa_salesorg'>
                        <attribute name='esa_salesorgid' />
                        <attribute name='esa_salesorganizationuniquecode' />
                        <order attribute='esa_salesorganizationuniquecode' descending='false' />
                        <link-entity name='esa_company' from='esa_companyid' to='esa_companyid' alias='aa'>
                          <link-entity name='team' from='teamid' to='esa_teamid' alias='ab'>
                            <filter type='and'>
                              <condition attribute='businessunitid' operator='eq' uiname='" + tempBusinessUnitReference.Name + "' uitype='businessunit' value='" + tempBusinessUnitReference.Id.ToString() + @"' />
                            </filter>
                           </link-entity>
                         </link-entity>
                       </entity>
                     </fetch>";
              
                error += "c";
                var fetchExpression = new FetchExpression(fetchXMLResourceBU);
                EntityCollection buRequests = service.RetrieveMultiple(fetchExpression);
                error += "d";
                if (buRequests.Entities.Count >= 1)
                {
                    error += "e";
                    SalesOrganization.Set(executionContext, buRequests.Entities.First().ToEntityReference());
                }
                else
                {
                    error += "f";
                    throw new Exception("Plugin Invalid Operation - No Sales Organization found");
                }
            }
            catch (Exception e ) {
                throw new Exception( error + " - " + e.Message);
            }
        }
    }
}
