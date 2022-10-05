using Microsoft.AspNetCore.Mvc;

namespace PI.Controllers
{
    // esta clase se utiliza para devolver mensajes de error al cliente
    public class MensajesController : Controller
    {
        // accíon que retorna pantalla de vusta bajo construcción
        public IActionResult BajoConstruccion()
        {
            return View();
        }
    }
}
