using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI.Handlers;
using PI.Controllers;
using PI.Models;
using System.Globalization;

namespace PI.Controllers
{
    public class AnalisisController : Controller
    {
        public IActionResult Index(string fechaAnalisis)
        {
            AnalisisHandler handler = new AnalisisHandler();
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            AnalisisModel analisisActual = handler.ObtenerUnAnalisis(fechaCreacionAnalisis);
            ViewData["NombreNegocio"] = handler.obtenerNombreNegocio(fechaCreacionAnalisis);
            ViewData["TituloPaso"] = "Progreso del análisis";
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            // var tipoAnalisis = handler.ObtenerTipoAnalisis();
            return View(analisisActual);
        }

        public IActionResult ConfiguracionAnalisis (string fechaAnalisis)
        {
            ViewBag.FechaAnalisis = fechaAnalisis;
            AnalisisHandler analisisHandler = new AnalisisHandler();
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;

            ConfigAnalisisModel configAnalisis = analisisHandler.ObtenerConfigAnalisis(fechaCreacionAnalisis);
            ViewData["fechaFormateada"] = fechaCreacionAnalisis.ToString("dd/MMM/yyyy - hh:mm tt", CultureInfo.InvariantCulture);
            ViewData["TituloPaso"] = "Configuración del análisis";
            return View(configAnalisis);
        }


        public IActionResult GuardarConfiguracion(string fechaAnalisis, decimal porcentajeSS = -1.0m, decimal porcentajePL = -1.0m)
        {
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            AnalisisHandler analisisHandler = new AnalisisHandler();
            ConfigAnalisisModel configAnalisis = new ConfigAnalisisModel
            {
                fechaAnalisis = fechaCreacionAnalisis,
                PorcentajePL = porcentajePL,
                PorcentajeSS = porcentajeSS
            };

            analisisHandler.ActualizarConfiguracionAnalisis(configAnalisis);
            return RedirectToAction("Index", "Analisis", new { fechaAnalisis = fechaCreacionAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }
    }
}
