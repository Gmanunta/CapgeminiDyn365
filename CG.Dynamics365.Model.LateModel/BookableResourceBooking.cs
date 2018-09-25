using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class BookableResourceBooking
    {
        public const string EntityName = "bookableresourcebooking";
        public const string Id = "bookableresourcebookingid";
        public const string Name = "name";
        public const string FoglioMissione = "cg_fogliodimissionegeneraleid";
        public const string WorkOrder = "msdyn_workorder";
        public const string AltraAttivita = "new_cg_altraattivita";
        public const string Resource = "resource";
        public const string Statecode = "statecode";
        public const string BookingStatus = "bookingstatus";
        public const string OraDiInizio = "starttime";
        public const string OraDiFine = "endtime";
        public const string OraDiArrivoPrevista = "msdyn_estimatedarrivaltime";
        public const string OraDiArrivoEffettiva = "msdyn_actualarrivaltime";
        public const string BookingColor = "cg_bookingcolorid";
        public const string BookingNote = "cg_bookableresourcebookingnote";
        public const string RichiestaDiIndisponibilita = "cg_timeoffrequestid";
        public const string ModificatoIl = "modifiedon";

        public class StatoPrenotazione
        {
            public const string FieldId = "bookingstatus";
            public const string FieldIdName = "bookingstatusname";
        }

    }
}
