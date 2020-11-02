using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Allors.Excel;
using Allors.Workspace.Remote;
using NLog;

namespace ExcelAddIn
{
    public class Authentication
    {
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Authentication(Ribbon ribbon, RemoteDatabase database, Client client, Configuration configuration)
        {
            this.Ribbon = ribbon;
            this.Database = database;
            this.Client = client;
            this.Configuration = configuration;
        }

        public Ribbon Ribbon { get; }

        public RemoteDatabase Database { get; }

        public Client Client { get; }

        public Configuration Configuration { get; }

        public async Task Switch()
        {
            if (this.Client != null)
            {
                if (this.Client.IsLoggedIn)
                {
                    await this.Logoff();
                }
                else
                {
                    await this.Login();
                }

                var loggedIn = this.Client.IsLoggedIn;

                this.Ribbon.UserLabel = loggedIn ? this.Client.UserName : "Not logged in";
                this.Ribbon.AuthenticationLabel = loggedIn ? "Logoff" : "Login";
            }
        }

        public async Task Logoff()
        {
            this.Client.IsLoggedIn = false;
            this.Database.HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task Login() => await this.Login(this.Database);

        private async Task Login(RemoteDatabase database)
        {
            try
            {
                if (!this.Client.IsLoggedIn)
                {
                    var autologin = this.Configuration.AutoLogin;
 
                    if (!string.IsNullOrWhiteSpace(autologin))
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
                            using (var loginForm = new LoginForm())
                            {
                                loginForm.Database = database;
                                loginForm.Uri = new Uri("TestAuthentication/Token", UriKind.Relative);
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
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }
    }
}
