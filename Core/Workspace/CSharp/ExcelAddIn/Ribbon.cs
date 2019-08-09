namespace ExcelAddIn
{
    using System;

    using Allors.Excel;

    using Microsoft.Office.Tools.Ribbon;

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

        private void Ribbon_Load(object sender, RibbonUIEventArgs eventArgs)
        {
        }
        
        private void SaveButtonClick(object sender, RibbonControlEventArgs eventArgs)
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

        private void RefreshButtonClick(object sender, RibbonControlEventArgs eventArgs)
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

        private void PeopleInitializeButtonClick(object sender, RibbonControlEventArgs eventArgs)
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
    }
}
