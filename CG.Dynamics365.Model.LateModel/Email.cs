using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class Email
    {
        public const string EntityName = "email";
        public const string Id = "emailid";
        public const string Subject = "subject";

        public const string BCC = "bcc";
        public const string Category = "category";
        public const string CC = "cc";
        public const string TO = "to";
        public const string Description = "description";
        public const string Sender = "emailsender";
        public const string From = "from";
        public const string Template = "templateid";
        public const string Regarding = "regardingobjectid";

        public sealed class Direction
        {
            public const string FieldName = "direction";
            public const bool Incoming = false;
            public const bool Outgoing = true;
        }
    }
}
