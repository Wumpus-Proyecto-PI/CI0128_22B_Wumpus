using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PI.EntityModels
{
    public partial class GastoFijo
    {
        public string Nombre { get; set; } = null!;
        public DateTime FechaAnalisis { get; set; }
        public decimal? Monto { get; set; }
        public int Orden { get; set; }
        [JsonIgnore]
        public virtual Analisis FechaAnalisisNavigation { get; set; } = null!;
    }
}
