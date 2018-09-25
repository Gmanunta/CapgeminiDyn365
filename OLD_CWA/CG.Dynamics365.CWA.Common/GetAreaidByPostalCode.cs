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
    public class GetAreaidByPostalCode : CodeActivity
    {
        [Input("PostalCode")]
        [ArgumentRequired]
        [ReferenceTarget("address1_postalcode")]
        public InArgument<String> Name { get; set; }

        [Output("Area")]
        [ReferenceTarget("territory")]
        public OutArgument<EntityReference> AreadiServizio { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            try
            {
                

                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);

                String postalcode = Name.Get(executionContext);
                QueryExpression query = new QueryExpression();
                query.EntityName = Cap.EntityName;
                query.NoLock = true;
                query.ColumnSet = new ColumnSet(Cap.Areadiservizio);

                query.Criteria.AddCondition(new ConditionExpression(Cap.Name, ConditionOperator.Equal, postalcode));

                EntityCollection postalcodeEntities = service.RetrieveMultiple(query);
                if (postalcodeEntities == null)
                    return ;
                if (postalcodeEntities.Entities.Count == 0)
                    return ;
                if (postalcodeEntities.Entities.Count > 1)
                    return;

                EntityReference areaidreference = postalcodeEntities.Entities[0].GetAttributeValue<EntityReference>(Cap.Areadiservizio);

                Entity areaidEntity = service.Retrieve(Area.EntityName, areaidreference.Id, new ColumnSet(Area.Name));
                if (areaidEntity == null)
                    return;
                
                //AreadiServizio.Set(executionContext, areaidreference);
                AreadiServizio.Set(executionContext, areaidEntity.ToEntityReference()); 

            }
            catch(Exception e) {
                throw e;
            }
           
        }
    }
}
