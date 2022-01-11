using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Drawing;
using System.Threading;

namespace TestProject1
{
    public class Selenium
    {
        public void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public void Navigate(IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
            WaitForPageLoad(driver);
        }

        public void WaitForPageLoad(IWebDriver driver)
        {
            OpenQA.Selenium.Support.UI.WebDriverWait Wait = new(driver, TimeSpan.FromSeconds(30));
            try
            {
                _ = Wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception)
            {

            }
        }

        public void NavigateBack(IWebDriver driver)
        {
            driver.Navigate().Back();
            WaitForPageLoad(driver);
        }

        public void ScrollToElement(IWebDriver driver, By element)
        {
            try
            {
                var searchedElement = driver.FindElement(element);
                OpenQA.Selenium.Interactions.Actions actions = new(driver);
                actions.MoveToElement(searchedElement).Perform();
            }
            catch (Exception)
            {

            }
        }

        public void WaitVisible(IWebDriver driver, By element)
        {
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
            _ = wait.Until(ExpectedConditions.ElementIsVisible(element));
            Wait(400);
        }

        public void WaitClickable(IWebDriver driver, By element)
        {
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
            _ = wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        public void WaitSelectable(IWebDriver driver, By element)
        {
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
            _ = wait.Until(ExpectedConditions.ElementToBeSelected(element));
        }

        public void WaitExists(IWebDriver driver, By element)
        {
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
            _ = wait.Until(ExpectedConditions.ElementExists(element));
        }

        public void Click(IWebDriver driver, By element, int milliseconds = 0)
        {
            WaitClickable(driver, element);
            driver.FindElement(element).Click();
            Wait(milliseconds);
        }

        public void DoubleClick(IWebDriver driver, By element, int milliseconds = 0)
        {
            WaitClickable(driver, element);

            OpenQA.Selenium.Interactions.Actions actions = new(driver);
            IWebElement webElement = driver.FindElement(element);
            actions.DoubleClick(webElement).Perform();
            Wait(milliseconds);
        }

        public void Clear(IWebDriver driver, By element)
        {
            WaitExists(driver, element);
            driver.FindElement(element).Clear();
        }

        public void SendKeys(IWebDriver driver, By element, string text, int milliseconds = 0)
        {
            WaitExists(driver, element);
            driver.FindElement(element).SendKeys(text);
            Wait(milliseconds);
        }

        public void SwitchFrame(IWebDriver driver, string frame)
        {
            _ = driver.SwitchTo().ParentFrame();
            _ = driver.SwitchTo().Frame(frame);
        }

        public void DefaultContentFrame(IWebDriver driver)
        {
            _ = driver.SwitchTo().DefaultContent();
        }

        public bool GetElementSelected(IWebDriver driver, By element)
        {
            WaitExists(driver, element);
            return driver.FindElement(element).Selected;
        }

        public string GetElementValue(IWebDriver driver, By element)
        {
            WaitExists(driver, element);
            return driver.FindElement(element).GetAttribute("value");
        }

        public string GetElementAttribute(IWebDriver driver, By element, string name)
        {
            WaitExists(driver, element);
            IWebElement webElement = driver.FindElement(element);
            Size elementSize = webElement.Size;

            string result = name switch
            {
                "width" => elementSize.Width.ToString(),
                "height" => elementSize.Height.ToString(),
                _ => driver.FindElement(element).GetAttribute(name),
            };
            return result;
        }

        public string GetElementCssProperty(IWebDriver driver, By element, string property)
        {
            WaitExists(driver, element);
            return driver.FindElement(element).GetCssValue(property);
        }

        public string GetElementText(IWebDriver driver, By element)
        {
            WaitExists(driver, element);
            return driver.FindElement(element).Text;
        }

        public int GetElementsCount(IWebDriver driver, By element)
        {
            return driver.FindElements(element).Count;
        }

        public void SetElementAttribute(IWebDriver driver, By element, string name, string value)
        {
            WaitExists(driver, element);
            IWebElement webElement = driver.FindElement(element);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0]." + name + "='" + value + "';", webElement);
        }

        public void SetSelectByValue(IWebDriver driver, By element, string value, int milliseconds = 0)
        {
            WaitExists(driver, element);
            var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(element));
            selectElement.SelectByValue(value);
            Wait(milliseconds);
        }

        public void SetSelectByText(IWebDriver driver, By element, string text)
        {
            WaitExists(driver, element);
            var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(driver.FindElement(element));
            selectElement.SelectByText(text);
        }

        public bool Exists(IWebDriver driver, By element)
        {
            try
            {
                IWebElement webElement = driver.FindElement(element);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Exists(IWebDriver driver, By element, int milliseconds = 0)
        {
            try
            {
                OpenQA.Selenium.Support.UI.WebDriverWait wait = new(driver, TimeSpan.FromMilliseconds(milliseconds));
                _ = wait.Until(ExpectedConditions.ElementExists(element));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsDisplayed(IWebDriver driver, By element)
        {
            try
            {
                IWebElement webElement = driver.FindElement(element);
                return webElement.Displayed;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*public bool IsVisible(IWebDriver driver, By element)
        {
            bool result;
            try
            {
                IWebElement webElement = driver.FindElement(element);
                webElement.
            }
        }*/

        public void SendKeys(IWebDriver driver, string text, int milliseconds = 0)
        {
            OpenQA.Selenium.Interactions.Actions actions = new(driver);
            actions.SendKeys(text).Build().Perform();
            Wait(milliseconds);
        }

        public void MouseOver(IWebDriver driver, By element, int milliseconds = 0)
        {
            IWebElement webElement = driver.FindElement(element);
            OpenQA.Selenium.Interactions.Actions actions = new(driver);
            actions.MoveToElement(webElement).Perform();
            Wait(milliseconds);
        }
    }
}