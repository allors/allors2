namespace Allors.Excel
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;

    using Microsoft.Office.Tools;
    using Microsoft.Office.Tools.Excel;

    using Application = Microsoft.Office.Interop.Excel.Application;
    using ListObject = Microsoft.Office.Tools.Excel.ListObject;
    using Workbook = Microsoft.Office.Tools.Excel.Workbook;
    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public partial class Host
    {
        public Host(Application application, CustomTaskPaneCollection customTaskPanes, ApplicationFactory applicationFactory)
        {
            this.Application = application;
            this.CustomTaskPanes = customTaskPanes;
            this.ApplicationFactory = applicationFactory;

            this.vstoWorksheetByInteropWorksheet = new Dictionary<Microsoft.Office.Interop.Excel.Worksheet, Worksheet>();

            this.Application.SheetActivate += (obj) =>
                {
                    this.ActivateWorksheet((Microsoft.Office.Interop.Excel.Worksheet)obj);
                };

            this.Application.WorkbookActivate += (obj) =>
                {
                    var interopWorkbook = (Microsoft.Office.Interop.Excel.Workbook)obj;
                    this.ActiveWorkbook = this.ApplicationFactory.GetVstoObject(interopWorkbook);

                    this.ActivateWorksheet(interopWorkbook.ActiveSheet);
                };

            this.Application.WorkbookNewSheet += (wb, sh) =>
                {
                    this.ActivateWorksheet((Microsoft.Office.Interop.Excel.Worksheet)sh);
                };

            // TODO: sheet.DeActivate
        }

        private Dictionary<Microsoft.Office.Interop.Excel.Worksheet, Microsoft.Office.Tools.Excel.Worksheet> vstoWorksheetByInteropWorksheet { get; }

        public Application Application { get; }

        public CustomTaskPaneCollection CustomTaskPanes { get; }

        public Worksheet ActiveWorksheet { get; private set; }

        public Workbook ActiveWorkbook { get; private set; }

        private ApplicationFactory ApplicationFactory { get; }
        public Worksheet GetVstoWorksheet(Microsoft.Office.Interop.Excel.Worksheet sheet)
        {
            this.vstoWorksheetByInteropWorksheet.TryGetValue(sheet, out var vstoWorksheet);
            return vstoWorksheet;
        }

        public ListObject GetVstoListObject(Microsoft.Office.Interop.Excel.ListObject interopListObject)
        {
            return this.ApplicationFactory.GetVstoObject(interopListObject);
        }

        private void ActivateWorksheet(Microsoft.Office.Interop.Excel.Worksheet interopWorksheet)
        {
            var vstoWorksheet = this.ApplicationFactory.GetVstoObject(interopWorksheet);
            this.vstoWorksheetByInteropWorksheet[interopWorksheet] = vstoWorksheet;

            this.ActiveWorksheet = vstoWorksheet;
        }

        public void EnsureSynchronizationContext()
        {
            if (SynchronizationContext.Current == null)
            {
                var form = new Form();
            }
        }
    }
}
