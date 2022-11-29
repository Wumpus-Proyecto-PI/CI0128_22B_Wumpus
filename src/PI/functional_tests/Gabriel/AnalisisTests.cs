using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using functional_tests.Pages;
using PiTests;
using System.Security.Policy;
using static functional_tests.TestServices;

namespace functional_tests.Gabriel
{
    [TestClass]
    public class TestsFuncionalesGabriel
    {
        private IWebDriver Driver;
        public const string URL = "https://localhost:7073/";
        AutenticacionPage AutenticacionPage;
        MisAnalisisPage MisAnalisisPage;
        MisNegociosPage MisNegociosPage;

        [TestInitialize]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Url = URL;
            Driver.Navigate().GoToUrl(Driver.Url);
            Driver.Manage().Window.Maximize();

            AutenticacionPage = new AutenticacionPage(Driver);
            AutenticacionPage.IniciarSesionUsuarioDefault();

            MisNegociosPage = new MisNegociosPage(Driver);
            MisNegociosPage.CrearNegocioEnMarchaPredeterminado();

            MisAnalisisPage = new MisAnalisisPage(Driver);
        }

        [TestCleanup]
        public void Cleanup()
        {
            MisNegociosPage.EliminarNegocioEnMarchaPredeterminado();
            MisNegociosPage.EliminarNegocioNoIniciadoPredeterminado();
            AutenticacionPage.CerrarSesion();
            Driver.Quit();
        }

        [TestMethod]
        public void CrearAnalisisNoIniciado() {

            // Preparación: Se realiza en el método de Setup

            // Acción:
            MisAnalisisPage.CrearAnalisisNoIniciado();
            string titulo = EsperarElemento(By.Id("nombre_seccion"), Driver).Text;

            //Assert
            Assert.AreEqual(titulo,"Progreso del análisis");
        }

        [TestMethod]
        public void CrearAnalisisEnMarcha()
        {
            // Preparación: Se realiza en el método de Setup

            //Acción:
            MisAnalisisPage.CrearAnalisisEnMarcha();
            string titulo = EsperarElemento(By.Id("nombre_seccion"), Driver).Text;

            //Assert
            Assert.AreEqual(titulo, "Progreso del análisis");
        }




    }
}
