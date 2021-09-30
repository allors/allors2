namespace Blazor.Bootstrap.ClientSide
{
    using System;
    using System.Net.Http;
    using Allors.Workspace;
    using Allors.Workspace.Blazor;
    using Allors.Workspace.Blazor.Bootstrap;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Remote;
    using BlazorStrap;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private const string ServerUrl = "http://localhost:5000";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<RemoteDatabase>((serviceProvider) =>
            {
                var http = serviceProvider.GetRequiredService<HttpClient>();
                http.BaseAddress = new Uri(ServerUrl);
                var database = new RemoteDatabase(http);
                return database;
            });
            services.AddSingleton<IDatabase>(provider => provider.GetRequiredService<RemoteDatabase>());

            services.AddSingleton<Workspace>((serviceProvider) =>
            {
                var objectFactory = new Allors.Workspace.ObjectFactory(MetaPopulation.Instance, typeof(User));
                var workspace = new Workspace(objectFactory);
                return workspace;
            });

            var implementationInstance = new AllorsAuthenticationStateProviderConfig
            {
                AuthenticationUrl = "TestAuthentication/Token",
            };
            services.AddSingleton(implementationInstance);
            services.AddScoped<AllorsAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AllorsAuthenticationStateProvider>());
            services.AddAuthorizationCore();

            services.AddBootstrapCSS();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<Blazor.Bootstrap.App>("app");
        }
    }
}
