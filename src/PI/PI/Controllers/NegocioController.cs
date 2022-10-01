using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI.Handlers;
using PI.Models;

namespace PI.Controllers
{
    public class NegocioController : Controller
    {
        // Retorna una lista con todos los modelos de negocio existentes
        public IActionResult Index()
        {
            NegocioHandler handler = new NegocioHandler();
            var negocios = handler.ObtenerNegocios();
            ViewData["Title"] = "Mis negocios";

            ViewData["TituloPaso"] = "Mis negocios";
            return View(negocios);
        }
        public IActionResult FormAgregarNegocio()
        {
            // Retorna el formulario (popup) para ingresar el nombre y tipo de negocio.
            return View();
        }

        public IActionResult agregarNegocio_BD(String nombreNegocio, String tipoNegocio, string correoUsuario)
        {
            // usuario quemado para efectos del primer sprint
            correoUsuario = "gabrielperezorozco@testing.com";
            // Inserta el negocio en la base de datos.
            NegocioHandler handler = new NegocioHandler();
            NegocioModel negocioIngresado =  handler.IngresarNegocio(nombreNegocio, tipoNegocio, correoUsuario);

            AnalisisHandler analisisHandler = new AnalisisHandler();
            DateTime ultimoFechaAnalisis = analisisHandler.UltimaFechaCreacion(negocioIngresado.ID.ToString());

            // Redirecciona al analisis por defecto
            return RedirectToAction("Index", "Analisis", new { fechaAnalisis = ultimoFechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }

        // Elimina el negocio indicado de la base de datos
        public IActionResult eliminarNegocio_BD(string IDNegocio)
        {
            NegocioHandler handler = new NegocioHandler();
            handler.EliminarNegocio(IDNegocio);

            var negocios = handler.ObtenerNegocios();
            // Redirecciona a la pantalla de negocios 
            return RedirectToAction("Index", "Negocio");
        }

        // Redirecciona al analisis por defecto (Luego a pantalla de mis analisis)
        public IActionResult direccionarAnalisis(string IDNegocio)
        {
            AnalisisHandler analisisHandler = new AnalisisHandler();
            DateTime ultimoFechaAnalisis = analisisHandler.UltimaFechaCreacion(IDNegocio);

            // Redirecciona al analisis por defecto
            return RedirectToAction("Index", "Analisis", new { fechaAnalisis = ultimoFechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }
    }
}
