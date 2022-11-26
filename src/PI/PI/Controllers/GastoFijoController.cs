using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PI.Controllers
{
    // Controlador del gasto fijo.  Administra el traspaso de acciones entre la vista y el modelo/bd referentes al gasto fijo.
    public class GastoFijoController : ManejadorUsuariosController
    {
        // Retorna una lista de gastos fijos que pertenecen al análisis con la fecha pasada por parámetro.
        public IActionResult GastoFijo(string fecha)
        {
            // para que se muestre el boton de volver al analisis
            ViewBag.BotonRetorno = "Progreso";

            ViewData["TituloPaso"] = "Gastos fijos";
            
            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];
            
            DateTime fechaConversion = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();

            // Actualiza la sumatoria de los gastos fijos. (anual)
            ViewBag.totalAnual = gastoFijoHandler.obtenerTotalAnual(fechaConversion);
            ViewData["NombreNegocio"] = gastoFijoHandler.obtenerNombreNegocio(fechaConversion);

            List<GastoFijoModel> gastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaConversion);

            return View(gastosFijos);
        }
    }
}
