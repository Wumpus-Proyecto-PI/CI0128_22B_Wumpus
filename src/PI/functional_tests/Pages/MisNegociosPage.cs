using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using UnitTestsResources;
using static functional_tests.TestServices;

namespace functional_tests.Pages
{
    public class MisNegociosPage
    {
        private IWebDriver Driver;
        private string NombreNegocioPredeterminado = "funcTest";
        public IWebElement BotonAgregarNegocio
        {
            get
            {
                
                return EsperarElemento(By.ClassName("card-agregar"), Driver);
            }
        }

        public IWebElement EntradaNombreNegocio
        {
            get
            {
                return EsperarElemento(By.Name("nombreNegocio"), Driver);
            }
        }

        public IWebElement RadioNoIniciado
        {
            get
            {
                return EsperarElemento(By.Id("tipoNoIniciado"), Driver);
            }
        }

        public IWebElement RadioEnMarcha
        {
            get
            {  
                return EsperarElemento(By.Id("tipoEnMarcha"), Driver);
            }
        }

        public IWebElement BotonAceptar
        {
            get
            {
                return EsperarElemento(By.ClassName("btn-primary"), Driver);
            }
        }

        public string TituloDePagina
        {
            get
            {
                return Driver.Title;
            }
        }

        public MisNegociosPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void CrearNegocioNoIniciadoPredeterminado()
        {
            BotonAgregarNegocio.Click();
            EntradaNombreNegocio.SendKeys(NombreNegocioPredeterminado + " No iniciado");
            RadioNoIniciado.Click();
            BotonAceptar.Click();
        }

        public void CrearNegocioEnMarchaPredeterminado()
        {
            BotonAgregarNegocio.Click();
            EntradaNombreNegocio.SendKeys(NombreNegocioPredeterminado + " En marcha");
            RadioEnMarcha.Click();
            BotonAceptar.Click();
        }

        public void EliminarNegocioNoIniciadoPredeterminado()
        {
            HandlerGenerico handler = new HandlerGenerico();
            string consulta =
                @$"if exists (Select Negocio.nombre from Negocio where Negocio.nombre = '{NombreNegocioPredeterminado + " No iniciado"}')
                begin
                    Delete from Negocio where Negocio.nombre = '{NombreNegocioPredeterminado + " No iniciado"}'
                end ";
            handler.EnviarConsultaGenerica(consulta);
        }

        public void EliminarNegocioEnMarchaPredeterminado()
        {
            HandlerGenerico handler = new HandlerGenerico();
            string consulta =
                @$"if exists (Select Negocio.nombre from Negocio where Negocio.nombre = '{NombreNegocioPredeterminado + " En marcha"}')
                begin
                    Delete from Negocio where Negocio.nombre = '{NombreNegocioPredeterminado + " En marcha"}'
                end ";
            handler.EnviarConsultaGenerica(consulta);
        }
    }
}
