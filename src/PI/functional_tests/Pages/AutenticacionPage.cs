using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using UnitTestsResources;

namespace functional_tests.Pages
{
    public class AutenticacionPage
    {
        private IWebDriver Driver;
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

        public AutenticacionPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public void IniciarSesionUsuarioDefault()
        {
            EntradaCorreo.SendKeys(CorreoPredeterminado);
            EntradaContraseña.SendKeys(ContraseñaPredeterminada);
            BotonIniciarSesion.Click();
        }
        public void RegistrarUsuarioPredeterminado()
        {
            EntradaCorreo.SendKeys(UsuarioRegistroPredeterminado);
            EntradaContraseña.SendKeys(ContraseñaPredeterminada);
            EntradaConfirmarContraseña.SendKeys(ContraseñaPredeterminada);
            BotonRegistrarse.Click();
        }

        public void CerrarSesion()
        {
            BotonCerrarSesion.Click();
        }

        public void EliminarUsuarioRegistrado()
        {
            HandlerGenerico handler = new HandlerGenerico();
            string consulta =
                @$"if exists (Select AspNetUsers.Email from AspNetUsers where AspNetUsers.Email = '{UsuarioRegistroPredeterminado}'
                begin
                    Delete from AspNetUsers where email = '{UsuarioRegistroPredeterminado}'
                end ";
            handler.EnviarConsultaGenerica(consulta);
        }
    }
}
