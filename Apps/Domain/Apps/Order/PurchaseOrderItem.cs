// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItem.cs" company="Allors bvba">
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
    public partial class PurchaseOrderItem
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public string GetActualUnitBasePriceAsCurrencyString
        {
            get
            {
                return this.ExistActualUnitPrice ? DecimalExtensions.AsCurrencyString(this.ActualUnitPrice, this.PurchaseOrderWherePurchaseOrderItem.CurrencyFormat) : string.Empty;
            }
        }

        public string GetUnitBasePriceAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.UnitBasePrice, this.PurchaseOrderWherePurchaseOrderItem.CurrencyFormat);
            }
        }

        public string GetTotalExVatAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalExVat, this.PurchaseOrderWherePurchaseOrderItem.CurrencyFormat);
            }
        }

        public string GetNothingAsCurrencyString
        {
            get
            {
                const decimal Nothing = 0;
                return Nothing.AsCurrencyString(this.PurchaseOrderWherePurchaseOrderItem.CurrencyFormat);
            }
        }

        public string SupplierReference
        {
            get
            {
                Extent<SupplierOffering> offerings = null;

                if (this.ExistProduct)
                {
                    offerings = this.Product.SupplierOfferingsWhereProduct;
                }

                if (this.ExistPart)
                {
                    offerings = this.Part.SupplierOfferingsWherePart;
                }

                if (offerings != null)
                {
                    offerings.Filter.AddEquals(SupplierOfferings.Meta.Supplier, this.PurchaseOrderWherePurchaseOrderItem.TakenViaSupplier);
                    foreach (SupplierOffering offering in offerings)
                    {
                        if (offering.FromDate <= this.PurchaseOrderWherePurchaseOrderItem.OrderDate &&
                            (!offering.ExistThroughDate || offering.ThroughDate >= this.PurchaseOrderWherePurchaseOrderItem.OrderDate))
                        {
                            return offering.ReferenceNumber;
                        }
                    }
                }

                return string.Empty;
            }
        }

        public void AppsConfirm(OrderItemConfirm method)
        {
            this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).InProcess;
        }

        public void AppsApprove(OrderItemApprove method)
        {
            this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).InProcess;
        }

        public void AppsCancel(OrderItemCancel method)
        {
            this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).Cancelled;
        }

        public void AppsReject(OrderItemReject method)
        {
            this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).Rejected;
        }

        public void AppsComplete(PurchaseOrderItemComplete method)
        {
            this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).Completed;
        }

        public void AppsFinish(OrderItemFinish method)
        {
            this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).Finished;
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            

            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).Created;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistPurchaseOrderWherePurchaseOrderItem)
                {
                    derivation.AddDependency(this.PurchaseOrderWherePurchaseOrderItem, this);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            
            derivation.Log.AssertAtLeastOne(this, PurchaseOrderItems.Meta.Product, PurchaseOrderItems.Meta.Part);
            derivation.Log.AssertExistsAtMostOne(this, PurchaseOrderItems.Meta.Product, PurchaseOrderItems.Meta.Part);

            this.AppsDeriveVatRegime(derivation);

            this.DeriveIsValidOrderItem(derivation);

            this.DeriveCurrentObjectState(derivation);
        }

        public void AppsDeriveVatRegime(IDerivation derivation)
        {
            if (this.ExistPurchaseOrderWherePurchaseOrderItem)
            {
                this.VatRegime = this.ExistAssignedVatRegime ? this.AssignedVatRegime : this.PurchaseOrderWherePurchaseOrderItem.VatRegime;

                this.AppsDeriveVatRate(derivation);
            }
        }

        public void AppsDeriveVatRate(IDerivation derivation)
        {
            if (!this.ExistDerivedVatRate && this.ExistVatRegime && this.VatRegime.ExistVatRate)
            {
                this.DerivedVatRate = this.VatRegime.VatRate;
            }

            if (!this.ExistDerivedVatRate && (this.ExistProduct))
            {
                this.DerivedVatRate = this.Product.VatRate;
            }
        }


        public void AppsOnDeriveIsValidOrderItem(IDerivation derivation)
        {
            if (this.ExistPurchaseOrderWherePurchaseOrderItem)
            {
                this.PurchaseOrderWherePurchaseOrderItem.RemoveValidOrderItem(this);

                if (!this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).Cancelled)
                    && !this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).Rejected))
                {
                    this.PurchaseOrderWherePurchaseOrderItem.AddValidOrderItem(this);
                }
            }
        }

        public void AppsOnDeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistOrderWhereValidOrderItem)
            {
                var order = this.PurchaseOrderWherePurchaseOrderItem;

                if (order.CurrentObjectState.Equals(new PurchaseOrderObjectStates(this.Strategy.Session).Cancelled))
                {
                    this.Cancel();
                }

                if (order.CurrentObjectState.Equals(new PurchaseOrderObjectStates(this.Strategy.Session).InProcess))
                {
                    if (this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).Created))
                    {
                        this.Confirm();
                    }
                }

                if (order.CurrentObjectState.Equals(new PurchaseOrderObjectStates(this.Strategy.Session).Completed))
                {
                    this.Complete();
                }

                if (order.CurrentObjectState.Equals(new PurchaseOrderObjectStates(this.Strategy.Session).Finished))
                {
                    this.Finish();
                }
            }

            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {
                var currentStatus = new PurchaseOrderItemStatusBuilder(this.Strategy.Session).WithPurchaseOrderItemObjectState(this.CurrentObjectState).Build();
                this.AddOrderItemStatus(currentStatus);
                this.CurrentOrderItemStatus = currentStatus;
            }

            if (this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).InProcess))
            {
                this.DeriveQuantities(derivation);

                this.PreviousQuantity = this.QuantityOrdered;
            }

            if (this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).Cancelled) ||
                this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).Rejected))
            {
                this.DeriveQuantities(derivation);
            }

            if (this.ExistCurrentObjectState)
            {
                this.CurrentObjectState.Process(this);
            }
        }

        public void AppsOnDeriveCurrentOrderStatus(IDerivation derivation)
        {
            if (this.ExistCurrentShipmentStatus && this.CurrentShipmentStatus.PurchaseOrderItemObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).PartiallyReceived))
            {
                this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).PartiallyReceived;
                this.DeriveCurrentObjectState(derivation);
            }

            if (this.ExistCurrentShipmentStatus && this.CurrentShipmentStatus.PurchaseOrderItemObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).Received))
            {
                this.CurrentObjectState = new PurchaseOrderItemObjectStates(this.Strategy.Session).Completed;
                this.DeriveCurrentObjectState(derivation);
            }
        }

        public void AppsOnDeriveDeliveryDate(IDerivation derivation)
        {
            if (this.AssignedDeliveryDate.HasValue)
            {
                this.DeliveryDate = this.AssignedDeliveryDate.Value;
            }
            else if (this.PurchaseOrderWherePurchaseOrderItem.DeliveryDate.HasValue)
            {
                this.DeliveryDate = this.PurchaseOrderWherePurchaseOrderItem.DeliveryDate.Value;
            }            
        }

        public void AppsOnDerivePrices()
        {
            this.UnitBasePrice = 0;
            this.UnitDiscount = 0;
            this.UnitSurcharge = 0;

            if (this.ActualUnitPrice.HasValue)
            {
                this.UnitBasePrice = this.ActualUnitPrice.Value;
                this.CalculatedUnitPrice = this.ActualUnitPrice.Value;
            }
            else
            {
                var order = this.PurchaseOrderWherePurchaseOrderItem;
                var productPurchasePrice = new SupplierOfferings(this.Strategy.Session).PurchasePrice(order.TakenViaSupplier, order.OrderDate, this.Product, this.Part);
                this.UnitBasePrice = productPurchasePrice != null ? productPurchasePrice.Price : 0M;
            }

            this.UnitVat = 0;
            this.TotalBasePrice = this.UnitBasePrice * this.QuantityOrdered;
            this.TotalDiscount = this.UnitDiscount * this.QuantityOrdered;
            this.TotalSurcharge = this.UnitSurcharge * this.QuantityOrdered;
            this.CalculatedUnitPrice = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;
            this.TotalVat = 0;
            this.TotalExVat = this.CalculatedUnitPrice * this.QuantityOrdered;
            this.TotalIncVat = this.TotalExVat + this.TotalVat;
        }

        public void AppsOnDeriveCurrentShipmentStatus(IDerivation derivation)
        {
            var quantityReceived = 0M;
            foreach (ShipmentReceipt shipmentReceipt in this.ShipmentReceiptsWhereOrderItem)
            {
                quantityReceived += shipmentReceipt.QuantityAccepted;
            }

            this.QuantityReceived = quantityReceived;

            if (quantityReceived > 0)
            {
                if (quantityReceived < this.QuantityOrdered)
                {
                    this.CurrentShipmentStatus = new PurchaseOrderItemStatusBuilder(this.Strategy.Session)
                        .WithPurchaseOrderItemObjectState(new PurchaseOrderItemObjectStates(this.Strategy.Session).PartiallyReceived)
                        .Build();
                }
                else
                {
                    this.CurrentShipmentStatus = new PurchaseOrderItemStatusBuilder(this.Strategy.Session)
                        .WithPurchaseOrderItemObjectState(new PurchaseOrderItemObjectStates(this.Strategy.Session).Received)
                        .Build();
                }

                this.AddShipmentStatus(this.CurrentShipmentStatus);
            }

            this.DeriveCurrentOrderStatus(derivation);

            if (this.ExistPurchaseOrderWherePurchaseOrderItem)
            {
                this.PurchaseOrderWherePurchaseOrderItem.DeriveCurrentShipmentStatus(derivation);
            }
        }

        public void AppsOnDeriveQuantities(IDerivation derivation)
        {
            NonSerializedInventoryItem inventoryItem = null;

            if (this.ExistProduct)
            {
                var good = this.Product as Good;
                if (good != null)
                {
                    var inventoryItems = good.InventoryItemsWhereGood;
                    inventoryItems.Filter.AddEquals(InventoryItems.Meta.Facility, this.PurchaseOrderWherePurchaseOrderItem.Facility);
                    inventoryItem = inventoryItems.First as NonSerializedInventoryItem;
                }
            }

            if (this.ExistPart)
            {
                var inventoryItems = this.Part.InventoryItemsWherePart;
                inventoryItems.Filter.AddEquals(InventoryItems.Meta.Facility, this.PurchaseOrderWherePurchaseOrderItem.Facility);
                inventoryItem = inventoryItems.First as NonSerializedInventoryItem;
            }

            if (this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).InProcess))
            {
                if (!this.ExistPreviousQuantity || !this.QuantityOrdered.Equals(this.PreviousQuantity))
                {
                    if (inventoryItem != null)
                    {
                        inventoryItem.OnDerive(x => x.WithDerivation(derivation));
                    }
                }
            }

            if (this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).Cancelled) ||
                this.CurrentObjectState.Equals(new PurchaseOrderItemObjectStates(this.Strategy.Session).Rejected))
            {
                if (inventoryItem != null)
                {
                    inventoryItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }
    }
}