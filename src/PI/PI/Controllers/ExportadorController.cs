using PI.Service;
using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
using PI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PI.Controllers
{
    // Controlador que se encarga de enviar el file result para que se descargue el reporte.
    public class ExportadorController : ManejadorUsuariosController
    {
        // Retorna el archivo que resulta del servicio del exportador de acuerdo a la fecha de análisis enviada.
        public FileResult Exportar(string fechaAnalisis)
        {
            ExportadorService exportador = new ExportadorService();
            return File(exportador.obtenerReporte(fechaAnalisis).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte " + DateTime.Now.ToString() + ".xlsx");
        }
    }
}
