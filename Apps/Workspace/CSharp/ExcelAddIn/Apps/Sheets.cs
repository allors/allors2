﻿using System.Linq;
using Allors.Excel.Customers;
using Allors.Excel.InventoryItems;
using Allors.Excel.PurchaseInvoices;
using Allors.Excel.Relations.CustomersOverdue;
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

            AddCustomStyles();

            worksheet.SetCustomPropertyValue("AllorsType", nameof(PurchaseInvoicesSheet));

            var sheet = new PurchaseInvoicesSheet(this, worksheet);
            sheet.Workbook = this.Host.Application.ActiveWorkbook;

            this.SheetByVstoWorksheet[worksheet] = sheet;
            return sheet;
        }

        public CustomersSheet CreateCustomers()
        {
            var worksheet = this.Host.ActiveWorksheet;

            AddCustomStyles();

            worksheet.SetCustomPropertyValue("AllorsType", nameof(CustomersSheet));

            var sheet = new CustomersSheet(this, worksheet);
            this.SheetByVstoWorksheet[worksheet] = sheet;
            return sheet;
        }

        public InventoryItemsSheet CreateInventoryItems()
        {
            var worksheet = this.Host.ActiveWorksheet;

            AddCustomStyles();

            worksheet.SetCustomPropertyValue("AllorsType", nameof(InventoryItemsSheet));

            var sheet = new InventoryItemsSheet(this, worksheet);
            this.SheetByVstoWorksheet[worksheet] = sheet;
            return sheet;
        }
        
        public SalesInvoicesOverdueSheet CreateSalesInvoicesOverdue()
        {
            var worksheet = this.Host.ActiveWorksheet;

            AddCustomStyles();

            worksheet.SetCustomPropertyValue("AllorsType", nameof(SalesInvoicesOverdueSheet));

            var sheet = new SalesInvoicesOverdueSheet(this, worksheet);
            this.SheetByVstoWorksheet[worksheet] = sheet;
            return sheet;
        }

        private void AddCustomStyles()
        {
            if (this.Host.ActiveWorkbook.Styles.Cast<Style>().All(v => v.Name != "headerStyle"))
            {
                Style style = this.Host.ActiveWorkbook.Styles.Add("headerStyle");
                style.Font.Name = "Arial";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
                style.Interior.Pattern = XlPattern.xlPatternSolid;
            }
          
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

                case nameof(CustomersSheet):
                    return new CustomersSheet(this, worksheet);

                case nameof(SalesInvoicesOverdueSheet):
                    return new SalesInvoicesOverdueSheet(this, worksheet);
            }

            return null;
        }
    }
}