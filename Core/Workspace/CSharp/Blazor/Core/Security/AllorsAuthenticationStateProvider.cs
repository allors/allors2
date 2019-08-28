namespace Allors.Blazor
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Allors.Workspace.Remote;
    using Microsoft.AspNetCore.Components;

    public partial class AllorsAuthenticationStateProvider : AuthenticationStateProvider
    {
        public string UserId { get; private set; }

        private readonly AllorsAuthenticationStateProviderConfig Config;

        private readonly RemoteDatabase Database;

        public AllorsAuthenticationStateProvider(AllorsAuthenticationStateProviderConfig config, RemoteDatabase database)
        {
            this.Config = config;
            this.Database = database;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsPrincipal user;

            if (!string.IsNullOrWhiteSpace(this.UserId))
            {
                var claim = new Claim(ClaimTypes.Name, this.UserId);
                var claims = new[] { claim };
                var identity = new ClaimsIdentity(claims, "Allors Identity Claims");

                user = new ClaimsPrincipal(identity);
            }
            else
            {
                user = new ClaimsPrincipal();
            }

            return Task.FromResult(new AuthenticationState(user));
        }

        public async Task<bool> LogIn(string userName, string password)
        {
            var uri = new Uri(this.Config.AuthenticationUrl, UriKind.Relative);
            var loggedIn = await this.Database.Login(uri, userName, password);
            if (loggedIn)
            {
                this.UserId = this.Database.UserId;
                this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
            }

            return loggedIn;
        }

        public void LogOut()
        {
            this.UserId = null;
            this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
        }
    }
}
