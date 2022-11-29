using DocumentFormat.OpenXml.Bibliography;
using PI.Handlers;
using PI.Models;
using unit_tests.SharedResources;

namespace unit_tests.Fabian
{
    // Clase de testing para el modelo gasto fijo y su interaccion con la base de datos
    [TestClass]
    public class GastoFijoTest
    {
        // Handler de negocio y análisis a probar.
        private NegocioTestingHandler? NegocioTestingHandler = null;
        private AnalisisHandler? AnalisisHandler = null;

        // negocio y análisis ficticios creados para la prueba
        private NegocioModel? NegocioFicticio = null;
        private AnalisisModel? AnalisisFicticio = null;

        private GastoFijoHandler gastoFijoHandler = new();

        // Crea un negocio de testing en nuestro usuario de testing y extraemos el analisis que genera
        // para que el test exista debe existir el siguiente usuario en la base
        // usuario: wumpustest@gmail.com 
        // id del usuario: 81727ffe-84fc-4263-b7e4-e763664968d9
        [TestInitialize]
        public void Setup()
        {
            // creamos un negocio y analisis ficticios al usuario de testing de wumpus
            NegocioTestingHandler = new();
            AnalisisHandler = new();
            NegocioFicticio = NegocioTestingHandler.IngresarNegocioFicticio(TestingUserModel.UserId, "Emprendimiento");
            AnalisisFicticio = AnalisisHandler.ObtenerAnalisisMasReciente(NegocioFicticio.ID);

            // Handler que será probado
            gastoFijoHandler = new();
        }

        // Eliminamos el negocio de prueba y sus datos
        [TestCleanup]
        public void CleanUp()
        {
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

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // assert
            try
            {
                gastoFijoHandler.ingresarGastoFijo(gasto.Nombre, gasto.Nombre, gasto.Monto.ToString(), gasto.FechaAnalisis);
            } catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }

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

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            // assert
            try
            {
                gastoFijoHandler.ingresarGastoFijo(gastoNuevo.Nombre, gastoNuevo.Nombre, gastoNuevo.Monto.ToString(), gastoNuevo.FechaAnalisis);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }

            List<GastoFijoModel> gastosPostInsercion = gastoFijoHandler.ObtenerGastosFijos(AnalisisFicticio.FechaCreacion);
            bool fueInsertado = gastosPostInsercion.Exists(x => x.Nombre == gastoNuevo.Nombre);
            // args: bool a evaluar, mensaje en caso de false.
            Assert.IsFalse(fueInsertado, $"'{gastoNuevo.Nombre}' se insertó en la base");
        }
    }
}
