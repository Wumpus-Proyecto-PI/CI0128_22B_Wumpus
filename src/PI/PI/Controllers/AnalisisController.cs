using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI.Handlers;
using PI.Controllers;
using PI.Models;
using System.Globalization;
using PI.Services;

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
            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];
            ViewBag.fechaAnalisis = fechaCreacionAnalisis;
            ViewBag.gananciaMensual = analisisActual.gananciaMensual;

            PasosProgresoControl controlDePasos = new();

            ViewBag.pasoDisponibleMaximo = controlDePasos.DeterminarPasoActivoMaximo(analisisActual);

            return View(analisisActual);
        }


        public IActionResult CrearAnalisis(int IDNegocio, string estadoNegocio)
        {
            AnalisisHandler analisisHandler = new AnalisisHandler();
            DateTime fechaCreacionAnalisis = analisisHandler.IngresarAnalisis(IDNegocio, estadoNegocio);

            return RedirectToAction("Index", "Analisis", new { fechaAnalisis = fechaCreacionAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }
        // Retorna la vista con todos los análisis creados
        public IActionResult MisAnalisis(int IDNegocio)
        {
            ViewData["Title"] = "Mis análisis";
            ViewData["TituloPaso"] = "Mis análisis";
            ViewBag.BotonRetorno = "Mis negocios";
            AnalisisHandler analisisHandler = new AnalisisHandler();
            ViewData["NombreNegocio"] = analisisHandler.obtenerNombreNegocio(IDNegocio);
            ViewBag.idNegocio = IDNegocio;
            return View(analisisHandler.ObtenerAnalisis(IDNegocio));
        }

        // Devuelve la vista de la configuracion de un analisis especifico 
        // (Retorna la vista de la configuracion | Parametros: la fecha del analisis cuya configuracion se quiere revisar)
        public IActionResult ConfiguracionAnalisis(string fechaAnalisis)
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
