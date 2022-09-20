using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PI.Controllers
{
    public class EstructuraOrgController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
