using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using UnitTestsResources;

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
                return Driver.FindElement(By.ClassName("card-agregar"));
            }
        }

        public IWebElement EntradaNombreNegocio
        {
            get
            {
                return Driver.FindElement(By.Name("nombreNegocio"));
            }
        }

        public IWebElement RadioNoIniciado
        {
            get
            {
                return Driver.FindElement(By.Id("tipoNoIniciado"));
            }
        }

        public IWebElement RadioEnMarcha
        {
            get
            {
                return Driver.FindElement(By.Id("tipoEnMarcha"));
            }
        }

        public IWebElement BotonAceptar
        {
            get
            {
                return Driver.FindElement(By.ClassName("btn-primary"));
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
