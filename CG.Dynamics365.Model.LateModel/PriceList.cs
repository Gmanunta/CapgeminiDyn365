using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class PriceList
    {
        public const string EntityName = "pricelevel";
        public const string Id = "pricelevelid";
        public const string Name = "name";

        public const string EndDate = "enddate";
        public const string BeginDate = "begindate";
        public const string SalesOrganization = "cg_salesorganizationid";
        public const string Currency = "transactioncurrencyid";
        public const string IsDefaultForCurrency = "cg_isdefaultforcurrency";
    }
}
