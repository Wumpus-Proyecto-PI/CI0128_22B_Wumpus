using Microsoft.AspNetCore.Mvc;
using PI.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PI.Services;
using PI.EntityHandlers;

namespace PI.Controllers
{
    // Controlador del gasto fijo.  Administra el traspaso de acciones entre la vista y el modelo/bd referentes al gasto fijo.
    public class GastoFijoController : Controller
    {

        private GastoFijoHandler? GastoFijoHandler = null;
        private AnalisisHandler? AnalisisHandler = null;
        private NegocioHandler? NegocioHandler = null;

        public GastoFijoController(DataBaseContext contexto)
        {
            GastoFijoHandler = new(contexto);
            AnalisisHandler = new(contexto);
            NegocioHandler = new(contexto);
        }

        // Retorna una lista de gastos fijos que pertenecen al análisis con la fecha pasada por parámetro.
        public async Task<IActionResult> GastoFijo(string fecha)
        {
            // para que se muestre el boton de volver al analisis
            ViewBag.BotonRetorno = "Progreso";

            ViewData["TituloPaso"] = "Gastos fijos";
            
            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];
            
            DateTime fechaConversion = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;

            // Convierte los porcentajes a valores válidos (divide entre 100).
            Configuracion configuracionAnalisis = await AnalisisHandler.ObtenerConfigAnalisisAsync(fechaConversion);
            decimal seguroSocial = (decimal)(configuracionAnalisis.PorcentajeSs / 100);
            decimal prestaciones = (decimal)(configuracionAnalisis.PorcentajePl / 100);

            // Actualiza los gastos fijos de la estructura organizativa para mostrarlos en la sección de gastos fijos.
            await GastoFijoHandler.ActualizarGastosPredeterminadosAsync(fechaConversion, seguroSocial, prestaciones);

            // Actualiza la sumatoria de los gastos fijos. (anual)
            ViewBag.totalAnual = await GastoFijoHandler.ObtenerTotalAnualAsync(fechaConversion);
            ViewData["NombreNegocio"] = await NegocioHandler.ObtenerNombreNegocioAsync(fechaConversion);

            List<GastoFijo> gastos = await GastoFijoHandler.ObtenerGastosFijosAsync(fechaConversion);

            string[] nombresEstOrg = { "Salarios netos", "Prestaciones laborales", "Seguridad social", "Beneficios de empleados" };

            List<GastoFijo> gastosFijos = new List<GastoFijo>();

            gastosFijos.Add(gastos.Find(x => x.Nombre == "Salarios netos"));
            gastosFijos.Add(gastos.Find(x => x.Nombre == "Prestaciones laborales"));
            gastosFijos.Add(gastos.Find(x => x.Nombre == "Seguridad social"));
            gastosFijos.Add(gastos.Find(x => x.Nombre == "Beneficios de empleados"));

            foreach (GastoFijo gastoFijo in gastos)
            {
                if (!nombresEstOrg.Contains(gastoFijo.Nombre))
                {
                    gastosFijos.Add(gastoFijo);
                }
            }

            return View(gastosFijos);
        }
    }
}
