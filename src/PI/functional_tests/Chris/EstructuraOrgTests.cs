using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using PiTests;
using functional_tests.Pages;

namespace PiTests.Chris
{
    [TestClass]
    public class EstructuraOrgTests
    {
        private IWebDriver Driver;
        public const string url = "https://localhost:7073";
        MisNegociosPage MisNegociosPage;
        MisAnalisisPage MisAnalisisPage;
        AutenticacionPage AutenticacionPage;
        ProgresoAnalisisPage ProgresoAnalisisPage;
        EstructuraOrgPage EstructuraOrgPage;

        [TestInitialize]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            Driver.Url = url;
            Driver.Navigate().GoToUrl(url);

            this.AutenticacionPage = new AutenticacionPage(Driver);
            AutenticacionPage.IniciarSesionUsuarioDefault();
            
            this.MisNegociosPage = new MisNegociosPage(Driver);
            MisNegociosPage.CrearNegocioNoIniciadoPredeterminado();
            
            this.MisAnalisisPage = new MisAnalisisPage(Driver);
            MisAnalisisPage.IngresarAlAnalisis();

            this.ProgresoAnalisisPage = new ProgresoAnalisisPage(Driver);
            ProgresoAnalisisPage.IngresarEstructuraOrg();

            this.EstructuraOrgPage = new EstructuraOrgPage(Driver); 
        }

        [TestCleanup]
        public void CleanUp()
        {
            MisNegociosPage.EliminarNegocioNoIniciadoPredeterminado();
            AutenticacionPage.CerrarSesion();
            Driver.Quit();
        }

        [TestMethod]
        public void CrearPuestoConBeneficios()
        {
            // preparacion
            // En el metodo de setup se realiza la preparacion

            // accion
            EstructuraOrgPage.CrearPuestoPredeterminado();

            // verificacion
            string texto = EstructuraOrgPage.TextoConBeneficios;
            Assert.AreEqual("1.00", texto);
        }

        [TestMethod]
        public void CrearPuestoSinBeneficios()
        {
            // preparacion
            // En el metodo de setup se realiza la preparacion
           
            // accion
            EstructuraOrgPage.CrearPuestoPredeterminadoSinBeneficios();

            // verificacion
            string texto = EstructuraOrgPage.TextoSinBeneficios;
            Assert.AreEqual("No tiene beneficios", texto);
        }
    }
}