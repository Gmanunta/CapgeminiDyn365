using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class Query
    {
        public static class SavedQuery
        {
            public const string EntityName = "savedquery";
            public const string Id = "savedqueryid";
            public const int ObjectTypeCode = 1039;
        }
        public static class UserQuery
        {
            public const string EntityName = "userquery";
            public const string Id = "userqueryid";
            public const int ObjectTypeCode = 4230;
        }

        public const string Name = "name";
        public const string FetchXML = "fetchxml";
        public const string LayoutXML = "layoutxml";
        public const string ReturnedTypeCode = "returnedtypecode";
    }
}
