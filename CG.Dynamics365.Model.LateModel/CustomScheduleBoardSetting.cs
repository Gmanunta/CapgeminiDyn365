using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public class CustomScheduleBoardSettings
    {
        public const string EntityName = "cap_customscheduleboardsettings";
        public const string Id = "cap_customscheduleboardsettingsid";
        public const string Name = "cap_name";

        public const string IsDefault = "cap_isdefault";
        public const string View1_Guid = "cg_viewguid1";
        public const string View2_Guid = "cg_viewguid2";
        public const string View3_Guid = "cg_viewguid3";
        public const string View4_Guid = "cg_viewguid4";
        public const string View5_Guid = "cg_viewguid5";
        public const string View6_Guid = "cg_viewguid6";
        public const string View1_Entity = "cg_viewentity1";
        public const string View2_Entity = "cg_viewentity2";
        public const string View3_Entity = "cg_viewentity3";
        public const string View4_Entity = "cg_viewentity4";
        public const string View5_Entity = "cg_viewentity5";
        public const string View6_Entity = "cg_viewentity6";
        public const string View1_Type = "cg_viewtype1";
        public const string View2_Type = "cg_viewtype2";
        public const string View3_Type = "cg_viewtype3";
        public const string View4_Type = "cg_viewtype4";
        public const string View5_Type = "cg_viewtype5";
        public const string View6_Type = "cg_viewtype6";
        public const string View1_Title = "cap_viewtitle1";
        public const string View2_Title = "cap_viewtitle2";
        public const string View3_Title = "cap_viewtitle3";
        public const string View4_Title = "cap_viewtitle4";
        public const string View5_Title = "cap_viewtitle5";
        public const string View6_Title = "cap_viewtitle6";
        public const string Resource_Fetch = "cg_resourcefetch";
        public const string Owner = "ownerid";
        public const string OwnerUser = "owninguser";
        public const string OwnerTeam = "owningteam";
        public const string IsCalendarReadonly = "cg_iscalendarreadonly";
        public const string IsGridsVisible = "cg_isgridsvisible";

        public const string ViewRefreshPeriod          = "cg_requirementviewrefreshperiod" ;
        public const string ScheduleBoardRefreshPeriod = "cg_scheduleboardrefreshperiod" ;

        public class Status
        {
            public const string FieldName = "statecode";
        }
        public class StatusReason
        {
            public const string FieldName = "statuscode";
            public const int Attivo_Attivo = 1;
            public const int Attivo_Bozza = 809020000;
            public const int Inattivo_Inattivo = 2;
        }
    }
}
