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
    // Controlador del gasto fijo.  Administra el traspaso de acciones entre la vista y el modelo/bd referentes al gasto fijo.
    public class ExportadorController : ManejadorUsuariosController
    {
        public FileResult Exportar(string fechaAnalisis)
        {
            ExportadorService exportador = new ExportadorService();
            return File(exportador.obtenerReporte(fechaAnalisis).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte " + DateTime.Now.ToString() + ".xlsx");
        }
    }
}
