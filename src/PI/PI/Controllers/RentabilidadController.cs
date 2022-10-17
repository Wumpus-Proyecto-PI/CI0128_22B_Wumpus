using Microsoft.AspNetCore.Mvc;
using PI.Models;

namespace PI.Controllers
{
    public class RentabilidadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
