using CG.Dynamics365.CWA.Common.Helpers;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;

namespace CG.Dynamics365.CWA.Common
{
    public class CalculateRollupField : CodeActivity
    {
        //------ Input parameters

        [Input("RecordId")]
        public InArgument<string> RecordId { get; set; }

        [Input("RecordType")]
        public InArgument<string> RecordType { get; set; }

        [Input("RecordUrl")]
        public InArgument<string> RecordUrl { get; set; }

        [Input("AttributesList")]
        public InArgument<string> AttributesList { get; set; }

        //------ Output parameters

        [Output("Response")]
        public OutArgument<string> Response { get; set; }

        //------ CWA Body

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.InitiatingUserId);

            //PluginSettings.EnsureCrmsServiceUser(service);
            //IOrganizationService servicesuperuser = serviceFactory.CreateOrganizationService(PluginSettings.CrmServiceUserId);

            string recordId = this.RecordId.Get(executionContext);
            string recordType = this.RecordType.Get(executionContext);
            string recordUrl = this.RecordUrl.Get(executionContext);
            string attributesListString = this.AttributesList.Get(executionContext);

            string responseText = "false";

            if (context == null)
                throw new InvalidWorkflowException("Failed to retrieve workflow context.");

            try
            {
                EntityReference entityRef = null;
                if (!string.IsNullOrEmpty(recordUrl))
                {
                    DynamicUrlParser url = new DynamicUrlParser(recordUrl);
                    string logicalName = url.GetEntityLogicalName(service);
                    entityRef = new EntityReference(logicalName, url.Id);
                }
                else
                    entityRef = new EntityReference(recordType, new Guid(recordId));

                if (entityRef == null)
                    throw new InvalidWorkflowException("Unable to configure entity reference for the CalculateRollupFieldRequest.");

                List<string> rollupAttributesList = attributesListString.Split(';').ToList<string>();

                foreach (string attribute in rollupAttributesList)
                {
                    if (string.IsNullOrEmpty(attribute))
                        continue;

                    CalculateRollupFieldRequest reqUpdateRollup = new CalculateRollupFieldRequest
                    {
                        FieldName = attribute,
                        Target = entityRef
                    };
                    service.Execute(reqUpdateRollup);
                }

                responseText = "true";
            }
            catch (Exception ex)
            {
                throw new InvalidWorkflowException(ex.Message);
            }
            finally
            {
                Response.Set(executionContext, responseText);
            }
        }

    }
}
