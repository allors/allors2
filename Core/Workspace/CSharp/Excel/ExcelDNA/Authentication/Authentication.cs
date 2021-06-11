namespace ExcelDNA
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
        public Authentication(Program program, RemoteDatabase database, Client client, AppConfig configuration)
        {
            this.Program = program;
            this.Database = database;
            this.Client = client;
            this.Configuration = configuration;
        }

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
            }
        }

        private void Logoff()
        {
            this.Client.IsLoggedIn = false;
            this.Database.Logoff();
        }

        private async Task Login(RemoteDatabase database)
        {
            try
            {
                if (!this.Client.IsLoggedIn)
                {
                    var autoLogin = this.Configuration.AutoLogin;
                    var authenticationTokenUrl = this.Configuration.AllorsAuthenticationTokenUrl;

                    if (!string.IsNullOrWhiteSpace(autoLogin))
                    {
                        var user = this.Configuration.AutoLogin;
                        var uri = new Uri(authenticationTokenUrl, UriKind.Relative);
                        this.Client.IsLoggedIn = await database.Login(uri, user, null);
                        if (this.Client.IsLoggedIn)
                        {
                            this.Client.UserName = user;
                        }
                    }
                    else
                    {
                        if (!this.Client.IsLoggedIn)
                        {
                            using var loginForm = new LoginForm
                            {
                                Database = database,
                                Uri = new Uri(authenticationTokenUrl, UriKind.Relative)
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
