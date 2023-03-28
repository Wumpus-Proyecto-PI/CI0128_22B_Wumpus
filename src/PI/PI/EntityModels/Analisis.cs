using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Analisis 
    {
        public Analisis()
        {
            GastoFijos = new HashSet<GastoFijo>();
            InversionInicials = new HashSet<InversionInicial>();
            Mes = new HashSet<Mes>();
            Productos = new HashSet<Producto>();
            Puestos = new HashSet<Puesto>();
        }

        public DateTime FechaCreacion { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaDescarga { get; set; }
        public decimal? GananciaMensual { get; set; }
        public int IdNegocio { get; set; }
        public int EstadoAnalisis { get; set; }

        public virtual Negocio IdNegocioNavigation { get; set; } = null!;

        [JsonIgnore]
        public virtual Configuracion? Configuracion { get; set; }

        [JsonIgnore]
        public virtual ICollection<GastoFijo> GastoFijos { get; set; }
        [JsonIgnore] 
        public virtual ICollection<InversionInicial> InversionInicials { get; set; }

        [JsonIgnore] 
        public virtual ICollection<Mes> Mes { get; set; }
        [JsonIgnore] 
        public virtual ICollection<Producto> Productos { get; set; }
        [JsonIgnore] 
        public virtual ICollection<Puesto> Puestos { get; set; }
    }
}
