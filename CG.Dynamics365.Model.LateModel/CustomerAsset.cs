using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class CustomerAsset
    {
        public const string EntityName = "msdyn_customerasset";
        public const string Id = "msdyn_customerassetid";
        public const string Name = "msdyn_name";

        public const string GoodIssueId = "cg_goodissueid";
        public const string InstallBaseOrderId = "cg_installbaseorderid";
        public const string Product = "msdyn_product";
        public const string InternalOrder = "cg_internalorderid";
        public const string ParentAsset = "msdyn_parentasset";

        public const string MachineSerialNumber = "cg_assetserialnumber";
        public const string Code = "cg_customerassetnumber"; //Deprecated to delete 

        public const string SoftwareVersion = "cg_softwareversion";

        public const string AccountId = "msdyn_account"; //ship to 
        public const string BillingAccount = "cg_billingaccountid";
        public const string PayerAccount   = "cg_payeraccountid";
        public const string ChildAssetIncrementNumber = "cg_childassetincrementnumber";
        public const string InstallationNumber   = "cg_installationnumber";
        public const string SalesOrganization = "cg_salesorganizationid";

        public const string WarrantyDate  = "cg_endwarrantydate";
        public const string WarrantyMonth = "cg_warrantymonths";

    }
}
