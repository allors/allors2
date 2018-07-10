namespace Intranet.Tests
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Threading.Tasks;

    using Allors;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Intranet.Pages;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using PuppeteerSharp;

    using Xunit;

    using ObjectFactory = Allors.ObjectFactory;
    using Page = PuppeteerSharp.Page;
    using Task = System.Threading.Tasks.Task;

    public abstract class Test : IDisposable, IAsyncLifetime
    {
        public const string ServerUrl = "http://localhost:5000";

        public static readonly string DatabaseInithUrl = $"{ServerUrl}/Test/Init";
        public static readonly string DatabaseTimeShiftUrl = $"{ServerUrl}/Test/TimeShift";

        protected Test(TestFixture fixture)
        {
        }

        public ISession Session { get; set; }

        public Browser Browser { get; set; }

        public Page Page { get; set; }

        public async Task InitializeAsync()
        {
            // Init Browser
            this.Browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                Args = new[] { "--start-maximized" }
            });

            // Init Server
            this.Page = (await this.Browser.PagesAsync())[0];
            var initResponse = await this.Page.GoToAsync(Test.DatabaseInithUrl);
            if (!initResponse.Ok)
            {
                throw new Exception("Server.Init() failed");
            }

            // Init Allors
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            var appConfiguration = new ConfigurationBuilder()
                .AddJsonFile(@"appSettings.json")
                .Build();
            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));

            var services = new ServiceCollection();
            services.AddAllorsTesting();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = new Configuration
            {
                ConnectionString = appConfiguration["allors"],
                ObjectFactory = objectFactory,
            };

            var database = new Database(serviceProvider, configuration);

            database.Init();
            this.Session = database.CreateSession();
            new Setup(this.Session, null).Apply();
            this.Session.Commit();

            await this.OnInitAsync();
        }

        public async Task DisposeAsync()
        {
            await this.Browser.CloseAsync();
        }

        public virtual void Dispose()
        {
        }

        public async Task<Page> Login(string userName = "administrator")
        {
            await this.Page.GoToAsync("http://localhost:4200");

            var loginPage = new LoginPage(this.Page);
            await loginPage.Login(userName);

            return this.Page;
        }
        
        protected virtual Task OnInitAsync()
        {
            return Task.CompletedTask;
        }
    }
}