// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderItem.cs" company="Allors bvba">
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
    using System.Linq;

    using Meta;

    public partial class PurchaseOrderItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseOrderItem, M.PurchaseOrderItem.PurchaseOrderItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public string SupplierReference
        {
            get
            {
                Extent<SupplierOffering> offerings = null;

                if (this.ExistPart)
                {
                    offerings = this.Part.SupplierOfferingsWherePart;
                }

                if (offerings != null)
                {
                    offerings.Filter.AddEquals(M.SupplierOffering.Supplier, this.PurchaseOrderWherePurchaseOrderItem.TakenViaSupplier);
                    foreach (SupplierOffering offering in offerings)
                    {
                        if (offering.FromDate <= this.PurchaseOrderWherePurchaseOrderItem.OrderDate &&
                            (!offering.ExistThroughDate || offering.ThroughDate >= this.PurchaseOrderWherePurchaseOrderItem.OrderDate))
                        {
                            return offering.SupplierProductId;
                        }
                    }
                }

                return string.Empty;
            }
        }

        public void AppsConfirm(OrderItemConfirm method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsApprove(OrderItemApprove method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsCancel(OrderItemCancel method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Cancelled;
        }

        public void AppsReject(OrderItemReject method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Rejected;
        }

        public void AppsComplete(PurchaseOrderItemComplete method)
        {
            this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Completed;
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseOrderItemState)
            {
                this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Created;
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
            
            this.AppsDeriveVatRegime(derivation);

            this.AppsOnDeriveIsValidOrderItem(derivation);

            this.AppsOnDeriveCurrentObjectState(derivation);
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
        }

        public void AppsOnDeriveIsValidOrderItem(IDerivation derivation)
        {
            if (this.ExistPurchaseOrderWherePurchaseOrderItem)
            {
                this.PurchaseOrderWherePurchaseOrderItem.RemoveValidOrderItem(this);

                if (!this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Cancelled)
                    && !this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Rejected))
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

                if (order.PurchaseOrderState.Equals(new PurchaseOrderStates(this.Strategy.Session).Cancelled))
                {
                    this.Cancel();
                }

                if (order.PurchaseOrderState.Equals(new PurchaseOrderStates(this.Strategy.Session).InProcess))
                {
                    if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Created))
                    {
                        this.Confirm();
                    }
                }

                if (order.PurchaseOrderState.Equals(new PurchaseOrderStates(this.Strategy.Session).Completed))
                {
                    this.Complete();
                }

                if (order.PurchaseOrderState.Equals(new PurchaseOrderStates(this.Strategy.Session).Finished))
                {
                    this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Finished;
                }
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).InProcess))
            {
                this.AppsOnDeriveQuantities(derivation);

                this.PreviousQuantity = this.QuantityOrdered;
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Cancelled) ||
                this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Rejected))
            {
                this.AppsOnDeriveQuantities(derivation);
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
                this.UnitBasePrice = new SupplierOfferings(this.Strategy.Session).PurchasePrice(order.TakenViaSupplier, order.OrderDate, this.Part);
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
                   this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).PartiallyReceived;
                }
                else
                {
                    this.PurchaseOrderItemState = new PurchaseOrderItemStates(this.Strategy.Session).Completed;
                }
            }

            if (this.ExistPurchaseOrderWherePurchaseOrderItem)
            {
                var purchaseOrder = (PurchaseOrder)this.PurchaseOrderWherePurchaseOrderItem;
                purchaseOrder.AppsOnDerivePurchaseOrderState(derivation);
            }
        }

        public void AppsOnDeriveQuantities(IDerivation derivation)
        {
            NonSerialisedInventoryItem inventoryItem = null;

            if (this.ExistPart)
            {
                var inventoryItems = this.Part.InventoryItemsWherePart;
                inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, this.PurchaseOrderWherePurchaseOrderItem.Facility);
                inventoryItem = inventoryItems.First as NonSerialisedInventoryItem;
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).InProcess))
            {
                if (!this.ExistPreviousQuantity || !this.QuantityOrdered.Equals(this.PreviousQuantity))
                {
                    if (inventoryItem != null)
                    {
                        inventoryItem.OnDerive(x => x.WithDerivation(derivation));
                    }
                }
            }

            if (this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Cancelled) ||
                this.PurchaseOrderItemState.Equals(new PurchaseOrderItemStates(this.Strategy.Session).Rejected))
            {
                if (inventoryItem != null)
                {
                    inventoryItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }
    }
}