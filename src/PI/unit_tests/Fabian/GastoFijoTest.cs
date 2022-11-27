using DocumentFormat.OpenXml.Bibliography;
using PI.Handlers;
using PI.Models;
using unit_tests.SharedResources;

namespace unit_tests.Fabian
{
    // class: clase de testing para el modelo gasto fijo y su interaccion con la base de datos
    [TestClass]
    public class GastoFijoTest
    {
        // handler de testing que permite crear un negocio de pruebas
        private NegocioTestingHandler? NegocioTestingHandler = null;

        // handler de analisis que nos permite obtener analisis del negocio
        private AnalisisHandler? AnalisisHandler = null;

        // negocio ficticio creado para la prueba
        private NegocioModel? NegocioFicticio = null;

        // analisis ficticio creado para la prueba
        private AnalisisModel? AnalisisFicticio = null;

        // en la inicializacion creamos un negocio de testing en nuestro usuario de testing y extraemos el analisis que genera
        [TestInitialize]
        public void Setup()
        {
            // creamos un negocio ficticio al usuario de testing de wumpus
            NegocioTestingHandler = new();

            // tambien le creamos un analisis vacio para realizar las pruebas
            AnalisisHandler = new();

            // para que el test exista debe existir el siguiente usuario en la base
            // usuario: wumpustest@gmail.com 
            // id del usuario: e690ef97-31c4-4064-bede-93aeedaf6857
            NegocioFicticio = NegocioTestingHandler.IngresarNegocioFicticio(TestingUserModel.UserId, "Emprendimiento");
            AnalisisFicticio = AnalisisHandler.ObtenerAnalisisMasReciente(NegocioFicticio.ID);
        }

        // en el cleanup eliminamos el negocio que creamos
        [TestCleanup]
        public void CleanUp()
        {
            // eliminar el negocio elimina todos los datos relacionados a el
            NegocioTestingHandler.EliminarNegocioFicticio();
        }

        // Evalúa que al insertar un nuevo gasto fijo con un nombre muy largo, genere excepción.
        [TestMethod]
        public void InsertarGastoFijo_ConNombreLargo_GeneraExcepcion()
        {
            // arrange
            string excepcionEsperada = "String or binary data would be truncated.\r\nThe statement has been terminated.";

            // creamos fijo que excede la cantidad de caracteres válidos para el nombre
            GastoFijoModel gasto = new GastoFijoModel
            {
                Nombre = "estoEsUnaPruebaDeBufferOverflow",
                Monto = 555m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                orden = 0,
            };

            // creamos handler con el metodo que deseamos probar
            GastoFijoHandler gastoFijoHandler = new();

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");

            try
            {
                gastoFijoHandler.ingresarGastoFijo(gasto.Nombre, gasto.Nombre, gasto.Monto.ToString(), gasto.FechaAnalisis);
            } catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }

            // assert
            List<GastoFijoModel> gastosPostInsercion = gastoFijoHandler.ObtenerGastosFijos(AnalisisFicticio.FechaCreacion);
            bool fueInsertado = gastosPostInsercion.Exists(x => x.Nombre == gasto.Nombre);

            // args: bool a evaluar, mensaje en caso de false.
            Assert.IsFalse(fueInsertado, $"'{gasto.Nombre}' se insertó en la base");
        }

        // Evalúa que se genera una excepción cuando se intenta ingresar un valor negativo para el monto.
        [TestMethod]
        public void IngresarGastoFijo_ConMontoNegativo_GeneraExcepcion()
        {
            // arrange

            GastoFijoModel gastoNuevo = new GastoFijoModel
            {
                Nombre = "Negativo",
                Monto = -10.3m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                orden = 0
            };

            String excepcionEsperada = "El valor del monto debe ser un número positivo";

            // creamos handler con el metodo que deseamos probar
            GastoFijoHandler gastoFijoHandler = new();

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");

            try
            {
                gastoFijoHandler.ingresarGastoFijo(gastoNuevo.Nombre, gastoNuevo.Nombre, gastoNuevo.Monto.ToString(), gastoNuevo.FechaAnalisis);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }

            // assert
            List<GastoFijoModel> gastosPostInsercion = gastoFijoHandler.ObtenerGastosFijos(AnalisisFicticio.FechaCreacion);
            bool fueInsertado = gastosPostInsercion.Exists(x => x.Nombre == gastoNuevo.Nombre);

            // args: bool a evaluar, mensaje en caso de false.
            Assert.IsFalse(fueInsertado, $"'{gastoNuevo.Nombre}' se insertó en la base");
        }
    }
}
