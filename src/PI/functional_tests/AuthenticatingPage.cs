using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using UnitTestsResources;

namespace PiTests
{
    public class AuthenticatingPage
    {
        private IWebDriver Driver;
        private string URL = "https://localhost:7073";
        private string CorreoPredeterminado = "wumpustest@gmail.com";
        private string UsuarioRegistroPredeterminado = "wumpusRegisterTest@gmail.com";
        private string ContrasenaPredeterminada = "wumpus";


        public IWebElement EntradaCorreo 
        { 
            get 
            { 
                return Driver.FindElement(By.Id("Input_Email")); 
            } 
        }

        public IWebElement EntradaContraseña
        {
            get
            {
                return Driver.FindElement(By.Id("Input_Password"));
            }
        }

        public IWebElement EntradaConfirmarContraseña
        {
            get
            {
                return Driver.FindElement(By.Id("Input_ConfirmPassword"));
            }
        }

        public IWebElement BotonIniciarSesion
        {
            get
            {
                return Driver.FindElement(By.Id("login-submit"));
            }
        }

        public IWebElement BotonCerrarSesion
        {
            get
            {
                return Driver.FindElement(By.ClassName("logout-button"));
            }
        }

        public IWebElement BotonRegistrarse
        {
            get
            {
                return Driver.FindElement(By.Id("registerSubmit"));
            }
        }

        public AuthenticatingPage(IWebDriver driver) {
            this.Driver = driver;
        }

        public void IniciarSesionUsuarioDefault()
        {
            this.EntradaCorreo.SendKeys(CorreoPredeterminado);
            this.EntradaContraseña.SendKeys(ContrasenaPredeterminada);
            this.BotonIniciarSesion.Click();
        }
        public void RegistrarUsuarioPredeterminado ()
        {
            this.EntradaCorreo.SendKeys(this.UsuarioRegistroPredeterminado);
            this.EntradaContraseña.SendKeys(this.ContrasenaPredeterminada);
            this.EntradaConfirmarContraseña.SendKeys(this.ContrasenaPredeterminada);
            this.BotonRegistrarse.Click();
        }

        public void CerrarSesion()
        {
            this.BotonCerrarSesion.Click();
        }

        public void EliminarUsuarioRegistrado()
        {
            HandlerGenerico handler = new HandlerGenerico();
            string consulta = 
                @$"if exists (Select AspNetUsers.Email from AspNetUsers where AspNetUsers.Email = '{this.UsuarioRegistroPredeterminado}')
                begin
                    Delete from AspNetUsers where email = '{this.UsuarioRegistroPredeterminado}'
                end ";
            handler.EnviarConsultaGenerica(consulta);
        }
    }
}
