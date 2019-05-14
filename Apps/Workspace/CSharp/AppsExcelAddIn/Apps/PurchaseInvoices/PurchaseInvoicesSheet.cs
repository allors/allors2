using System.Runtime.InteropServices;
using Allors.Workspace.Meta;
using AppsExcelAddIn.Apps.Extensions;
using Workspace.Domain.Apps;

namespace Allors.Excel.PurchaseInvoices
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using Allors.Excel;
    using Allors.Protocol.Remote;
    using Allors.Workspace.Data;
    using Allors.Workspace.Domain;

    using Microsoft.Office.Interop.Excel;

    using ListObject = Microsoft.Office.Tools.Excel.ListObject;
    using Result = Allors.Workspace.Client.Result;
    using Sheets = Allors.Excel.Sheets;
    using Task = System.Threading.Tasks.Task;
    using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

    public class PurchaseInvoicesSheet : Sheet
    {
        private const string PurchaseInvoicesListObjectName = "PurchaseInvoicesListObject";
        private AppsExcelAddIn.Apps.PurchaseInvoices.DataSet dataSet;
        private ListObject listObject;

        private Result result;

        public PurchaseInvoicesSheet(Sheets sheets, Worksheet worksheet)
            : base(sheets, worksheet)
        {
        }

        public PurchaseInvoice[] PurchaseInvoices { get; private set; }

        public ListObject PurchaseInvoicesListObject
        {
            get
            {
                if (this.listObject == null)
                {
                    this.listObject = this.FindListObject(PurchaseInvoicesListObjectName);
                    if (this.listObject == null)
                    {
                        var cell = this.Worksheet.Range["$A$1:$A$1"];
                        this.listObject = this.Worksheet.Controls.AddListObject(cell, PurchaseInvoicesListObjectName);
                    }
                }

                return this.listObject;
            }
        }
        
        public override async Task Refresh()
        {
            await this.Load();

            this.ToListObject();

            this.Sheets.Mediator.OnStateChanged();
        }

        protected override async Task OnSaving()
        {
            await this.Load();

            this.ToWorkspace();
        }

        protected override void OnSaved(ErrorResponse response)
        {
            if (!response.HasErrors)
            {
                MessageBox.Show(@"Successfully saved");
            }
        }
        
        private void ToListObject()
        {
            this.dataSet = new AppsExcelAddIn.Apps.PurchaseInvoices.DataSet();

            this.dataSet.PurchaseInvoice.SetColumnsOrder(
                this.dataSet.PurchaseInvoice.InvoiceNumberColumn.ColumnName,
                this.dataSet.PurchaseInvoice.BilledFromNameColumn.ColumnName,
                this.dataSet.PurchaseInvoice.CustomerReferenceColumn.ColumnName,
                this.dataSet.PurchaseInvoice.InvoiceDateColumn.ColumnName,
                this.dataSet.PurchaseInvoice.DescriptionColumn.ColumnName,
                this.dataSet.PurchaseInvoice.TotalExVatColumn.ColumnName,
                this.dataSet.PurchaseInvoice.TotalIncVatColumn.ColumnName,
                this.dataSet.PurchaseInvoice.CurrencyIsoCodeColumn.ColumnName,
                this.dataSet.PurchaseInvoice.PurchaseInvoiceStateNameColumn.ColumnName
                );

            foreach (var purchaseInvoice in this.PurchaseInvoices)
            {
                var row = this.dataSet.PurchaseInvoice.NewPurchaseInvoiceRow();

                row.InvoiceNumber = purchaseInvoice.InvoiceNumber;
                row.BilledFromName = purchaseInvoice.BilledFrom?.Name;
                row.CustomerReference = purchaseInvoice.CustomerReference;
                row.InvoiceDate = purchaseInvoice.InvoiceDate;
                row.Description = purchaseInvoice.Description;
                row.TotalExVat = purchaseInvoice.TotalExVat;
                row.TotalIncVat = purchaseInvoice.TotalIncVat;
                row.CurrencyIsoCode = purchaseInvoice.Currency?.IsoCode;
                row.PurchaseInvoiceStateName = purchaseInvoice.PurchaseInvoiceState?.Name;

                this.dataSet.PurchaseInvoice.Rows.Add(row);
            }
           
            this.PurchaseInvoicesListObject.SetDataBinding(this.dataSet, this.dataSet.PurchaseInvoice.TableName);

            // Headers

            int index = -1;
            var headers = this.PurchaseInvoicesListObject.HeaderRowRange;
            var data = new object[headers.Rows.Count, headers.Columns.Count];
            data[0, ++index] = "Ref Nr.";
            data[0, ++index] = "Supplier Name";
            data[0, ++index] = "Invoice Nr.";
            data[0, ++index] = "Invoice Date";
            data[0, ++index] = "Description";
            data[0, ++index] = "Amount Net";
            data[0, ++index] = "Amount";
            data[0, ++index] = "Currency";
            data[0, ++index] = "Status";

            headers.Value2 = data;
            headers.Style = "headerStyle";
            headers.EntireColumn.AutoFit();

            var states = this.PurchaseInvoices.Select(v => v.PurchaseInvoiceState).Distinct().ToArray();

            var rowCount = this.PurchaseInvoices.Length;

            foreach (PurchaseInvoiceState purchaseInvoiceState in states)
            {
                if (Equals(purchaseInvoiceState.UniqueId, PurchaseInvoiceStates.PaidId))
                {
                    FormatCondition format = this.Worksheet.Range[$"I2:I{rowCount}"].FormatConditions.Add(
                        XlFormatConditionType.xlCellValue, XlFormatConditionOperator.xlEqual, purchaseInvoiceState.Name);

                    format.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightPink);
                }

                if (Equals(purchaseInvoiceState.UniqueId, PurchaseInvoiceStates.AwaitingApprovalId))
                {
                    FormatCondition format = this.Worksheet.Range[$"I2:I{rowCount}"].FormatConditions.Add(
                        XlFormatConditionType.xlCellValue, XlFormatConditionOperator.xlEqual, purchaseInvoiceState.Name);

                    format.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SandyBrown);
                    format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Khaki);
                }

                if (Equals(purchaseInvoiceState.UniqueId, PurchaseInvoiceStates.NotPaidId))
                {
                    FormatCondition format = this.Worksheet.Range[$"I2:I{rowCount}"].FormatConditions.Add(
                        XlFormatConditionType.xlCellValue, XlFormatConditionOperator.xlEqual, purchaseInvoiceState.Name);

                    format.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                    format.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                }
            }
        }

        private void ToWorkspace()
        {
            var listRows = this.PurchaseInvoicesListObject.ListRows;
            foreach (ListRow row in listRows)
            {
                var range = row.Range;
                var cells = range.Cells;

                var values = cells.Cast<Range>().Select(cell => cell.Value).ToArray();

                //var id = Convert.ToInt64(values[0].ToString());
                //var puchaseInvoice = (PurchaseInvoice)this.Context.Session.Get(id);
                //puchaseInvoice.InvoiceNumber = values[2];
                //puchaseInvoice.CustomerReference = values[3];
            }
        }

        private async Task Load()
        {
            var pull = new Pull
            {
                Extent = new Workspace.Data.Filter(M.PurchaseInvoice.ObjectType),
                
                Results = new []
                {
                    new Workspace.Data.Result
                    {
                        Fetch = new Fetch()
                        {
                            Include = new Tree(M.PurchaseInvoice.Class)
                                .Add(M.PurchaseInvoice.BilledFrom)
                                .Add(M.PurchaseInvoice.Currency)
                                .Add(M.PurchaseInvoice.PurchaseInvoiceState)
                        }
                    } , 
                }
            };
            
            this.result = await this.Load(pull);
            this.PurchaseInvoices = this.result.GetCollection<PurchaseInvoice>("PurchaseInvoices");
        }
    }
}
