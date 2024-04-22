using FluentAssertions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutoTestHomework;

public class Tests
{
    public ChromeDriver driver;
    //[SetUp]
    //public void Setup()
    //{
    //}

    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");

        driver = new ChromeDriver(options);
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
        
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("mgliezz@gmail.com");
        
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("h0wXyTE5!1");
        
        var button = driver.FindElement(By.Name("button"));
        button.Click();
        
        var news = driver.FindElement(By.CssSelector("[data-tid='Feed']"));
        var current_url = driver.Url;
        
        Assert.That(current_url == "https://staff-testing.testkontur.ru/news",
            "current url:" + current_url + " expected: https://staff-testing.testkontur.ru/news");
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}