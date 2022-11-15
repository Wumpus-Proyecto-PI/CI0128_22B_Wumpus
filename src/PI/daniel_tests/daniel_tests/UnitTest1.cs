using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Threading;

namespace daniel_tests
{
    public class Tests
    {
        private IWebDriver driver = new ChromeDriver();
        public const string url = "https://localhost:7073";

        [SetUp]
        public void Setup()
        {
            driver.Url = url;
            driver.Navigate().GoToUrl(url);
        }

        [TearDown]
        public void CierreDriver()
        {
            driver.Close();
        }

        [Test]
        public void Test1()
        {
            var inputUsername = driver.FindElement(By.Name("Input.Email"));
            // inputUsername.Click();
            inputUsername.SendKeys("test@gmail.com");
            var inputPassword = driver.FindElement(By.Name("Input.Password"));
            inputPassword.SendKeys("test12");

            var loginButton = driver.FindElement(By.Id("login-submit"));
            loginButton.Click();

            var logoutButton = driver.FindElement(By.ClassName("logout-button"));

            Assert.AreEqual("Cerrar sesión", logoutButton.Text);
            logoutButton.Click();
        }
    }
}