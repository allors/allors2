namespace Blazor.Server
{
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;
    using Blazor.Client;
    using BlazorAppServerSide.Areas.Identity;
    using BlazorStrap;
    using Identity;
    using Identity.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public bool IsClientSideBlazor { get; } = true;

        public bool IsServerSideBlazor => !this.IsClientSideBlazor;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);

            // Allors
            services.AddAllors();
            services.AddSingleton<IPolicyService, PolicyService>();
            services.AddSingleton<IExtentService, ExtentService>();

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .UseAllors()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            if (this.IsClientSideBlazor)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                // Enable Dual Authentication
                services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddCookie(cfg => cfg.SlidingExpiration = true)
                    .AddJwtBearer(cfg =>
                    {
                        cfg.RequireHttpsMetadata = false;
                        cfg.SaveToken = true;

                        cfg.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidIssuer = this.Configuration["Tokens:Issuer"],
                            ValidAudience = this.Configuration["Tokens:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"])),
                        };
                    });

                services.ConfigureApplicationCookie(options =>
                {
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return System.Threading.Tasks.Task.CompletedTask;
                    };
                });

                services.AddMvc().AddNewtonsoftJson();
                services.AddResponseCompression(opts =>
                {
                    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "application/octet-stream" });
                });
            }

            if (this.IsServerSideBlazor)
            {
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

                services.AddBootstrapCSS();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Allors
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

            if (this.IsClientSideBlazor)
            {
                app.UseResponseCompression();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseBlazorDebugging();
                }

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseStaticFiles();
                app.UseClientSideBlazorFiles<Client.Startup>();

                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
                });
            }

            if (this.IsServerSideBlazor)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();
                }

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
}
