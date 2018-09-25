using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class TimeOffRequest
    {
        public const string EntityName = "msdyn_timeoffrequest";
        public const string Id = "msdyn_timeoffrequestid";
        public const string Name = "msdyn_name";


        public const string Resource= "msdyn_resource";
        public const string StartTime = "msdyn_starttime";
        public const string EndTime = "msdyn_endtime";
        public const string ApprovedBy = "msdyn_approvedby";
        public const string Color = "cg_timeoffrequestcolorid";

        public class Status
        {
            public const string FieldName = "statecode";
            public const int Attivo = 0;
            public const int Inattivo = 1;
        }
    }
}
