namespace ExcelAddIn
{
    using System;
    using System.Configuration;
    using System.Net.Http;

    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Client;

    public partial class ExcelAddIn
    {
        private const string AllorsDatabaseAddressKey = "allors.database.address";

        private const string ProductionKey = "production";

        private void ExcelAddInStartup(object sender, EventArgs e)
        {
            var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true })
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings[AllorsDatabaseAddressKey]),
            };

            var database = new Database(httpClient);
            var workspace = new Workspace(Config.ObjectFactory);
            var client = new Client(database, workspace);

            var host = new Host(this.Application, this.CustomTaskPanes, Globals.Factory);

            var mediator = new Mediator();
            var sheets = new Sheets(host, client, mediator);
            var commands = new Commands(sheets);

            this.Application.WindowActivate += (wb, wn) => mediator.OnStateChanged();
            this.Application.WorkbookOpen += wb => mediator.OnStateChanged();
            this.Application.WorkbookActivate += wb => mediator.OnStateChanged();
            this.Application.WorkbookNewSheet += (wb, sh) => mediator.OnStateChanged();
            this.Application.SheetActivate += sh => mediator.OnStateChanged();

            this.Login(database);
        }

        private void ExcelAddInShutdown(object sender, EventArgs e)
        {
        }

        private async void Login(Database database)
        {
            try
            {
                var production = bool.Parse(ConfigurationManager.AppSettings[ProductionKey] ?? "True");

                if (!production)
                {
                    await database.HttpClient.GetAsync("/local/login");
                }
            }
            catch
            {
            }
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(this.ExcelAddInStartup);
            this.Shutdown += new System.EventHandler(this.ExcelAddInShutdown);
        }
        
        #endregion
    }
}
