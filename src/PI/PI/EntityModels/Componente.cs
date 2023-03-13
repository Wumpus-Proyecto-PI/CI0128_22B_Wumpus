using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Componente
    {
        public string NombreComponente { get; set; } = null!;
        public string NombreProducto { get; set; } = null!;
        public DateTime FechaAnalisis { get; set; }
        public decimal? Monto { get; set; }
        public decimal? Cantidad { get; set; }
        public string? Unidad { get; set; }

        public virtual Producto Producto { get; set; } = null!;
    }
}
