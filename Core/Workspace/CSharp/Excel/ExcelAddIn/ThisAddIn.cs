namespace ExcelAddIn
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Windows.Forms;
    using Allors.Excel;
    using Allors.Workspace;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Remote;
    using Application;
    using Allors.Excel.Embedded;
    using Microsoft.Extensions.DependencyInjection;
    using Nito.AsyncEx;
    using ObjectFactory = Allors.Workspace.ObjectFactory;

    public partial class ThisAddIn
    {
        private RemoteDatabase database;

        private void ThisAddIn_Startup(object sender, System.EventArgs e) => AsyncContext.Run(async () =>
        {
            var configuration = new Configuration();

            var httpClientHandler = new HttpClientHandler { UseDefaultCredentials = true };
            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(configuration.AllorsDatabaseAddress),
            };

            var serviceProvider = new ServiceCollection()
                // TODO: use DI logging
                //.AddLogging()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<IErrorService, ErrorService>()
                .BuildServiceProvider();

            this.database = new RemoteDatabase(httpClient);
            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
            var workspace = new Workspace(objectFactory);
            this.Client = new Client(this.database, workspace);
            var program = new Program(serviceProvider, this.Client);

            this.AddIn = new AddIn(this.Application, program);
            this.Ribbon.AddIn = this.AddIn;
            this.Ribbon.Authentication = new Authentication(this.Ribbon, database, this.Client, configuration);
            await program.OnStart(this.AddIn);
        });

        public Ribbon Ribbon { get; set; }

        public AddIn AddIn { get; private set; }

        public Client Client { get; private set; }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            SynchronizationContext windowsFormsSynchronizationContext = new WindowsFormsSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(windowsFormsSynchronizationContext);

            this.Ribbon = new Ribbon();
            return this.Ribbon;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e) => AsyncContext.Run(async () =>
        {
            await this.AddIn.Program.OnStop();
        });

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
