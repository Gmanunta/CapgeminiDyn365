using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class Account
    {
        public const string EntityName = "account";
        public const string Id = "accountid";
        public const string Name = "name";
        public const string StateCode = "statecode";

        public const string IsIntercompany = "cg_isintercompany";
        public const string CustomerCode = "cg_customercode";
        public const string LanguageIso = "cg_languageiso";
        public const string LanguageID = "cg_languageid";
        public const string CountryID = "cg_countryid";
        public const string AddressCountry = "address1_country";
        public const string AddressStateOrProvice = "address1_stateorprovince";
        public const string Email = "emailaddress1";
        public const string EmailMarketing = "ama_emailmarketing";

        public class AccountGroupCode
        {
            public const string FieldName = "cg_account_group_code";
            public const int CUS1 = 859190007;
            public const int CUSE = 859190003;
            public const int CUSG = 859190002;
            public const int CUSP = 859190005;
            public const int CUSK = 859190000;
            public const int ECGS = 859190001;
            public const int ECGB = 859190008;
            public const int ECIN = 859190004;
            public const int ESEI = 859190006;
        }
    }
}
