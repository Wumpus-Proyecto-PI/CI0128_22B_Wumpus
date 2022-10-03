using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI.Handlers;
using PI.Controllers;
using PI.Models;

namespace PI.Controllers
{
    public class AnalisisController : Controller
    {
/*        public IActionResult Index()
        {
            AnalisisHandler handler = new AnalisisHandler();
            // var tipoAnalisis = handler.ObtenerTipoAnalisis();
            return View();
        }*/

        public IActionResult Index(string fechaAnalisis)
        {
            AnalisisHandler handler = new AnalisisHandler();
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            AnalisisModel analisisActual = handler.ObtenerUnAnalisis(fechaCreacionAnalisis);
            ViewData["NombreNegocio"] = handler.obtenerNombreNegocio(fechaCreacionAnalisis);
            ViewData["TituloPaso"] = "Progreso del análisis";
            // var tipoAnalisis = handler.ObtenerTipoAnalisis();
            return View(analisisActual);
        }

        public static bool hayPuestos(AnalisisModel analisis) {
            bool resultado = false;
            EstructuraOrgHandler estHandler = new EstructuraOrgHandler();
            List<PuestoModel> puestos = estHandler.ObtenerListaDePuestos(analisis.FechaCreacion);
            if (puestos.Count > 0) {
                resultado = true;
            }
            return resultado;
        }

        public static bool contieneGastosFijos(AnalisisModel analisis) {
            bool resultado = false;
            GastoFijoHandler gastosHandler = new GastoFijoHandler();
            List<GastoFijoModel> gastosFijos = gastosHandler.ObtenerGastosFijos(analisis.FechaCreacion);
            for (int i = 0; i<gastosFijos.Count(); i += 1) {
                if (gastosFijos[i].Nombre != "Seguridad social" && gastosFijos[i].Nombre != "Prestaciones laborales" && gastosFijos[i].Nombre != "Beneficios de empleados" && gastosFijos[i].Nombre != "Salarios netos") {
                    resultado = true;
                    break;
                }
            }
            return resultado;
        }
    }
}
