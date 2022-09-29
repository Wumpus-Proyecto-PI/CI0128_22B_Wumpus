using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI.Handlers;

namespace PI.Controllers
{
    public class NegocioController : Controller
    {
        // Retorna una lista con todos los modelos de negocio existentes
        public IActionResult Index()
        {
            NegocioHandler handler = new NegocioHandler();
            var negocios = handler.ObtenerNegocios();
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
            handler.IngresarNegocio(nombreNegocio, tipoNegocio, correoUsuario);
            
            // Redirecciona a la página donde tiene todos los negocios
            return RedirectToAction("Index");
        }
    }
}
