namespace Tests
{
    using System;
    using System.Globalization;

    using Allors;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Pages;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using OpenQA.Selenium;

    using ObjectFactory = Allors.ObjectFactory;

    public abstract class Test : IDisposable
    {
        public const string ClientUrl = "http://localhost:4200";
        public const string ServerUrl = "http://localhost:5000";

        public static readonly string DatabaseInithUrl = $"{ServerUrl}/Test/Init";
        public static readonly string DatabaseTimeShiftUrl = $"{ServerUrl}/Test/TimeShift";

        protected Test(TestFixture fixture)
        {
            // Start Driver
            this.DriverManager = new DriverManager();
            this.DriverManager.Start();

            // Init Server
            this.Driver.Navigate().GoToUrl(Test.DatabaseInithUrl);

            // Init Allors
            CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            const string FileName = @"base.appSettings.json";
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

            database.Init();
            this.Session = database.CreateSession();
            new Setup(this.Session, null).Apply();
            this.Session.Commit();

            new Population(this.Session, null).Execute();

            this.Session.Commit();
        }

        public ISession Session { get; set; }

        public DriverManager DriverManager { get; }

        public IWebDriver Driver => this.DriverManager.Driver;

        public virtual void Dispose()
        {
            this.DriverManager.Stop();
        }

        public HomePage Login(string userName = "administrator")
        {
            this.Driver.Navigate().GoToUrl(Test.ClientUrl + "/login");

            var page = new LoginPage(this.Driver);
            return page.Login();
        }
    }
}