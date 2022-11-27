using PI.Handlers;
using PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unit_tests.SharedResources;

namespace unit_tests.Wilmer
{
    [TestClass]
    public class WilmerTests
    {
        #region SetUp y CleanUp
        // handler de testing uqe permite crear un negocio de pruebas
        private NegocioTestingHandler? NegocioTestingHandler = null;

        // handler de analisis que nos permite obtener analisis del negocio
        private AnalisisHandler? AnalisisHandler = null;

        // negocio ficticio creado para la prueba
        private NegocioModel? NegocioFicticio = null;

        // analisis ficticio creado para la prueba
        private AnalisisModel? AnalisisFicticio = null;

        // handler de testing usado para manejar la creacion de puestos semilla
        PuestoTestingHandler? PuestoTestingHandler = null;

        // handler de testing usado para manejar la lectura de gastos fijos en la base
        GastosFijosTestingHandler? GastosFijosTesting = null;

        // en la inicializacion creamos un negocio de testing en nuestro usuario de testing y extraemos el analisis que genera
        [TestInitialize]
        public void Setup()
        {

            // creamos un negocio ficticio al usuario de testing de wumpus
            NegocioTestingHandler = new();

            // tambien le creamos un analisis vacio para realizar las pruebas
            AnalisisHandler = new();

            // creamos instancia de handler de testing usado para manejar la creacion de puestos semilla
            PuestoTestingHandler = new();

            // Creamos handler para testing de gastos fijos
            GastosFijosTesting = new();

            // para que el test exista debe existir el siguiente usuario en la base
            // usuario: wumpustest@gmail.com 
            // id del usuario: e690ef97-31c4-4064-bede-93aeedaf6857
            NegocioFicticio = NegocioTestingHandler.IngresarNegocioFicticio(TestingUserModel.UserId, "Emprendimiento");
            AnalisisFicticio = AnalisisHandler.ObtenerAnalisisMasReciente(NegocioFicticio.ID);

            PuestoTestingHandler.InsertarPuestosSemillaEnBase(AnalisisFicticio.FechaCreacion);
        }

        // en el cleanup eliminamos el negocio que creamos
        [TestCleanup]
        public void CleanUp()
        {
            // eliminar el negocio elimina todos lso datos relacionados a el
            NegocioTestingHandler.EliminarNegocioFicticio();
            NegocioTestingHandler = null;
            NegocioFicticio = null;
            AnalisisHandler = null;
            AnalisisFicticio = null;
            PuestoTestingHandler = null;
        }
        #endregion

        #region Testing Gastos Fijos

        [TestMethod]
        public void ActualizarSalariosNetoNoTiraExpeciones()
        {
            // Preparacion
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            decimal seguroSocial = 0.05m;
            decimal prestaciones = 0.0m;

            // action
            gastoFijoHandler.actualizarSalariosNeto(AnalisisFicticio.FechaCreacion, seguroSocial, prestaciones);

            // assert

            // leemos los gastos fijos
            List<GastoFijoModel> gastosFijos = GastosFijosTesting.leerGastosFijosDeBase(AnalisisFicticio.FechaCreacion);

            // buscamos el gasto fijo correspondiente a los salarios netos
            // solo buscamos por el nombre porque este es unico ya que es llave primaria en la base de datos
            GastoFijoModel? gastoSalariosNetos = gastosFijos.Find(x => x.Nombre == "Salarios netos");

            // si es nulo el gasto fijo significa que no se ingreso en la base de datos
            Assert.IsNotNull(gastoSalariosNetos, "El gasto de salarios netos no se encontró en la base de datos");
        }

        #endregion
    }
}
