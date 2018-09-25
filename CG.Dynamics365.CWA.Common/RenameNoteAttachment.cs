using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.CWA.Common
{
    public class RenameNoteAttachment : CodeActivity
    {
        //------ Input parameters

        [Input("Note")]
        [ArgumentRequired]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> Note { get; set; }
        [Input("Prefix")]
        public InArgument<string> Prefix { get; set; }
        [Input("Name")]
        public InArgument<string> Name { get; set; }
        [Input("Suffix")]
        public InArgument<string> Suffix { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.InitiatingUserId);

            var noteReference = this.Note.Get(executionContext);
            var prefix = this.Prefix.Get(executionContext);
            var name = this.Name.Get(executionContext);
            var suffix = this.Suffix.Get(executionContext);

            if (context == null)
                throw new InvalidWorkflowException("Workflow context non recuperato con successo.");

            try
            {
                var annotation = service.Retrieve(noteReference.LogicalName, noteReference.Id, new ColumnSet("filename", "isdocument"));

                if (annotation.GetAttributeValue<bool>("isdocument") == false)
                    return;

                var filename = annotation.GetAttributeValue<string>("filename") ?? string.Empty;
                var extension = Path.GetExtension(filename);
                var filenameWithoutExtention = Path.GetFileNameWithoutExtension(filename);

                var newFilename = (string.IsNullOrWhiteSpace(name)) ? $"{prefix}{filenameWithoutExtention}{suffix}{extension}" : $"{prefix}{name}{suffix}{extension}";

                annotation["filename"] = MakeValidFileName(newFilename);
                service.Update(annotation);
            }
            catch (Exception e)
            {
                throw new InvalidWorkflowException(e.Message);
            }

        }

        private string MakeValidFileName(string name)
        {
            string invalidPathChars = new string(Path.GetInvalidFileNameChars());
            string invalidPathCharsEscaped = System.Text.RegularExpressions.Regex.Escape(invalidPathChars);
            string invalidPathCharsRegex = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidPathCharsEscaped);
            string validPathName = System.Text.RegularExpressions.Regex.Replace(name, invalidPathCharsRegex, "_");
            return validPathName;
        }
    }
}
