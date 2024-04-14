using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Selenium_test_Grigorevska
{
    public class Selenium_test
    {
        [Test]
        public void Authorization()
        {
            var options = new ChromeOptions();
            options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
            
            var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
            Thread.Sleep(1000);

            var login = driver.FindElement(By.Id("Username"));
            login.SendKeys("mgliezz@gmail.com");

            var password = driver.FindElement(By.Name("Password"));
            password.SendKeys("h0wXyTE5!1");
            Thread.Sleep(1000);

            var button = driver.FindElement(By.Name("button"));
            button.Click();
            Thread.Sleep(3000);

            var current_url = driver.Url;
            Assert.That(current_url == "https://staff-testing.testkontur.ru/news");
            
            driver.Quit();
        }
    }
}