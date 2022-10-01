using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PI.Controllers
{
    public class GastoFijoController : Controller
    {
        public IActionResult GastoFijo(string fecha)
        {

            ViewData["TituloPaso"] = "Gastos fijos";
            DateTime fechaConversion = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();

            ViewBag.totalMensual = gastoFijoHandler.obtenerTotalMensual(fechaConversion);
            ViewData["NombreNegocio"] = gastoFijoHandler.obtenerNombreNegocio(fechaConversion);

            List<GastoFijoModel> gastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaConversion);

            return View(gastosFijos);
        }
    }
}
