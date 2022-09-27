using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;

namespace PI.Controllers
{
    public class EstructuraOrgController : Controller
    {
        public IActionResult Index()
        {
            ViewData["TituloPaso"] = "Estructura organizativa";
            EstructuraOrgHandler estructura = new EstructuraOrgHandler();
            ViewData["NombreNegocio"] = "Un negocio";
            // fecha quemada de testing
            string fechaAnalisis = "2022-09-26 12:00:00 AM";
            List<PuestoModel> puestos = estructura.ObtenerListaDePuestos(fechaAnalisis);
            return View(puestos);
        }
    }
}
