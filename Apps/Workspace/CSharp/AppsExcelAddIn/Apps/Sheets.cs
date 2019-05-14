using Allors.Excel.PurchaseInvoices;
using Microsoft.Office.Interop.Excel;

namespace Allors.Excel
{
    using Allors.Excel.People;

    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public partial class Sheets
    {
        public PeopleSheet CreatePeople()
        {
            var worksheet = this.Host.ActiveWorksheet;
            worksheet.SetCustomPropertyValue("AllorsType", nameof(PeopleSheet));

            var sheet = new PeopleSheet(this, worksheet);
            this.SheetByVstoWorksheet[worksheet] = sheet;
            return sheet;
        }

        public PurchaseInvoicesSheet CreatePurchaseInvoices()
        {
            var worksheet = this.Host.ActiveWorksheet;

            Style style = this.Host.ActiveWorkbook.Styles.Add("headerStyle");
            style.Font.Name = "Arial";
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
            style.Interior.Pattern = XlPattern.xlPatternSolid;

            worksheet.SetCustomPropertyValue("AllorsType", nameof(PurchaseInvoicesSheet));

            var sheet = new PurchaseInvoicesSheet(this, worksheet);
            this.SheetByVstoWorksheet[worksheet] = sheet;
            return sheet;
        }

        private Sheet Instantiate(Worksheet worksheet)
        {
            var allorsType = (string)worksheet.GetCustomPropertyValue("AllorsType");

            switch (allorsType)
            {
                case nameof(PeopleSheet):
                    return new PeopleSheet(this, worksheet);

                case nameof(PurchaseInvoicesSheet):
                    return new PurchaseInvoicesSheet(this, worksheet);
            }

            return null;
        }
    }
}
