using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

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
            var inputUsername = driver.FindElement(By.Name("Input.Email"));
            // inputUsername.Click();
            inputUsername.SendKeys("wumpustest@gmail.com");
            var inputPassword = driver.FindElement(By.Name("Input.Password"));
            inputPassword.SendKeys("wumpus");

            var loginButton = driver.FindElement(By.Id("login-submit"));
            loginButton.Click();

            var logoutButton = driver.FindElement(By.ClassName("logout-button"));

            Assert.AreEqual("Cerrar sesión", logoutButton.Text);
            logoutButton.Click();
        }
    }
}
