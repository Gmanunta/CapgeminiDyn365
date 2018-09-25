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
    public class AddAttachmentToEmail : CodeActivity
    {
        [Input("Email")]
        [ReferenceTarget("email")]
        [ArgumentRequired]
        public InArgument<EntityReference> EmailMessage { get; set; }

        [Input("Note string GUID")]
        public InArgument<String> NoteStringGuid { get; set; }

        [Input("Note Reference")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteReference { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(null);

            String noteStringGuid = NoteStringGuid.Get(executionContext);
            EntityReference noteReference = NoteReference.Get(executionContext);
            EntityReference emailReference = EmailMessage.Get(executionContext);
            Entity note;

            if (noteReference == null)
            {
                note = service.Retrieve(Note.EntityName, new Guid(noteStringGuid), new ColumnSet(true));
            }
            else
            {
                note = service.Retrieve(Note.EntityName, noteReference.Id, new ColumnSet(true));
            }

            if (note == null)
                throw new InvalidPluginExecutionException("Errore nota nulla " + noteStringGuid);
            if (emailReference == null)
                throw new InvalidPluginExecutionException("Errore email nulla ");


            Entity emailAttachment = new Entity(EmailAttachment.EntityName);
            emailAttachment[EmailAttachment.Objectid] = emailReference;
            emailAttachment[EmailAttachment.ObjectTypeCode] = "email";
            emailAttachment[EmailAttachment.FileName] = note.GetAttributeValue<string>(Note.FileName);
            emailAttachment[EmailAttachment.Body] = note.GetAttributeValue<string>(Note.FileBody);
            emailAttachment[EmailAttachment.Mimetype] = note.GetAttributeValue<string>(Note.FileMimeType);
            service.Create(emailAttachment);
        }
    }
}
