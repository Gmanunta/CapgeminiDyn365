using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG.Dynamics365.Model.LateBound
{
    public static class AltraAttivita
    {
        public const string EntityName = "cg_altraattivita";
        public const string Id = "cg_altraattivitaid";
        public const string Name = "cg_subject";

        public class TipoDiAttivita
        {
            public const string FieldName = "cg_activitytype";
            public const int Training = 140160000;
            public const int EventoInSede = 140160001;
            public const int Altro = 140160002;
        }
    }
}
