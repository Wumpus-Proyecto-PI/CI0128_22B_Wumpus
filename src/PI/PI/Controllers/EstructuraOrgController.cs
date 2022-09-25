using Microsoft.AspNetCore.Mvc;
using PI.Handlers;

namespace PI.Controllers
{
    public class EstructuraOrgController : Controller
    {
        public IActionResult Index()
        {
            EstructuraOrgHandler negocioHandler = new EstructuraOrgHandler();
            var negocios = negocioHandler.ObtenerNegocios();
            ViewData["NombreNegocio"] = negocios[0].nombre;
            ViewData["TituloPaso"] = "Estructura Organizativa";
            return View(negocios);
        }
    }
}
