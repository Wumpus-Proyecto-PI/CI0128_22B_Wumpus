using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using functional_tests.Pages;

namespace PiTests.Fabian
{
    [TestClass]
    public class NegociosTests
    {
        private IWebDriver Driver;
        public const string URL = "https://localhost:7073";
        MisNegociosPage MisNegociosPage;
        MisAnalisisPage MisAnalisisPage;
        AutenticacionPage AutenticacionPage;

        [TestInitialize]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            Driver.Manage().Window.Maximize();

            Driver.Navigate().GoToUrl(URL);
            this.AutenticacionPage = new AutenticacionPage(Driver);
            AutenticacionPage.IniciarSesionUsuarioDefault();
        }

        [TestCleanup]
        public void CierreDriver()
        {
            MisNegociosPage.EliminarNegocioEnMarchaPredeterminado();
            MisNegociosPage.EliminarNegocioNoIniciadoPredeterminado();
            AutenticacionPage.CerrarSesion();
            Driver.Quit();
        }

        [TestMethod]
        public void CrearNegocioNoIniciadoTest()
        {
            // arrange
            string estadoDelNegocioEsperado = "No iniciado";

            // action
            this.MisNegociosPage = new MisNegociosPage(Driver);
            MisNegociosPage.CrearNegocioNoIniciadoPredeterminado();

            this.MisAnalisisPage = new MisAnalisisPage(Driver);

            string estadoDelNegocioResultado = MisAnalisisPage.TextEstadoDelNegocio;

            // assert
            Assert.AreEqual(estadoDelNegocioEsperado, estadoDelNegocioResultado);
        }


        [TestMethod]
        public void CrearNegocioEnMarchaTest()
        {
            // arrange
            string estadoDelNegocioEsperado = "En marcha";

            // action
            this.MisNegociosPage = new MisNegociosPage(Driver);
            MisNegociosPage.CrearNegocioEnMarchaPredeterminado();

            this.MisAnalisisPage = new MisAnalisisPage(Driver);

            string estadoDelNegocioResultado = MisAnalisisPage.TextEstadoDelNegocio;

            // assert
            Assert.AreEqual(estadoDelNegocioEsperado, estadoDelNegocioResultado);
        }
    }
}
