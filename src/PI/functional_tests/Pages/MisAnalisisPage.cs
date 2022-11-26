using OpenQA.Selenium;
using UnitTestsResources;
using static functional_tests.TestServices; 

namespace functional_tests.Pages
{
    public class MisAnalisisPage
    {
        private IWebDriver Driver;

        public String TextEstadoDelNegocio
        {
            get
            { 
                return EsperarElemento(By.Id("estado-negocio"), Driver).Text;
            }
        }

        public IWebElement TarjetaAnalisis
        {
            get
            {
                return EsperarElemento(By.ClassName("card-analisis"), Driver);
            }
        }

        public void IngresarAlAnalisis()
        {
            TarjetaAnalisis.Click();
        }

        public String TituloDePagina
        {
            get
            {
                return Driver.Title;
            }
        }

        public MisAnalisisPage(IWebDriver driver)
        {
            Driver = driver;
        }

    }
}
