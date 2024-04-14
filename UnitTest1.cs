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
            // задаем параметры
            var options = new ChromeOptions();
            options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
            
            // перейти по url https://staff-testing.testkontur.ru/
            var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
            Thread.Sleep(1000);

            // ввести логин
            var login = driver.FindElement(By.Id("Username"));
            login.SendKeys("mgliezz@gmail.com");

            // ввести пароль
            var password = driver.FindElement(By.Name("Password"));
            password.SendKeys("h0wXyTE5!1");
            Thread.Sleep(1000);

            // нажать кнопку Войти
            var button = driver.FindElement(By.Name("button"));
            button.Click();
            Thread.Sleep(3000);
            
            // проверить соответствие значений нового урл
            var current_url = driver.Url;
            Assert.That(current_url == "https://staff-testing.testkontur.ru/news");
            
            driver.Quit();
        }
    }
}