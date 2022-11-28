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

        public IWebElement TarjetaCrearAnalisis {
            get {
                return EsperarElemento(By.ClassName("card-agregar"), Driver);
            }
        }

        public IWebElement BotonCrearAnalisisNoIniciado {
            get {
                return EsperarElemento(By.Id("no-iniciado"), Driver);
            }
        }

        public IWebElement BotonCrearAnalisisEnMarcha
        {
            get
            {
                return EsperarElemento(By.Id("en-marcha"), Driver);
            }
        }

        public IWebElement BotonAceptarCreacionAnalisis
        {
            get
            {
                return EsperarElemento(By.ClassName("btn-primary"), Driver);
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

        public void CrearAnalisisNoIniciado() {
            TarjetaCrearAnalisis.Click();
            BotonCrearAnalisisNoIniciado.Click();
            BotonAceptarCreacionAnalisis.Click();
        }

        public void CrearAnalisisEnMarcha()
        {
            TarjetaCrearAnalisis.Click();
            BotonCrearAnalisisEnMarcha.Click();
            BotonAceptarCreacionAnalisis.Click();
        }


        public MisAnalisisPage(IWebDriver driver)
        {
            Driver = driver;
        }

    }
}
