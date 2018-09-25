using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class Product
    {
        public const string EntityName = "product";
        public const string Id = "productid";
        public const string Name = "name";

        public const string ProductHierarchy = "cg_producthierarchy";
        public const string ProductIDNumber = "productnumber";
        public const string MainUnit = "cg_mainunit";

        public const string Description = "description";
        public const string VendorId = "vendorid";
        public const string VendorName = "vendorname";
        public const string ValidToDate = "validtodate";
        public const string ValidFromDate = "validfromdate";
        public const string ProductURL = "producturl";
        public const string UPCCode = "msdyn_upccode";
        public const string DefaultUomScheduleId = "defaultuomscheduleid";
        public const string Taxable = "msdyn_taxable";
        public const string SupplierName = "suppliername";
        public const string SubjectId = "subjectid";
        public const string StockWeight = "stockweight";
        public const string StockVolume = "stockvolume";
        public const string IsStockItem = "isstockitem";
        public const string StandardCost = "standardcost";
        public const string Size = "size";
        public const string OverriddenCreatedOn = "overriddencreatedon";
        public const string QuantityOnHand = "quantityonhand";
        public const string PurchaseName = "msdyn_purchasename";
        public const string ProductTypeCode = "producttypecode";
        public const string ProductLine = "cg_productline";
        public const string ParentProductId = "parentproductid";
        public const string ModifiedOn = "modifiedon";
        public const string ModifiedBy = "modifiedby";
        public const string MaterialGroupCodeDescription = "cg_materialgroupdescription";
        public const string MaterialGroupCode = "cg_materialgroupcode";
        public const string Price = "price";
        public const string IsKit = "iskit";
        public const string HierarchyPath = "hierarchypath";
        public const string ExchangeRate = "exchangerate";
        public const string DefaultVendor = "msdyn_defaultvendor";
        public const string DefaultUomId = "defaultuomid";
        public const string PriceLevelId = "pricelevelid";
        public const string QuantityDecimal = "quantitydecimal";
        public const string CurrentCost = "currentcost";
        public const string ConvertToCustomerAsset = "msdyn_converttocustomerasset";

        public class ProductStructure
        {
            public const string FieldName = "productstructure";
            public const int Product = 1;
            public const int ProductFamily = 2;
            public const int ProductBundle = 3;
        }

        public class FieldServiceProductType
        {
            public const string FieldName = "msdyn_fieldserviceproducttype";
            public const int Inventory = 690970000;
            public const int NonInventory = 690970001;
            public const int Service = 690970002;
        }

        public class Status
        {
            public const string FieldName = "statecode";
            public const int Draft = 2;
            public const int Active = 0;
        }

        public class StatusReason
        {
            public const string FieldName = "statuscode";
            public const int Draft = 0;
            public const int Active = 1;
        }
    }
}
