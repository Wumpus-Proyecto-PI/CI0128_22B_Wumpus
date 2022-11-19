using OpenQA.Selenium;
using UnitTestsResources;

namespace functional_tests.Pages
{
    public class MisAnalisisPage
    {
        private IWebDriver Driver;

        public String TextEstadoDelNegocio
        {
            get
            {
                return Driver.FindElement(By.Id("estado-negocio")).Text;
            }
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
