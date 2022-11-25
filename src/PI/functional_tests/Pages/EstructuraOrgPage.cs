using OpenQA.Selenium;
using UnitTestsResources;

namespace functional_tests.Pages
{
    public class EstructuraOrgPage
    {
        private IWebDriver Driver;
        private string NombrePuestoPredeterminado = "puesto-prueba"; 

        public IWebElement InputNombrePuesto
        {
            get
            {
                return Driver.FindElement(By.Id("nombre_puesto"));
            }
        }

        public IWebElement InputSalarioMensual
        {
            get
            {
                return Driver.FindElement(By.Id("salario_mensual"));
            }
        }

        public IWebElement InputNumPlazas
        {
            get
            {
                return Driver.FindElement(By.Id("num-plazas"));
            }
        }

        public IWebElement InputBeneficios
        {
            get
            {
                return Driver.FindElement(By.Id("beneficios"));
            }
        }

        public IWebElement BotonGuardarPuesto
        {
            get
            {
                return Driver.FindElement(By.Id("est-org-boton"));
            }
        }

        public IWebElement BotonVolver
        {
            get
            {
                return Driver.FindElement(By.Id("boton-volver"));
            }
        }

        public String TextoSinBeneficios
        {
            get
            {
                return Driver.FindElement(By.Id("no-beneficios")).Text;
            }
        }

        public String TextoConBeneficios
        {
            get
            {
                return Driver.FindElement(By.Id("con-beneficios")).Text;
            }
        }

        public void CrearPuestoPredeterminado()
        {
            InputNombrePuesto.SendKeys(NombrePuestoPredeterminado);
            InputSalarioMensual.SendKeys("1");
            InputNumPlazas.SendKeys("1");
            InputBeneficios.SendKeys("1");
            BotonGuardarPuesto.Click();

        }

        public void CrearPuestoPredeterminadoSinBeneficios()
        {
            InputNombrePuesto.SendKeys(NombrePuestoPredeterminado);
            InputSalarioMensual.SendKeys("1");
            InputNumPlazas.SendKeys("1");
            BotonGuardarPuesto.Click();

        }

        public void EliminarPuestoPredeterminado()
        {
            HandlerGenerico handler = new HandlerGenerico();
            string consulta =
                @$"if exists (Select Puesto.nombre from Puesto where Puesto.nombre = '{NombrePuestoPredeterminado}')
                begin
                    Delete from Puesto where Puesto.nombre = '{NombrePuestoPredeterminado}'
                end ";
            handler.EnviarConsultaGenerica(consulta);
        }

        public String TituloDePagina
        {
            get
            {
                return Driver.Title;
            }
        }

        public EstructuraOrgPage(IWebDriver driver)
        {
            Driver = driver;
        }

    }
}