using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using System.Dynamic;

namespace PI.Controllers
{
    public class EstructuraOrgController : Controller
    {
        public IActionResult Index(string fecha)
        {
            ViewData["TituloPaso"] = "Estructura organizativa";
            EstructuraOrgHandler estructura = new EstructuraOrgHandler();
            ViewData["NombreNegocio"] = "Un negocio";
            // fecha quemada de testing
            //fecha = "2022-09-28 23:22:17.427";
            DateTime fechaAnalisis = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);

            List<PuestoModel> puestos = estructura.ObtenerListaDePuestos(fechaAnalisis);

            ViewBag.FechaAnalisis = fechaAnalisis;
            return View(puestos);
        }
    }
}
