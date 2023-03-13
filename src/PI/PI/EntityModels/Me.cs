using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Me
    {
        public Me()
        {
            Egresos = new HashSet<Egreso>();
            Ingresos = new HashSet<Ingreso>();
        }

        public string Nombre { get; set; } = null!;
        public DateTime FechaAnalisis { get; set; }
        public decimal? InversionPorMes { get; set; }

        public virtual Analisi FechaAnalisisNavigation { get; set; } = null!;
        public virtual ICollection<Egreso> Egresos { get; set; }
        public virtual ICollection<Ingreso> Ingresos { get; set; }
    }
}
