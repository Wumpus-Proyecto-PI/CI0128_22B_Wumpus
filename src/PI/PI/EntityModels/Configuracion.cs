using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public virtual Analisis FechaAnalisisNavigation { get; set; } = null!;
    }
}
