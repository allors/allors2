// <copyright file="SalesInvoicesOverdueSheet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel.Relations.CustomersOverdue
{
    using Allors.Workspace.Meta;
    using ExcelAddIn.Base.Extensions;
    using ExcelAddIn.Base.Relation.SalesInvoicesOverdue;
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

    public class SalesInvoicesOverdueSheet : Sheet
    {
        private const string SalesInvoicesListObjectName = "SalesInvoicesListObject";
        private DataSet1 dataSet;
        private ListObject listObject;

        private Result result;

        public SalesInvoicesOverdueSheet(Sheets sheets, Worksheet worksheet)
            : base(sheets, worksheet)
        {
        }

        public SalesInvoice[] SalesInvoices { get; private set; }

        public ListObject SalesInvoicesListObject
        {
            get
            {
                if (this.listObject == null)
                {
                    this.listObject = this.FindListObject(SalesInvoicesListObjectName);
                    if (this.listObject == null)
                    {
                        var cell = this.Worksheet.Range["$A$1:$A$1"];
                        this.listObject = this.Worksheet.Controls.AddListObject(cell, SalesInvoicesListObjectName);
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
            this.dataSet = new DataSet1();

            this.dataSet.SalesInvoices.SetColumnsOrder(
                this.dataSet.SalesInvoices.CustomerNumberColumn.ColumnName,
                this.dataSet.SalesInvoices.CustomerNameColumn.ColumnName,
                this.dataSet.SalesInvoices.InvoiceNumberColumn.ColumnName,
                this.dataSet.SalesInvoices.InvoiceDateColumn.ColumnName,
                this.dataSet.SalesInvoices.DueDateColumn.ColumnName,
                this.dataSet.SalesInvoices.DescriptionColumn.ColumnName,
                this.dataSet.SalesInvoices.InvoiceTypeColumn.ColumnName,
                this.dataSet.SalesInvoices.InternalOrganisationNameColumn.ColumnName,
                this.dataSet.SalesInvoices.StatusColumn.ColumnName,
                this.dataSet.SalesInvoices.InvoiceAmountColumn.ColumnName,
                this.dataSet.SalesInvoices.PaidAmountColumn.ColumnName,
                this.dataSet.SalesInvoices.BalanceAmountColumn.ColumnName,
                this.dataSet.SalesInvoices.ActionsColumn.ColumnName,
                this.dataSet.SalesInvoices.ContactsColumn.ColumnName

            );
            foreach (var salesInvoice in this.SalesInvoices)
            {
                var row = this.dataSet.SalesInvoices.NewSalesInvoicesRow();

                row.CustomerNumber = salesInvoice.BillToCustomer?.Id.ToString();
                row.CustomerName = salesInvoice.BillToCustomer?.PartyName;
                row.InvoiceNumber = salesInvoice.InvoiceNumber;
                row.InvoiceNumber = salesInvoice.InvoiceNumber;

                // TODO Duedate as derived field?
                //row.DueDate = ??
                row.Description = salesInvoice.Description;
                row.InvoiceType = salesInvoice.SalesInvoiceType?.Name;
                row.InternalOrganisationName = salesInvoice.BilledFrom?.PartyName;
                row.Status = "?";
                row.InvoiceAmount = salesInvoice.TotalIncVat;
                row.PaidAmount = salesInvoice.AmountPaid;
                row.BalanceAmount = row.InvoiceAmount - row.PaidAmount;
                row.Actions = salesInvoice.InternalComment;
                row.Contacts = salesInvoice.BillToContactPerson?.UserEmail;

                this.dataSet.SalesInvoices.Rows.Add(row);
            }

            this.SalesInvoicesListObject.SetDataBinding(this.dataSet, this.dataSet.SalesInvoices.TableName);

            // Headers

            var index = -1;
            var headers = this.SalesInvoicesListObject.HeaderRowRange;
            var data = new object[headers.Rows.Count, headers.Columns.Count];
            data[0, ++index] = "Cust Nr";
            data[0, ++index] = "Customer Name";
            data[0, ++index] = "Invoice";
            data[0, ++index] = "Date";
            data[0, ++index] = "Overdue date";
            data[0, ++index] = "Description";
            data[0, ++index] = "Type";
            data[0, ++index] = "Entity";
            data[0, ++index] = "Status";
            data[0, ++index] = "Amount";
            data[0, ++index] = "Payment";
            data[0, ++index] = "Balance";
            data[0, ++index] = "Actions";
            data[0, ++index] = "Contacts";

            headers.Value2 = data;
            headers.Style = "headerStyle";
            headers.EntireColumn.AutoFit();
        }

        private void ToWorkspace()
        {
            var listRows = this.SalesInvoicesListObject.ListRows;
            foreach (ListRow row in listRows)
            {
                var range = row.Range;
                var cells = range.Cells;

                var values = cells.Cast<Range>().Select(cell => cell.Value).ToArray();

            }
        }

        private async Task Load()
        {
            var pull = new Pull
            {
                Extent = new Workspace.Data.Filter(M.SalesInvoice.ObjectType),

                Results =  new[]
                {
                    new Workspace.Data.Result()
                    {
                        Fetch = new Fetch()
                        {
                            Include =  new Tree(M.SalesInvoice.Class)
                                .Add(M.SalesInvoice.BilledFrom)
                                .Add(M.SalesInvoice.BillToCustomer)
                                .Add(M.SalesInvoice.BillToContactPerson, this.ContactTree)
                                .Add(M.SalesInvoice.Currency)
                                .Add(M.SalesInvoice.SalesInvoiceType)

                        }
                    }
                },
            };
            this.result = await this.Load(pull);
            this.SalesInvoices = this.result.GetCollection<SalesInvoice>("SalesInvoices");
        }

        private Tree ContactTree
            => new Tree(M.Person.Class)
                .Add(M.Person.Salutation)
                .Add(M.Person.GeneralCorrespondence, this.GeneralCorrespondenceTree)
        ;

        private Tree GeneralCorrespondenceTree
            => new Tree(M.ContactMechanism.Interface)
                .Add(M.ContactMechanism.ContactMechanismType)
        ;
    }
}
