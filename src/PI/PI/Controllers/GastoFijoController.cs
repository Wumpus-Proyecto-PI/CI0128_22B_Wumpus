﻿using Microsoft.AspNetCore.Mvc;
using PI.Handlers;
using PI.Models;
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
            ViewData["TituloPaso"] = "Gastos Fijos";
            DateTime fechaConversion = DateTime.ParseExact("2022-09-28 23:22:17.427", "yyyy-MM-dd HH:mm:ss.fff", null);
            ViewBag.fechaAnalisis = fechaConversion;

            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            List<GastoFijoModel> gastosFijos = gastoFijoHandler.ObtenerGastosFijos(fechaConversion);

            ViewBag.totalMensual = gastoFijoHandler.obtenerTotalMensual(fechaConversion);


            return View(gastosFijos);
        }
    }
}
