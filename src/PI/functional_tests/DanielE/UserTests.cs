using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using PiTests;

namespace PiTests.DanielE
{
    [TestClass]
    public class UserTests
    {
        private IWebDriver? Driver = null;
        public const string url = "https://localhost:7073";
        private AuthenticatingPage? authenticatingPage = null;

        [TestInitialize]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Url = url;
            Driver.Manage().Window.Maximize();
            Driver.Navigate().GoToUrl(url);
            authenticatingPage = new(Driver);
        }

        [TestCleanup]
        public void CleanUp()
        {
            authenticatingPage.CerrarSesion();
            authenticatingPage.EliminarUsuarioRegistrado();
            authenticatingPage = null;

            Driver.Close();
            Driver = null;
        }

        [TestMethod]
        public void LoginTest()
        {
            // preparacion
            // en el setup creamos AuthenticatingPage con el que se hace la prueba

            // accion
            authenticatingPage.IniciarSesionUsuarioDefault();
            
            // verificacion
            IWebElement botonLogout = authenticatingPage.BotonCerrarSesion;
            Assert.AreEqual("Cerrar sesión", botonLogout.Text);
        }

        [TestMethod]
        public void RegistroDeUsuario()
        {
            // preparacion
            // en el setup creamos AuthenticatingPage con el que se hace la prueba
            Driver.Navigate().GoToUrl("https://localhost:7073/Identity/Account/Register");
            // accion
            authenticatingPage.RegistrarUsuarioPredeterminado();

            // verificacion
            IWebElement botonLogout = authenticatingPage.BotonCerrarSesion;
            Assert.AreEqual("Cerrar sesión", botonLogout.Text);
        }
    }
}
