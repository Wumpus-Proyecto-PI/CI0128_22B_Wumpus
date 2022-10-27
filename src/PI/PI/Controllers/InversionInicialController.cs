using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;

namespace PI.Controllers
{
    public class InversionInicialController : Controller
    {
        // Retorna una lista de gastos iniciales que pertenecen al análisis con la fecha pasada por parámetro.
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
            ViewBag.montoTotal = inversionInicialHandler.obtenerTotal(fechaAnalisis);
            
            return View(inversionInicialHandler.ObtenerGastosIniciales(fechaAnalisis));
        }
    }
}