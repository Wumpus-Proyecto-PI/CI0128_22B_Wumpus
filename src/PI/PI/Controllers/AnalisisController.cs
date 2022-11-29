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
using Microsoft.AspNetCore.Components;

namespace PI.Controllers
{
    // Controlador del analisis
    public class AnalisisController : ManejadorUsuariosController
    {
        public IActionResult CrearAnalisis(int idNegocio, string estadoNegocio)
        {
            AnalisisHandler analisisHandler = new AnalisisHandler();
            DateTime fechaCreacionAnalisis = analisisHandler.IngresarAnalisis(idNegocio, estadoNegocio);

            return RedirectToAction("Index", "Analisis", new { fechaAnalisis = fechaCreacionAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }

        // Recibe el id del negocio del que se quiere obtener los análisis creados.
        // Retorna una lista de análisis creados que pertenen al negocio.
        public IActionResult MisAnalisis(int idNegocio)
        {
            Console.WriteLine("id: " + idNegocio);
            // Título de la pestaña en el navegador.
            ViewData["Title"] = "Mis análisis";
            // Título del paso en el que se está en el layout
            ViewData["TituloPaso"] = ViewData["Title"];
            // Muestra el botón de regreso que lleva a la vista de los negocios.
            ViewBag.BotonRetorno = "Mis negocios";

            AnalisisHandler analisisHandler = new AnalisisHandler();
            ViewData["NombreNegocio"] = analisisHandler.obtenerNombreNegocio(idNegocio);
            ViewBag.idNegocio = idNegocio;
            return View(analisisHandler.ObtenerAnalisis(idNegocio));
        }
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
            ViewBag.gananciaMensual = analisisActual.GananciaMensual;
            PasosProgresoControl controlDePasos = new();

            ViewBag.pasoDisponibleMaximo = controlDePasos.EstaActivoMaximo(analisisActual);
            
            return View(analisisActual);
        }

        // Guarda la configuracion del analisis y devuelve a la vista del analisis
        // (Retorna la vista del analisis que se estaba configurando | Parametros: fecha del analisis, Porcentaje del seguro social, Porcentaje de prestaciones laborales)

        // Redirige a la pantalla de "mis análisis" de un negocio
        public IActionResult EliminarAnalisis(string fechaAnalisis, int idNegocio)
        {
            AnalisisHandler analisisHandler = new();
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            analisisHandler.EliminarAnalisis(fechaCreacionAnalisis);

            return RedirectToAction("MisAnalisis", "Analisis", new { idNegocio = idNegocio });
        }
    }
}
