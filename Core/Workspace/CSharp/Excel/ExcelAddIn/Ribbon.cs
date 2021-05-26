namespace ExcelAddIn
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Office = Microsoft.Office.Core;
    using Allors.Excel;
    using Allors.Excel.Interop;
    using Microsoft.Office.Core;

    [ComVisible(true)]
    public class Ribbon : IRibbonExtensibility
    {
        private IRibbonUI ribbon;
        private string userLabel = "Not logged in";
        private string authenticationLabel = "Log in";

        public AddIn AddIn { get; set; }

        public Authentication Authentication { get; set; }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("ExcelAddIn.Ribbon.xml");
        }

        #endregion

        #region Ribbon Labels

        public string UserLabel
        {
            get => userLabel;
            set
            {
                userLabel = value;
                this.ribbon.Invalidate();
            }
        }

        public string AuthenticationLabel
        {
            get => authenticationLabel;
            set
            {
                authenticationLabel = value;
                this.ribbon.Invalidate();
            }
        }

        public string GetUserLabel(IRibbonControl control)
        {
            return this.UserLabel;
        }

        public string GetAuthenticateLabel(IRibbonControl control)
        {
            return this.AuthenticationLabel;
        }

        #endregion

        #region Ribbon Callbacks

        public async void OnAuthenticate(IRibbonControl control)
        {
            try
            {
                await this.Authentication.Switch();
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

        public void Ribbon_Load(IRibbonUI ribbonUI) => this.ribbon = ribbonUI;

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
