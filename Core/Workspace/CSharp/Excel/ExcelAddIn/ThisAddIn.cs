namespace ExcelAddIn
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Windows.Forms;
    using Allors.Excel;
    using Allors.Excel.Interop;
    using Allors.Workspace;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Remote;
    using Application;
    using ObjectFactory = Allors.Workspace.ObjectFactory;

    public partial class ThisAddIn
    {
        private RemoteDatabase database;

        private async void ThisAddIn_Startup(object sender, EventArgs e)
        {
            var configuration = new AppConfig();

            var httpClientHandler = new HttpClientHandler { UseDefaultCredentials = true };
            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(configuration.AllorsDatabaseAddress),
            };

            var serviceProvider = new ServiceLocator();

            this.database = new RemoteDatabase(httpClient);
            var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
            var workspace = new Workspace(objectFactory);
            this.Client = new Client(this.database, workspace);
            var program = new Program(serviceProvider, this.Client);
            var office = new OfficeCore();

            this.AddIn = new AddIn(this.Application, program, office);
            this.Ribbon.AddIn = this.AddIn;
            this.Ribbon.Authentication = new Authentication(this.Ribbon, program, this.database, this.Client, configuration);
            await program.OnStart(this.AddIn);
        }

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

        private async void ThisAddIn_Shutdown(object sender, EventArgs e) => await this.AddIn.Program.OnStop();


        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new EventHandler(this.ThisAddIn_Startup);
            this.Shutdown += new EventHandler(this.ThisAddIn_Shutdown);
        }

        #endregion
    }
}
