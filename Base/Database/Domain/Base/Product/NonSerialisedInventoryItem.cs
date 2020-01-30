// <copyright file="NonSerialisedInventoryItem.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Meta;

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

            // TODO: Remove OnDerive
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
                if (shipmentReceipt.ExistShipmentItem
                    && shipmentReceipt.ShipmentItem.ShipmentItemState.Equals(new ShipmentItemStates(this.Session()).Received))
                {
                    var purchaseShipment = (PurchaseShipment)shipmentReceipt.ShipmentItem.ShipmentWhereShipmentItem;
                    if (purchaseShipment.ShipmentState.Equals(new ShipmentStates(this.Strategy.Session).Received))
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

            //foreach (PickListItem pickListItem in this.PickListItemsWhereInventoryItem)
            //{
            //    foreach (ItemIssuance itemIssuance in pickListItem.ItemIssuancesWherePickListItem)
            //    {
            //        quantityCommittedOut += itemIssuance.Quantity;
            //    }

            //    if (pickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Strategy.Session).Picked))
            //    {
            //        quantityCommittedOut -= pickListItem.QuantityPicked;
            //    }
            //}

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
            var nonUnifiedGoods = this.Part.NonUnifiedGoodsWherePart;
            var unifiedGood = this.Part as UnifiedGood;

            if (nonUnifiedGoods.Count > 0 || unifiedGood != null)
            {
                if (unifiedGood != null)
                {
                    salesOrderItems.Filter.AddEquals(M.SalesOrderItem.Product, unifiedGood);
                }

                if (nonUnifiedGoods.Count > 0)
                {
                    salesOrderItems.Filter.AddContainedIn(M.SalesOrderItem.Product, (Extent)nonUnifiedGoods);
                }

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

                        // HACK: DerivedRoles
                        var salesOrderItemDerivedRoles = (SalesOrderItemDerivedRoles)salesOrderItem;

                        salesOrderItemDerivedRoles.QuantityShortFalled -= diff;

                        // var inventoryAssignment = salesOrderItem.SalesOrderItemInventoryAssignmentsWhereSalesOrderItem.FirstOrDefault();
                        // if (inventoryAssignment != null)
                        // {
                        //    inventoryAssignment.Quantity += diff;
                        // }
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

                    var inventoryAssignment = salesOrderItem.SalesOrderItemInventoryAssignments.FirstOrDefault();
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
