using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Puesto
    {
        public Puesto()
        {
            EsEmpleadoDes = new HashSet<EsEmpleadoDe>();
        }

        public string Nombre { get; set; } = null!;
        public DateTime FechaAnalisis { get; set; }
        public int? CantidadPlazas { get; set; }
        public decimal? SalarioBruto { get; set; }
        public long Orden { get; set; }
        public decimal? Beneficios { get; set; }

        public virtual Analisis FechaAnalisisNavigation { get; set; } = null!;
        public virtual ICollection<EsEmpleadoDe> EsEmpleadoDes { get; set; }
    }
}
