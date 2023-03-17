using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using System.Security.Claims;
using PI.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace PI.Controllers
{
    // Controlador del negocio. Administra el traspaso de acciones entre la vista y el modelo/bd referentes al negocio
    public class NegocioController : ManejadorUsuariosController
    {
        private DataBaseContext? Contexto = null;
        public NegocioController (DataBaseContext contexto)
        {
            Contexto = contexto;
        }

        // Retorna una lista con todos los modelos de negocio existentes y el título del paso.
        public async Task<IActionResult> Index()
        {
            // obtenemos el id del user de identity asp.net
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // con el user id buscamos los negocios en nuestras tablas
            // var negocios = _handler.ObtenerNegocios(userId);

            var negocios = await Contexto.Negocios.AsNoTracking().Where(x => x.IdUsuario == userId).ToListAsync();

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
        public async Task<IActionResult> AgregarNegocio_BD(string nombreNegocio, string tipoNegocio)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Negocio nuevoNegocio = new Negocio
            {
                Nombre = nombreNegocio,
                FechaCreacion = DateTime.Now,
                IdUsuario = userId
            };

            await Contexto.Negocios.AddAsync(nuevoNegocio);
            await Contexto.SaveChangesAsync();

            // Redirecciona al analisis por defecto
            // TODO redireccionar a la página de análisis creados (que contiene las opciones de visualizar, descargar, comparar, crear y crear copia)
            return RedirectToAction("MisAnalisis", "Analisis", new { IDNegocio = nuevoNegocio.Id });
        }

        // Elimina el negocio indicado por parámetro (mediante el ID) de la base de datos
        public async Task<IActionResult> EliminarNegocio_BD(int IdNegocio)
        {
            Contexto.Negocios.Remove(await Contexto.Negocios.FindAsync(IdNegocio));

            await Contexto.SaveChangesAsync();
            // Redirecciona a la pantalla de negocios 
            return RedirectToAction("Index", "Negocio");
        }
    }
}
