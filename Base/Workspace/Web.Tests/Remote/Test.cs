using OpenQA.Selenium.IE;

namespace Tests.Remote
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Allors;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain.NonLogging;

    using NUnit.Framework;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium.Remote;

    using Protractor;

    public abstract class Test
    {
        public static DirectoryInfo AppLocation => new DirectoryInfo("../../../Web");
        
        public const string BaseUrl = "http://localhost:" + Fixture.Port;

        public const string AppUrl = BaseUrl;

        public const string InitUrl = BaseUrl + "/Test/Init";
        public const string LoginUrl = BaseUrl + "/Test/Login";
        public const string TimeShiftUrl = BaseUrl + "/Test/TimeShift";

        public const string UnitTestsUrl = BaseUrl + "/UnitTests";

        private const int ImplicitWait = 1;
        private const int ScriptTimeout = 30;

        public NgWebDriver Driver { get; private set; }

        public ISession Session { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.SetupWebdriver();

            var configuration = new Configuration { ObjectFactory = Config.ObjectFactory };
            var database = new Database(configuration);
            Config.Default = database;
            Config.TimeShift = null;
            database.Init();

            this.Driver.WrappedDriver.Navigate().GoToUrl(Test.InitUrl);

            var javaScriptExecutor = (IJavaScriptExecutor)this.Driver.WrappedDriver;
            javaScriptExecutor.ExecuteScript("window.sessionStorage.clear();");
            javaScriptExecutor.ExecuteScript("window.localStorage.clear();");
            this.Driver.Manage().Cookies.DeleteAllCookies();
            this.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(ImplicitWait));
            this.Driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(ScriptTimeout));
            this.Driver.Manage().Window.Maximize();

            this.Session = Config.Default.CreateSession();
            new Setup(this.Session, null).Apply();
            this.Session.Commit();
        }

        [TearDown]
        public virtual void TearDown()
        {
            try
            {
                this.Session.Rollback();
                this.Session = null;
            }
            finally
            {
                try
                {
                    // Bumb web.config to new date, causing a restart
                    var webConfig = Enumerable.First<FileInfo>(AppLocation.GetFiles(), v => v.Name.ToLowerInvariant().Equals("web.config"));
                    File.SetLastWriteTimeUtc(webConfig.FullName, DateTime.UtcNow);
                }
                finally
                {
                    this.TearDownWebdriver();
                }
            }
        }

        protected void Derive(params IObject[] objects)
        {
            var derivation = new Derivation(this.Session, objects);
            var validation = derivation.Derive();
            if (validation.HasErrors)
            {
                throw new Exception("Derivation Error");
            }
        }

        private void SetupWebdriver()
        {
            RemoteWebDriver seleniumDriver;

            //var firefoxBinaryFileInfo = new FileInfo(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe");
            //if (firefoxBinaryFileInfo.Exists)
            //{
            //    var firefoxBinary = new FirefoxBinary(firefoxBinaryFileInfo.FullName);
            //    seleniumDriver = new FirefoxDriver(firefoxBinary, null);
            //}
            //else
            //{
            //    seleniumDriver = new FirefoxDriver();
            //}

            seleniumDriver = new ChromeDriver();

            //seleniumDriver = new InternetExplorerDriver(new InternetExplorerOptions
            //{
            //    EnableNativeEvents = false,
            //    UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Accept,
            //    IntroduceInstabilityByIgnoringProtectedModeSettings = true,
            //    EnablePersistentHover = true
            //});

            //seleniumDriver = new PhantomJSDriver();


            this.Driver = new NgWebDriver(seleniumDriver);
        }

        private void TearDownWebdriver()
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
                    this.KillWebdriver();
                }
            }
        }

        private void KillWebdriver()
        {
            if (this.Driver.WrappedDriver is ChromeDriver)
            {
                Process.Start("taskkill", "/F /IM chrome.exe");
            }
            else if (this.Driver.WrappedDriver is FirefoxDriver)
            {
                Process.Start("taskkill", "/F /IM firefox.exe");
            }
            else if (this.Driver.WrappedDriver is InternetExplorerDriver)
            {
                Process.Start("taskkill", "/F /IM iexplore.exe");
            }
            else if (this.Driver.WrappedDriver is PhantomJSDriver)
            {
                Process.Start("taskkill", "/F /IM phantomjs.exe");
            }
            else
            {
                throw new Exception("Can not kill driver of type " + this.Driver.WrappedDriver.GetType().Name);
            }

            // Make sure process is killed
            Thread.Sleep(1000);
        }
    }
}