namespace Intranet.Tests
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Xml;

    using Allors;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;

    using Intranet.Pages;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using OpenQA.Selenium;

    using ObjectFactory = Allors.ObjectFactory;

    public abstract class Test : IDisposable
    {
        public const string ClientUrl = "http://localhost:4200";
        public const string ServerUrl = "http://localhost:5000";

        public static readonly string DatabaseInitUrl = $"{ServerUrl}/Test/Init";
        public static readonly string DatabaseTimeShiftUrl = $"{ServerUrl}/Test/TimeShift";

        private static string population;

        protected Test(TestFixture fixture)
        {
            // Init Browser
            this.Driver = fixture.Driver;

            // Init Server
            this.Driver.Navigate().GoToUrl(Test.DatabaseInitUrl);

            // Init Allors
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            var appConfiguration = new ConfigurationBuilder().AddJsonFile(@"appSettings.json").Build();
            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));

            var services = new ServiceCollection();
            services.AddAllors();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = new Configuration
            {
                ConnectionString = appConfiguration["allors"],
                ObjectFactory = objectFactory,
            };

            var database = new Database(serviceProvider, configuration);

            FileInfo fileInfo;
            using (var md5 = MD5.Create())
            {
                var assemblyFile = typeof(User).Assembly.Location;
                var assemblyBytes = File.ReadAllBytes(assemblyFile);
                var assemblyHash = md5.ComputeHash(assemblyBytes);
                var assemblyHashString = string.Concat(assemblyHash.Select(v => v.ToString("X2")));
                fileInfo = new FileInfo($"population.{assemblyHashString}.xml");
            }

            if (population == null && fileInfo.Exists)
            {
                population = File.ReadAllText(fileInfo.FullName);
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

                    // TODO: Using Demo instead of Test Population
                    new Demo(session, null).Execute();
                    // new Population(this.Session, null).Execute();

                    session.Commit();

                    using (var stringWriter = new StringWriter())
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        database.Save(writer);
                        population = stringWriter.ToString();
                        File.WriteAllText(fileInfo.FullName, population);
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

        public DashboardPage Login(string userName = "administrator")
        {
            this.Driver.Navigate().GoToUrl(Test.ClientUrl + "/login");

            var page = new LoginPage(this.Driver);
            return page.Login();
        }
    }
}