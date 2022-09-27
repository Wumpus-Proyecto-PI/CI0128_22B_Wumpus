using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PI.Controllers
{
    public class GastoFijoController : Controller
    {
        public IActionResult GastoFijo()
        {
            ViewData["NombreNegocio"] = "Nombre del negocio";
            return View();
        }
    }
}
