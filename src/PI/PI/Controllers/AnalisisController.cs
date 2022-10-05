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
        // Se encarga de verificar si existen puestos dentro de un análisis.
        public static bool hayPuestos(AnalisisModel analisis) {
            bool resultado = false;
            // Se crea instancia del handler
            EstructuraOrgHandler estHandler = new EstructuraOrgHandler();
            // Se obtiene de la base de datos los diferentes puestos del Análisis.
            List<PuestoModel> puestos = estHandler.ObtenerListaDePuestos(analisis.FechaCreacion);
            // Se determina si la cantidad de puestos que posee es mayor a 0
            if (puestos.Count > 0) {
                resultado = true;
            }
            return resultado;
        }
        // Determina si un análisis contiene gastos fijos.
        public static bool contieneGastosFijos(AnalisisModel analisis) {
            bool resultado = false;
            // Se crea instancia del handler
            GastoFijoHandler gastosHandler = new GastoFijoHandler();
            // Mediante el handler, se obtiene de la base de datos la cantidad de gastos fijos que contiene un análisis.
            List<GastoFijoModel> gastosFijos = gastosHandler.ObtenerGastosFijos(analisis.FechaCreacion);
            // Por cada uno de los gastos fijos obtenidos, se verifica si corresponde a uno de los gastos fijos por defecto de los análisis.
            // Si alguno de los gastos fijos obtenidos es diferente a todos ellos, se determina que si se le han agregado gastos fijos al análisis.
            for (int i = 0; i<gastosFijos.Count(); i += 1) {
                if (gastosFijos[i].Nombre != "Seguridad social" && gastosFijos[i].Nombre != "Prestaciones laborales" && gastosFijos[i].Nombre != "Beneficios de empleados" && gastosFijos[i].Nombre != "Salarios netos") {
                    resultado = true;
                    break;
                }
            }
            return resultado;
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
