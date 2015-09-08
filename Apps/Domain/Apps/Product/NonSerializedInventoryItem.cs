// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonSerializedInventoryItem.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class NonSerializedInventoryItem
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new NonSerializedInventoryItemObjectStates(this.Strategy.Session).Good;
            }

            if (!this.ExistAvailableToPromise)
            {
                this.AvailableToPromise = 0;
            }

            if (!this.ExistQuantityCommittedOut)
            {
                this.QuantityCommittedOut = 0;
            }

            if (!this.ExistQuantityExpectedIn)
            {
                this.QuantityExpectedIn = 0;
            }

            if (!this.ExistQuantityOnHand)
            {
                this.QuantityOnHand = 0;
            }

            if (!this.ExistFacility)
            {
                if (Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation != null)
                {
                    this.Facility = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation.DefaultFacility;
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Log.AssertAtLeastOne(this, NonSerializedInventoryItems.Meta.Good, NonSerializedInventoryItems.Meta.Part);
            derivation.Log.AssertExistsAtMostOne(this, NonSerializedInventoryItems.Meta.Good, NonSerializedInventoryItems.Meta.Part);

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
            
            this.DeriveCurrentObjectState(derivation);

            this.AppsOnDeriveSku(derivation);
            this.AppsOnDeriveName(derivation);
            this.AppsOnDeriveUnitOfMeasure(derivation);

            if (this.ExistGood)
            {
                this.Good.DeriveAvailableToPromise();
            }

            this.PreviousQuantityOnHand = this.QuantityOnHand;
        }

        public void AppsOnDeriveQuantityOnHand(IDerivation derivation)
        {
            this.QuantityOnHand = 0M;

            foreach (InventoryItemVariance inventoryItemVariance in this.InventoryItemVariances)
            {
                this.QuantityOnHand += inventoryItemVariance.Quantity;
            }

            foreach (PickListItem pickListItem in this.PickListItemsWhereInventoryItem)
            {
                if (pickListItem.ActualQuantity.HasValue && pickListItem.PickListWherePickListItem.CurrentObjectState.Equals(new PickListObjectStates(this.Strategy.Session).Picked))
                {
                    this.QuantityOnHand -= pickListItem.ActualQuantity.Value;
                }
            }

            foreach (ShipmentReceipt shipmentReceipt in this.ShipmentReceiptsWhereInventoryItem)
            {
                 if (shipmentReceipt.ShipmentItem.ShipmentWhereShipmentItem.CurrentObjectState.Equals(new PurchaseShipmentObjectStates(this.Strategy.Session).Completed))
                {
                    this.QuantityOnHand += shipmentReceipt.QuantityAccepted;
                }
            }
        }

        public void AppsOnDeriveQuantityCommittedOut(IDerivation derivation)
        {
            this.QuantityCommittedOut = 0M;

            foreach (PickListItem pickListItem in this.PickListItemsWhereInventoryItem)
            {
                if (pickListItem.ActualQuantity.HasValue && pickListItem.PickListWherePickListItem.CurrentObjectState.Equals(new PickListObjectStates(this.Strategy.Session).Picked))
                {
                    this.QuantityCommittedOut -= pickListItem.ActualQuantity.Value;
                }
            }

            foreach (SalesOrderItem salesOrderItem in this.SalesOrderItemsWhereReservedFromInventoryItem)
            {
                if (salesOrderItem.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).InProcess) && !salesOrderItem.SalesOrderWhereSalesOrderItem.ScheduledManually)
                {
                    this.QuantityCommittedOut += salesOrderItem.QuantityOrdered;
                }
            }
        }

        public void AppsOnDeriveQuantityExpectedIn(IDerivation derivation)
        {
            this.QuantityExpectedIn = 0M;

            if (this.ExistGood)
            {
                foreach (PurchaseOrderItem purchaseOrderItem in this.Good.PurchaseOrderItemsWhereProduct)
                {
                    if (purchaseOrderItem.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).InProcess))
                    {
                        this.QuantityExpectedIn += purchaseOrderItem.QuantityOrdered;
                        this.QuantityExpectedIn -= purchaseOrderItem.QuantityReceived;
                    }
                }
            }

            if (this.ExistPart)
            {
                foreach (PurchaseOrderItem purchaseOrderItem in this.Part.PurchaseOrderItemsWherePart)
                {
                    if (purchaseOrderItem.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).InProcess))
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

        public void AppsOnDeriveCurrentObjectState(IDerivation derivation)
        {
            

            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {
                var currentStatus = new NonSerializedInventoryItemStatusBuilder(this.Strategy.Session).WithNonSerializedInventoryItemObjectState(this.CurrentObjectState).Build();
                this.AddNonSerializedInventoryItemStatus(currentStatus);
                this.CurrentInventoryItemStatus = currentStatus;
            }

            if (this.ExistCurrentObjectState)
            {
                this.CurrentObjectState.Process(this);
            }

            this.AppsOnDeriveProductCategories(derivation);
        }

        public void AppsReplenishSalesOrders(IDerivation derivation)
        {
            Extent<SalesOrderItem> salesOrderItems = this.Strategy.Session.Extent<SalesOrderItem>();
            salesOrderItems.Filter.AddEquals(SalesOrderItems.Meta.CurrentObjectState, new SalesOrderItemObjectStates(this.Strategy.Session).InProcess);
            salesOrderItems.AddSort(SalesOrderItems.Meta.DeliveryDate, SortDirection.Ascending);

            if (this.ExistGood)
            {
                salesOrderItems.Filter.AddEquals(SalesOrderItems.Meta.PreviousProduct, this.Good);                
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

                    salesOrderItem.DeriveAddToShipping(derivation, diff);
                    salesOrderItem.SalesOrderWhereSalesOrderItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        public void AppsDepleteSalesOrders(IDerivation derivation)
        {
            Extent<SalesOrderItem> salesOrderItems = this.Strategy.Session.Extent<SalesOrderItem>();
            salesOrderItems.Filter.AddEquals(SalesOrderItems.Meta.CurrentObjectState, new SalesOrderItemObjectStates(this.Strategy.Session).InProcess);
            salesOrderItems.AddSort(SalesOrderItems.Meta.DeliveryDate, SortDirection.Descending);

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

                    salesOrderItem.DeriveSubtractFromShipping(derivation, diff);
                    salesOrderItem.SalesOrderWhereSalesOrderItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        public void AppsOnDeriveProductCategories(IDerivation derivation)
        {
            this.RemoveDerivedProductCategories();

            if (this.ExistGood)
            {
                foreach (ProductCategory productCategory in this.Good.ProductCategories)
                {
                    this.AddDerivedProductCategory(productCategory);
                    this.AddParentCategories(productCategory);
                }
            }
        }

        private void AddParentCategories(ProductCategory productCategory)
        {
            if (productCategory.ExistParents)
            {
                foreach (ProductCategory parent in productCategory.Parents)
                {
                    this.AddDerivedProductCategory(parent);
                    this.AddParentCategories(parent);
                }
            }
        }

        public void AppsOnDeriveSku(IDerivation derivation)
        {
            this.Sku = this.ExistGood ? this.Good.Sku : string.Empty;
        }

        public void AppsOnDeriveName(IDerivation derivation)
        {
            if (this.ExistGood)
            {
                this.Name = this.Good.Name;
            }

            if (this.ExistPart)
            {
                this.Name = this.Part.Name;
            }
        }

        public void AppsOnDeriveUnitOfMeasure(IDerivation derivation)
        {
            if (this.ExistGood)
            {
                this.UnitOfMeasure = this.Good.UnitOfMeasure;
            }

            if (this.ExistPart)
            {
                this.UnitOfMeasure = this.Part.UnitOfMeasure;
            }
        }
    }
}