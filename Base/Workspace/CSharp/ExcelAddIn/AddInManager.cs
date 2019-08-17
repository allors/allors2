// <copyright file="AddInManager.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ExcelAddIn
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Client;
    using BaseExcelAddIn.Base;
    using Microsoft.Office.Interop.Excel;
    using Microsoft.Office.Tools;
    using Microsoft.Office.Tools.Excel;
    using NLog;
    using Application = Microsoft.Office.Interop.Excel.Application;
    using Sheets = Allors.Excel.Sheets;
    using Workbook = Microsoft.Office.Interop.Excel.Workbook;

    public partial class AddInManager
    {
        private const string AllorsDatabaseAddressKey = "allors.database.address";
        private const string EnvironmentKey = "environment";
        private const string UserKey = "user";

        private readonly Application application;
        private readonly CustomTaskPaneCollection customTaskPanes;
        private readonly ApplicationFactory factory;

        private Database database;
        private Workspace workspace;
        private Client client;

        private Host host;

        private Mediator mediator;
        private Sheets sheets;
        private Commands commands;

        public AddInManager(Application application, CustomTaskPaneCollection customTaskPanes, ApplicationFactory factory)
        {
            this.application = application;
            this.customTaskPanes = customTaskPanes;
            this.factory = factory;
        }

        public bool IsLoggedIn { get; set; }

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public async Task Init()
        {
            try
            {
                if (this.database == null)
                {
                    // Make sure we have a SynchronizationContext
                    var form = new Form();

                    var httpClientHandler = new HttpClientHandler { UseDefaultCredentials = true };
                    var httpClient = new HttpClient(httpClientHandler)
                    {
                        BaseAddress = new Uri(ConfigurationManager.AppSettings[AllorsDatabaseAddressKey]),
                    };

                    this.database = new Database(httpClient);
                    this.workspace = new Workspace(Config.ObjectFactory);
                    this.client = new Client(this.database, this.workspace);

                    this.host = new Host(this.application, this.customTaskPanes, this.factory);

                    this.mediator = new Mediator();
                    this.sheets = new Sheets(this.host, this.client, this.mediator);
                    this.commands = new Commands(this.sheets);

                    this.application.WindowActivate += this.ApplicationOnWindowActivate;
                    this.application.WorkbookOpen += this.ApplicationOnWorkbookOpen;
                    this.application.WorkbookBeforeClose += this.ApplicationOnWorkbookBeforeClose;
                    this.application.WorkbookActivate += this.ApplicationOnWorkbookActivate;
                    this.application.WorkbookNewSheet += this.ApplicationOnWorkbookNewSheet;
                    this.application.SheetActivate += this.ApplicationOnSheetActivate;

                    Globals.Ribbons.Ribbon.Init(this.commands, this.sheets, this.mediator);
                }

                // TODO: Make this lazy
                await this.Login(this.database);
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        public void Logoff()
        {
            this.IsLoggedIn = false;
            this.database.HttpClient.DefaultRequestHeaders.Authorization = null;
        }

        private async Task Login(Database database)
        {
            try
            {
                if (!this.IsLoggedIn)
                {
                    var environment = ConfigurationManager.AppSettings[EnvironmentKey];
                    var production = environment?.ToLower().Equals("prod") ?? false;

                    if (!production)
                    {
                        var user = ConfigurationManager.AppSettings[UserKey] ?? @"administrator";
                        var uri = new Uri("/TestAuthentication/Token", UriKind.Relative);
                        this.IsLoggedIn = await database.Login(uri, user, null);
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
                                this.IsLoggedIn = true;
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

                        if (!this.IsLoggedIn)
                        {
                            using (var loginForm = new LoginForm())
                            {
                                loginForm.Database = database;
                                loginForm.Uri = new Uri("/TestAuthentication/Token", UriKind.Relative);
                                var result = loginForm.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    this.IsLoggedIn = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.Logger.Error(e);
            }
        }

        private void ApplicationOnSheetActivate(object sheet) => this.mediator.OnStateChanged();

        private void ApplicationOnWorkbookNewSheet(Workbook workbook, object sheet) => this.mediator.OnStateChanged();

        private void ApplicationOnWorkbookActivate(Workbook workbook) => this.mediator.OnStateChanged();

        private void ApplicationOnWorkbookOpen(Workbook workbook) => this.mediator.OnStateChanged();

        private void ApplicationOnWorkbookBeforeClose(Workbook workbook, ref bool cancel)
        {
            // this.mediator.OnStateChanged();

            // var populationXml = this.workspace.Save();
            // var xmlPart = workbook.CustomXMLParts.Add(populationXml);
        }

        private void ApplicationOnWindowActivate(Workbook wb, Window wn) => this.mediator.OnStateChanged();
    }
}
