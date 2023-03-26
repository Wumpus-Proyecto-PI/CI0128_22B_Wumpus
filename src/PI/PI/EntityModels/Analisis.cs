using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Analisis 
    {
        public Analisis()
        {
            Egresos = new HashSet<Egreso>();
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
        public virtual ICollection<Egreso> Egresos { get; set; }
        public virtual ICollection<GastoFijo> GastoFijos { get; set; }
        public virtual ICollection<InversionInicial> InversionInicials { get; set; }
        public virtual ICollection<Mes> Mes { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
        public virtual ICollection<Puesto> Puestos { get; set; }
    }
}
