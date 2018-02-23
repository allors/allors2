namespace ExcelAddIn
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Client;

    using Microsoft.Office.Tools;
    using Microsoft.Office.Tools.Excel;

    using NLog;

    using Application = Microsoft.Office.Interop.Excel.Application;

    public partial class AddInManager
    {
        private const string AllorsDatabaseAddressKey = "allors.database.address";
        private const string EnvironmentKey = "environment";
        private const string UserKey = "user";

        private readonly Application application;
        private readonly CustomTaskPaneCollection customTaskPanes;
        private readonly ApplicationFactory factory;

        public AddInManager(Application application, CustomTaskPaneCollection customTaskPanes, ApplicationFactory factory)
        {
            this.application = application;
            this.customTaskPanes = customTaskPanes;
            this.factory = factory;
        }

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public async Task Init()
        {
            try
            {
                // Make sure we have a SynchroniztionContext
                var form = new Form();

                var httpClientHandler = new HttpClientHandler { UseDefaultCredentials = true };
                var httpClient = new HttpClient(httpClientHandler)
                        {
                            BaseAddress = new Uri(ConfigurationManager.AppSettings[AllorsDatabaseAddressKey]),
                        };

                var database = new Database(httpClient);
                var workspace = new Workspace(Config.ObjectFactory);
                var client = new Client(database, workspace);

                var host = new Host(this.application, this.customTaskPanes, this.factory);

                var mediator = new Mediator();
                var sheets = new Sheets(host, client, mediator);
                var commands = new Commands(sheets);

                this.application.WindowActivate += (wb, wn) => mediator.OnStateChanged();
                this.application.WorkbookOpen += wb => mediator.OnStateChanged();
                this.application.WorkbookActivate += wb => mediator.OnStateChanged();
                this.application.WorkbookNewSheet += (wb, sh) => mediator.OnStateChanged();
                this.application.SheetActivate += sh => mediator.OnStateChanged();

                Globals.Ribbons.Ribbon.Init(commands, sheets, mediator);

                await this.Login(database);
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        private async Task Login(Database database)
        {
            try
            {
                var environment = ConfigurationManager.AppSettings[EnvironmentKey];
                var production = environment?.ToLower().Equals("prod") ?? false;

                if (!production)
                {
                    var user = ConfigurationManager.AppSettings[UserKey] ?? @"administrator";
                    var uri = new Uri("/TestAuthentication/Token", UriKind.Relative);
                    var request = new { UserName = user, Password = string.Empty };
                    using (var response = await database.PostAsJsonAsync(uri, request))
                    {
                        response.EnsureSuccessStatusCode();
                        var authResult = await database.ReadAsAsync<AuthenticationResult>(response);
                        if (!authResult.Authenticated)
                        {
                            throw new Exception("Not authenticated");
                        }

                        database.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);
                    }
                }
            }
            catch (Exception e)
            {
                this.Logger.Error(e);
            }
        }
    }

    public class AuthenticationResult
    {
        public bool Authenticated { get; set; }

        public string Token { get; set; }
    }
}
