// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using Allors;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services.Production;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Allors
            var objectFactory = new Allors.ObjectFactory(MetaPopulation.Instance, typeof(User));
            var configuration = new Configuration
                                    {
                                        ObjectFactory = objectFactory,
                                        ConnectionString = this.Configuration.GetConnectionString("DefaultConnection")
                                    };

            var database = new Database(configuration);

            var timeService = new TimeService();
            var mailService = new MailService();
            var serviceLocator = new ServiceLocator
                                     {
                                         TimeServiceFactory = () => timeService,
                                         MailServiceFactory = () => mailService
                                     };
            database.SetServiceLocator(serviceLocator.Assert());

            services.AddSingleton<IDatabase>(database);
            services.AddScoped<IAllorsContext, AllorsContext>();

            // Add framework services.
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Allors
            app.UseAuthentication();

            app.UseMvc();

            app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
