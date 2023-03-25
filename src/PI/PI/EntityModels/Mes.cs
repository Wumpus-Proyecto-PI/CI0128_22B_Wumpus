using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Mes
    {
        public Mes()
        {
            Egresos = new HashSet<Egreso>();
            Ingresos = new HashSet<Ingreso>();
        }

        public string Nombre { get; set; } = null!;
        public DateTime FechaAnalisis { get; set; }
        public decimal? InversionPorMes { get; set; }

        [JsonIgnore]
        public virtual Analisis FechaAnalisisNavigation { get; set; } = null!;
        public virtual ICollection<Egreso> Egresos { get; set; }
        public virtual ICollection<Ingreso> Ingresos { get; set; }
    }
}
