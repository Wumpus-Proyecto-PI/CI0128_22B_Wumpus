using functional_tests.Pages;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace PiTests.Wilmer
{
    [TestClass]
    public class GastosFijosTests
    {
        private IWebDriver Driver;
        public const string url = "https://localhost:7073";
        MisNegociosPage MisNegociosPage;
        MisAnalisisPage MisAnalisisPage;
        AutenticacionPage AutenticacionPage;
        ProgresoAnalisisPage ProgresoAnalisisPage;
        EstructuraOrgPage EstructuraOrgPage;
        GastosFijosPage GastosFijosPage;

        [TestInitialize]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            Driver.Manage().Window.Maximize();
            Driver.Url = url;
            Driver.Navigate().GoToUrl(url);

            this.AutenticacionPage = new AutenticacionPage(Driver);
            AutenticacionPage.IniciarSesionUsuarioDefault();

            this.MisNegociosPage = new MisNegociosPage(Driver);
            MisNegociosPage.CrearNegocioEnMarchaPredeterminado();

            this.MisAnalisisPage = new MisAnalisisPage(Driver);
            MisAnalisisPage.IngresarAlAnalisis();

            this.ProgresoAnalisisPage = new ProgresoAnalisisPage(Driver);
            ProgresoAnalisisPage.IngresarEstructuraOrg();
            
            this.EstructuraOrgPage = new EstructuraOrgPage(Driver);
            EstructuraOrgPage.CrearPuestoPredeterminado();
            EstructuraOrgPage.BotonVolver.Click();

            Thread.Sleep(500);

            ProgresoAnalisisPage.IngresarGastosFijos();
            this.GastosFijosPage = new GastosFijosPage(Driver);
        }

        [TestCleanup]
        public void CleanUp()
        {
            MisNegociosPage.EliminarNegocioEnMarchaPredeterminado();
            AutenticacionPage.CerrarSesion();
            Driver.Quit();
        }

        [TestMethod]
        public void CrearGastoFijo()
        {
            // preparacion
            string gasto = "Electricidad";
            string monto = "100";
            // accion
            GastosFijosPage.CrearGastoFijo(gasto, monto);

            // verificacion
            string texto = GastosFijosPage.TextoGastoCreado;
            Assert.AreEqual("Electricidad", texto);
        }

        [TestMethod]
        public void NoSePermiteIngresarMontosNegativos()
        {
            // preparacion
            string gasto = "Electricidad";
            string monto = "-100";
            // accion
            GastosFijosPage.CrearGastoFijo(gasto, monto);

            // verificacion
            string texto = GastosFijosPage.TextoError;
            Assert.IsTrue(texto.Contains("Error: el monto es un número negativo"));
        }
    }
}
