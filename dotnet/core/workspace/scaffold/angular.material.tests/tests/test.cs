// <copyright file="Test.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.Globalization;
    using System.IO;
    using Allors;
    using Allors.Database.Adapters.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;
    using libs.angular.material.custom.src.auth;
    using libs.angular.material.custom.src.dashboard;
    using libs.angular.material.custom.src.main;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using OpenQA.Selenium;
    using ObjectFactory = Allors.ObjectFactory;

    public abstract class Test : IDisposable
    {
        public const string ClientUrl = "http://localhost:4200";
        public const string ServerUrl = "http://localhost:5000/allors";

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

            const string root = "/config/core";
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddCrossPlatform(".");
            configurationBuilder.AddCrossPlatform(root);
            configurationBuilder.AddCrossPlatform(Path.Combine(root, "commands"));
            configurationBuilder.AddEnvironmentVariables();
            var appConfiguration = configurationBuilder.Build();

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

            var config = new Config();
            new Setup(this.Session, config).Apply();
            this.Session.Commit();

            new Population(this.Session, null).Execute();

            this.Session.Commit();
        }

        public IWebDriver Driver => this.DriverManager.Driver;

        public DriverManager DriverManager { get; }

        public ISession Session { get; set; }

        public Sidenav Sidenav => new MainComponent(this.Driver).Sidenav;

        public virtual void Dispose() => this.DriverManager.Stop();

        public DashboardComponent Login(string userName = "administrator")
        {
            this.Driver.Navigate().GoToUrl(Test.ClientUrl + "/login");

            var page = new LoginComponent(this.Driver);
            return page.Login();
        }
    }
}
