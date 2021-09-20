// <copyright file="Test.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using Allors;
    using Allors.Database.Adapters.SqlClient;
    using Allors.Domain;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;
    using Allors.Services;
    using Bogus;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using OpenQA.Selenium;
    using src.app.auth;
    using src.app.main;
    using Database = Allors.Database.Adapters.SqlClient.Database;
    using ObjectFactory = Allors.ObjectFactory;

    public abstract class Test : IDisposable
    {
        public const string ClientUrl = "http://localhost:4200";
        public const string ServerUrl = "http://localhost:5000";

        public static readonly string DatabaseInitUrl = $"{ServerUrl}/Test/Init";
        public static readonly string DatabaseTimeShiftUrl = $"{ServerUrl}/Test/TimeShift";

        private static readonly FileInfo populationFileInfo;
        private static string population;

        static Test()
        {
            var domainPrint = typeof(User).Assembly.Fingerprint();
            var testPrint = typeof(Test).Assembly.Fingerprint();
            var testPopulationPrint = typeof(Marker).Assembly.Fingerprint();
            populationFileInfo = new FileInfo($"population.{domainPrint}.{testPrint}.{testPopulationPrint}.xml");
        }

        protected Test(TestFixture fixture)
        {
            // Init Browser
            this.DriverManager = new DriverManager();
            this.DriverManager.Start();

            // Init Server
            this.Driver.Navigate().GoToUrl(Test.DatabaseInitUrl);

            CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            // Init Allors
            var configurationBuilder = new ConfigurationBuilder();

            const string root = "/config/base";
            configurationBuilder.AddCrossPlatform(".");
            configurationBuilder.AddCrossPlatform(root);
            configurationBuilder.AddCrossPlatform(Path.Combine(root, "server"));
            configurationBuilder.AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();
            services.AddAllors();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(builder => builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace));
            services.AddSingleton<Faker>();

            this.ServiceProvider = services.BuildServiceProvider();

            var loggerFactory = this.ServiceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");

            this.Logger = (ILogger)this.ServiceProvider.GetService(typeof(ILogger<>).MakeGenericType(new[] { this.GetType() }));

            var database = new Database(this.ServiceProvider, new Configuration
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ObjectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User)),
            });

            if ((population == null) && populationFileInfo.Exists)
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
                    var config = new Config();
                    new Setup(session, config).Apply();
                    session.Commit();

                    new IntranetPopulation(session, null).Execute();

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

        public ServiceProvider ServiceProvider { get; set; }

        public ILogger Logger { get; set; }

        public ISession Session { get; set; }

        public DriverManager DriverManager { get; }

        public IWebDriver Driver => this.DriverManager.Driver;

        public Sidenav Sidenav => new MainComponent(this.Driver).Sidenav;

        public virtual void Dispose() => this.DriverManager.Stop();

        public void Login(string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
            {
                userName = new UserGroups(this.Session).Administrators.Members.First.UserName;
            }

            this.Driver.Navigate().GoToUrl(Test.ClientUrl + "/login");
            var login = new LoginComponent(this.Driver);
            login.Login(userName);
        }
    }
}
