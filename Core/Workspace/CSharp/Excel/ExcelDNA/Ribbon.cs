using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;
using InteropApplication = Microsoft.Office.Interop.Excel.Application;

namespace ExcelDNA
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    using Allors.Excel;
    using Allors.Excel.Interop;
    using Allors.Workspace;
    using Allors.Workspace.Domain;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Remote;
    using Application;
    using ExcelDna.Integration;
    using RestSharp;
    using RestSharp.Serializers.NewtonsoftJson;

    [ComVisible(true)]
    public class Ribbon : ExcelRibbon
    {
        private RemoteDatabase database;

        public IRibbonUI RibbonUI { get; private set; }

        public Client Client { get; private set; }

        public AddIn AddIn { get; private set; }

        public Program Program { get; private set; }

        public Authentication Authentication { get; private set; }

        public AppConfig AppConfig { get; private set; }

        public override string GetCustomUI(string _)
        {
            try
            {
                SynchronizationContext windowsFormsSynchronizationContext = new WindowsFormsSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(windowsFormsSynchronizationContext);

                this.AppConfig = new AppConfig();

                var restClient = new RestClient(this.AppConfig.AllorsDatabaseAddress).UseNewtonsoftJson();
                this.database = new RemoteDatabase(restClient);

                var objectFactory = new ObjectFactory(MetaPopulation.Instance, typeof(User));
                var workspace = new Workspace(objectFactory);
                this.Client = new Client(this.database, workspace);
                this.Program = new Program(new ServiceLocator(), this.Client);
                this.AddIn = new AddIn((InteropApplication)ExcelDnaUtil.Application, this.Program);
                this.Authentication = new Authentication(this.Program, this.database, this.Client, this.AppConfig);

                return RibbonResources.Ribbon;
            }
            catch (Exception e)
            {
                e.Handle();
                throw;
            }
        }

        public async void OnLoad(IRibbonUI ribbon)
        {
            this.RibbonUI = ribbon;

            try
            {
                await this.Program.OnStart(this.AddIn);
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        #region Ribbon Labels

        public string UserLabel { get; set; } = "Not logged in";

        public string AuthenticationLabel { get; set; } = "Log in";


        public string GetUserLabel(IRibbonControl control) => this.UserLabel;

        public string GetAuthenticateLabel(IRibbonControl control) => this.AuthenticationLabel;

        #endregion

        #region Ribbon Callbacks

        public async void OnAuthenticate(IRibbonControl _)
        {
            try
            {
                await this.Authentication.Switch();

                this.UserLabel = this.Client.IsLoggedIn ? this.Client.UserName : "Not logged in";
                this.AuthenticationLabel = this.Client.IsLoggedIn ? "Logoff" : "Login";
                this.RibbonUI.Invalidate();

            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        public async void OnClick(IRibbonControl control)
        {
            if (this.AddIn == null)
            {
                return;
            }

            try
            {
                await this.AddIn.Program.OnHandle(control.Id);
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        public bool GetEnabled(IRibbonControl control) => this.AddIn.Program.IsEnabled(control.Id, control.Tag);

        #endregion

        #region Ribbon Helpers
        public override object LoadImage(string imageId)
        {
            // This will return the image resource with the name specified in the image='xxxx' tag
            return RibbonResources.ResourceManager.GetObject(imageId);
        }
        #endregion
    }
}
