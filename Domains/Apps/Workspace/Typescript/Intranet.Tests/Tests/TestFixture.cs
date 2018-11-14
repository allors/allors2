namespace Tests.Intranet
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;

    public class TestFixture : IDisposable 
    {
        public TestFixture()
        {
            CultureInfo.CurrentCulture = new CultureInfo("nl-BE");

            this.StartWebdriver();
        }

        public IWebDriver Driver { get; set; }

        public string DownloadPath { get; private set; }

        public void Dispose()
        {
            this.StopWebdriver();
        }

        private void StartWebdriver()
        {
            this.DownloadPath = Path.GetTempPath() + "SeleniumDownloads\\";

            if (!Directory.Exists(this.DownloadPath))
            {
                Directory.CreateDirectory(this.DownloadPath);
            }

            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", this.DownloadPath);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddArgument("--start-maximized");
            options.AddUserProfilePreference("browser.cache.disk.enable", false);
            options.AddUserProfilePreference("browser.cache.memory.enable", false);
            options.AddUserProfilePreference("browser.cache.offline.enable", false);
            options.AddUserProfilePreference("network.http.use-cache", false);
            options.AddArguments("disable-infobars");

            this.Driver = new ChromeDriver(Environment.CurrentDirectory, options);

            // Move to monitor on the left
            this.Driver.Manage().Window.Position = new Point(-1000, 0);
            this.Driver.Manage().Window.Maximize();
        }

        private void StopWebdriver()
        {
            try
            {
                this.Driver.Close();
            }
            finally
            {
                try
                {
                    this.Driver.Quit();
                }
                finally
                {
                    if (this.Driver is ChromeDriver)
                    {
                        Process.Start("taskkill", "/F /IM chrome.exe");
                        Process.Start("taskkill", "/F /IM chromedriver.exe");
                        Directory.Delete(this.DownloadPath, true);
                    }
                    else if (this.Driver is FirefoxDriver)
                    {
                        Process.Start("taskkill", "/F /IM firefox.exe");
                    }
                    else if (this.Driver is InternetExplorerDriver)
                    {
                        Process.Start("taskkill", "/F /IM iexplore.exe");
                    }
                    else
                    {
                        throw new Exception("Can not kill driver of type " + this.Driver.GetType().Name);
                    }
                }
            }
        }
    }
}