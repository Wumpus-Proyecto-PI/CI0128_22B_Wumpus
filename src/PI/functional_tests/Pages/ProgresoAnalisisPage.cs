using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static functional_tests.TestServices;

namespace functional_tests.Pages
{
    public class ProgresoAnalisisPage
    {
        private IWebDriver Driver; 

        public IWebElement TarjetaEstructuraOrg
        {
            get
            { 
                return EsperarElemento(By.ClassName("titulo-carta"), Driver);
            }
        }

        public void IngresarEstructuraOrg()
        {
            if (TarjetaEstructuraOrg.Text == "Estructura Organizativa")
            {
                TarjetaEstructuraOrg.Click();
            }
            
        }

        public void IngresarGastosFijos()
        {
            Driver.FindElement(By.XPath("/html/body/main/div/div[2]/div[1]/div[2]/div")).Click();

        }

        public ProgresoAnalisisPage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}
