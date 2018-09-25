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
using CG.Dynamics365.CWA.Common.Helpers;

namespace CG.Dynamics365.CWA.Common
{
    public class GetNoteGuidByUrl : CodeActivity
    {
        [Input("Url")]
        [ArgumentRequired]
        public InArgument<String> DynamicUrl { get; set; }

        [Output("NoteGuid")]
        public OutArgument<string> NoteGuid { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            string err = "";
            try
            {
                err += "Error code ";
                IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
                IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);


                string url = DynamicUrl.Get(executionContext);
                err += "1";
                DynamicUrlParser tempParser = new DynamicUrlParser(url);
                tempParser.GetEntityId(service);

                err += "20";
                EntityReference Eref = tempParser.GetEntityId(service);

                QueryExpression query = new QueryExpression();
                query.EntityName = Note.EntityName;
                query.TopCount = 1;
                query.NoLock = true;
                query.ColumnSet = new ColumnSet(false);
                query.Criteria.AddCondition(Note.Object, ConditionOperator.Equal, Eref.Id);
                //query.Criteria.AddCondition(Note.FileName, ConditionOperator.EndsWith, ".doc");
                err += "31";
                EntityCollection results = service.RetrieveMultiple(query);

                if (results.Entities.Count > 0)
                {
                    err += "04";
                    NoteGuid.Set(executionContext, results.Entities[0].ToEntityReference().Id.ToString());
                }
                else
                {
                    throw new Exception("Non ho trovato l annotazione ");
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " - " + err);
            }

        }
    }
}
