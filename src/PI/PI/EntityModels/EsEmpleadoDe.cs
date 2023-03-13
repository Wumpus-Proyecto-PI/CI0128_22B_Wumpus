using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class EsEmpleadoDe
    {
        public string NombreEmpleado { get; set; } = null!;
        public DateTime FechaEmpleado { get; set; }
        public string NombreJefe { get; set; } = null!;
        public DateTime FechaJefe { get; set; }

        public virtual Puesto Puesto { get; set; } = null!;
    }
}
