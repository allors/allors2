namespace Tests
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    using Allors;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Html5;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.Extensions;

    using Pages.ApplicationTests;

    using Tests.ApplicationTests;

    using ObjectFactory = Allors.ObjectFactory;

    public abstract class Test : IDisposable
    {
        public const string ClientUrl = "http://localhost:4200";
        public const string ServerUrl = "http://localhost:5000";

        public static readonly string DatabaseInitUrl = $"{ServerUrl}/Test/Init";
        public static readonly string DatabaseTimeShiftUrl = $"{ServerUrl}/Test/TimeShift";

        private static FileInfo populationFileInfo;
        private static string population;

        static Test()
        {
            var domainPrint = typeof(User).Assembly.Fingerprint();
            var testPrint = typeof(Test).Assembly.Fingerprint();
            populationFileInfo = new FileInfo($"population.{domainPrint}.{testPrint}.xml");
        }

        protected Test(TestFixture fixture)
        {
            // Init Browser
            this.Driver = fixture.Driver;

            // Init Server
            this.Driver.Navigate().GoToUrl(Test.DatabaseInitUrl);

            // Init Allors
            CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            const string FileName = @"apps.appSettings.json";
            var userSettings = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/allors/{FileName}";
            var systemSettings = $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}/allors/{FileName}";

            var appConfiguration = new ConfigurationBuilder()
                .AddJsonFile(@"appSettings.json")
                .AddJsonFile(systemSettings, true)
                .AddJsonFile(userSettings, true)
                .AddEnvironmentVariables()
                .Build();

            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));

            var services = new ServiceCollection();
            services.AddAllors();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = new Configuration
            {
                ConnectionString = appConfiguration["ConnectionStrings:DefaultConnection"],
                ObjectFactory = objectFactory,
            };

            var database = new Database(serviceProvider, configuration);

            if (population == null && populationFileInfo.Exists)
            {
                population = File.ReadAllText(populationFileInfo.FullName);
            }

            if (population != null)
            {
                using (var stringReader = new StringReader(population))
                using (var reader = XmlReader.Create(stringReader))
                {
                    database.Load(reader);
                }
            }
            else
            {
                database.Init();

                using (var session = database.CreateSession())
                {
                    new Setup(session, null).Apply();
                    session.Commit();

                    new Population(session, null).Execute();

                    session.Commit();

                    using (var stringWriter = new StringWriter())
                    {

                        using (var writer = XmlWriter.Create(stringWriter))
                        {
                            database.Save(writer);
                        }

                        population = stringWriter.ToString();
                        File.WriteAllText(populationFileInfo.FullName, population);
                    }
                }
            }

            this.Session = database.CreateSession();

        }

        public ISession Session { get; set; }

        public IWebDriver Driver { get; set; }

        public virtual void Dispose()
        {
        }

        public DashboardPage Login(string userName = "administrator", bool reset = true)
        {
            this.Driver.Navigate().GoToUrl(Test.ClientUrl + "/login");

            if (reset)
            {
                this.Driver.Manage().Cookies.DeleteAllCookies();
                this.Driver.ExecuteJavaScript("localStorage.clear();");
                this.Driver.ExecuteJavaScript("sessionStorage.clear();");
            }

            var page = new LoginPage(this.Driver);
            return page.Login();
        }
    }
}