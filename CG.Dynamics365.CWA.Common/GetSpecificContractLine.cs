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
    public class GetSpecificContractLine : CodeActivity
    {//commento
        [Input("Product")]
        [ArgumentRequired]
        [ReferenceTarget("product")]
        public InArgument<EntityReference> ProductReference { get; set; }

        [Input("Contract")]
        [ArgumentRequired]
        [ReferenceTarget("contract")]
        public InArgument<EntityReference> ContractReference { get; set; }

        [Output("ContractLine")]
        [ReferenceTarget("contractdetail")]

        public OutArgument<EntityReference> ContractLine { get; set; }

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

            tracingService.Trace("Entered GetSpecificContractLine.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}, Primary Entity Id :{2}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId, context.PrimaryEntityId);
            try
            {

               
                EntityReference productReference = ProductReference.Get(executionContext);
                EntityReference contractReference = ContractReference.Get(executionContext);
                 
                QueryExpression queryContractLine = new QueryExpression(ContractDetail.EntityName);
                queryContractLine.Criteria.AddCondition(new ConditionExpression(ContractDetail.ProductId, ConditionOperator.Equal,productReference. Id));
                queryContractLine.Criteria.AddCondition(new ConditionExpression(ContractDetail.ContractId, ConditionOperator.Equal, contractReference.Id));
                EntityCollection collectionContractLines = service.RetrieveMultiple(queryContractLine);
                if (collectionContractLines.Entities.Count > 0)
                    this.ContractLine.Set(executionContext, new EntityReference(ContractDetail.EntityName, collectionContractLines.Entities[0].Id));
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());
                throw e;
            }

        }
    }
}
