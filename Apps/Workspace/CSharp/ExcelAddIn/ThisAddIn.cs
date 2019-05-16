using System;
using System.Threading.Tasks;
using Allors.Excel;
using NLog;
using Office = Microsoft.Office.Core;

namespace ExcelAddIn
{
    public partial class ThisAddIn
    {
        public AddInManager AddInManager { get; private set; }

        public Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public async void InitAddInManager()
        {
            try
            {
                if (this.AddInManager == null)
                {
                    this.AddInManager = new AddInManager(this.Application, this.CustomTaskPanes, Globals.Factory);
                }

                await this.AddInManager.Init();
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        private async void ThisAddIn_Startup(object sender, System.EventArgs @event)
        {
           
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

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
