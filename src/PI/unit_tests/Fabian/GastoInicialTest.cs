using PI.Handlers;
using PI.Models;
using unit_tests.SharedResources;

namespace unit_tests.Fabian
{
    // Clase de testing para el modelo gasto inicial y su interaccion con la base de datos.
    [TestClass]
    public class GastoInicialTest
    {
        // Handler de negocio y análisis a probar.
        private NegocioTestingHandler? NegocioTestingHandler = null;
        private AnalisisHandler? AnalisisHandler = null;

        // negocio y análisis ficticios creados para la prueba
        private NegocioModel? NegocioFicticio = null;
        private AnalisisModel? AnalisisFicticio = null;
        private InversionInicialHandler inversionInicialHandler = null;

        // Crea un negocio de testing en nuestro usuario de testing y extraemos el analisis que genera
        // para que el test exista debe existir el siguiente usuario en la base
        // usuario: wumpustest@gmail.com 
        // id del usuario: 81727ffe-84fc-4263-b7e4-e763664968d9
        [TestInitialize]
        public void Setup()
        {
            // Negocio y analisis ficticios al usuario de testing de wumpus
            NegocioTestingHandler = new();
            AnalisisHandler = new();
            NegocioFicticio = NegocioTestingHandler.IngresarNegocioFicticio(TestingUserModel.UserId, "Emprendimiento");
            AnalisisFicticio = AnalisisHandler.ObtenerAnalisisMasReciente(NegocioFicticio.ID);
            // Handler que será probado
            inversionInicialHandler = new();
        }

        // Eliminamos el negocio de prueba y sus datos
        [TestCleanup]
        public void CleanUp()
        {
            NegocioTestingHandler.EliminarNegocioFicticio();
        }

        // Evalúa que al insertar un nuevo gasto inicial no se modifiquen los gastos iniciales ya existentes
        [TestMethod]
        public void InsertarGastoInicial_NoModificaOtrosGastosIniciales()
        {
            // arrange
            List<GastoInicialModel> gastosPreInsercion = InsertarGastosInicialesSemillaEnBase(AnalisisFicticio.FechaCreacion);

            GastoInicialModel gasto = new GastoInicialModel
            {
                Nombre = "Nuevo gasto",
                Monto = 555m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            // action
            string fechaAnalisis = gasto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff");
            inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gasto);

            // assert
            List<GastoInicialModel> gastosPostInsercion = inversionInicialHandler.ObtenerGastosIniciales(fechaAnalisis);
            bool FueInsertado = gastosPostInsercion.Exists(x => x.Nombre == gasto.Nombre);

            // removemos de la lista el gasto que insertamos para comparar si el resto de gastos se vieron afectados
            gastosPostInsercion.RemoveAll(x => x.Nombre == gasto.Nombre);

            bool puestoIguales = SonListasIguales(gastosPreInsercion, gastosPostInsercion);

            // args: bool a evaluar, mensaje en caso de false.
            Assert.IsTrue(puestoIguales, "Los gastos iniciales pre-inserción son diferentes a los gastos iniciales post-inserción");
            Assert.IsTrue(FueInsertado, $"'{gasto.Nombre}' no se insertó en la base");
        }

        // Evalúa que el buffer del nombre se desborde y genere excepción.
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

            

            // action
            string fechaAnalisis = gasto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff");
            
            // assert
            try
            {
                inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gasto);
            } catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        // Evalúa que el buffer del monto se desborde y genere excepción.
        [TestMethod]
        public void InsertarGastoInicial_ConMontoLargo_GeneraExcepcion()
        {
            // arrange
            string excepcionEsperada = "Error converting data type numeric to decimal.";

            GastoInicialModel gasto = new GastoInicialModel
            {
                Nombre = "Monto se excede",
                Monto = 12345678912345612.12m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            

            // action
            string fechaAnalisis = gasto.FechaAnalisis.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // assert
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
            GastoInicialModel gastoNuevo = new GastoInicialModel
            {
                Nombre = "áéíóú_ñ",
                Monto = 555.00m,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            };

            

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");
            inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gastoNuevo);

            List<GastoInicialModel> gastosPostInsercion = inversionInicialHandler.ObtenerGastosIniciales(fechaAnalisis);
            bool FueInsertado = gastosPostInsercion.Exists(x => x.Nombre == gastoNuevo.Nombre);

            // assert
            // args: bool a evaluar, mensaje en caso de false.
            Assert.IsTrue(FueInsertado, $"'{gastoNuevo.Nombre}' no se insertó en la base");
        }

        // Evalúa que solamente se elimine el gasto inicial que corresponde, dentro del mismo análisis.
        [TestMethod]
        public void EliminarGastoInicialExistente()
        {
            // arrange
            List<GastoInicialModel> gastosPreInsercion = InsertarGastosInicialesSemillaEnBase(AnalisisFicticio.FechaCreacion);
            String nombreGastoFijoVictima = gastosPreInsercion[0].Nombre;
            

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

            // assert
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
            

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // assert
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
            

            // action
            string fechaAnalisis = AnalisisFicticio.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss.fff");
            inversionInicialHandler.IngresarGastoInicial(fechaAnalisis, gastoNuevo);
            
            // assert
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
