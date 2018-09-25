using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public class WorkOrderProduct
    {
        public const string EntityName = "msdyn_workorderproduct";
        public const string Id = "msdyn_workorderproductid";
        public const string Name = "msdyn_name";

        public const string Product = "msdyn_product";
        public const string SerialNumber = "cg_serialnumber";

        public const string WorkOrder = "msdyn_workorder";
        public const string WorkOrderCase = "msdyn_workorderincident";
        public const string AddictionalCost = "msdyn_additionalcost";
        public const string AgreementBookingProduct = "msdyn_agreementbookingproduct";
        public const string Allocated = "msdyn_allocated";
        public const string Booking = "msdyn_booking";
        public const string WareHouse = "msdyn_warehouse";
        public const string UnitCost = "msdyn_unitcost";
        public const string LineOrder = "msdyn_lineorder";

        public const string Symptom = "cg_symptomid";

        public const string ErrorMessage = "cg_errormessage";
        public const string Notes = "msdyn_internaldescription";
        public const string Causal = "cg_causalid";
        public const string ConfigurationIndex = "cg_configurationindex";
        public const string ExternalReference= "cg_externalreference";

        public const string CustomerAssetId = "msdyn_customerasset";
        public const string ReplaceCustomerAssetId = "cg_replacedcustomerassetid";
        public const string SoftwareRevision = "cg_softwarerevision";
        public const string ReplacedProduct = "cg_replacedproductid";
        public const string ReplacedAssetSerialNumber = "cg_replacedassetserialnumber";
        public const string SalesOrganization = "cg_salesorganizationid";
        public const string DeliveryNumber = "cg_deliverynumber";
        public const string Description = "msdyn_description";

        public class LineStatus
        {
            public const string FieldName = "msdyn_linestatus";
            public const int Estimated = 690970000;
            public const int Used = 690970001;
        }

        public class FailureRate
        {
            public const string FieldName = "cg_failurerate";
            public const int Unknown = 859190000;
            public const int Always  = 859190001;
            public const int Random  = 859190002;
        }

        public class RequestedReplacement
        {
            public const string FieldName = "cg_requestedreplacement";
            public const int Exchange = 859190000;
            public const int AlNotAvailable = 859190001;
            public const int Warranty = 859190002;
        }

        public class InWarranty
        {
            public const string FieldName = "cg_isinwarranty";
            public const int Warranty = 859190000;
            public const int NotInWarranty = 859190001;
            public const int WarrantyToDefine = 859190002;

        }

    }
}
