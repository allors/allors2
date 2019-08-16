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

using System;
using System.Linq;
using Allors.Domain.NonLogging;

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

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistNonSerialisedInventoryItemState)
            {
                this.NonSerialisedInventoryItemState = new NonSerialisedInventoryItemStates(this.Strategy.Session).Good;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistName)
            {
                this.Name = $"{this.Part?.Name} at {this.Facility?.Name} with state {this.NonSerialisedInventoryItemState?.Name}";
            }

            this.BaseOnDeriveQuantityOnHand(derivation);
            this.BaseOnDeriveQuantityCommittedOut(derivation);
            this.BaseOnDeriveQuantityExpectedIn(derivation);
            this.BaseOnDeriveQuantityAvailableToPromise(derivation);

            this.BaseOnDeriveUnitOfMeasure(derivation);

            this.Part.OnDerive(x => x.WithDerivation(derivation));
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistPreviousQuantityOnHand && this.QuantityOnHand > this.PreviousQuantityOnHand)
            {
                this.BaseReplenishSalesOrders(derivation);
            }

            if (this.ExistPreviousQuantityOnHand && this.QuantityOnHand < this.PreviousQuantityOnHand)
            {
                this.BaseDepleteSalesOrders(derivation);
            }

            this.PreviousQuantityOnHand = this.QuantityOnHand;
        }

        public void BaseOnDeriveQuantityOnHand(IDerivation derivation)
        {
            var settings = this.Strategy.Session.GetSingleton().Settings;

            // TODO: Test for changes in these relations for performance reasons
            var quantityOnHand = 0M;

            if (!settings.InventoryStrategy.OnHandNonSerialisedStates.Contains(this.NonSerialisedInventoryItemState))
            {
                return;  // This Inventorty Item's State is not counted for On-Hand inventory
            }

            foreach (InventoryItemTransaction inventoryTransaction in this.InventoryItemTransactionsWhereInventoryItem)
            {
                var reason = inventoryTransaction.Reason;

                if (reason.IncreasesQuantityOnHand == true)
                {
                    quantityOnHand += inventoryTransaction.Quantity;
                }
                else if (reason.IncreasesQuantityOnHand == false)
                {
                    quantityOnHand -= inventoryTransaction.Quantity;
                }
            }

            foreach (PickListItem pickListItem in this.PickListItemsWhereInventoryItem)
            {
                if (pickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Strategy.Session).Picked))
                {
                    quantityOnHand -= pickListItem.QuantityPicked;
                }
            }

            foreach (ShipmentReceipt shipmentReceipt in this.ShipmentReceiptsWhereInventoryItem)
            {
                if (shipmentReceipt.ExistShipmentItem)
                {
                    var purchaseShipment = (PurchaseShipment)shipmentReceipt.ShipmentItem.ShipmentWhereShipmentItem;
                    if (purchaseShipment.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Delivered))
                    {
                        quantityOnHand += shipmentReceipt.QuantityAccepted;
                    }
                }
            }

            this.QuantityOnHand = quantityOnHand;
        }

        public void BaseOnDeriveQuantityCommittedOut(IDerivation derivation)
        {
            // TODO: Test for changes in these relations for performance reasons
            var quantityCommittedOut = 0M;

            foreach (PickListItem pickListItem in this.PickListItemsWhereInventoryItem)
            {
                foreach (ItemIssuance itemIssuance in pickListItem.ItemIssuancesWherePickListItem)
                {
                    foreach (OrderShipment orderShipment in itemIssuance.ShipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        var orderKind = orderShipment.OrderItem?.OrderWhereValidOrderItem?.OrderKind;
                        if (orderKind?.ScheduleManually == true)
                        {
                            quantityCommittedOut += pickListItem.Quantity;
                        }
                    }
                }

                if (pickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Strategy.Session).Picked))
                {
                    quantityCommittedOut -= pickListItem.QuantityPicked;
                }
            }

            foreach (InventoryItemTransaction inventoryTransaction in this.InventoryItemTransactionsWhereInventoryItem)
            {
                var reason = inventoryTransaction.Reason;

                if (reason.IncreasesQuantityCommittedOut == true)
                {
                    quantityCommittedOut += inventoryTransaction.Quantity;
                }
                else if (reason.IncreasesQuantityCommittedOut == false)
                {
                    quantityCommittedOut -= inventoryTransaction.Quantity;
                }
            }

            this.QuantityCommittedOut = quantityCommittedOut;
        }

        public void BaseOnDeriveQuantityExpectedIn(IDerivation derivation)
        {
            // TODO: Test for changes in these relations for performance reasons
            this.QuantityExpectedIn = 0M;

            foreach (PurchaseOrderItem purchaseOrderItem in this.Part.PurchaseOrderItemsWherePart)
            {
                var facility = purchaseOrderItem.PurchaseOrderWherePurchaseOrderItem.Facility;
                if (purchaseOrderItem.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).InProcess)
                    && this.Facility.Equals(facility))
                {
                    this.QuantityExpectedIn += purchaseOrderItem.QuantityOrdered;
                    this.QuantityExpectedIn -= purchaseOrderItem.QuantityReceived;
                }
            }
        }

        public void BaseOnDeriveQuantityAvailableToPromise(IDerivation derivation)
        {
            this.AvailableToPromise = this.QuantityOnHand - this.QuantityCommittedOut;

            if (this.AvailableToPromise < 0)
            {
                this.AvailableToPromise = 0;
            }
        }

        public void BaseReplenishSalesOrders(IDerivation derivation)
        {
            var salesOrderItems = this.Strategy.Session.Extent<SalesOrderItem>();
            salesOrderItems.Filter.AddEquals(M.SalesOrderItem.SalesOrderItemState, new SalesOrderItemStates(this.Strategy.Session).InProcess);
            salesOrderItems.AddSort(M.OrderItem.DeliveryDate, SortDirection.Ascending);
            var goods = this.Part is NonUnifiedPart nonUnifiedPart ? nonUnifiedPart.NonUnifiedGoodsWherePart : new[] { this.Part };

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
                        salesOrderItem.QuantityShortFalled -= diff;

                        //var inventoryAssignment = salesOrderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault();
                        //if (inventoryAssignment != null)
                        //{
                        //    inventoryAssignment.Quantity += diff;
                        //}
                    }
                }
            }
        }

        public void BaseDepleteSalesOrders(IDerivation derivation)
        {
            var salesOrderItems = this.Strategy.Session.Extent<SalesOrderItem>();
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

                    var inventoryAssignment = salesOrderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault();
                    if (inventoryAssignment != null)
                    {
                        inventoryAssignment.Quantity = diff;
                    }
                }
            }
        }

        public void BaseOnDeriveUnitOfMeasure(IDerivation derivation)
        {
            if (this.ExistPart)
            {
                this.UnitOfMeasure = this.Part.UnitOfMeasure;
            }
        }

        public void BaseDelete(DeletableDelete method)
        {
            foreach (InventoryItemVersion version in this.AllVersions)
            {
                version.Delete();
            }
        }
    }
}
