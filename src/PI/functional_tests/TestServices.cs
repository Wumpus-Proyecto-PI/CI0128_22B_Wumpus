using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace functional_tests
{
    public class TestServices
    {
        static public IWebElement EsperarElemento(By identificador, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            wait.Until(condition =>
            {       
                try
                {
                    var elementToBeDisplayed = driver.FindElement(identificador);
                    return elementToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException ex)
                {
                    var elementToBeDisplayed = driver.FindElement(identificador);
                    return elementToBeDisplayed.Displayed;
                }
                catch (TimeoutException e)
                {
                    var elementToBeDisplayed = driver.FindElement(identificador);
                    return elementToBeDisplayed.Displayed;
                }
                    
            });
            Thread.Sleep(500);

            return driver.FindElement(identificador);
            
        }
    }
}
