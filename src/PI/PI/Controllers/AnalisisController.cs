﻿using Microsoft.AspNetCore.Mvc;
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
    // Controlador del analisis
    public class AnalisisController : Controller
    {
        // Devuelve la vista principal del analisis especifico
        // (Retorna la vista del analisis | Parametros: fecha del analisis que se desea visualizar)
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

        // Indica si el analisis posee puestos
        // (Retorna un bool que indica si hay puestos o no | Parametros: modelo del analisis que se desea verificar)
        public static bool hayPuestos(AnalisisModel analisis) {
            bool resultado = false;
            EstructuraOrgHandler estHandler = new EstructuraOrgHandler();
            List<PuestoModel> puestos = estHandler.ObtenerListaDePuestos(analisis.FechaCreacion);
            if (puestos.Count > 0) {
                resultado = true;
            }
            return resultado;
        }

        // Indica si el analisis posee gastos fijos
        // (Retorna un bool que indica si hay gastos fijos o no | Parametros: modelo del analisis que se desea verificar)
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

        // Devuelve la vista de la configuracion de un analisis especifico 
        // (Retorna la vista de la configuracion | Parametros: la fecha del analisis cuya configuracion se quiere revisar)
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

        // Guarda la configuracion del analisis y devuelve a la vista del analisis
        // (Retorna la vista del analisis que se estaba configurando | Parametros: fecha del analisis, Porcentaje del seguro social, Porcentaje de prestaciones laborales)
        public IActionResult GuardarConfiguracion(string fechaAnalisis, decimal porcentajeSS = -1.0m, decimal porcentajePL = -1.0m)
        {
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            AnalisisHandler analisisHandler = new AnalisisHandler();

            // Crea una instancia de clase con la configuracion establecida
            ConfigAnalisisModel configAnalisis = new ConfigAnalisisModel
            {
                fechaAnalisis = fechaCreacionAnalisis,
                PorcentajePL = porcentajePL,
                PorcentajeSS = porcentajeSS
            };
            // Actualiza la configuracion del analisis
            analisisHandler.ActualizarConfiguracionAnalisis(configAnalisis);
            return RedirectToAction("Index", "Analisis", new { fechaAnalisis = fechaCreacionAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }
    }
}
