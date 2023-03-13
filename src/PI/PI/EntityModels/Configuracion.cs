using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Configuracion
    {
        public DateTime FechaAnalisis { get; set; }
        public int TipoNegocio { get; set; }
        public decimal? PorcentajeSs { get; set; }
        public decimal? PorcentajePl { get; set; }

        public virtual Analisi FechaAnalisisNavigation { get; set; } = null!;
    }
}
