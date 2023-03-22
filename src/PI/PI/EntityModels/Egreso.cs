using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Egreso
    {
        public string Mes { get; set; } = null!;
        public DateTime FechaAnalisis { get; set; }
        public string Tipo { get; set; } = null!;
        public decimal Monto { get; set; }
        public virtual Mes Me { get; set; } = null!;
    }
}
