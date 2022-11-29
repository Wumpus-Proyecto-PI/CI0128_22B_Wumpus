using PI.Handlers;
using PI.Models;
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

        FlujoDeCajaTestingHandler? FlujoDeCajaTestingHandler = null;

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

            FlujoDeCajaTestingHandler = new();

            // para que el test exista debe existir el siguiente usuario en la base
            // usuario: wumpustest@gmail.com 
            // id del usuario: 81727ffe-84fc-4263-b7e4-e763664968d9
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

        [TestMethod]
        public void ActualizarSeguroSocialNoTiraExpeciones()
        {
            // Preparacion
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            decimal seguroSocial = 0.05m;

            // action
            gastoFijoHandler.actualizarSeguroSocial(AnalisisFicticio.FechaCreacion, seguroSocial);

            // assert

            // leemos los gastos fijos
            List<GastoFijoModel> gastosFijos = GastosFijosTesting.leerGastosFijosDeBase(AnalisisFicticio.FechaCreacion);

            // buscamos el gasto fijo correspondiente al seguro social
            // solo buscamos por el nombre porque este es unico ya que es llave primaria en la base de datos
            GastoFijoModel? gastoSeguroSocial = gastosFijos.Find(x => x.Nombre == "Seguridad social");

            // si es nulo el gasto fijo significa que no se ingreso en la base de datos
            Assert.IsNotNull(gastoSeguroSocial, "El gasto de seguro social no se encontró en la base de datos");

            Assert.AreEqual(80031.60m, gastoSeguroSocial.Monto, "No se calculó correctamente el monto");
        }

        [TestMethod]
        public void ActualizarPrestacionesNoTiraExpeciones()
        {
            // Preparacion
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();
            decimal prestaciones = 0.05m;

            // action
            gastoFijoHandler.actualizarPrestaciones(AnalisisFicticio.FechaCreacion, prestaciones);

            // assert

            // leemos los gastos fijos
            List<GastoFijoModel> gastosFijos = GastosFijosTesting.leerGastosFijosDeBase(AnalisisFicticio.FechaCreacion);

            // buscamos el gasto fijo correspondiente a las prestaciones laborales
            // solo buscamos por el nombre porque este es unico ya que es llave primaria en la base de datos
            GastoFijoModel? gastoPrestaciones = gastosFijos.Find(x => x.Nombre == "Prestaciones laborales");

            // si es nulo el gasto fijo significa que no se ingreso en la base de datos
            Assert.IsNotNull(gastoPrestaciones, "El gasto de prestaciones laborales no se encontró en la base de datos");

            Assert.AreEqual(80031.60m, gastoPrestaciones.Monto, "No se calculó correctamente el monto");
        }

        [TestMethod]
        public void ActualizarBeneficiosNoTiraExpeciones()
        {
            // Preparacion
            GastoFijoHandler gastoFijoHandler = new GastoFijoHandler();

            // action
            gastoFijoHandler.actualizarBeneficios(AnalisisFicticio.FechaCreacion);

            // assert

            // leemos los gastos fijos
            List<GastoFijoModel> gastosFijos = GastosFijosTesting.leerGastosFijosDeBase(AnalisisFicticio.FechaCreacion);

            // buscamos el gasto fijo correspondiente a los beneficios de los empleados
            // solo buscamos por el nombre porque este es unico ya que es llave primaria en la base de datos
            GastoFijoModel? gastoBeneficios = gastosFijos.Find(x => x.Nombre == "Beneficios de empleados");

            // si es nulo el gasto fijo significa que no se ingreso en la base de datos
            Assert.IsNotNull(gastoBeneficios, "El gasto de beneficios no se encontró en la base de datos");

            Assert.AreEqual(14163588.00m, gastoBeneficios.Monto, "No se calculó correctamente el monto");
        }
        #endregion

        #region Testing Producto Handler

        [TestMethod]
        public void ActualizarPorcentajeDeVentasNoTiraExpeciones()
        {
            // Preparacion
            ProductoModel productoPrueba = new ProductoModel 
            { 
                Nombre = "Empanadas",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 5,
                PorcentajeDeVentas = 0.5m
            };

            ProductoHandler productoHandler = new ProductoHandler();
            // Se inserta el producto en la base de datos
            productoHandler.InsertarProducto("Empanadas", productoPrueba);

            // action
            // Se cambia el porcentaje de ventas para probar la actualizacion
            productoPrueba.PorcentajeDeVentas = 0.10m;
            productoHandler.ActualizarPorcentajeVentas(productoPrueba, AnalisisFicticio.FechaCreacion);

            // assert
            // leemos los productos
            List<ProductoModel> productos = productoHandler.ObtenerProductos(AnalisisFicticio.FechaCreacion);

            // buscamos el producto correspondiente
            ProductoModel? productoResultado = productos.Find(x => x.Nombre == "Empanadas");

            // si es nulo el gasto fijo significa que no se ingreso en la base de datos
            Assert.IsNotNull(productoResultado, "El producto no se encontró en la base de datos");

            Assert.AreEqual(productoResultado.PorcentajeDeVentas, productoPrueba.PorcentajeDeVentas, "No se guardo correctamente el porcentaje");
        }

        [TestMethod]
        public void ActualizarPrecioNoTiraExpeciones()
        {
            // Preparacion
            ProductoModel productoPrueba = new ProductoModel
            {
                Nombre = "Empanadas",
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Lote = 5,
                PorcentajeDeVentas = 0.5m,
                Precio = 500m
            };

            ProductoHandler productoHandler = new ProductoHandler();
            // Se inserta el producto en la base de datos
            productoHandler.InsertarProducto("Empanadas", productoPrueba);

            // action
            // Se cambia el precio para probar la actualizacion
            productoPrueba.Precio = 1000m;
            productoHandler.ActualizarPrecio(productoPrueba, AnalisisFicticio.FechaCreacion);

            // assert
            // leemos los productos
            List<ProductoModel> productos = productoHandler.ObtenerProductos(AnalisisFicticio.FechaCreacion);

            // buscamos el producto correspondiente
            ProductoModel? productoResultado = productos.Find(x => x.Nombre == "Empanadas");

            // si es nulo el gasto fijo significa que no se ingreso en la base de datos
            Assert.IsNotNull(productoResultado, "El producto no se encontró en la base de datos");

            Assert.AreEqual(productoResultado.Precio, productoPrueba.Precio, "No se guardo correctamente el precio");
        }

        #endregion

        #region Testing Flujo de caja Handler


        [TestMethod]
        public void CrearIngresosSeGuardanEnLaBase()
        {
            // Preparacion
            FlujoDeCajaHandler flujoDeCajaHandler = new FlujoDeCajaHandler();
            List<MesModel> meses = flujoDeCajaHandler.ObtenerMeses(AnalisisFicticio.FechaCreacion);

            // Action. Se llama al metodo del handler que crea los ingresos
            flujoDeCajaHandler.CrearFlujoDeCaja(AnalisisFicticio.FechaCreacion);

            // assert
            // Buscamos los ingresos en la base de datos
            List<IngresoModel> ingresos = flujoDeCajaHandler.ObtenerIngresos(AnalisisFicticio.FechaCreacion);

            // Se busca un ingreso especifico para comparar
            IngresoModel? ingresoResultado = ingresos.Find(x => x.Mes == "Mes 6");

            // si es nula la lista de ingresos significa que no se ingreso en la base de datos
            Assert.IsNotNull(ingresos, "El ingreso no se encontró en la base de datos");
            Assert.AreEqual("Mes 6", ingresoResultado.Mes);

        }

        [TestMethod]
        public void ActualizarIngresoDeContadoEnLaBase()
        {
            // Preparacion
            FlujoDeCajaHandler flujoDeCajaHandler = new FlujoDeCajaHandler();
            flujoDeCajaHandler.CrearFlujoDeCaja(AnalisisFicticio.FechaCreacion);
            List<MesModel> meses = flujoDeCajaHandler.ObtenerMeses(AnalisisFicticio.FechaCreacion);

            // Se define el tipo de ingreso
            string tipoIngreso = "contado";
            decimal monto = 500m;

            IngresoModel ingresoPrueba = new IngresoModel
            {
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Mes = meses[0].NombreMes,
                Monto = monto,
                Tipo = tipoIngreso
            };

            // action. Se llama al metodo del handler que crea los ingresos
            flujoDeCajaHandler.ActualizarIngreso(ingresoPrueba);

            // assert

            // Buscamos el ingreso en la base de datos
            IngresoModel ingreso = FlujoDeCajaTestingHandler.obtenerIngreso(ingresoPrueba.FechaAnalisis, ingresoPrueba.Mes, ingresoPrueba.Tipo);

            // si es nulo el ingreso significa que no se ingreso en la base de datos
            Assert.IsNotNull(ingreso, "El ingreso no se encontró en la base de datos");
            Assert.AreEqual(monto, ingreso.Monto, "No se guardó correctamente el monto");
            Assert.AreEqual(tipoIngreso, ingreso.Tipo, "No se guardó correctamente el tipo");
        }
        
        
        [TestMethod]
        public void ActualizarIngresoACreditoEnLaBase()
        {
            // Preparacion
            FlujoDeCajaHandler flujoDeCajaHandler = new FlujoDeCajaHandler();
            flujoDeCajaHandler.CrearFlujoDeCaja(AnalisisFicticio.FechaCreacion);
            List<MesModel> meses = flujoDeCajaHandler.ObtenerMeses(AnalisisFicticio.FechaCreacion);

            // Se define el tipo de ingreso
            string tipoIngreso = "credito";
            decimal monto = 10000m;

            IngresoModel ingresoPrueba = new IngresoModel
            {
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Mes = meses[0].NombreMes,
                Monto = monto,
                Tipo = tipoIngreso
            };

            // action. Se llama al metodo del handler que crea los ingresos
            flujoDeCajaHandler.ActualizarIngreso(ingresoPrueba);

            // assert

            // Buscamos el ingreso en la base de datos
            IngresoModel ingreso = FlujoDeCajaTestingHandler.obtenerIngreso(ingresoPrueba.FechaAnalisis, ingresoPrueba.Mes, ingresoPrueba.Tipo);

            // si es nulo el ingreso significa que no se ingreso en la base de datos
            Assert.IsNotNull(ingreso, "El ingreso no se encontró en la base de datos");
            Assert.AreEqual(monto, ingreso.Monto, "No se guardó correctamente el monto");
            Assert.AreEqual(tipoIngreso, ingreso.Tipo, "No se guardó correctamente el tipo");
        }

        [TestMethod]
        public void ActualizarOtrosIngresoEnLaBase()
        {
            // Preparacion
            FlujoDeCajaHandler flujoDeCajaHandler = new FlujoDeCajaHandler();
            flujoDeCajaHandler.CrearFlujoDeCaja(AnalisisFicticio.FechaCreacion);
            List<MesModel> meses = flujoDeCajaHandler.ObtenerMeses(AnalisisFicticio.FechaCreacion);

            // Se define el tipo de ingreso
            string tipoIngreso = "otros";
            decimal monto = 50000m;

            IngresoModel ingresoPrueba = new IngresoModel
            {
                FechaAnalisis = AnalisisFicticio.FechaCreacion,
                Mes = meses[0].NombreMes,
                Monto = monto,
                Tipo = tipoIngreso
            };

            // action. Se llama al metodo del handler que crea los ingresos
            flujoDeCajaHandler.ActualizarIngreso(ingresoPrueba);

            // assert

            // Buscamos el ingreso en la base de datos
            IngresoModel ingreso = FlujoDeCajaTestingHandler.obtenerIngreso(ingresoPrueba.FechaAnalisis, ingresoPrueba.Mes, ingresoPrueba.Tipo);

            // si es nulo el ingreso significa que no se ingreso en la base de datos
            Assert.IsNotNull(ingreso, "El ingreso no se encontró en la base de datos");
            Assert.AreEqual(monto, ingreso.Monto, "No se guardó correctamente el monto");
            Assert.AreEqual(tipoIngreso, ingreso.Tipo, "No se guardó correctamente el tipo");
        }

        #endregion
    }
}

