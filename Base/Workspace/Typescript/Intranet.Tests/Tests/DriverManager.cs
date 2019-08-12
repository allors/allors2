using System.Runtime.InteropServices;

namespace Tests
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;

    public class DriverManager : IDisposable
    {
        public IWebDriver Driver { get; set; }

        public string DownloadPath { get; private set; }

        public void Dispose()
        {
            this.Stop();
        }

        public void Start()
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

            options.AddArgument("no-sandbox");

            var chromeWebDriver = Environment.GetEnvironmentVariable("ChromeWebDriver");

            this.Driver = Directory.Exists(chromeWebDriver) ?
                new ChromeDriver(chromeWebDriver, options) :
                new ChromeDriver(Environment.CurrentDirectory, options);

            // Move to monitor on the left
            this.Driver.Manage().Window.Position = new Point(-800, 0);
            try
            {
                this.Driver.Manage().Window.Maximize();
            }
            catch
            {
                // MacOS will throw an error on Maximize()
                this.Driver.Manage().Window.FullScreen();
            }
        }

        public void Stop()
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
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        if (this.Driver is ChromeDriver)
                        {
                            try
                            {
                                Process.Start("taskkill", "/F /IM chrome.exe").WaitForExit();
                            }
                            finally
                            {
                                try
                                {
                                    Process.Start("taskkill", "/F /IM chromedriver.exe").WaitForExit();
                                }
                                finally
                                {
                                    Directory.Delete(this.DownloadPath, true);
                                }
                            }
                        }
                        else if (this.Driver is FirefoxDriver)
                        {
                            Process.Start("taskkill", "/F /IM firefox.exe").WaitForExit();
                        }
                        else if (this.Driver is InternetExplorerDriver)
                        {
                            Process.Start("taskkill", "/F /IM iexplore.exe").WaitForExit();
                        }
                        else
                        {
                            throw new Exception("Can not stop driver of type " + this.Driver.GetType().Name);
                        }

                    }
                }
            }
        }
    }
}