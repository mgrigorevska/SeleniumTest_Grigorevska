using FluentAssertions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutoTestHomework;

public class Tests
{
    public ChromeDriver driver;
    
    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--disable-extensions");

        driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        
        Authorization();
    }

    [Test]
    public void LogoutText()
    {
        // кликаем на боковое меню
        var sidebar = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebar.Click();
        
        // кликаем на кнопку "Выйти"
        var logout = driver.FindElement(By.CssSelector("[data-tid='LogoutButton']"));
        logout.Click();
        
        // ждем пока появится нужный элемент
        
        // берем текст со страницы
        var logoutText = driver.FindElement(By.ClassName("body-wrapper")).Text;
        
        // Проверяем соответствие текста
        Assert.That(logoutText.Contains("Вы вышли из учетной записи") & logoutText.Contains("Вернуться в Кадровый Портал"),
            "CURRENT TEXT: " + logoutText + "\nEXPECTED: Вы вышли из учетной записи\nВернуться в Кадровый Портал");
    }


    [Test]
    public void CommunityTitle()
    {
        // кликаем на боковое меню
        var sidebar = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebar.Click();
        
        // кликаем на кнопку "Сообщества"
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .First(element => element.Displayed);
        community.Click();

        // проверяем соответствие текста заголовка
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(communityTitle.Text == "Сообщества",
            "CURRENT TEXT: " + communityTitle.Text + "\nEXPECTED: Сообщества");
    }

    [Test]

    public void CommunityPopUp()
    {
       // driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
       // driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
       
        var sidebar = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebar.Click();
        
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .First(element => element.Displayed);
        community.Click();
        
        //Thread.Sleep(500);
        // ждем появления элемента
        
        
        // кликаем на выпадающий список
        var popUpMenu = driver.FindElements(By.CssSelector("[data-tid='PopupMenu__caption']"))
            .First(element => element.Displayed);
        
        popUpMenu.Click();
        
        Thread.Sleep(500);
        var menuItem = driver.FindElement(By.CssSelector("[href*='isMember']"));              //("[href='https://staff-testing.testkontur.ru/communities?activeTab=isMember']"));               //.CssSelector("[data-tid='PopupContentInner']"));
            //.First(element => element.Displayed);
        //Console.WriteLine(menuItem);    
        menuItem.Click();
        //Console.WriteLine(menuItem.Text);
        
        Thread.Sleep(1000);
        
        // проверяем соотвествие текста элемента
        var communityMember = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']"));
        
        
        Console.WriteLine(communityMember.Text);
       // Assert.That(communityMember.Text.Contains("Я участник"));
       //     "CURRENT TEXT: " + communityMember.Text + "\nEXPECTED: Я участник");
        
        
        Thread.Sleep(1000);
    }

    
    [Test]

    public void EmptySearchImg()
    {
       // кликаем на боковое меню
       var sidebar = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
       sidebar.Click();
       
       // кликаем на кнопку "Файлы"
       var community = driver.FindElements(By.CssSelector("[data-tid='Files']"))
           .First(element => element.Displayed);
       community.Click();

       // кликаем на иконку поиска
       var search = driver.FindElement(By.CssSelector("[data-tid='Search']"));
       search.Click();
       
       // проверяем наличие картинки с котом
       var emptySearch = driver.FindElement(By.CssSelector("[data-tid='ModalPageBody']"));
       var image = emptySearch.FindElement(By.TagName("svg"));
       Assert.That(image.Displayed, Is.True, "No cat image displayed");
       
    }

    [Test]

    public void Test1()
    {
        
        var searchIcon = driver.FindElement(By.CssSelector("[data-tid='Services']"));
        Thread.Sleep(3000);
        searchIcon.Click();
        
        Thread.Sleep(3000);
        
        var searchBar = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        
        searchBar.Click();
        Thread.Sleep(3000);

        searchBar.SendKeys("  0000");

        //var search = driver.FindElement(By.CssSelector("[data-tid='InputLikeText__input']"));
        //var search = driver.FindElement(By.CssSelector("[class='react-ui-1uzh48y']"));
        //search.SendKeys("fer");
        Thread.Sleep(3000);
        
    }

    [Test]

    public void ProfileDate()
    {
        // кликаем на боковое меню
        var sidebar = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebar.Click();
        //Thread.Sleep(3000);
        
        var ava = driver.FindElements(By.CssSelector("[data-tid='Avatar']")).Last();
        ava.Click();
        
       Thread.Sleep(1000); //подождем пока прогрузится профиль
       
        
        var date = driver.FindElement(By.CssSelector("[data-tid='CalendarChoose']"));
        date.Click();
        var profileTitle = driver.FindElement(By.CssSelector("[data-tid='Item']")).Text;
        var calendar = driver.FindElement(By.CssSelector("[data-tid='Calendar']")).Displayed;
        
        Assert.Multiple(() =>
        {
            Assert.That(profileTitle == "Профиль",
                "CURRENT TEXT: " + profileTitle + "\nEXPECTED: Профиль");
            Assert.That(calendar, Is.True, "calendar is not displayed");
        });
        
        
        //Thread.Sleep(3000);
    }


    public void Authorization()
    {
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("mgliezz@gmail.com");
        
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("h0wXyTE5!1");
        
        var button = driver.FindElement(By.Name("button"));
        button.Click();
    }

    [TearDown]
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
    }
}

