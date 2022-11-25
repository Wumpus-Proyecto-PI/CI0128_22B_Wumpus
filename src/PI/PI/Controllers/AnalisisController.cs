﻿using Microsoft.AspNetCore.Mvc;
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

            // Convierte los porcentajes a valores válidos (divide entre 100).
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            ConfigAnalisisModel configuracionAnalisis = handler.ObtenerConfigAnalisis(fechaCreacionAnalisis);
            decimal seguroSocial = configuracionAnalisis.PorcentajeSS / 100;
            decimal prestaciones = configuracionAnalisis.PorcentajePL / 100;

            // Actualiza los gastos fijos de la estructura organizativa para mostrarlos en la sección de gastos fijos.
            gastoFijoHandler.actualizarGastosPredeterminados(fechaCreacionAnalisis, seguroSocial, prestaciones);
            
            return View(analisisActual);
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
            return RedirectToAction("Index", "Analisis", new  { fechaAnalisis = fechaAnalisis});
        }
        // método que redirige al progreso del análisis correspondiente.
        public IActionResult EliminarAnalisis(string fechaAnalisis, int idNegocio)
        {
            Console.WriteLine("idEliminar: " + idNegocio);

            AnalisisHandler analisisHandler = new();
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            analisisHandler.EliminarAnalisis(fechaCreacionAnalisis);

            return RedirectToAction("MisAnalisis", "Analisis", new { idNegocio = idNegocio });
        }
    }
}
