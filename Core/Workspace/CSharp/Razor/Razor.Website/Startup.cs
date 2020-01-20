namespace Razor
{
    using Allors.Database.Adapters.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;
    using Allors.Security;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ObjectFactory = Allors.ObjectFactory;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);

            services.AddAllors();
            services.AddSingleton<IPolicyService, PolicyService>();
            services.AddSingleton<IExtentService, ExtentService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDefaultIdentity<IdentityUser>()
                .AddAllorsStores();

            services.AddSingleton<Allors.Workspace.Local.LocalDatabase>();
            services.AddSingleton<Allors.Workspace.IDatabase>(provider => provider.GetRequiredService<Allors.Workspace.Local.LocalDatabase>());
            services.AddSingleton<Allors.Workspace.Workspace>((serviceProvider) =>
            {
                var objectFactory = new Allors.Workspace.ObjectFactory(Allors.Workspace.Meta.MetaPopulation.Instance, typeof(Allors.Workspace.Domain.User));
                var workspace = new Allors.Workspace.Workspace(objectFactory);
                return workspace;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
