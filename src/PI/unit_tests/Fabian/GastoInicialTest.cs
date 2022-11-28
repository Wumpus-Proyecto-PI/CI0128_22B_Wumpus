using DocumentFormat.OpenXml.Bibliography;
using PI.Handlers;
using PI.Models;
using unit_tests.SharedResources;

namespace unit_tests.Fabian
{
    // class: clase de testing para el modelo gasto inicial y su interaccion con la base de datos
    [TestClass]
    public class GastoInicialTest
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
            // eliminar el negocio elimina todos lso datos relacionados a el
            NegocioTestingHandler.EliminarNegocioFicticio();
        }

        // este test prueba que al insertar un nuevo puesto no se modifiquen los puestos ya existentes
        [TestMethod]
        public void InsertarGastoInicial_NoModificaOtrosGastosIniciales()
        {
            // arrange

            // ingresamos una lista de puestos inicial
            List<GastoInicialModel> gastosPreInsercion = InsertarGastosInicialesSemillaEnBase(AnalisisFicticio.FechaCreacion);

            // creamos gastoInicial que vamos a insertar de prueba
            GastoInicialModel gasto = new GastoInicialModel
            {
                Nombre = "Nuevo gasto",
                Monto = 555m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            InversionInicialHandler inversionInicialHandler = new();

            // action
            string fechaAnalisis = gasto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff");
            inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gasto);

            // assert
            // obtenemos los gastos de la base
            List<GastoInicialModel> gastosPostInsercion = inversionInicialHandler.ObtenerGastosIniciales(fechaAnalisis);
            bool FueInsertado = gastosPostInsercion.Exists(x => x.Nombre == gasto.Nombre);

            // removemos de la lista el que insertamos para comparar si el resto de gastos se vieron afectados
            gastosPostInsercion.RemoveAll(x => x.Nombre == gasto.Nombre);

            bool puestoIguales = SonListasIguales(gastosPreInsercion, gastosPostInsercion);

            // args: bool a evaluar, mensaje en caso de false.
            Assert.IsTrue(puestoIguales, "Los gastos iniciales pre-inserción son diferentes a los gastos iniciales post-inserción");
            Assert.IsTrue(FueInsertado, $"'{gasto.Nombre}' no se insertó en la base");
        }


        // Evalúa que al insertar un nuevo puesto no se modifiquen los puestos ya existentes
        [TestMethod]
        public void InsertarGastoInicial_ConNombreLargo_GeneraExcepcion()
        {
            // arrange
            string excepcionEsperada = "String or binary data would be truncated.\r\nThe statement has been terminated.";

            // creamos gastoInicial que excede la cantidad de caracteres válidos
            GastoInicialModel gasto = new GastoInicialModel
            {
                Nombre = "estoEsUnaPruebaDeBufferOverflow",
                Monto = 555m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            InversionInicialHandler inversionInicialHandler = new();

            // action
            string fechaAnalisis = gasto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff");

            try
            {
                inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gasto);
            } catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        // Evalúa que el buffer permitido se desborde y genere excepción.
        [TestMethod]
        public void InsertarGastoInicial_ConMontoLargo_GeneraExcepcion()
        {
            // arrange
            string excepcionEsperada = "Error converting data type numeric to decimal.";

            // creamos gastoInicial que excede la cantidad de caracteres válidos
            GastoInicialModel gasto = new GastoInicialModel
            {
                Nombre = "Monto se excede",
                Monto = 12345678912345612.12m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            InversionInicialHandler inversionInicialHandler = new();

            // action
            string fechaAnalisis = gasto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff");

            try
            {
                inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gasto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }

        }

        // Evalúa que se pueda ingresar un nombre con tildes y caractéres especiales (ñ) para un gasto inicial.
        [TestMethod]
        public void InsertarGastoInicial_NombreConTildes()
        {
            // arrange

            // creamos gastoInicial que excede la cantidad de caracteres válidos
            GastoInicialModel gastoNuevo = new GastoInicialModel
            {
                Nombre = "áéíóú_ñ",
                Monto = 555.00m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            InversionInicialHandler inversionInicialHandler = new();

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");
            inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gastoNuevo);

            List<GastoInicialModel> gastosPostInsercion = inversionInicialHandler.ObtenerGastosIniciales(fechaAnalisis);
            bool FueInsertado = gastosPostInsercion.Exists(x => x.Nombre == gastoNuevo.Nombre);

            // assert
            // args: bool a evaluar, mensaje en caso de false.
            Assert.IsTrue(FueInsertado, $"'{gastoNuevo.Nombre}' no se insertó en la base");
        }


        // Evalúa que solamente se elimine el gasto inicial que corresponde dentro del mismo análisis.
        [TestMethod]
        public void EliminarGastoInicialExistente()
        {
            // arrange
            List<GastoInicialModel> gastosPreInsercion = InsertarGastosInicialesSemillaEnBase(AnalisisFicticio.FechaCreacion);

            String nombreGastoFijoVictima = gastosPreInsercion[0].Nombre;

            // creamos handler con el metodo que deseamos probar
            InversionInicialHandler inversionInicialHandler = new();

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");
            inversionInicialHandler.EliminarGastoInicial(fechaAnalisis, nombreGastoFijoVictima);

            List<GastoInicialModel> gastosPostInsercion = inversionInicialHandler.ObtenerGastosIniciales(fechaAnalisis);
            bool fueEliminado = gastosPostInsercion.Exists(x => x.Nombre == nombreGastoFijoVictima) == false;

            // assert
            // args: bool a evaluar, mensaje en caso de false.
            Assert.IsTrue(fueEliminado, $"'{nombreGastoFijoVictima}' no se borró en la base");
        }


        // Evalúa que se genera una excepción cuando se intenta eliminar un gasto inicial no existente.
        [TestMethod]
        public void EliminarGastoInicialNoExistente_AfectaCeroFilas()
        {
            // arrange
            List<GastoInicialModel> gastosPreInsercion = InsertarGastosInicialesSemillaEnBase(AnalisisFicticio.FechaCreacion);
            
            String nombreGastoFijoVictima = "GastoNoExistente";
            int filasAfectadasEsperadas = 0;
            int filasAfectadasResultado = -1;

            // creamos handler con el metodo que deseamos probar
            InversionInicialHandler inversionInicialHandler = new();

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");
            filasAfectadasResultado = inversionInicialHandler.EliminarGastoInicial(fechaAnalisis, nombreGastoFijoVictima);
            Assert.AreEqual(filasAfectadasEsperadas, filasAfectadasResultado);
        }


        // Evalúa que se genera una excepción cuando se intenta ingresar un valor negativo para el monto.
        [TestMethod]
        public void IngresarGastoInicial_ConMontoNegativo_GeneraExcepcion()
        {
            // arrange
            List<GastoInicialModel> gastosPreInsercion = InsertarGastosInicialesSemillaEnBase(AnalisisFicticio.FechaCreacion);

            GastoInicialModel gastoNuevo = new GastoInicialModel
            {
                Nombre = "Negativo",
                Monto = -10.3m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            String excepcionEsperada = "El valor del monto debe ser un número positivo";

            // creamos handler con el metodo que deseamos probar
            InversionInicialHandler inversionInicialHandler = new();

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");

            try
            {
                inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gastoNuevo);
            } catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }
        
        // Evalúa que se pueda ingresar un monto con valores decimales.
        [TestMethod]
        public void IngresarGastoInicial_ConMontoDecimal()
        {
            // arrange
            GastoInicialModel gastoNuevo = new GastoInicialModel
            {
                Nombre = "Decimales",
                Monto = 15489.36m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // creamos handler con el metodo que deseamos probar
            InversionInicialHandler inversionInicialHandler = new();

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");
            inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gastoNuevo);
            List<GastoInicialModel> gastosPostInsercion = inversionInicialHandler.ObtenerGastosIniciales(fechaAnalisis);
            bool fueInsertado = gastosPostInsercion.Exists(x => x.Nombre == gastoNuevo.Nombre);
            
            Assert.IsTrue(fueInsertado, $"'{gastoNuevo.Nombre}' no se insertó en la base");
        }

        // metodos que asisten a los metodos de testing

        // brief: metodo que compara si dos listas de gasto inicial model son iguales
        // details: se compara cada uno de los atributos del gasto inicial
        // return: true si las dos listas son iguales y false en caso contrario
        static public bool SonListasIguales(List<GastoInicialModel> esperada, List<GastoInicialModel> actual)
        {
            esperada = esperada.OrderBy(x => x.Nombre).ToList();
            actual = actual.OrderBy(x => x.Nombre).ToList();

            bool listasIguales = true;
            if (esperada.Count != actual.Count)
            {
                listasIguales = false;
            }
            else
            {
                for (int i = 0; i < esperada.Count && listasIguales == true; ++i)
                {
                    listasIguales = SonGastosIguales(esperada[i], actual[i]);
                }
            }
            return listasIguales;
        }

        // brief: metodo que compara si dos gastos iniciales son iguales
        // details: se compara cada uno de los atributos del gasto inicial
        // return: true si los dos son iguales y false en caso contrario
        static public bool SonGastosIguales(GastoInicialModel esperado, GastoInicialModel actual)
        {
            return esperado.Nombre == actual.Nombre && esperado.Monto == actual.Monto;
        }

        // brief: metodo que inserta en la base de datos una lista semilla de puestos
        public List<GastoInicialModel> InsertarGastosInicialesSemillaEnBase(DateTime fechaCreacion)
        {
            InversionInicialHandler inversionInicialHandler = new();
            List<GastoInicialModel> gastosInicialesSemilla = new List<GastoInicialModel>();
            gastosInicialesSemilla.Add(new GastoInicialModel
            {
                Nombre = "Cocina",
                FechaAnalisis = fechaCreacion,
                Monto = 1234m
            });
            gastosInicialesSemilla.Add(new GastoInicialModel
            {
                Nombre = "Local",
                FechaAnalisis = fechaCreacion,
                Monto = 1000000m
            });
            gastosInicialesSemilla.Add(new GastoInicialModel
            {
                Nombre = "Uniformes",
                FechaAnalisis = fechaCreacion,
                Monto = 500000m
            });

            foreach (var gasto in gastosInicialesSemilla)
            {
                inversionInicialHandler.IngresarGastoInicial(fechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff"), gasto);
            }

            return gastosInicialesSemilla;
        }
    }
}
