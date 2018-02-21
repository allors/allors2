namespace Allors.Excel
{
    using Microsoft.Office.Interop.Excel;
    using Microsoft.Office.Tools;
    using Microsoft.Office.Tools.Excel;

    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;
    using Workbook = Microsoft.Office.Tools.Excel.Workbook;

    public partial class Host
    {
        public Host(Application application, CustomTaskPaneCollection customTaskPanes, ApplicationFactory applicationFactory)
        {
            this.Application = application;
            this.CustomTaskPanes = customTaskPanes;
            this.ApplicationFactory = applicationFactory;
        }

        public Application Application { get; }

        public CustomTaskPaneCollection CustomTaskPanes { get; }

        public ApplicationFactory ApplicationFactory { get; }

        public Worksheet ActiveWorksheet => (Worksheet)this.ApplicationFactory.GetVstoObject(this.Application.ActiveWorkbook.ActiveSheet);

        public Workbook ActiveWorkbook => (Workbook)this.ApplicationFactory.GetVstoObject(this.Application.ActiveWorkbook);
    }
}
