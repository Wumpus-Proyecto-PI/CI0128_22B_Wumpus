using Microsoft.AspNetCore.Mvc;
using PI.Handlers;

namespace PI.Controllers
{
    public class FlujoDeCajaController : Controller
    {
        public IActionResult IndexFlujoDeCaja(string fechaAnalisis)
        {
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            // Datos que ocupa la vista
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            // Título de la pestaña en el navegador.
            ViewData["Title"] = "Flujo de caja";
            // Título del paso en el que se está en el layout
            ViewData["TituloPaso"] = ViewData["Title"];
            // Muestra el botón de regreso hacia el progreso (pasos) del análisis.
            ViewBag.BotonRetorno = "Progreso";

            InversionInicialHandler inversionInicialHandler = new InversionInicialHandler();
            ViewData["NombreNegocio"] = inversionInicialHandler.obtenerNombreNegocio(fechaCreacionAnalisis);

            return View();
        }
    }
}
