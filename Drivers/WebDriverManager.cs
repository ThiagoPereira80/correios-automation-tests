using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace CorreiosAutomation.Drivers
{
    public static class WebDriverManager
    {
        private static IWebDriver _driver;

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-extensions");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--no-sandbox");
                
                // Para execução headless (CI/CD - mais rápido)
                // options.AddArgument("--headless");

                _driver = new ChromeDriver(options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                _driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(10);
            }
            return _driver;
        }

        public static void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }
    }
}