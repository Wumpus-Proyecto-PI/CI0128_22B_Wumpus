using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Producto
    {
        public Producto()
        {
            Componentes = new HashSet<Componente>();
        }

        public string Nombre { get; set; } = null!;
        public DateTime FechaAnalisis { get; set; }
        public int? Lote { get; set; }
        public decimal? PorcentajeDeVentas { get; set; }
        public decimal? Precio { get; set; }
        public decimal CostoVariable { get; set; }
        public decimal? ComisionDeVentas { get; set; }

        public virtual Analisi FechaAnalisisNavigation { get; set; } = null!;
        public virtual ICollection<Componente> Componentes { get; set; }
    }
}
