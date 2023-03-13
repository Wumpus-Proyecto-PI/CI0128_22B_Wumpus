using Microsoft.AspNetCore.Mvc;
using System;
using PI.Handlers;
using PI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PI.Areas.Identity.Data;
using PI.Data;
using System.Security.Claims;
using PI.EntityModels;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace PI.Controllers
{
    // Controlador del negocio. Administra el traspaso de acciones entre la vista y el modelo/bd referentes al negocio
    public class NegocioController : ManejadorUsuariosController
    {
        private DataBaseContext? _handler = null;
        public NegocioController (DataBaseContext handler)
        {
            _handler = handler;
        }


        // Retorna una lista con todos los modelos de negocio existentes y el título del paso.
        public IActionResult Index()
        {
            // obtenemos el id del user de identity asp.net
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // con el user id buscamos los negocios en nuestras tablas
            // var negocios = _handler.ObtenerNegocios(userId);

            var negocios = _handler.Negocios.AsNoTracking().Where(x => x.IdUsuario == userId).ToList();

            ViewData["Title"] = "Mis negocios";
            ViewData["TituloPaso"] = "Mis negocios";
            return View(negocios);
            // return View(new List<NegocioModel> ());
        }

        // Retorna el formulario para ingresar el nombre y estado del negocio.
        public IActionResult FormAgregarNegocio()
        {
            ViewData["Title"] = "Nuevo negocio";
            return View();
        }

        // agrega un negocio con los datos pasados por parámetros a la base de datos.
        public async Task<IActionResult> agregarNegocio_BD(String nombreNegocio, String tipoNegocio)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Negocio nuevoNegocio = new();
            nuevoNegocio.Nombre = nombreNegocio;
            nuevoNegocio.FechaCreacion = DateTime.Now;
            nuevoNegocio.Id = await _handler.Negocios.MaxAsync(x => x.Id) + 1;
            nuevoNegocio.IdUsuario = userId;

            await _handler.Negocios.AddAsync(nuevoNegocio);

            await _handler.SaveChangesAsync();
            // Redirecciona al analisis por defecto
            // TODO redireccionar a la página de análisis creados (que contiene las opciones de visualizar, descargar, comparar, crear y crear copia)
            // return RedirectToAction("MisAnalisis", "Analisis", new { IDNegocio = nuevoNegocio.Id });
            return RedirectToAction("Index", "Negocio");
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
