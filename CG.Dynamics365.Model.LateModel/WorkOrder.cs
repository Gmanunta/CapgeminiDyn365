using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public class WorkOrder
    {
        public const string EntityName = "msdyn_workorder";
        public const string Id = "msdyn_workorderid";
        public const string Name = "msdyn_name";
        public const int ObjectTypeCode = 10097;

        public const string Case = "msdyn_servicerequest";
        public const string ServiceAccount = "msdyn_serviceaccount";
        public const string CustomerAsset = "msdyn_customerasset";
        public const string SubStatus = "msdyn_substatus";
        public const string WorkOrderType = "msdyn_workordertype";
        public const string ExchangeRate = "exchangerate";
        public const string Address1 = "msdyn_address1";
        public const string Address2 = "msdyn_address2";
        public const string Address3 = "msdyn_address3";

        public const string Agreement = "msdyn_agreement";
        public const string BillingAccount = "msdyn_billingaccount";
        public const string BookingSummary = "msdyn_bookingsummary";
        public const string ChildIndex = "msdyn:childindex";

        public const string AreaNotes = "cg_areanotes";
        public const string Email1 = "cg_emailaddress1";
        public const string Email2 = "cg_emailaddress2";
        public const string InternalOrder = "cg_internalorderid";
        public const string SigningPerson = "cg_signingperson";

        public const string BillingAccountAddress = "cg_billingaccountaddress";
        public const string BillingAccountFiscalCode = "cg_billingaccountfiscalcodepi";
        public const string BillingAccountProvince = "cg_billingaccountprovince";
        public const string BillingAccountZipCode = "cg_billingaccountzipcode";

        public const string ServiceAccountAddress = "cg_serviceaccountaddress";
        public const string ServiceAccountFiscalCode = "cg_serviceaccountfiscalcodepi";
        public const string ServiceAccountProvince = "cg_serviceaccountprovince";
        public const string ServiceAccountZipCode = "cg_serviceaccountzipcode";
        public const string ModifiedBy = "modifiedby";
        public const string SignaturePicture = "cg_signaturepicture";
        public const string JobReportNote = "cg_jobreportnote";
        public const string LastBookableResource = "cg_lastbookableresourceid";
        public const string SoftwareVersion = "cg_softwareversion";
        public const string ExtimateDurationMinutes = "msdyn_primaryincidentestimatedduration";
        public const string ExtimateDurationHours = "cg_primaryincidentextimatedduration";

        public const string TechnicianNote = "cg_techniciannote";


        public class SystemStatus
        {
            public const string FieldName = "msdyn_systemstatus";
            public const int Open_Unscheduled = 690970000;
            public const int Open_Scheduled = 690970001;
            public const int Open_InProgress = 690970002;
            public const int Open_Completed = 690970003;
            public const int Closed_Posted = 690970004;
            public const int Closed_Canceled = 690970005;
        }
    }
}
