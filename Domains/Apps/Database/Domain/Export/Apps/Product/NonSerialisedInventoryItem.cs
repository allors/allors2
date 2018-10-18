// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonSerialisedInventoryItem.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Meta;

    public partial class NonSerialisedInventoryItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.NonSerialisedInventoryItem, M.NonSerialisedInventoryItem.NonSerialisedInventoryItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistNonSerialisedInventoryItemState)
            {
                this.NonSerialisedInventoryItemState = new NonSerialisedInventoryItemStates(this.Strategy.Session).Good;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistName && this.ExistPart && this.Part.ExistName)
            {
                this.Name = this.Part.Name;
            }

            this.AppsOnDeriveQuantityOnHand(derivation);
            this.AppsOnDeriveQuantityCommittedOut(derivation);
            this.AppsOnDeriveQuantityExpectedIn(derivation);
            this.AppsOnDeriveQuantityAvailableToPromise(derivation);

            if (this.ExistPreviousQuantityOnHand && this.QuantityOnHand > this.PreviousQuantityOnHand)
            {
                this.AppsReplenishSalesOrders(derivation);
            }

            if (this.ExistPreviousQuantityOnHand && this.QuantityOnHand < this.PreviousQuantityOnHand)
            {
                this.AppsDepleteSalesOrders(derivation);
            }

            this.AppsOnDeriveName(derivation);
            this.AppsOnDeriveUnitOfMeasure(derivation);

            this.PreviousQuantityOnHand = this.QuantityOnHand;
        }

        public void AppsOnDeriveQuantityOnHand(IDerivation derivation)
        {
            // TODO: Test for changes in these relations for performance reasons
            this.QuantityOnHand = 0M;

            if (!this.Part.GetInventoryStrategy.OnHandNonSerialisedStates.Contains(this.NonSerialisedInventoryItemState))
            {
                return;  // This Inventorty Item's State is not counted for On-Hand inventory
            }

            foreach (InventoryItemTransaction inventoryTransaction in this.InventoryItemTransactionsWhereInventoryItem)
            {
                var reason = inventoryTransaction.Reason;

                if (reason.IncreasesQuantityOnHand == true)
                {
                    this.QuantityOnHand += inventoryTransaction.Quantity;
                }
                else if (reason.IncreasesQuantityOnHand == false)
                {
                    this.QuantityOnHand -= inventoryTransaction.Quantity;
                }
            }

            foreach (PickListItem pickListItem in this.PickListItemsWhereInventoryItem)
            {
                if (pickListItem.ActualQuantity.HasValue && pickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Strategy.Session).Picked))
                {
                    this.QuantityOnHand -= pickListItem.ActualQuantity.Value;
                }
            }

            foreach (ShipmentReceipt shipmentReceipt in this.ShipmentReceiptsWhereInventoryItem)
            {
                if (shipmentReceipt.ExistShipmentItem)
                {
                    var purchaseShipment = (PurchaseShipment)shipmentReceipt.ShipmentItem.ShipmentWhereShipmentItem;
                    if (purchaseShipment.PurchaseShipmentState.Equals(new PurchaseShipmentStates(this.Strategy.Session).Completed))
                    {
                        this.QuantityOnHand += shipmentReceipt.QuantityAccepted;
                    }
                }
            }
        }

        public void AppsOnDeriveQuantityCommittedOut(IDerivation derivation)
        {
            // TODO: Test for changes in these relations for performance reasons
            this.QuantityCommittedOut = 0M;

            foreach (PickListItem pickListItem in this.PickListItemsWhereInventoryItem)
            {
                if (pickListItem.ActualQuantity.HasValue && pickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Strategy.Session).Picked))
                {
                    this.QuantityCommittedOut -= pickListItem.ActualQuantity.Value;
                }
            }

            foreach (SalesOrderItem salesOrderItem in this.SalesOrderItemsWhereReservedFromNonSerialisedInventoryItem)
            {
                var order = (SalesOrder)salesOrderItem.SalesOrderWhereSalesOrderItem;
                if (salesOrderItem.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Completed) ||
                    salesOrderItem.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Finished) ||
                    salesOrderItem.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).InProcess) && !order.ScheduledManually)
                {
                    this.QuantityCommittedOut += salesOrderItem.QuantityOrdered;
                }
            }

            foreach (InventoryItemTransaction inventoryTransaction in this.InventoryItemTransactionsWhereInventoryItem)
            {
                var reason = inventoryTransaction.Reason;

                if (reason.IncreasesQuantityCommittedOut == true)
                {
                    this.QuantityCommittedOut += inventoryTransaction.Quantity;
                }
                else if (reason.IncreasesQuantityCommittedOut == false)
                {
                    this.QuantityCommittedOut -= inventoryTransaction.Quantity;
                }
            }
        }

        public void AppsOnDeriveQuantityExpectedIn(IDerivation derivation)
        {
            // TODO: Test for changes in these relations for performance reasons
            this.QuantityExpectedIn = 0M;

            if (this.ExistPart)
            {
                foreach (PurchaseOrderItem purchaseOrderItem in this.Part.PurchaseOrderItemsWherePart)
                {
                    if (purchaseOrderItem.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Completed) ||
                        purchaseOrderItem.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Finished) ||
                        purchaseOrderItem.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).InProcess))
                    {
                        this.QuantityExpectedIn += purchaseOrderItem.QuantityOrdered;
                        this.QuantityExpectedIn -= purchaseOrderItem.QuantityReceived;
                    }
                }
            }
        }

        public void AppsOnDeriveQuantityAvailableToPromise(IDerivation derivation)
        {
            this.AvailableToPromise = this.QuantityOnHand - this.QuantityCommittedOut;

            if (this.AvailableToPromise < 0)
            {
                this.AvailableToPromise = 0;
            }
        }

        public void AppsReplenishSalesOrders(IDerivation derivation)
        {
            Extent<SalesOrderItem> salesOrderItems = this.Strategy.Session.Extent<SalesOrderItem>();
            salesOrderItems.Filter.AddEquals(M.SalesOrderItem.SalesOrderItemState, new SalesOrderItemStates(this.Strategy.Session).InProcess);
            salesOrderItems.AddSort(M.OrderItem.DeliveryDate, SortDirection.Ascending);

            var goods = this.Part?.GoodsWherePart;

            if (goods != null)
            {
                salesOrderItems.Filter.AddContainedIn(M.SalesOrderItem.Product, (Extent)goods);

                salesOrderItems = this.Strategy.Session.Instantiate(salesOrderItems);

                var extra = this.QuantityOnHand - this.PreviousQuantityOnHand;

                foreach (SalesOrderItem salesOrderItem in salesOrderItems)
                {
                    if (extra > 0 && salesOrderItem.QuantityShortFalled > 0)
                    {
                        decimal diff;
                        if (extra >= salesOrderItem.QuantityShortFalled)
                        {
                            diff = salesOrderItem.QuantityShortFalled;
                        }
                        else
                        {
                            diff = extra;
                        }

                        extra -= diff;

                        salesOrderItem.AppsOnDeriveAddToShipping(derivation, diff);
                        salesOrderItem.SalesOrderWhereSalesOrderItem.OnDerive(x => x.WithDerivation(derivation));
                    }
                }
            }
        }

        public void AppsDepleteSalesOrders(IDerivation derivation)
        {
            Extent<SalesOrderItem> salesOrderItems = this.Strategy.Session.Extent<SalesOrderItem>();
            salesOrderItems.Filter.AddEquals(M.SalesOrderItem.SalesOrderItemState, new SalesOrderItemStates(this.Strategy.Session).InProcess);
            salesOrderItems.Filter.AddExists(M.OrderItem.DeliveryDate);
            salesOrderItems.AddSort(M.OrderItem.DeliveryDate, SortDirection.Descending);

            salesOrderItems = this.Strategy.Session.Instantiate(salesOrderItems);

            var subtract = this.PreviousQuantityOnHand - this.QuantityOnHand;

            foreach (SalesOrderItem salesOrderItem in salesOrderItems)
            {
                if (subtract > 0 && salesOrderItem.QuantityRequestsShipping > 0)
                {
                    decimal diff;
                    if (subtract >= salesOrderItem.QuantityRequestsShipping)
                    {
                        diff = salesOrderItem.QuantityRequestsShipping;
                    }
                    else
                    {
                        diff = subtract;
                    }

                    subtract -= diff;

                    salesOrderItem.AppsOnDeriveSubtractFromShipping(derivation, diff);
                    salesOrderItem.SalesOrderWhereSalesOrderItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        public void AppsOnDeriveName(IDerivation derivation)
        {
            if (this.ExistPart)
            {
                this.Name = this.Part.Name;
            }
        }

        public void AppsOnDeriveUnitOfMeasure(IDerivation derivation)
        {
            if (this.ExistPart)
            {
                this.UnitOfMeasure = this.Part.UnitOfMeasure;
            }
        }

        public void AppsDelete(DeletableDelete method)
        {
            foreach (InventoryItemVersion version in this.AllVersions)
            {
                version.Delete();
            }
        }
    }
}