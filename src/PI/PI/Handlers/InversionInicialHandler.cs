using Microsoft.AspNetCore.Mvc;

namespace PI.Handlers
{
    public class InversionInicialHandler : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
