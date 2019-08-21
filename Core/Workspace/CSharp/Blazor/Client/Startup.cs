namespace Blazor.Client
{
    using System.Net.Http;
    using Allors.Workspace;
    using Allors.Workspace.Client;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Database>((serviceProvider) =>
            {
                var http = serviceProvider.GetRequiredService<HttpClient>();
                var database = new Database(http);
                return database;
            });

            services.AddSingleton<Workspace>((serviceProvider) =>
            {
                var objectFactory = new Allors.Workspace.ObjectFactory(MetaPopulation.Instance, typeof(User));
                var workspace = new Workspace(objectFactory);
                return workspace;
            });

            services.AddAuthorizationCore();
            services.AddScoped<AllorsAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AllorsAuthenticationStateProvider>());
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
