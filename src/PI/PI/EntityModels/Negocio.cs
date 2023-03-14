using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class Negocio
    {
        public Negocio()
        {
            Analisis = new HashSet<Analisis>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? IdUsuario { get; set; }

        public virtual AspNetUser? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Analisis> Analisis { get; set; }
    }
}
