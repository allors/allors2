namespace Allors.Excel
{
    using System;
    using System.Collections.Generic;

    using NLog;

    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public partial class Sheets
    {
        private readonly Dictionary<Microsoft.Office.Interop.Excel.Worksheet, Sheet> sheetByWorksheet;
        
        public Sheets(Host host, Client client, Mediator mediator)
        {
            this.Host = host;
            this.Client = client;
            this.Mediator = mediator;

            this.sheetByWorksheet = new Dictionary<Microsoft.Office.Interop.Excel.Worksheet, Sheet>();

            this.Mediator.StateChanged += this.MediatorOnStateChanged;
        }

        public Host Host { get; }

        public Client Client { get; }

        public Mediator Mediator { get; }

        public Sheet ActiveSheet => this[this.Host.ActiveWorksheet];

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected Dictionary<Microsoft.Office.Interop.Excel.Worksheet, Sheet> SheetByWorksheet
        {
            get
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet interopWorksheet in this.Host.Application.Worksheets)
                {
                    if (!this.sheetByWorksheet.ContainsKey(interopWorksheet))
                    {
                        var worksheet = this.Host.ApplicationFactory.GetVstoObject(interopWorksheet);
                        var sheet = this.Instantiate(worksheet);
                        if (sheet != null)
                        {
                            this.sheetByWorksheet[interopWorksheet] = sheet;
                        }
                    }
                }

                return this.sheetByWorksheet;
            }
        }

        public Sheet this[Worksheet worksheet]
        {
            get
            {
                Sheet sheet;
                this.SheetByWorksheet.TryGetValue(worksheet.InnerObject, out sheet);
                return sheet;
            }
        }

        private void MediatorOnStateChanged(object sender, EventArgs eventArgs)
        {
            foreach (var sheet in this.SheetByWorksheet.Values)
            {
                sheet.MediatorOnStateChanged();
            }
        }
    }
}
