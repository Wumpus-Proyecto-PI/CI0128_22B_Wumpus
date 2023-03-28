using Microsoft.AspNetCore.Mvc;
using PI.EntityHandlers;
using PI.Services;

namespace PI.Controllers
{
    // Controlador que se encarga de enviar el file result para que se descargue el reporte.
    public class ExportadorController : ManejadorUsuariosController
    {

        private ProductoHandler? ProductoHandler = null;
        private FlujoDeCajaHandler? FlujoDeCajaHandler = null;
        private GastoFijoHandler? GastoFijoHandler = null;
        private AnalisisHandler? AnalisisHandler = null;

        public ExportadorController(ProductoHandler? productoHandler, FlujoDeCajaHandler? flujoDeCajaHandler, GastoFijoHandler? gastoFijoHandler, AnalisisHandler? analisisHandler)
        {
            ProductoHandler = productoHandler;
            FlujoDeCajaHandler = flujoDeCajaHandler;
            GastoFijoHandler = gastoFijoHandler;
            AnalisisHandler = analisisHandler;
        }
        // Retorna el archivo que resulta del servicio del exportador de acuerdo a la fecha de análisis enviada.
        public async Task<FileResult> Exportar(string fechaAnalisis)
        {
            ExportadorService exportador = new ExportadorService(ProductoHandler, FlujoDeCajaHandler, GastoFijoHandler, AnalisisHandler);
            return File((await exportador.obtenerReporte(fechaAnalisis)).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte " + DateTime.Now.ToString() + ".xlsx");
        }
    }
}
