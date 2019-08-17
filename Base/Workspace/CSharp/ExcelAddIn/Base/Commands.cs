// <copyright file="Commands.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using ExcelAddIn;
    using Microsoft.Office.Interop.Excel;
    using System;
    using System.Threading.Tasks;

    public partial class Commands
    {
        private void OnError(Exception e) => e.Handle();

        public async Task PeopleNew()
        {
            EnsureEmptyWorksheet();

            var sheet = this.Sheets.CreatePeople();
            await sheet.Refresh();
        }

        public async Task PurchaseInvoicesNew()
        {
            EnsureEmptyWorksheet();

            var sheet = this.Sheets.CreatePurchaseInvoices();
            await sheet.Refresh();
        }

        private static void EnsureEmptyWorksheet()
        {
            if (Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet != null)
            {
                // If the active worksheet is empty, we can use it, else add a new workbook (that becomes the active one)
                Worksheet worksheet = Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet;
                if (Globals.ThisAddIn.Application.WorksheetFunction.CountA(worksheet.Cells) > 0)
                {
                    Globals.ThisAddIn.Application.Workbooks.Add();
                }
            }
        }

        public async Task CustomersNew()
        {
            EnsureEmptyWorksheet();

            var sheet = this.Sheets.CreateCustomers();
            await sheet.Refresh();
        }

        public async Task SalesInvoicesOverdueNew()
        {
            EnsureEmptyWorksheet();

            var sheet = this.Sheets.CreateSalesInvoicesOverdue();
            await sheet.Refresh();
        }
    }
}
