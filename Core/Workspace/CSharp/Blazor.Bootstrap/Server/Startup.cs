namespace Blazor.Server
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;
    using Blazor.Client;
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

        public bool IsServerSideBlazor { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            this.IsServerSideBlazor = !services.Any(x => x.ServiceType == typeof(HttpClient));

            services.AddSingleton(this.Configuration);

            // Allors
            services.AddAllors();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPolicyService, PolicyService>();
            services.AddSingleton<IExtentService, ExtentService>();

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .UseAllors()
                .AddDefaultTokenProviders();

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

            services.AddSingleton<Allors.Workspace.Local.LocalDatabase>();
            services.AddSingleton<Allors.Workspace.IDatabase>(provider => provider.GetRequiredService<Allors.Workspace.Local.LocalDatabase>());

            services.AddSingleton<Allors.Workspace.Workspace>((serviceProvider) =>
            {
                var objectFactory = new Allors.Workspace.ObjectFactory(Allors.Workspace.Meta.MetaPopulation.Instance, typeof(User));
                var workspace = new Allors.Workspace.Workspace(objectFactory);
                return workspace;
            });


            if (this.IsServerSideBlazor)
            {
                services.AddServerSideBlazor();

                // TODO: Authentication
                //var implementationInstance = new AllorsAuthenticationStateProviderConfig
                //{
                //    AuthenticationUrl = "/TestAuthentication/Token",
                //};
                //services.AddSingleton(implementationInstance);
                //services.AddScoped<AllorsAuthenticationStateProvider>();
                //services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AllorsAuthenticationStateProvider>());
                //services.AddAuthorizationCore();

                services.AddBootstrapCSS();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseResponseCompression();

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                if (!this.IsServerSideBlazor)
                {
                    app.UseBlazorDebugging();
                }
            }

            app.UseStaticFiles();

            if (!this.IsServerSideBlazor)
            {
                app.UseClientSideBlazorFiles<Startup>();
            }

            app.UseRouting();

            if (this.IsServerSideBlazor)
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapBlazorHub<App>(selector: "app");
                    endpoints.MapFallbackToPage("/_Host");
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapFallbackToClientSideBlazor<Startup>("index.html");
                });
            }
        }
    }
}
