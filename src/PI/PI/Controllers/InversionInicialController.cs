using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;

namespace PI.Controllers
{
    public class InversionInicialController : Controller
    {
        public IActionResult InversionInicial(string fechaAnalisis)
        {
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            // Datos que ocupa la vista
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            ViewData["Title"] = "Inversión inicial";
            ViewData["TituloPaso"] = ViewData["Title"];
            ViewBag.BotonRetorno = "Progreso";

            InversionInicialHandler inversionInicialHandler = new InversionInicialHandler();
            ViewData["NombreNegocio"] = inversionInicialHandler.obtenerNombreNegocio(fechaCreacionAnalisis);
            
            return View(inversionInicialHandler.ObtenerGastosIniciales(fechaAnalisis));
        }
    }
}