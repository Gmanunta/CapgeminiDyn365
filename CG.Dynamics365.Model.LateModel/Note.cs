using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class Note
    {
        public const string EntityName = "annotation";
        public const string ID = "annotationid";
        public const string Owner = "ownerid";
        public const string WithAttachment = "isdocument";
        public const string FileName = "filename";
        public const string FileSize = "filesize";
        public const string FileBody = "documentbody";
        public const string FileMimeType = "mimetype";
        public const string Object = "objectid";
        public const string ObjectTypeCode = "objecttypecode";
        public const string Description = "notetext";
        public const string Title = "subject";

        public const string CreatedOn = "createdon";
    }
}
