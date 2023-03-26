using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI.EntityHandlers;
using PI.Controllers;
using PI.EntityModels;
using System.Globalization;
using PI.Services;
using Microsoft.AspNetCore.Components;

namespace PI.Controllers
{
    // Controlador del analisis
    public class AnalisisController : ManejadorUsuariosController
    {
        private AnalisisHandler? AnalisisHandler = null;
        public AnalisisController(AnalisisHandler analisisHandler)
        {
            AnalisisHandler = analisisHandler;
        }
        public async Task<IActionResult> CrearAnalisis(int idNegocio, string estadoNegocio)
        {
            DateTime fechaCreacionAnalisis = await AnalisisHandler.IngresarAnalisis(idNegocio, estadoNegocio);

            return RedirectToAction("Index", "Analisis", new { fechaAnalisis = fechaCreacionAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }

        // Recibe el id del negocio del que se quiere obtener los análisis creados.
        // Retorna una lista de análisis creados que pertenen al negocio.
        public async Task<IActionResult> MisAnalisis(int idNegocio)
        {
            Console.WriteLine("id: " + idNegocio);
            // Título de la pestaña en el navegador.
            ViewData["Title"] = "Mis análisis";
            // Título del paso en el que se está en el layout
            ViewData["TituloPaso"] = ViewData["Title"];
            // Muestra el botón de regreso que lleva a la vista de los negocios.
            ViewBag.BotonRetorno = "Mis negocios";

            ViewData["NombreNegocio"] = await AnalisisHandler.ObtenerNombreNegocioAsync(idNegocio);
            ViewBag.idNegocio = idNegocio;

            return View(await AnalisisHandler.ObtenerAnalisis(idNegocio));
        }
        // Devuelve la vista principal del analisis especifico
        // (Retorna la vista del analisis | Parametros: fecha del analisis que se desea visualizar)
        public async Task<IActionResult> Index(DateTime fechaAnalisis)
        {
            // DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            Analisis analisisActual = await AnalisisHandler.ObtenerUnAnalisis(fechaAnalisis);
            ViewData["NombreNegocio"] = await AnalisisHandler.ObtenerNombreNegocioAsync(fechaAnalisis);
            ViewData["TituloPaso"] = "Progreso del análisis";
            // se asigna el titulo en la pestaña del cliente
            ViewData["Title"] = ViewData["TituloPaso"];
            ViewData["IDNegocio"] = analisisActual.IdNegocio;
            ViewBag.fechaAnalisis = fechaAnalisis;
            ViewBag.gananciaMensual = analisisActual.GananciaMensual;
            PasosProgresoControl controlDePasos = new();

            ViewBag.pasoDisponibleMaximo = controlDePasos.EstaActivoMaximo(analisisActual);
            
            return View(analisisActual);
        }

        // Guarda la configuracion del analisis y devuelve a la vista del analisis
        // (Retorna la vista del analisis que se estaba configurando | Parametros: fecha del analisis, Porcentaje del seguro social, Porcentaje de prestaciones laborales)

        // Redirige a la pantalla de "mis análisis" de un negocio
        public async Task<IActionResult> EliminarAnalisis(string fechaAnalisis, int idNegocio)
        {
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            await AnalisisHandler.EliminarAnalisis(fechaCreacionAnalisis);

            return RedirectToAction("MisAnalisis", "Analisis", new { idNegocio = idNegocio });
        }
    }
}
