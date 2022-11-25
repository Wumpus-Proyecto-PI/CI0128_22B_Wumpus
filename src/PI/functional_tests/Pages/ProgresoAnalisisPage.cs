using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace functional_tests.Pages
{
    public class ProgresoAnalisisPage
    {
        private IWebDriver Driver; 

        public IWebElement TarjetaEstructuraOrg
        {
            get
            {
                return Driver.FindElement(By.ClassName("titulo-carta"));
            }
        }

        public void IngresarEstructuraOrg()
        {
            if (TarjetaEstructuraOrg.Text == "Estructura Organizativa")
            {
                TarjetaEstructuraOrg.Click();
            }
            
        }

        public ProgresoAnalisisPage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}
