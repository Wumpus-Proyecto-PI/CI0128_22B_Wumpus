using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using PI.Handlers;
using PI.Models;
using unit_tests.SharedResources;

namespace unit_tests.Chris
{
    [TestClass]
    public class GastoVariableTesting
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
        GastosVariablesTestingHandler? GastosVariablesTestingHandler = null;

        [TestInitialize]
        public void Setup()
        {
            NegocioTestingHandler = new();

            AnalisisHandler = new();

            GastosVariablesTestingHandler = new(); 

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
            NegocioTestingHandler = null;
            NegocioFicticio = null;
            AnalisisHandler = null;
            AnalisisFicticio = null;
            GastosVariablesTestingHandler = null;
        }

        [TestMethod]
        public void InsertarGastoVariable_ParametrosCorrectos_ConComponentes()
        {
            //arrange

            List<ComponenteModel> componentes = new List<ComponenteModel>();

            componentes.Add(new ComponenteModel
            {
                NombreProducto = "producto-test",
                Nombre = "componente-test",
                Costo = 1,
                Unidad = "Kilos",
                Cantidad = 1,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            });

            ProductoModel producto = new ProductoModel()
            {
                Nombre = "producto-test",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 1,
                CostoVariable = 1,
                ComisionDeVentas = 1,
                PorcentajeDeVentas = 1,
                Precio = 1,
                Componentes = componentes
            };

            // creamos handler con el metodo que deseamos probar
            ProductoHandler productoHandler = new();
            ComponenteHandler componenteHandler = new();

            //action
            productoHandler.InsertarProducto(producto.Nombre, producto);
            componenteHandler.AgregarComponente(componentes[0]);

            //assert
            List<ProductoModel> productosPostInsercion = GastosVariablesTestingHandler.leerProductosDeBase(AnalisisFicticio.FechaCreacion);
            List<ComponenteModel> componentesPostInsercion = GastosVariablesTestingHandler.leerComponentesDeBase(producto.Nombre, AnalisisFicticio.FechaCreacion);

            bool? productoIngresado = productosPostInsercion.Exists(x => x.Nombre == producto.Nombre);
            bool? componenteIngresado = componentesPostInsercion.Exists(x => x.Nombre == componentes[0].Nombre);
            
            Assert.IsTrue(productoIngresado, "El producto no se ingresó en la base de datos");
            Assert.IsTrue(componenteIngresado, "El componente no se ingresó en la base de datos");
        }

        [TestMethod]
        public void InsertarGastoVariable_ParametrosCorrectos_SinComponentes()
        {
            //arrange

            ProductoModel producto = new ProductoModel()
            {
                Nombre = "producto-test",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 1,
                CostoVariable = 1,
                ComisionDeVentas = 1,
                PorcentajeDeVentas = 1,
                Precio = 1
            };

            // creamos handler con el metodo que deseamos probar
            ProductoHandler productoHandler = new();
            ComponenteHandler componenteHandler = new();

            //action
            productoHandler.InsertarProducto(producto.Nombre, producto);

            //assert
            List<ProductoModel> productosPostInsercion = GastosVariablesTestingHandler.leerProductosDeBase(AnalisisFicticio.FechaCreacion);
            List<ComponenteModel> componentesPostInsercion = GastosVariablesTestingHandler.leerComponentesDeBase(producto.Nombre, AnalisisFicticio.FechaCreacion);

            bool? productoIngresado = productosPostInsercion.Exists(x => x.Nombre == producto.Nombre);
            bool? componentesVacios = componentesPostInsercion.Count() == 0;

            Assert.IsTrue(productoIngresado, "El producto no se ingresó en la base de datos");
            Assert.IsTrue(componentesVacios, "Si hay componentes en la base de datos");
        }

        [TestMethod]
        public void InsertarGastoVariable_NombreLargo()
        {
            //arrange
            string excepcionEsperada = "String or binary data would be truncated.\r\nThe statement has been terminated.";

            ProductoModel producto = new ProductoModel()
            {
                Nombre = "producto-test-que-es-muy-largo-para-ingresarse",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 1,
                CostoVariable = 1,
                ComisionDeVentas = 1,
                PorcentajeDeVentas = 1,
                Precio = 1
            };

            // creamos handler con el metodo que deseamos probar
            ProductoHandler productoHandler = new();

            //action y assert
            try
            {
                productoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        [TestMethod]
        public void InsertarGastoVariable_ComisionDeVentasLarga()
        {
            //arrange
            string excepcionEsperada = "Error converting data type varchar to decimal.";

            ProductoModel producto = new ProductoModel()
            {
                Nombre = "producto-test",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 1,
                CostoVariable = 1,
                ComisionDeVentas = 12345678912345612123,
                PorcentajeDeVentas = 1,
                Precio = 1
            };

            // creamos handler con el metodo que deseamos probar
            ProductoHandler productoHandler = new();

            //action y assert
            try
            {
                productoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        [TestMethod]
        public void InsertarGastoVariable_MontoLargo()
        {
            //arrange
            string excepcionEsperada = "Error converting data type varchar to decimal.";

            ProductoModel producto = new ProductoModel()
            {
                Nombre = "producto-test",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 1,
                CostoVariable = 1,
                ComisionDeVentas = 1,
                PorcentajeDeVentas = 1,
                Precio = 11221212121212121212
            };

            // creamos handler con el metodo que deseamos probar
            ProductoHandler productoHandler = new();

            //action y assert
            try
            {
                productoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        [TestMethod]
        public void InsertarGastoVariable_CostoVariableLargo()
        {
            //arrange
            string excepcionEsperada = "Error converting data type varchar to decimal.";

            ProductoModel producto = new ProductoModel()
            {
                Nombre = "producto-test",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 1,
                CostoVariable = 11212121212121212112,
                ComisionDeVentas = 1,
                PorcentajeDeVentas = 1,
                Precio = 1
            };

            // creamos handler con el metodo que deseamos probar
            ProductoHandler productoHandler = new();

            //action y assert
            try
            {
                productoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        [TestMethod]
        public void InsertarGastoVariable_LoteNegativo()
        {
            //arrange
            string excepcionEsperada = "Error converting data type varchar to decimal.";

            ProductoModel producto = new ProductoModel()
            {
                Nombre = "producto-test",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = -1,
                CostoVariable = 1,
                ComisionDeVentas = 1,
                PorcentajeDeVentas = 1,
                Precio = 1
            };

            // creamos handler con el metodo que deseamos probar
            ProductoHandler productoHandler = new();

            //action y assert
            try
            {
                productoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        [TestMethod]
        public void InsertarGastoVariable_MontoNegativo()
        {
            //arrange
            string excepcionEsperada = "Error converting data type varchar to decimal.";

            ProductoModel producto = new ProductoModel()
            {
                Nombre = "producto-test",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 1,
                CostoVariable = 1,
                ComisionDeVentas = 1,
                PorcentajeDeVentas = 1,
                Precio = -1
            };

            // creamos handler con el metodo que deseamos probar
            ProductoHandler productoHandler = new();

            //action y assert
            try
            {
                productoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        [TestMethod]
        public void EliminarGastoVariable_Existente()
        {
            //arrange
            //action
            //assert
        }

        [TestMethod]
        public void EliminarGastoVariable_NoExistente()
        {
            //arrange
            //action
            //assert
        }
    }
}
