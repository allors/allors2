using System;
using System.Linq;
using System.Windows.Forms;
using Allors.Protocol.Remote;
using Allors.Workspace;
using Allors.Workspace.Data;
using Allors.Workspace.Domain;
using Allors.Workspace.Meta;
using AppsExcelAddIn.Apps.Inventory;
using ExcelAddIn.Apps.Extensions;
using Microsoft.Office.Interop.Excel;
using Filter = Allors.Workspace.Data.Filter;
using ListObject = Microsoft.Office.Tools.Excel.ListObject;
using Task = System.Threading.Tasks.Task;
using Worksheet = Microsoft.Office.Tools.Excel.Worksheet;

namespace Allors.Excel.InventoryItems
{
    public class InventoryItemsSheet : Sheet
    {
        private const string InventoryItemsListObjectName = "InventoryItemsListObject";
        private DataSet1 dataSet;
        private ListObject listObject;

        public InventoryItemsSheet(Sheets sheets, Worksheet worksheet)
            : base(sheets, worksheet)
        {
        }

        public InventoryOwnership[] InventoryOwnerships { get; private set; }

        public PurchaseInvoice[] PurchaseInvoices { get; set; }

        public ListObject InventoryItemsListObject
        {
            get
            {
                if (listObject == null)
                {
                    listObject = FindListObject(InventoryItemsListObjectName);
                    if (listObject == null)
                    {
                        var cell = Worksheet.Range["$A$1:$A$1"];
                        listObject = Worksheet.Controls.AddListObject(cell, InventoryItemsListObjectName);
                    }
                }

                return listObject;
            }
        }

        public override async Task Refresh()
        {
            await Load();

            Worksheet.Name = "Serialized Stock";
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

            ToListObject();

            Sheets.Mediator.OnStateChanged();
        }

        protected override async Task OnSaving()
        {
            await Load();

            ToWorkspace();
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
            dataSet = new DataSet1();

            dataSet.InventorItems.SetColumnsOrder(
                dataSet.InventorItems.InternalReferenceColumn.ColumnName,
                dataSet.InventorItems.EquipmentCategoryColumn.ColumnName,
                dataSet.InventorItems.DescriptionColumn.ColumnName,
                dataSet.InventorItems.BrandNameColumn.ColumnName,
                dataSet.InventorItems.ModelNameColumn.ColumnName,
                dataSet.InventorItems.SerialNumberColumn.ColumnName,
                dataSet.InventorItems.OwnerStatusColumn.ColumnName,
                dataSet.InventorItems.StatusColumn.ColumnName,
                dataSet.InventorItems.SupplierNameColumn.ColumnName,
                dataSet.InventorItems.SupplierInvoiceNumberColumn.ColumnName,
                dataSet.InventorItems.CountryCodeColumn.ColumnName,
                dataSet.InventorItems.LocationColumn.ColumnName,
                dataSet.InventorItems.AcquisitionYearColumn.ColumnName,
                dataSet.InventorItems.OperatingHoursColumn.ColumnName,
                dataSet.InventorItems.ManufacturingYearColumn.ColumnName,
                dataSet.InventorItems.OwnerNameColumn.ColumnName,
                dataSet.InventorItems.HsCodeColumn.ColumnName,
                dataSet.InventorItems.LengthColumn.ColumnName,
                dataSet.InventorItems.WidthColumn.ColumnName,
                dataSet.InventorItems.HeightColumn.ColumnName,
                dataSet.InventorItems.WeightColumn.ColumnName,
                dataSet.InventorItems.InvoiceNumberColumn.ColumnName,
                dataSet.InventorItems.CustomerNameColumn.ColumnName,
                dataSet.InventorItems.CustomerCountryColumn.ColumnName,
                dataSet.InventorItems.InternalCommentColumn.ColumnName

                );

            foreach (var inventoryOwnership in InventoryOwnerships)
            {
                if (inventoryOwnership.InventoryItem is SerialisedInventoryItem)
                {
                    var row = dataSet.InventorItems.NewInventorItemsRow();

                    var serialisedItem = ((SerialisedInventoryItem) inventoryOwnership.InventoryItem).SerialisedItem;

                    row.InternalReference = serialisedItem.ItemNumber;
                    row.EquipmentCategory = "?";
                    row.Description = inventoryOwnership.InventoryItem.Part.Description;
                    row.BrandName = inventoryOwnership.InventoryItem.Part.Brand?.Name;
                    row.ModelName = inventoryOwnership.InventoryItem.Part.Model?.Name;
                    row.SerialNumber = serialisedItem.SerialNumber;
                    row.OwnerStatus = serialisedItem.Ownership?.Name; ;
                    row.Status = serialisedItem.Ownership?.Name;
                    row.SupplierName = serialisedItem.SuppliedBy?.Name;
                    row.SupplierInvoiceNumber = string.Join(",", this.PurchaseInvoices.Where(v => v.PurchaseOrders.Any(p => p.Equals(serialisedItem.PurchaseOrder))).Select(v => v.InvoiceNumber));

                    var supplier = this.PurchaseInvoices.FirstOrDefault(v => v.PurchaseOrders.Any(p => p.Equals(serialisedItem.PurchaseOrder)))?.BilledFrom;
                    if (supplier != null)
                    {
                        var address = supplier.GeneralCorrespondence as PostalAddress;
                        row.CountryCode = address?.Country?.IsoCode ?? "?";
                    }
                    else
                    {
                        row.SetCountryCodeNull();
                    }

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
                    row.Length = serialisedItem?.SerialisedItemCharacteristics
                        .FirstOrDefault(v => string.Equals(v?.SerialisedItemCharacteristicType?.Name, "Length", StringComparison.OrdinalIgnoreCase))
                        ?.Value;
                    row.Width = serialisedItem?.SerialisedItemCharacteristics
                        .FirstOrDefault(v => string.Equals(v?.SerialisedItemCharacteristicType?.Name, "Width", StringComparison.OrdinalIgnoreCase))
                        ?.Value;
                    row.Height = serialisedItem?.SerialisedItemCharacteristics
                        .FirstOrDefault(v => string.Equals(v?.SerialisedItemCharacteristicType?.Name, "Height", StringComparison.OrdinalIgnoreCase))
                        ?.Value;
                    row.Weight = serialisedItem?.SerialisedItemCharacteristics
                        .FirstOrDefault(v => string.Equals(v?.SerialisedItemCharacteristicType?.Name, "Weight", StringComparison.OrdinalIgnoreCase))
                        ?.Value;
                    row.InvoiceNumber = "?";
                    row.CustomerName = "?";
                    row.CustomerCountry = "?";
                    row.InternalComment = serialisedItem.InternalComment;

                    dataSet.InventorItems.Rows.Add(row);
                }
            }

            InventoryItemsListObject.SetDataBinding(dataSet, dataSet.InventorItems.TableName);

            // Headers

            int index = -1;
            var headers = InventoryItemsListObject.HeaderRowRange;
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
            var listRows = InventoryItemsListObject.ListRows;
            foreach (ListRow row in listRows)
            {
                var range = row.Range;
                var cells = range.Cells;

                var values = cells.Cast<Range>().Select(cell => cell.Value).ToArray();
            }
        }

        private async Task Load()
        {
            var now = DateTime.UtcNow;

            var pulls = new[]
            {
                new Pull
                {
                    Extent = new Filter(M.InventoryOwnership.ObjectType)
                    {
                        Predicate = new And(
                            new LessThan{ RoleType = M.InventoryOwnership.FromDate, Value = now},
                            new Or(
                                new Not(new Exists(M.InventoryOwnership.ThroughDate)),
                                new GreaterThan{ RoleType = M.InventoryOwnership.ThroughDate, Value = now }
                                )
                        )
                    },

                    Results = new[]
                    {
                        new Result
                        {
                            Fetch = new Fetch
                            {
                                Include = new Tree(M.InventoryOwnership.Class)
                                    .Add(M.InventoryOwnership.Owner)
                                    .Add(M.InventoryOwnership.InventoryItem, InventoryItemTree),
                            }
                        }
                    }
                },
                new Pull
                {
                    Extent = new Filter(M.PurchaseInvoice.ObjectType),

                    Results = new[]
                    {
                        new Result
                        {
                            Fetch = new Fetch()
                            {
                                Include = new Tree(M.PurchaseInvoice.Class)
                                    .Add(M.PurchaseInvoice.PurchaseOrders)
                            }
                        }
                    }
                }
            };

            var result = await Load(pulls);
            this.InventoryOwnerships = result.GetCollection<InventoryOwnership>("InventoryOwnerships");
            this.PurchaseInvoices = result.GetCollection<PurchaseInvoice>("PurchaseInvoices");
        }

        private Tree InventoryItemTree
            => new Tree(M.InventoryItem.Interface)
                .Add(M.InventoryItem.Facility)
                .Add(M.InventoryItem.Part, PartTree)
        ;

        private Tree PartTree
            => new Tree(M.Part.Interface)
                .Add(M.Part.ProductType)
                .Add(M.Part.Brand)
                .Add(M.Part.Model)
                .Add(M.Part.ManufacturedBy)
                .Add(M.Part.SuppliedBy)
                .Add(M.Part.DefaultFacility)
                .Add(M.Part.SerialisedItems, SerialisedItemTree)
        ;

        private Tree SerialisedItemTree
            => new Tree(M.SerialisedItem.Class)
                .Add(M.SerialisedItem.OwnedBy)
                .Add(M.SerialisedItem.Ownership)
                .Add(M.SerialisedItem.SuppliedBy, this.PartyTree)
                .Add(M.SerialisedItem.PurchaseOrder)
                .Add(M.SerialisedItem.SerialisedItemCharacteristics, this.SerialisedItemCharacteristicsTree);

        private Tree SerialisedItemCharacteristicsTree
            => new Tree(M.SerialisedItemCharacteristic.Class)
                .Add(M.SerialisedItemCharacteristic.SerialisedItemCharacteristicType);

        private Tree PartyTree
            => new Tree(M.Party.Interface)
                .Add(M.Party.GeneralCorrespondence, this.PostallAddressTree);

        private Tree PostallAddressTree
            => new Tree(M.PostalAddress.Class)
                .Add(M.PostalAddress.Country);
    }
}
