using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PI.Handlers;
using PI.Controllers;

namespace PI.Controllers
{
    public class AnalisisController : Controller
    {
        public IActionResult Index()
        {
            AnalisisHandler handler = new AnalisisHandler();
            // var tipoAnalisis = handler.ObtenerTipoAnalisis();
            return View();
        }
    }
}
