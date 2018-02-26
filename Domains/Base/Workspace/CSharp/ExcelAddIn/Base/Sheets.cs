namespace Allors.Excel
{
    using System;
    using System.Collections.Generic;

    using NLog;

    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public partial class Sheets
    {
        public Sheets(Host host, Client client, Mediator mediator)
        {
            this.Host = host;
            this.Client = client;
            this.Mediator = mediator;

            this.SheetByVstoWorksheet = new Dictionary<Worksheet, Sheet>();

            this.Host.Application.SheetActivate += (obj) =>
                {
                    var interopWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)obj;
                    var vstoWorksheet = this.Host.GetVstoWorksheet(interopWorksheet);
                    var sheet = this.Instantiate(vstoWorksheet);
                    if (sheet != null)
                    {
                        this.SheetByVstoWorksheet[vstoWorksheet] = sheet;
                    }
                };

            this.Mediator.StateChanged += this.MediatorOnStateChanged;
        }

        public Host Host { get; }

        public Client Client { get; }

        public Mediator Mediator { get; }

        public Sheet ActiveSheet => this[this.Host.ActiveWorksheet];

        public Dictionary<Worksheet, Sheet> SheetByVstoWorksheet { get; }

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public Sheet this[Worksheet worksheet]
        {
            get
            {
                if (worksheet == null)
                {
                    return null;
                }

                this.SheetByVstoWorksheet.TryGetValue(worksheet, out var sheet);
                return sheet;
            }
        }

        private void MediatorOnStateChanged(object sender, EventArgs eventArgs)
        {
            foreach (var sheet in this.SheetByVstoWorksheet.Values)
            {
                sheet.MediatorOnStateChanged();
            }
        }
    }
}
