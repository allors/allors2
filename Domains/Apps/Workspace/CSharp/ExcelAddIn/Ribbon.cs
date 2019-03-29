namespace ExcelAddIn
{
    using System;

    using Allors.Excel;

    using Microsoft.Office.Tools.Ribbon;

    public partial class Ribbon
    {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private Commands Commands { get; set; }

        private Sheets Sheets { get; set; }

        public void Init(Commands commands, Sheets sheets, Mediator mediator)
        {
            this.Sheets = sheets;
            this.Commands = commands;

            mediator.StateChanged += this.MediatorOnStateChanged;
        }

        private void SaveButtonClick(object sender, RibbonControlEventArgs e)
        {
            this.Commands?.Save();
        }

        private void RefreshButtonClick(object sender, RibbonControlEventArgs e)
        {
            this.Commands?.Refresh();
        }

        private void PeopleInitializeButtonClick(object sender, RibbonControlEventArgs e)
        {
            this.Commands?.PeopleNew();
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
