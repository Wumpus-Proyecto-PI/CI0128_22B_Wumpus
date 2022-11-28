using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PI.Handlers;
using PI.Models;
using unit_tests.SharedResources;

namespace unit_tests.Chris
{
    public class GastoVariableTesting : HandlerGenerico
    {
        // handler de testing que permite crear un negocio de pruebas
        private NegocioTestingHandler? NegocioTestingHandler = null;

        // handler de analisis que nos permite obtener analisis del negocio
        private AnalisisHandler? AnalisisHandler = null;

        // negocio ficticio creado para la prueba
        private NegocioModel? NegocioFicticio = null;

        // analisis ficticio creado para la prueba
        private AnalisisModel? AnalisisFicticio = null;

        // handler de testing usado para manejar la creacion de puestos semilla
        GastosVariablesTestingHandler? GastoVariableTestingHandler = null;
    }
}
