namespace Allors.Server
{
    using Allors.Adapters.Object.SqlClient;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services.Base;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Cors.Internal;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    using Newtonsoft.Json;

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
            var mailService = new MailService { DefaultSender = this.Configuration["DefaultSender"] };
            var securityService = new SecurityService();
            var serviceLocator = new ServiceLocator
                                     {
                                         TimeServiceFactory = () => timeService,
                                         MailServiceFactory = () => mailService,
                                         SecurityServiceFactory = () => securityService
                                     };
            database.SetServiceLocator(serviceLocator.Assert());

            services.AddSingleton<IDatabase>(database);
            services.AddScoped<IAllorsContext, AllorsContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var authenticationContext = new FileAuthenticationContext(this.Configuration["AuthenticationKey"])
                                            {
                                                Issuer = @"https://issuer.allors.com",
                                                Audience = @"https://app.allors.com"
                                            };
            services.AddSingleton<IAuthenticationContext>(authenticationContext);

            // Add framework services.
            services.AddCors(options =>
                {
                    options.AddPolicy(
                        "AllowAll",
                        builder =>
                            {
                                builder
                                    .AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials();
                            });
                });

            services.AddAuthorization(auth =>
                {
                    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                        .RequireAuthenticatedUser().Build());
                });

            services.AddMvc();
            services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IAuthenticationContext authenticationContext)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Use(async (context, next) =>
                        {
                            var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                            var message = error?.Error.GetType().ToString();

                            if (error?.Error is SecurityTokenExpiredException)
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";

                                await context.Response.WriteAsync(JsonConvert.SerializeObject(message));
                            }
                            else if (error?.Error != null)
                            {
                                context.Response.StatusCode = 500;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(message));
                            }
                            else await next();
                        });
                });

            app.UseCors("AllowAll");

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
                                               {
                                                   AutomaticAuthenticate = true,
                                                   AutomaticChallenge = true,
                                                   TokenValidationParameters = new TokenValidationParameters
                                                                                   {
                                                                                       IssuerSigningKey = authenticationContext.Key,
                                                                                       ValidIssuer = authenticationContext.Issuer,
                                                                                       ValidAudience = authenticationContext.Audience,
                                                                                   }
                                               });


            app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
