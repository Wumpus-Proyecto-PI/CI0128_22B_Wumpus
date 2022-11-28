using OpenQA.Selenium;
using static functional_tests.TestServices;

namespace functional_tests.Pages
{
    public class GastosFijosPage
    {
        private IWebDriver Driver;

        public IWebElement InputNombreGastoFijo
        {
            get
            {
                return EsperarElemento(By.Id("nombre-gasto"), Driver);
            }
        }

        public IWebElement InputMontoAnual
        {
            get
            {
                return EsperarElemento(By.Id("monto-gasto"), Driver);
            }
        }

        public IWebElement BotonGuardarGasto
        {
            get
            {
                return EsperarElemento(By.Id("guardar-gastoFijo"), Driver);
            }
        }

        public IWebElement BotonVolver
        {
            get
            {
                return EsperarElemento(By.Id("boton-volver"), Driver);
            }
        }

        public String TextoGastoCreado
        {
            get
            {
                return EsperarElemento(By.ClassName("gastoFijo-clickeable"), Driver).FindElement(By.ClassName("gastoFijo-nombre")).Text;
            }
        }

        public String TextoError
        {
            get
            {
                return EsperarElemento(By.ClassName("error-gastoFijo"), Driver).Text;
            }
        }

        public void CrearGastoFijo(string nombre, string monto)
        {
            InputNombreGastoFijo.SendKeys(nombre);
            InputMontoAnual.SendKeys(Keys.Backspace);
            InputMontoAnual.SendKeys(monto);
            BotonGuardarGasto.Click();
        }

        public String TituloDePagina
        {
            get
            {
                return Driver.Title;
            }
        }

        public GastosFijosPage(IWebDriver driver)
        {
            Driver = driver;
        }

    }
}
