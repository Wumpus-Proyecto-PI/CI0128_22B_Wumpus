using System;
using System.Collections.Generic;

namespace PI.EntityModels
{
    public partial class AjustesGastofijo
    {
        public string CorreoUsuario { get; set; } = null!;
        public string Nombre { get; set; } = null!;

        public virtual AjustesUsuario CorreoUsuarioNavigation { get; set; } = null!;
    }
}
