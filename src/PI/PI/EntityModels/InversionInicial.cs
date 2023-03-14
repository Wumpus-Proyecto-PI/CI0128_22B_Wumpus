using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class InversionInicial
    {
        public DateTime FechaAnalisis { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal? Valor { get; set; }

        public virtual Analisis FechaAnalisisNavigation { get; set; } = null!;
    }
}
