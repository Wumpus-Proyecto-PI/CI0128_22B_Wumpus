using Microsoft.AspNetCore.Mvc;
using PI.EntityHandlers;
using PI.EntityModels;

namespace PI.Controllers
{ 
    public class InversionInicialController : ManejadorUsuariosController
    {
        public InversionInicialHandler? InversionInicialHandler = null; 
        public InversionInicialController(InversionInicialHandler? inversionInicialHandler)
        {
            InversionInicialHandler = inversionInicialHandler;  
        }

        // Recibe la fecha del análisis del que se quiere obtener los gastos iniciales.
        // Retorna una lista de gastos iniciales que pertenecen al análisis con la fecha pasada por parámetro.
        public async Task<IActionResult> InversionInicial(string fechaAnalisis)
        {
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);

            // Datos que ocupa la vista
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            // Título de la pestaña en el navegador.
            ViewData["Title"] = "Inversión inicial";
            // Título del paso en el que se está en el layout
            ViewData["TituloPaso"] = ViewData["Title"];
            // Muestra el botón de regreso hacia el progreso (pasos) del análisis.
            ViewBag.BotonRetorno = "Progreso";

           
            ViewData["NombreNegocio"] = InversionInicialHandler.obtenerNombreNegocio(fechaCreacionAnalisis);
            ViewBag.montoTotal = await InversionInicialHandler.ObtenerMontoTotalAsync(fechaCreacionAnalisis);

            List<InversionInicial> inversionInicialList = await InversionInicialHandler.ObtenerGastosInicialesAsync(fechaCreacionAnalisis);
            
            return View(inversionInicialList);
        }
    }
}