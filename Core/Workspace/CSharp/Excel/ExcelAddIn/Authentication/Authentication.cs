namespace ExcelAddIn
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Allors.Excel;
    using Allors.Workspace.Remote;
    using Application;

    public class Authentication
    {
        public Authentication(Ribbon ribbon, Program program, RemoteDatabase database, Client client, AppConfig configuration)
        {
            this.Ribbon = ribbon;
            this.Program = program;
            this.Database = database;
            this.Client = client;
            this.Configuration = configuration;
        }

        public Ribbon Ribbon { get; }

        public IProgram Program { get; }

        public RemoteDatabase Database { get; }

        public Client Client { get; }

        public AppConfig Configuration { get; }

        public async Task Switch()
        {
            if (this.Client != null)
            {
                var wasLoggedIn = this.Client.IsLoggedIn;

                if (this.Client.IsLoggedIn)
                {
                    this.Logoff();
                }
                else
                {
                    await this.Login(this.Database);
                }

                var isLoggedIn = this.Client.IsLoggedIn;

                if (wasLoggedIn != isLoggedIn)
                {
                    if (this.Client.IsLoggedIn)
                    {
                        await this.Program.OnLogin();
                    }
                    else
                    {
                        await this.Program.OnLogout();
                    }
                }

                this.Ribbon.UserLabel = isLoggedIn ? this.Client.UserName : "Not logged in";
                this.Ribbon.AuthenticationLabel = isLoggedIn ? "Logoff" : "Login";
            }
        }

        private void Logoff()
        {
            this.Client.IsLoggedIn = false;
            this.Database.HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        private async Task Login(RemoteDatabase database)
        {
            try
            {
                if (!this.Client.IsLoggedIn)
                {
                    var autoLogin = this.Configuration.AutoLogin;

                    if (!string.IsNullOrWhiteSpace(autoLogin))
                    {
                        var user = this.Configuration.AutoLogin;
                        var uri = new Uri("TestAuthentication/Token", UriKind.Relative);
                        this.Client.IsLoggedIn = await database.Login(uri, user, null);
                        if (this.Client.IsLoggedIn)
                        {
                            this.Client.UserName = user;
                        }
                    }
                    else
                    {
                        // Check if there is some sort of automated login
                        var uri = new Uri("/Ping/Token", UriKind.Relative);

                        HttpResponseMessage response = null;
                        try
                        {
                            response = await database.PostAsJsonAsync(uri, null);
                            if (response.IsSuccessStatusCode)
                            {
                                this.Client.IsLoggedIn = true;
                            }
                        }
                        catch (Exception)
                        {
                            // Catch the Unauthorized exception only
                            if (response?.StatusCode != HttpStatusCode.Unauthorized)
                            {
                                throw;
                            }
                        }

                        if (!this.Client.IsLoggedIn)
                        {
                            using var loginForm = new LoginForm
                            {
                                Database = database,
                                Uri = new Uri("TestAuthentication/Token", UriKind.Relative)
                            };

                            var result = loginForm.ShowDialog();

                            if (result == DialogResult.OK)
                            {
                                this.Client.UserName = loginForm.UserName;
                                this.Client.IsLoggedIn = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }
    }
}
