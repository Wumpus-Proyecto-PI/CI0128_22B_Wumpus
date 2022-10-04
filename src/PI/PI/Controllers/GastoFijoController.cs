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
            AnalisisHandler analisisHandler = new AnalisisHandler();
            ConfigAnalisisModel configuracionAnalisis = analisisHandler.ObtenerConfigAnalisis(fechaConversion);
            decimal seguroSocial = configuracionAnalisis.PorcentajeSS / 100;
            decimal prestaciones = configuracionAnalisis.PorcentajePL / 100;


            gastoFijoHandler.actualizarSalariosNeto(fechaConversion, seguroSocial, prestaciones);
            gastoFijoHandler.actualizarSeguroSocial(fechaConversion, seguroSocial);
            gastoFijoHandler.actualizarPrestaciones(fechaConversion, prestaciones);
            gastoFijoHandler.actualizarBeneficios(fechaConversion);

            ViewBag.totalMensual = gastoFijoHandler.obtenerTotalMensual(fechaConversion);
            ViewData["NombreNegocio"] = gastoFijoHandler.obtenerNombreNegocio(fechaConversion);

            List<GastoFijoModel> gastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaConversion);

            return View(gastosFijos);
        }
    }
}
