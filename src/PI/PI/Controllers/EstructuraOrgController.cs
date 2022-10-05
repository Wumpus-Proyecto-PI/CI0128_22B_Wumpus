using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using System.Dynamic;

namespace PI.Controllers
{
    // clase controladora que se encarga de las vistas y los llamados necesarios con respecto a la estrutura organizativa
    public class EstructuraOrgController : Controller
    {
        // Acción que retorna la vista predeterminada de la estrutura organizativa
        // Recibe un string que indica la fecha del análisis a la que pertenece la estrutura organizativa actual
        // Retorna una vista cshtml con un componente razor que permite la creación de puestos y de beneficios desde el lado del cliente
        public IActionResult Index(string fecha)
        {
            // asignamos el título del paso en el que se está en el layout
            ViewData["TituloPaso"] = "Estructura organizativa";

            // creamos instancia del handler de la estrutura organizativa para cargar los puestos
            EstructuraOrgHandler estructura = new EstructuraOrgHandler();

            // convertimos a Datetime la fecha del análisis porque de esta forma lo utiliza la vista
            DateTime fechaAnalisis = DateTime.ParseExact(fecha, "yyyy-MM-dd HH:mm:ss.fff", null);
            // enviamos la fecha a la vista con viewbag
            ViewBag.FechaAnalisis = fechaAnalisis;

            // asignamos el nombre del negocio en la vista
            // este se extrae de la base de datos con la fecha del análisis
            ViewData["NombreNegocio"] = estructura.obtenerNombreNegocio(fechaAnalisis);

            // cargamos una lista con todos los puestos que existen en la base de datos
            List<PuestoModel> puestos = estructura.ObtenerListaDePuestos(fechaAnalisis);
            return View(puestos);
        }
    }
}
