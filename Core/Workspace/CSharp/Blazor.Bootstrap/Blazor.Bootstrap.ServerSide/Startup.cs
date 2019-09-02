namespace Blazor.Bootstrap.ServerSide
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Blazor.Bootstrap;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;
    using Blazor.Bootstrap.ServerSide.Areas.Identity;
    using BlazorStrap;
    using Identity;
    using Identity.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);

            services.AddAllors();
            services.AddSingleton<IPolicyService, PolicyService>();
            services.AddSingleton<IExtentService, ExtentService>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .UseAllors()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingAuthenticationStateProvider<ApplicationUser>>();

            services.AddSingleton<Allors.Workspace.Local.LocalDatabase>();
            services.AddSingleton<Allors.Workspace.IDatabase>(provider => provider.GetRequiredService<Allors.Workspace.Local.LocalDatabase>());
            services.AddSingleton<Allors.Workspace.Workspace>((serviceProvider) =>
            {
                var objectFactory = new Allors.Workspace.ObjectFactory(Allors.Workspace.Meta.MetaPopulation.Instance, typeof(Allors.Workspace.Domain.User));
                var workspace = new Allors.Workspace.Workspace(objectFactory);
                return workspace;
            });

            // Server Side Blazor doesn't register HttpClient by default
            if (!services.Any(x => x.ServiceType == typeof(HttpClient)))
            {
                // Setup HttpClient for server side in a client side compatible fashion
                services.AddScoped<HttpClient>(s =>
                {
                    // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                    var uriHelper = s.GetRequiredService<IUriHelper>();
                    return new HttpClient
                    {
                        BaseAddress = new Uri(uriHelper.GetBaseUri()),
                    };
                });
            }

            services.AddBootstrapCSS();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var objectFactory = new Allors.ObjectFactory(MetaPopulation.Instance, typeof(User));

            var database = new Database(
                app.ApplicationServices,
                new Configuration
                {
                    ObjectFactory = objectFactory,
                    ConnectionString = this.Configuration.GetConnectionString("DefaultConnection"),
                    CommandTimeout = 600,
                });

            var databaseService = app.ApplicationServices.GetRequiredService<IDatabaseService>();
            databaseService.Database = database;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub<App>(selector: "app");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
