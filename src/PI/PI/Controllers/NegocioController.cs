using Microsoft.AspNetCore.Mvc;
using System;
using PI.Handlers;
using PI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PI.Areas.Identity.Data;
using PI.Data;
using System.Security.Claims;

namespace PI.Controllers
{
    // Controlador del negocio. Administra el traspaso de acciones entre la vista y el modelo/bd referentes al negocio
    public class NegocioController : ManejadorUsuariosController
    {
        // Retorna una lista con todos los modelos de negocio existentes y el título del paso.
        public IActionResult Index()
        {
            NegocioHandler handler = new NegocioHandler();

            // obtenemos el id del user de identity asp.net
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // con el user id buscamos los negocios en nuestras tablas
            var negocios = handler.ObtenerNegocios(userId);

            ViewData["Title"] = "Mis negocios";
            ViewData["TituloPaso"] = "Mis negocios";
            return View(negocios);
        }

        // Retorna el formulario para ingresar el nombre y estado del negocio.
        public IActionResult FormAgregarNegocio()
        {
            ViewData["Title"] = "Nuevo negocio";
            return View();
        }

        // agrega un negocio con los datos pasados por parámetros a la base de datos.
        public IActionResult agregarNegocio_BD(String nombreNegocio, String tipoNegocio)
        {
            // usuario quemado para efectos del primer sprint.
            // TODO cambiar por un correo de usuario auténtico.
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // string correoUsuario = "gabrielperezorozco@testing.com";

            // Inserta el negocio en la base de datos.
            NegocioHandler handler = new NegocioHandler();
            NegocioModel negocioIngresado = handler.IngresarNegocio(nombreNegocio, tipoNegocio, userId);

            // Redirecciona al analisis por defecto
            // TODO redireccionar a la página de análisis creados (que contiene las opciones de visualizar, descargar, comparar, crear y crear copia)
            return RedirectToAction("MisAnalisis", "Analisis", new { IDNegocio = negocioIngresado.ID});
        }

        // Elimina el negocio indicado por parámetro (mediante el ID) de la base de datos
        public IActionResult eliminarNegocio_BD(string IDNegocio)
        {
            NegocioHandler handler = new NegocioHandler();
            handler.EliminarNegocio(IDNegocio);

            // Redirecciona a la pantalla de negocios 
            return RedirectToAction("Index", "Negocio");
        }

        // Redirecciona al analisis por defecto
        // // TODO redireccionar a pantalla de mis analisis
        public IActionResult direccionarAnalisis(string IDNegocio)
        {
            AnalisisHandler analisisHandler = new AnalisisHandler();
            DateTime ultimoFechaAnalisis = analisisHandler.UltimaFechaCreacion(IDNegocio);

            // Redirecciona al analisis por defecto
            return RedirectToAction("Index", "Analisis", new { fechaAnalisis = ultimoFechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff") });
        }
    }
}
