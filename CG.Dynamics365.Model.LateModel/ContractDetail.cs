using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class ContractDetail
    {
        public const string EntityName = "contractdetail";
        public const string Id = "contractdetailid";
        public const string Name = "title";

        public const string CustomerId = "customerid";
        public const string ContractId = "contractid";
        public const string ItemNumber = "cg_itemnumber";
        public const string CalculatedPriceForPeriod = "cg_calculatedpriceforperiod";
        public const string PeriodId = "cg_periodid";
        public const string TotalPrice = "price";
        public const string PeriodPrice = "cg_periodprice";
        public const string ProductId = "productid";
        public const string ProductHierarchy = "cg_producthierarchy";
        public const string StartDateForBilling = "cg_startdateforbillingplaninvoiceplan";
        public const string EndDateForBilling = "cg_enddatebillingplaninvoiceplan";
    }
}
