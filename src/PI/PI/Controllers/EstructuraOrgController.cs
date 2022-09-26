using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;

namespace PI.Controllers
{
    public class EstructuraOrgController : Controller
    {
        public IActionResult Index()
        {
            EstructuraOrgHandler estructura = new EstructuraOrgHandler();

            // fecha quemada de testing
            string fechaAnalisis = "2002-09-09 12:00:00 AM";
            List<PuestoModel> puestos = estructura.ObtenerListaDePuestos(fechaAnalisis);
            return View(puestos);
        }
    }
}
