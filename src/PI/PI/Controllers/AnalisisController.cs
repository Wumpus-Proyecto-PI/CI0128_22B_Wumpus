using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI.Handlers;
using PI.Controllers;
using PI.Models;

namespace PI.Controllers
{
    public class AnalisisController : Controller
    {
/*        public IActionResult Index()
        {
            AnalisisHandler handler = new AnalisisHandler();
            // var tipoAnalisis = handler.ObtenerTipoAnalisis();
            return View();
        }*/

        public IActionResult Index(string fechaAnalisis)
        {
            AnalisisHandler handler = new AnalisisHandler();
            DateTime fechaCreacionAnalisis = DateTime.ParseExact(fechaAnalisis, "yyyy-MM-dd HH:mm:ss.fff", null);
            AnalisisModel analisisActual = handler.ObtenerUnAnalisis(fechaCreacionAnalisis);

            // var tipoAnalisis = handler.ObtenerTipoAnalisis();
            return View(analisisActual);
        }
    }
}
