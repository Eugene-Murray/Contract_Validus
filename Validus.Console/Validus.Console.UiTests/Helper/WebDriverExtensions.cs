using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Validus.Console.UiTests.Helper
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static void Retry(this Action userAction)
        {
            var retry = true;
            do
            {
                try
                {
                    userAction();
                    retry = false;
                }
                catch (StaleElementReferenceException)
                {
                    retry = true;
                }
                catch (InvalidElementStateException)
                {
                    retry = true;
                }
            } while (retry);
        }
    }
}
