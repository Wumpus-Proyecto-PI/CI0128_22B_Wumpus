using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using System.Dynamic;

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
            string fecha = "2022-09-26 00:00:00.000";
            DateTime fechaAnalisis = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);

            List<PuestoModel> puestos = estructura.ObtenerListaDePuestos(fecha);

            ViewBag.FechaAnalisis = fechaAnalisis;
            return View(puestos);
        }
    }
}
