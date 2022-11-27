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
            if (TarjetaEstructuraOrg.Text == "Gastos Fijos")
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
