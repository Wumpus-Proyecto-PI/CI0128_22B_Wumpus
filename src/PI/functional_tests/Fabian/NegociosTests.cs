using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using functional_tests.Pages;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Options;

namespace PiTests
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
            // Para correr sin que muestre el navegador
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            Driver = new ChromeDriver(chromeOptions);

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
