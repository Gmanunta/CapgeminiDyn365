using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class Case
    {
        public const string EntityName = "incident";
        public const string Id = "incidentid";
        public const string Name = "title";
        public const string Owner = "ownerid";

        public const string CustomerId = "customerid";
        public const string InstallBaseOrderId = "cg_installbaseorderid";
        public const string GoodIssueId = "cg_goodissueid";
        public const string OwningTeam = "owningteam";
        public const string PrimaryContactId = "primarycontactid";
        public const string CustomerAssetPart = "cg_customerassetpartid";
        public const string CustomerAssetMain = "cg_customerassetmainunitid";
        public const string Province = "cg_province";

        public class InWarranty
        {
            public const string FieldName = "cg_isinwarranty";
            public const int Warranty         = 859190000;
            public const int NotInWarranty    = 859190001;
            public const int WarrantyToDefine = 859190002;

        }

        public const string CaseNumber = "ticketnumber";
        
        public class CaseType
        {
            public const string FieldName = "casetypecode";
            public const int InstallationCase   = 859190000;
            public const int CustomerCare       = 859190001;
            public const int InternalCase       = 859190002;
        }

    }


}
