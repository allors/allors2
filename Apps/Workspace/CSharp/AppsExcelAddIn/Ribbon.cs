using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace AppsExcelAddIn
{
    using Allors.Excel;

    using Nito.AsyncEx;

    public partial class Ribbon
    {
        private Commands Commands { get; set; }

        private Sheets Sheets { get; set; }

        public void Init(Commands commands, Sheets sheets, Mediator mediator)
        {
            this.Sheets = sheets;
            this.Commands = commands;

            mediator.StateChanged += this.MediatorOnStateChanged;
        }

        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void saveButton_Click(object sender, RibbonControlEventArgs eventArgs)
        {
            try
            {
                AsyncContext.Run(
                    async () =>
                        {
                            if (this.Commands != null)
                            {
                                await this.Commands.Save();
                            }
                        });
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        private void refreshButton_Click(object sender, RibbonControlEventArgs eventArgs)
        {
            try
            {
                AsyncContext.Run(
                    async () =>
                        {
                            if (this.Commands != null)
                            {
                                await this.Commands.Refresh();
                            }
                        });
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        private void peopleInitializeButton_Click(object sender, RibbonControlEventArgs eventArgs)
        {
            try
            {
                AsyncContext.Run(
                    async () =>
                        {
                            if (this.Commands != null)
                            {
                                await this.Commands.PeopleNew();
                            }
                        });
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }

        private void MediatorOnStateChanged(object sender, EventArgs eventArgs)
        {
            var sheet = this.Sheets.ActiveSheet;
            var existSheet = sheet != null;

            this.saveButton.Enabled = existSheet && this.Commands.CanSave;
            this.refreshButton.Enabled = existSheet && this.Commands.CanRefresh;

            this.peopleInitializeButton.Enabled = this.Commands.CanNew;
        }

        private void PurchaseInvoicesInitializeButton_Click(object sender, RibbonControlEventArgs eventArgse)
        {
            try
            {
                AsyncContext.Run(
                    async () =>
                    {
                        if (this.Commands != null)
                        {
                            await this.Commands.PurchaseInvoicesNew();
                        }
                    });
            }
            catch (Exception e)
            {
                e.Handle();
            }
        }
    }
}
