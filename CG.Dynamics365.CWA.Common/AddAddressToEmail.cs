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
    public class AddAddressToEmail : CodeActivity
    {
        [Input("Email")]
        [ReferenceTarget("email")]
        [ArgumentRequired]
        public InArgument<EntityReference> EmailMessage { get; set; }


        [Input("Email Address")]
        [ArgumentRequired]
        public InArgument<String> EmailAddress { get; set; }

        [Input("Address Role")]
        [ArgumentRequired]
        public InArgument<String> AddressRole { get; set; }

        //Test Commit 3
        //
        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            String fieldName;
            String addressRole = (AddressRole.Get(executionContext) ?? "To").ToUpper();

            switch (AddressRole.Get(executionContext))
            {
                case "TO":
                    fieldName = Email.TO;
                    break;
                case "CC":
                    fieldName = Email.CC;
                    break;
                case "BCC":
                    fieldName = Email.BCC;
                    break;
                case "FROM":
                    fieldName = Email.From;
                    break;
                default:
                    throw new InvalidPluginExecutionException("Invalid Address role");
            }

            EntityReference emailMessageReference = EmailMessage.Get(executionContext);
            Entity emailMessage = service.Retrieve(Email.EntityName, emailMessageReference.Id, new ColumnSet(fieldName));
            EntityCollection party = emailMessage.GetAttributeValue<EntityCollection>(fieldName);

            Entity addressToAdd = new Entity("activityparty");
            addressToAdd["addressused"] = EmailAddress.Get(executionContext);
            party.Entities.Add(addressToAdd);

            emailMessage[fieldName] = party;

            service.Update(emailMessage);
        }
    }
}
