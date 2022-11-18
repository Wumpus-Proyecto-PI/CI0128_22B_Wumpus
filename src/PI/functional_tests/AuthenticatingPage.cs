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
        private string ContraseñaPredeterminada = "wumpus";


        private IWebElement EntradaCorreo 
        { 
            get 
            { 
                return Driver.FindElement(By.Id("Input_Email")); 
            } 
        }

        private IWebElement EntradaContraseña
        {
            get
            {
                return Driver.FindElement(By.Id("Input_Password"));
            }
        }

        private IWebElement EntradaConfirmarContraseña
        {
            get
            {
                return Driver.FindElement(By.Id("Input_ConfirmPassword"));
            }
        }

        private IWebElement BotonIniciarSesion
        {
            get
            {
                return Driver.FindElement(By.Id("login-submit"));
            }
        }

        private IWebElement BotonCerrarSesion
        {
            get
            {
                return Driver.FindElement(By.ClassName("logout-button"));
            }
        }

        private IWebElement BotonRegistrarse
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
            this.EntradaContraseña.SendKeys(ContraseñaPredeterminada);
            this.BotonIniciarSesion.Click();
        }
        public void RegistrarUsuarioPredeterminado ()
        {
            this.EntradaCorreo.SendKeys(this.UsuarioRegistroPredeterminado);
            this.EntradaContraseña.SendKeys(this.ContraseñaPredeterminada);
            this.EntradaConfirmarContraseña.SendKeys(this.ContraseñaPredeterminada);
            this.BotonRegistrarse.Click();
        }

        public void CerrarSesion()
        {
            this.BotonCerrarSesion.Click();
        }

        public void EliminarUsuarioRegistrado()
        {
            HandlerGenerico handler = new HandlerGenerico();
            string consulta = $"Delete from AspNetUsers where email = {this.CorreoPredeterminado}";
            handler.EnviarConsultaGenerica(consulta);
        }
    }
}
