using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutoTestHomework;

public class Tests
{
    public ChromeDriver driver;
    public WebDriverWait wait;
    
    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--disable-extensions");

        driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        
        Authorization();
        
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/news"));
    }
    
    [Test]
    // проверка соответсвия текста на странице выхода из аккаунта
    public void LogoutText()
    {
        // кликаем на боковое меню
        var sidebar = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebar.Click();
        
        // ждем пока появится кнопка "Выйти" и кликаем  
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='LogoutButton']")));
        var logout = driver.FindElement(By.CssSelector("[data-tid='LogoutButton']"));
        logout.Click();
        
        // ждем пока появится нужный урл
        wait.Until(ExpectedConditions.UrlContains("Logout"));
        
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
    // проверяем, меняется ли текст после после выбора в фильтре "Я участник". Тест упадет, тк строка на стаффе содержит пробел
    public void CommunityPopUpText()
    {
        // завезем переменную под образец текста
        const string sampleText = "Я участник";
        
        // переходим на стр Сообщества
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        
        // ждем появления заголовка
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Title']")));
       
        // кликаем на выпадающий список
        var popUpMenu = driver.FindElements(By.CssSelector("[data-tid='PopupMenu__caption']"))
            .First(element => element.Displayed);
        popUpMenu.Click();
        
        // ждем пока нужный элемент в списке станет доступным и кликаем
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='PopupContent']")));
        var menuItem = driver.FindElement(By.CssSelector("[href*='isMember']"));
        menuItem.Click();
        
        // проверяем соответствие текста
        Assert.That(popUpMenu.Text == sampleText, "CURRENT TEXT: " + popUpMenu.Text + ", TEXT LEN: " + popUpMenu.Text.Length 
                                                  + "\nEXPECTED: " + sampleText + ", TEXT LEN: " + sampleText.Length);
        
    }

    
    [Test]
    // проверка отображения картинки с котом в разделе файлы при пустом поле поиска
    public void FilesEmptySearchImg()
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
    // проверка поиска мероприятия по названию
    public void EventSearch()
    {
        // заведем переменную под запрос
        const string Query = "Два стула"; 
        
        //  находим и кликаем иконку поиска
        var searchIcon = driver.FindElement(By.CssSelector("[data-tid='Services']"));
        searchIcon.Click();
        
        // кликаем на сроку поиска
        var searchBar = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        searchBar.Click();
        
        // вводим в строку запрос
        var search = driver.FindElement(By.CssSelector("[data-tid='SearchBar'] input"));
        search.SendKeys(Query);

        // дожидаемся появления результатов поиска и кликаем
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='ComboBoxMenu__item']")));
        var searchResult = driver.FindElement(By.CssSelector("[data-tid='ComboBoxMenu__item']"));
        searchResult.Click();
        
        // проверяем, содержит ли заголовок мероприятия искомую строку
        var EventTitle = driver.FindElement(By.CssSelector("[data-tid='Title']")).Text;
        Assert.That(EventTitle.Contains(Query), "Название мероприятия не содержит строку запроса.\nИскомая строка - " + Query);
       
    }

    [Test]
    // проверка соответсвия текста заголовка в профиле и отображения календаря 
    public void ProfileDate()
    {
        // кликаем на боковое меню
        var sidebar = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebar.Click();
        
        var avatar = driver.FindElements(By.CssSelector("[data-tid='Avatar']")).Last();
        avatar.Click();
        
        //подождем пока прогрузится профиль
        wait.Until(ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/profile"));
        
        // кликаем на "Выбрать дату"
        var chooseDate = driver.FindElement(By.CssSelector("[data-tid='CalendarChoose']"));
        chooseDate.Click();
        
        var profileTitle = driver.FindElement(By.CssSelector("[data-tid='Item']")).Text;
        var calendar = driver.FindElement(By.CssSelector("[data-tid='Calendar']")).Displayed;
        
        // проверяем соответсвие текста и отображение календаря
        Assert.Multiple(() =>
        {
            Assert.That(profileTitle == "Профиль",
                "CURRENT TEXT: " + profileTitle + "\nEXPECTED: Профиль");
            Assert.That(calendar, Is.True, "calendar is not displayed");
        });
        
    }


    public void Authorization()
    {
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("mgliezz@gmail.com");
        
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("***");
        
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

