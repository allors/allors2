namespace Blazor.Bootstrap.ServerSide
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Allors.Database.Adapters.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;
    using Blazor.Bootstrap.ServerSide.Areas.Identity;
    using BlazorStrap;
    using Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
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

            services.AddDefaultIdentity<IdentityUser>()
               .AddAllorsStores();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

            services.AddSingleton<Allors.Workspace.Local.LocalDatabase>();
            services.AddSingleton<Allors.Workspace.IDatabase>(provider => provider.GetRequiredService<Allors.Workspace.Local.LocalDatabase>());
            services.AddSingleton((serviceProvider) =>
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
                    // Creating the NavigationManager needs to wait until the JS Runtime is initialized, so defer it.
                    var navigationManager = s.GetRequiredService<NavigationManager>();
                    return new HttpClient
                    {
                        BaseAddress = new Uri(navigationManager.BaseUri),
                    };
                });
            }

            services.AddBootstrapCSS();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.GetRequiredService<IDatabaseService>().Database = new Database(
                 app.ApplicationServices,
                 new Configuration
                 {
                     ObjectFactory = new Allors.ObjectFactory(MetaPopulation.Instance, typeof(User)),
                     ConnectionString = this.Configuration.GetConnectionString("DefaultConnection"),
                     CommandTimeout = 600,
                 });

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
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
