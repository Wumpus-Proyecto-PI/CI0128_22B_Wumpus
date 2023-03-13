using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class AjustesUsuario
    {
        public AjustesUsuario()
        {
            AjustesGastofijos = new HashSet<AjustesGastofijo>();
        }

        public string CorreoUsuario { get; set; } = null!;
        public string? Nombre { get; set; }
        public string? Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string? Contrasenya { get; set; }

        public virtual ICollection<AjustesGastofijo> AjustesGastofijos { get; set; }
    }
}
