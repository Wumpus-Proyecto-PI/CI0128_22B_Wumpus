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
            decimal seguroSocial = 0.1m;
            decimal prestaciones = 0.2m;

            ViewData["TituloPaso"] = "Gastos fijos";
            DateTime fechaConversion = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();

            gastoFijoHandler.actualizarSalariosNeto(fechaConversion, seguroSocial, prestaciones);
            gastoFijoHandler.actualizarSeguroSocial(fechaConversion, seguroSocial);
            gastoFijoHandler.actualizarPrestaciones(fechaConversion, prestaciones);
            gastoFijoHandler.actualizarBeneficios(fechaConversion);

            ViewBag.totalMensual = gastoFijoHandler.obtenerTotalMensual(fechaConversion);
            ViewData["NombreNegocio"] = gastoFijoHandler.obtenerNombreNegocio(fechaConversion);

            List<GastoFijoModel> gf_unorded = gastoFijoHandler.ObtenerGastosFijos(fechaConversion);
            // ordena la lista de gastos fijos
            List<GastoFijoModel> gastosFijos = gf_unorded.OrderBy(x => x.orden).ToList();

            return View(gastosFijos);
        }
    }
}
