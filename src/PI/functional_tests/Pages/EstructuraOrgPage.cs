using OpenQA.Selenium;
using UnitTestsResources;
using static functional_tests.TestServices; 

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
                return EsperarElemento(By.Id("nombre_puesto"), Driver);
            }
        }

        public IWebElement InputSalarioMensual
        {
            get
            {
                return EsperarElemento(By.Id("salario_mensual"), Driver);
            }
        }

        public IWebElement InputNumPlazas
        {
            get
            {
                return EsperarElemento(By.Id("num-plazas"), Driver);
            }
        }

        public IWebElement InputBeneficios
        {
            get
            {
                return EsperarElemento(By.Id("beneficios"), Driver);
            }
        }

        public IWebElement BotonGuardarPuesto
        {
            get
            {                
                return EsperarElemento(By.ClassName("est-org-boton"), Driver);
            }
        }

        public IWebElement BotonVolver
        {
            get
            {  
                return EsperarElemento(By.Id("boton-volver"), Driver);
            }
        }

        public String TextoSinBeneficios
        {
            get
            {
                return EsperarElemento(By.Id("no-beneficios"), Driver).Text;
            }
        }

        public String TextoConBeneficios
        {
            get
            {
                return EsperarElemento(By.Id("con-beneficios"), Driver).Text;
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