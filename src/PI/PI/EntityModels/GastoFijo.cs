using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class GastoFijo
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaAnalisis { get; set; }
        public decimal? Monto { get; set; }
        public int Orden { get; set; }

        public virtual Analisis FechaAnalisisNavigation { get; set; } = null!;
    }
}
