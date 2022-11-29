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

        // handler de productos
        ProductoHandler? ProductoHandler = null;

        // handler de componentes
        ComponenteHandler? ComponenteHandler = null; 
        
        // en la inicializacion creamos un negocio de testing en nuestro usuario de testing y extraemos el analisis que genera
        [TestInitialize]
        public void Setup()
        {
            // Se inicializan los handlers 
            NegocioTestingHandler = new();

            AnalisisHandler = new();

            ProductoHandler = new(); 

            ComponenteHandler = new();

            // para que el test exista debe existir el siguiente usuario en la base
            // usuario: wumpustest@gmail.com 
            // id del usuario: 81727ffe-84fc-4263-b7e4-e763664968d9
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

        // Prueba la insecion de un puesto con atributos correctos y que posea componentes 
        [TestMethod]
        public void InsertarGastoVariable_ParametrosCorrectos_ConComponentes()
        {
            //arrange

            // Crea lista de componentes que se va a insertar en el producto
            List<ComponenteModel> componentes = new List<ComponenteModel>();

            // Se agrega un componente a la lista
            componentes.Add(new ComponenteModel
            {
                NombreProducto = "producto-test",
                Nombre = "componente-test",
                Costo = 1,
                Unidad = "Kilos",
                Cantidad = 1,
                FechaAnalisis = AnalisisFicticio.FechaCreacion
            });

            // Se crea un producto de prueba 
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

            //action

            // Se inserta el producto y su componente a la base de datos
            ProductoHandler.InsertarProducto(producto.Nombre, producto);
            ComponenteHandler.AgregarComponente(componentes[0]);

            //assert

            // Listas que contienen los productos y componentes despues de la insercion 
            List<ProductoModel> productosPostInsercion = ProductoHandler.ObtenerProductos(AnalisisFicticio.FechaCreacion);
            List<ComponenteModel> componentesPostInsercion = ComponenteHandler.ObtenerComponentes(producto.Nombre, AnalisisFicticio.FechaCreacion);
            
            // Variable booleanas que comprueban si existe el componente y el producto en la lista
            bool? productoIngresado = productosPostInsercion.Exists(x => x.Nombre == producto.Nombre);
            bool? componenteIngresado = componentesPostInsercion.Exists(x => x.Nombre == componentes[0].Nombre);
            
            Assert.IsTrue(productoIngresado, "El producto no se ingresó en la base de datos");
            Assert.IsTrue(componenteIngresado, "El componente no se ingresó en la base de datos");
        }

        // Prueba la insecion de un puesto con atributos correctos y que no posea componentes
        [TestMethod]
        public void InsertarGastoVariable_ParametrosCorrectos_SinComponentes()
        {
            //arrange

            // Se crea un producto de prueba
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

            //action

            // Se inserta el producto creado a la base de datos
            ProductoHandler.InsertarProducto(producto.Nombre, producto);

            //assert

            // Lista con los componentes y los productos de la base despues de la insercion. 
            List<ProductoModel> productosPostInsercion = ProductoHandler.ObtenerProductos(AnalisisFicticio.FechaCreacion);
            List<ComponenteModel> componentesPostInsercion = ComponenteHandler.ObtenerComponentes(producto.Nombre, AnalisisFicticio.FechaCreacion);

            // Variables booleana que comprueban que el producto exista y que no posea componentes
            bool? productoIngresado = productosPostInsercion.Exists(x => x.Nombre == producto.Nombre);
            bool? componentesVacios = componentesPostInsercion.Count() == 0;

            Assert.IsTrue(productoIngresado, "El producto no se ingresó en la base de datos");
            Assert.IsTrue(componentesVacios, "Si hay componentes en la base de datos");
        }

        // Prueba la insercion fallida de un producto cuando su nombre es muy largo
        [TestMethod]
        public void InsertarGastoVariable_NombreLargo()
        {
            //arrange

            // String con la excepcion que se genera al ingresar un producto con nombre muy largo 
            string excepcionEsperada = "String or binary data would be truncated.\r\nThe statement has been terminated.";

            // Se crea un producto de prueba
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

            //action y assert
            try
            {
                ProductoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        // Prueba la insercion fallida de un producto cuando la comision de ventas es muy larga
        [TestMethod]
        public void InsertarGastoVariable_ComisionDeVentasLarga()
        {
            //arrange

            // String con la excepcion que se espera
            string excepcionEsperada = "Error converting data type varchar to decimal.";

            // Se crea un producto de prueba
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

            //action y assert
            try
            {
                ProductoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        // Prueba la insercion fallida de un producto cuando el monto o precio es muy grande
        [TestMethod]
        public void InsertarGastoVariable_MontoLargo()
        {
            //arrange

            // String con la excepcion que se espera
            string excepcionEsperada = "Error converting data type varchar to decimal.";

            // Se crea un producto de prueba
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

            //action y assert
            try
            {
                ProductoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        // Prueba la insercion fallida de un producto cuando el costo variable es muy grande 
        [TestMethod]
        public void InsertarGastoVariable_CostoVariableLargo()
        {
            //arrange

            // String con la excepcion que se espera
            string excepcionEsperada = "Error converting data type varchar to decimal.";

            // Se crea un producto de prueba
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

            //action y assert
            try
            {
                ProductoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        // Prueba la insercion fallida de un producto cuando el lote es un valor negativo 
        [TestMethod]
        public void InsertarGastoVariable_LoteNegativo()
        {
            //arrange

            // String con la excepcion que se espera
            string excepcionEsperada = "El valor del lote debe ser un número positivo";

            // Se crea un producto de prueba
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

            //action y assert
            try
            {
                ProductoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        // Prueba la insercion fallida de un producto cuando el precio o monto es negativo
        [TestMethod]
        public void InsertarGastoVariable_MontoNegativo()
        {
            //arrange

            // String con la excepcion que se espera
            string excepcionEsperada = "El valor del monto debe ser un número positivo";

            // Se crea un producto de prueba
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

            //action y assert
            try
            {
                ProductoHandler.InsertarProducto(producto.Nombre, producto);
            }
            catch (Exception e)
            {
                Assert.AreEqual(excepcionEsperada, e.Message);
            }
        }

        // Prueba la eliminacion de un producto que si se encuentra almacenado en la base de datos 
        [TestMethod]
        public void EliminarGastoVariable_Existente()
        {
            //arrange

            // Se crea un producto de prueba
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

            // Se inserta el producto para luego eliminarlo
            ProductoHandler.InsertarProducto(producto.Nombre, producto);

            // Lista con los productos antes de la eliminacion 
            List<ProductoModel> productosPostInsercion = ProductoHandler.ObtenerProductos(AnalisisFicticio.FechaCreacion); 
            int cantidadAntesDeBorrar = productosPostInsercion.Count();

            //action

            // Se elimina el producto
            ProductoHandler.EliminarProducto(producto);

            // Lista con los productos luego de ser eliminado
            List<ProductoModel> productosPostEliminado = ProductoHandler.ObtenerProductos(AnalisisFicticio.FechaCreacion); 
            int cantidadDespuesDeBorrar = productosPostEliminado.Count();

            //assert
            Assert.AreNotEqual(cantidadAntesDeBorrar, cantidadDespuesDeBorrar);
        }

        // Prueba la eliminacion de un producto que no se encuentra almacenado en la base de datos
        [TestMethod]
        public void EliminarGastoVariable_NoExistente()
        {
            //arrange

            // Se crea un producto de prueba 
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

            // No se inserta ya que lo que se va a probar es cuando no esta en la base

            // Lista con los productos antes de la eliminacion
            List<ProductoModel> productosPreEliminacion = ProductoHandler.ObtenerProductos(AnalisisFicticio.FechaCreacion); 
            int cantidadAntesDeBorrar = productosPreEliminacion.Count();

            //action
            ProductoHandler.EliminarProducto(producto);

            // Lista con los productos despues de la eliminacion
            List<ProductoModel> productosPostEliminado = ProductoHandler.ObtenerProductos(AnalisisFicticio.FechaCreacion); 
            int cantidadDespuesDeBorrar = productosPostEliminado.Count();

            //assert
            Assert.AreEqual(cantidadAntesDeBorrar, cantidadDespuesDeBorrar);
        }
    }
}
