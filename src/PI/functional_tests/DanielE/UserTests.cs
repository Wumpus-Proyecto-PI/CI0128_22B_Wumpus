using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using PiTests;

namespace PiTests.DanielE
{
    [TestClass]
    public class UserTests
    {
        private IWebDriver driver = new ChromeDriver();
        public const string url = "https://localhost:7073";

        [TestInitialize]
        public void Setup()
        {
            driver.Url = url;
            driver.Navigate().GoToUrl(url);
        }

        [TestCleanup]
        public void CierreDriver()
        {
            driver.Close();
        }

        [TestMethod]
        public void LoginTest()
        {
            AuthenticatingPage authenticatingPage = new(driver);
            authenticatingPage.IniciarSesionUsuarioDefault();
            IWebElement botonLogout = authenticatingPage.BotonCerrarSesion;
            Assert.AreEqual("Cerrar sesión", botonLogout.Text);
            authenticatingPage.CerrarSesion();
        }
    }
}
