// <copyright file="ThisAddIn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace ExcelAddIn
{
    using System;
    using Allors.Excel;
    using NLog;

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

        private async void ThisAddIn_Startup(object sender, EventArgs @event)
        {

        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
        }

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