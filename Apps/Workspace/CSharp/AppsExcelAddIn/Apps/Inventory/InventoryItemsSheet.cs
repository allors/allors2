using System.Runtime.InteropServices;
using Allors.Workspace.Meta;
using ExcelAddIn;
using ExcelAddIn.Apps.Extensions;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Workspace.Domain.Apps;

namespace Allors.Excel.InventoryItems
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

    public class InventoryItemsSheet : Sheet
    {
        private const string InventoryItemsListObjectName = "InventoryItemsListObject";
        private ExcelAddIn.Apps.Inventory.DataSet1 dataSet;
        private ListObject listObject;
        
        public InventoryItemsSheet(Sheets sheets, Worksheet worksheet)
            : base(sheets, worksheet)
        {
        }

        public InventoryOwnership[] InventoryOwnerships { get; private set; }

        public ListObject InventoryItemsListObject
        {
            get
            {
                if (this.listObject == null)
                {
                    this.listObject = this.FindListObject(InventoryItemsListObjectName);
                    if (this.listObject == null)
                    {
                        var cell = this.Worksheet.Range["$A$1:$A$1"];
                        this.listObject = this.Worksheet.Controls.AddListObject(cell, InventoryItemsListObjectName);
                    }
                }

                return this.listObject;
            }
        }
        
        public override async Task Refresh()
        {
            await this.Load();

            this.Worksheet.Name = "STOCK AVIACO";
            //var groupByBilledTo = this.PurchaseInvoices.GroupBy(v => v.BilledTo).OrderBy(v => v.Key.PartyName);

            //foreach (IGrouping<InternalOrganisation, PurchaseInvoice> grouping in groupByBilledTo)
            //{
            //    Microsoft.Office.Interop.Excel.Worksheet workSheet = this.Workbook.Worksheets.Add();
                
            //    var toolWorksheet = Globals.Factory.GetVstoObject(workSheet);

            //    workSheet.Name = grouping.Key.PartyName;

            //    var purchaseInvoicesSheet = new PurchaseInvoicesSheet(this.Sheets, toolWorksheet);
            //    purchaseInvoicesSheet.PurchaseInvoices = grouping.ToArray();
            //    purchaseInvoicesSheet.Payments = this.Payments;
            //    purchaseInvoicesSheet.ToListObject();
            //}

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
            this.dataSet = new ExcelAddIn.Apps.Inventory.DataSet1();

            this.dataSet.InventorItems.SetColumnsOrder(
                this.dataSet.InventorItems.InternalReferenceColumn.ColumnName,
                this.dataSet.InventorItems.EquipmentCategoryColumn.ColumnName,
                this.dataSet.InventorItems.DescriptionColumn.ColumnName,
                this.dataSet.InventorItems.BrandNameColumn.ColumnName,
                this.dataSet.InventorItems.ModelNameColumn.ColumnName,
                this.dataSet.InventorItems.SerialNumberColumn.ColumnName,
                this.dataSet.InventorItems.OwnerStatusColumn.ColumnName,
                this.dataSet.InventorItems.StatusColumn.ColumnName,
                this.dataSet.InventorItems.SupplierNameColumn.ColumnName,
                this.dataSet.InventorItems.SupplierInvoiceNumberColumn.ColumnName,
                this.dataSet.InventorItems.CountryCodeColumn.ColumnName,
                this.dataSet.InventorItems.LocationColumn.ColumnName,
                this.dataSet.InventorItems.AcquisitionYearColumn.ColumnName,
                this.dataSet.InventorItems.OperatingHoursColumn.ColumnName,
                this.dataSet.InventorItems.ManufacturingYearColumn.ColumnName,
                this.dataSet.InventorItems.OwnerNameColumn.ColumnName,
                this.dataSet.InventorItems.HsCodeColumn.ColumnName,
                this.dataSet.InventorItems.LengthColumn.ColumnName,
                this.dataSet.InventorItems.WidthColumn.ColumnName,
                this.dataSet.InventorItems.HeightColumn.ColumnName,
                this.dataSet.InventorItems.WeightColumn.ColumnName,
                this.dataSet.InventorItems.InvoiceNumberColumn.ColumnName,
                this.dataSet.InventorItems.CustomerNameColumn.ColumnName,
                this.dataSet.InventorItems.CustomerCountryColumn.ColumnName,
                this.dataSet.InventorItems.InternalCommentColumn.ColumnName
              
                );

            foreach (var inventoryOwnership in this.InventoryOwnerships)
            {
                foreach (SerialisedItem serialisedItem in inventoryOwnership.InventoryItem.Part.SerialisedItems)
                {
                    var row = this.dataSet.InventorItems.NewInventorItemsRow();

                    row.InternalReference = serialisedItem.ItemNumber;
                    row.EquipmentCategory = "?";
                    row.Description = inventoryOwnership.InventoryItem.Part.Description;
                    row.BrandName = inventoryOwnership.InventoryItem.Part.Brand?.Name;
                    row.ModelName = inventoryOwnership.InventoryItem.Part.Model?.Name;
                    row.SerialNumber = serialisedItem.SerialNumber;
                    row.OwnerStatus = "?";
                    row.Status = serialisedItem.Ownership?.Name;
                    row.SupplierName = serialisedItem.SuppliedBy?.PartyName;
                    row.SupplierInvoiceNumber = "?";
                    row.CountryCode = "?";
                    row.Location = inventoryOwnership.InventoryItem.Facility?.Name;

                    if (serialisedItem.ExistAcquisitionYear)
                    {
                        row.AcquisitionYear = serialisedItem.AcquisitionYear.GetValueOrDefault();
                    }
                    else
                    {
                        row.SetAcquisitionYearNull();
                    }

                    row.OperatingHours = 0;

                    if (serialisedItem.ExistManufacturingYear)
                    {
                        row.ManufacturingYear = serialisedItem.ManufacturingYear.GetValueOrDefault();
                    }
                    else
                    {
                        row.SetManufacturingYearNull();
                    }

                    row.OwnerName = serialisedItem.OwnedBy?.PartyName;
                    
                    row.HsCode = inventoryOwnership.InventoryItem.Part.HsCode;
                    row.Length = 0;
                    row.Width = 0;
                    row.Height = 0;
                    row.Weight = 0;
                    row.InvoiceNumber = "?";
                    row.CustomerName = "?";
                    row.CustomerCountry = "?";
                    row.InternalComment = serialisedItem.InternalComment;

                    this.dataSet.InventorItems.Rows.Add(row);
                }
            }
           
            this.InventoryItemsListObject.SetDataBinding(this.dataSet, this.dataSet.InventorItems.TableName);

            // Headers

            int index = -1;
            var headers = this.InventoryItemsListObject.HeaderRowRange;
            var data = new object[headers.Rows.Count, headers.Columns.Count];
            data[0, ++index] = "Aviaco Ref.";
            data[0, ++index] = "Equipment Category";
            data[0, ++index] = "GSE Description";
            data[0, ++index] = "Brand";
            data[0, ++index] = "Model";
            data[0, ++index] = "Serial Number";
            data[0, ++index] = "Owner Status";
            data[0, ++index] = "Status";
            data[0, ++index] = "Supplier";
            data[0, ++index] = "Supplier Invoice Nr.";
            data[0, ++index] = "Country";
            data[0, ++index] = "Location";
            data[0, ++index] = "Acquisition Date";
            data[0, ++index] = "Operation Hours Act";
            data[0, ++index] = "Manufacturing Year";
            data[0, ++index] = "Owner";
            data[0, ++index] = "HS Code";
            data[0, ++index] = "Length mm";
            data[0, ++index] = "Width mm";
            data[0, ++index] = "Height mm";
            data[0, ++index] = "Weight mm";
            data[0, ++index] = "Invoice Nr.";
            data[0, ++index] = "Customer Name";
            data[0, ++index] = "Customer Country";
            data[0, ++index] = "Comment";

            headers.Value2 = data;
            headers.Style = "headerStyle";
            headers.EntireColumn.AutoFit();
        }

      

        private void ToWorkspace()
        {
            var listRows = this.InventoryItemsListObject.ListRows;
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
                Extent = new Workspace.Data.Filter(M.InventoryOwnership.ObjectType),
                
                Results = new []
                {
                    new Workspace.Data.Result
                    {
                        Fetch = new Fetch()
                        {
                            Include = new Tree(M.InventoryOwnership.Class)
                                .Add(M.InventoryOwnership.InternalOrganisation)
                                .Add(M.InventoryOwnership.InventoryItem, this.InventoryItemTree)
                        }
                    } , 
                }
            };
            
            var result = await this.Load(pull);
            this.InventoryOwnerships = result.GetCollection<InventoryOwnership>("InventoryOwnerships");
        }

        private Tree InventoryItemTree
            => new Tree(M.InventoryItem.Interface)
                .Add(M.InventoryItem.Part, this.PartTree)
        ;

        private Tree PartTree
            => new Tree(M.Part.Interface)
                .Add(M.Part.ProductType)
                .Add(M.Part.Brand)
                .Add(M.Part.Model)
                .Add(M.Part.ManufacturedBy)
                .Add(M.Part.SuppliedBy)
                .Add(M.Part.DefaultFacility)
                .Add(M.Part.SerialisedItems, this.SerialisedItemTree)
        ;

        private Tree SerialisedItemTree
            => new Tree(M.SerialisedItem.Class)
                .Add(M.SerialisedItem.OwnedBy)
                .Add(M.SerialisedItem.Ownership)
        ;
    }
}
