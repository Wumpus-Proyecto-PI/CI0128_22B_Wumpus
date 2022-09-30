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
        public IActionResult GastoFijo()
        {

            ViewData["TituloPaso"] = "Gastos Fijos";
            DateTime fechaConversion = DateTime.ParseExact("2022-09-29 17:25:27.807", "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            List<GastoFijoModel> gastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaConversion);

            ViewBag.totalMensual = gastoFijoHandler.obtenerTotalMensual(fechaConversion);
            ViewData["NombreNegocio"] = gastoFijoHandler.obtenerNombreNegocio(fechaConversion);


            return View(gastosFijos);
        }
    }
}
