using System.Linq;

namespace Allors.Domain
{
    using Meta;
    using Resources;
    using System;
    using System.Collections.Generic;

    public partial class SalesOrderItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
        {
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemShipmentState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemInvoiceState),
            new TransitionalConfiguration(M.SalesOrderItem, M.SalesOrderItem.SalesOrderItemPaymentState),
        };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public Part Part
        {
            get
            {
                if (this.ExistProduct)
                {
                    var nonUnifiedGood = this.Product as NonUnifiedGood;
                    var unifiedGood = this.Product as UnifiedGood;
                    return unifiedGood ?? nonUnifiedGood?.Part;
                }

                return null;
            }
        }
       
        public bool IsValid
        {
            get
            {
                if (this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Cancelled) ||
                    this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Rejected) ||
                    (this.IsSubTotalItem && this.QuantityOrdered == 0) ||
                    (!this.IsSubTotalItem && this.ActualUnitPrice == 0) ||
                    !this.ExistShipToAddress || !this.ExistShipToParty ||
                    (this.ExistCalculatedUnitPrice && this.CalculatedUnitPrice == 0))
                {
                    return false;
                }

                return true;
            }
        }
        
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSalesOrderItemState)
            {
                this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Created;
            }

            if (this.ExistProduct && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).ProductItem;
            }

            if (!this.ExistSalesOrderItemShipmentState)
            {
                this.SalesOrderItemShipmentState = new SalesOrderItemShipmentStates(this.Strategy.Session).NotShipped;
            }

            if (!this.ExistSalesOrderItemInvoiceState)
            {
                this.SalesOrderItemInvoiceState = new SalesOrderItemInvoiceStates(this.Strategy.Session).NotInvoiced;
            }

            if (!this.ExistSalesOrderItemPaymentState)
            {
                this.SalesOrderItemPaymentState = new SalesOrderItemPaymentStates(this.Strategy.Session).NotPaid;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.IsModified(this))
            {
                var salesOrder = this.SalesOrderWhereSalesOrderItem;
                derivation.MarkAsModified(salesOrder);
                derivation.AddDependency(salesOrder, this);

                if(this.ExistOrderShipmentsWhereOrderItem)
                {
                    var orderShipments = this.OrderShipmentsWhereOrderItem;
                    foreach (OrderShipment orderShipment in orderShipments)
                    {
                        derivation.MarkAsModified(orderShipment);
                        derivation.AddDependency(orderShipment, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            foreach (SalesOrderItem featureItem in this.OrderedWithFeatures)
            {
                featureItem.OnDerive(x => x.WithDerivation(derivation));
            }

            if (this.ExistPreviousProduct && !this.PreviousProduct.Equals(this.Product))
            {
                derivation.Validation.AddError(this, M.SalesOrderItem.Product, ErrorMessages.SalesOrderItemProductChangeNotAllowed);
            }
            else
            {
                this.PreviousProduct = this.Product;
            }

            if (this.ExistSalesOrderItemWhereOrderedWithFeature && !this.ExistProductFeature)
            {
                derivation.Validation.AssertExists(this, M.SalesOrderItem.ProductFeature);
            }

            if (this.ExistProduct && this.ExistQuantityOrdered && this.QuantityOrdered < this.QuantityShipped)
            {
                derivation.Validation.AddError(this, M.SalesOrderItem.QuantityOrdered, ErrorMessages.SalesOrderItemLessThanAlreadeyShipped);
            }

            if (!this.ExistAssignedShipToAddress && this.ExistAssignedShipToParty)
            {
                this.AssignedShipToAddress = this.AssignedShipToParty.ShippingAddress;
            }

            derivation.Validation.AssertExistsAtMostOne(this, M.SalesOrderItem.Product, M.SalesOrderItem.ProductFeature);
            derivation.Validation.AssertExistsAtMostOne(this, M.SalesOrderItem.SerialisedItem, M.SalesOrderItem.ProductFeature);
            derivation.Validation.AssertExistsAtMostOne(this, M.SalesOrderItem.ReservedFromSerialisedInventoryItem, M.SalesOrderItem.ReservedFromNonSerialisedInventoryItem);
            derivation.Validation.AssertExistsAtMostOne(this, M.SalesOrderItem.ActualUnitPrice, M.SalesOrderItem.DiscountAdjustment, M.SalesOrderItem.SurchargeAdjustment);
            derivation.Validation.AssertExistsAtMostOne(this, M.SalesOrderItem.RequiredMarkupPercentage, M.SalesOrderItem.RequiredProfitMargin, M.SalesOrderItem.DiscountAdjustment, M.SalesOrderItem.SurchargeAdjustment);

            var internalOrganisation = this.SalesOrderWhereSalesOrderItem.TakenBy;

            if (this.Part != null && internalOrganisation != null)
            {
                if (this.Part.InventoryItemKind.Equals(new InventoryItemKinds(this.Strategy.Session).Serialised))
                {
                    if (this.ExistSerialisedItem)
                    {
                        if (this.SerialisedItem.ExistSerialisedInventoryItemsWhereSerialisedItem)
                        {
                            this.ReservedFromSerialisedInventoryItem = this.SerialisedItem.SerialisedInventoryItemsWhereSerialisedItem.FirstOrDefault(v => v.Quantity == 1); 
                        }
                    }
                    else
                    {
                        var inventoryItems = this.Part.InventoryItemsWherePart;
                        inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, this.SalesOrderWhereSalesOrderItem.OriginFacility);
                        this.ReservedFromSerialisedInventoryItem = inventoryItems.First as SerialisedInventoryItem;
                    }
                }
                else
                {
                    var inventoryItems = this.Part.InventoryItemsWherePart;
                    inventoryItems.Filter.AddEquals(M.InventoryItem.Facility, this.SalesOrderWhereSalesOrderItem.OriginFacility);
                    this.ReservedFromNonSerialisedInventoryItem = inventoryItems.First as NonSerialisedInventoryItem;
                }
            }

            if (this.ExistQuantityShipNow && this.QuantityShipNow != 0)
            {
                this.AppsShipNow(derivation);

                if (this.ExistReservedFromNonSerialisedInventoryItem)
                {
                    this.ReservedFromNonSerialisedInventoryItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }

            if (derivation.IsCreated(this) && this.ExistSerialisedItem)
            {
                this.Details = this.SerialisedItem.Details;
            }
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            if (this.ExistSalesOrderItemInvoiceState && this.SalesOrderItemInvoiceState.Equals(new SalesOrderItemInvoiceStates(this.Strategy.Session).Invoiced))
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.QuantityOrdered, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.ActualUnitPrice, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.InvoiceItemType, Operations.Write));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Product, Operations.Write));
            }
        }

        public void AppsCancel(OrderItemCancel method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Cancelled;
        }

        public void AppsConfirm(OrderItemConfirm method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsReject(OrderItemReject method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Rejected;
        }

        public void AppsApprove(OrderItemApprove method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsContinue(SalesOrderItemContinue method)
        {
            this.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).InProcess;
        }

        public void AppsOnDeriveQuantities(IDerivation derivation)
        {
            var order = (SalesOrder)this.SalesOrderWhereSalesOrderItem;
            if (order.ScheduledManually)
            {
                foreach (SalesOrderItem item in this.OrderedWithFeatures)
                {
                    this.QuantityOrdered = item.QuantityOrdered;
                }

                if (this.ExistReservedFromNonSerialisedInventoryItem &&
                    this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).InProcess) &&
                    (this.QuantityReserved > 0 || this.QuantityPendingShipment > 0))
                {
                    if (this.ExistPreviousQuantity && !this.QuantityOrdered.Equals(this.PreviousQuantity))
                    {
                        var diff = this.QuantityOrdered - this.PreviousQuantity;

                        if (diff < 0)
                        {
                            var leftToShip = this.PreviousQuantity - this.QuantityShipped - this.QuantityPendingShipment;
                            var shipmentCorrection = leftToShip + diff;

                            this.QuantityReserved += shipmentCorrection;

                            if (this.QuantityShortFalled > 0)
                            {
                                this.QuantityShortFalled += diff;
                                if (this.QuantityShortFalled < 0)
                                {
                                    this.QuantityShortFalled = 0;
                                }
                            }

                            if (this.QuantityRequestsShipping > this.QuantityReserved)
                            {
                                this.QuantityRequestsShipping = this.QuantityReserved;
                            }

                            if (this.ExistQuantityPendingShipment && shipmentCorrection < 0)
                            {
                                this.DecreasePendingShipmentQuantity(derivation, 0 - shipmentCorrection);
                            }

                            this.ReservedFromNonSerialisedInventoryItem.OnDerive(x => x.WithDerivation(derivation));
                        }
                    }

                    this.PreviousQuantity = this.QuantityOrdered;
                }

                if (this.ExistReservedFromNonSerialisedInventoryItem &&
                    (this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Cancelled) ||
                     this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Rejected)))
                {
                    if (this.ExistQuantityPendingShipment)
                    {
                        this.DecreasePendingShipmentQuantity(derivation, this.QuantityPendingShipment);
                    }

                    this.ReservedFromNonSerialisedInventoryItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }
            else
            {
                foreach (SalesOrderItem item in this.OrderedWithFeatures)
                {
                    this.QuantityOrdered = item.QuantityOrdered;
                }

                if (this.ExistReservedFromNonSerialisedInventoryItem 
                    && this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).InProcess)
                    && this.SalesOrderItemShipmentState.NotShipped)
                {
                    if (this.ExistPreviousReservedFromNonSerialisedInventoryItem && !this.ReservedFromNonSerialisedInventoryItem.Equals(this.PreviousReservedFromNonSerialisedInventoryItem))
                    {
                        this.PreviousReservedFromNonSerialisedInventoryItem.OnDerive(x => x.WithDerivation(derivation));

                        this.SetQuantitiesWithInventoryFirstTime(derivation);
                    }
                    else
                    {
                        if (this.ExistPreviousQuantity && !this.QuantityOrdered.Equals(this.PreviousQuantity))
                        {
                            var diff = this.QuantityOrdered - this.PreviousQuantity;

                            if (diff > 0)
                            {
                                this.QuantityReserved += diff;

                                if (diff > this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise)
                                {
                                    this.QuantityRequestsShipping += this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise;
                                    this.QuantityShortFalled += diff - this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise;
                                }
                                else
                                {
                                    this.QuantityRequestsShipping += diff;
                                }
                            }
                            else
                            {
                                var leftToShip = this.PreviousQuantity - this.QuantityShipped - this.QuantityPendingShipment;

                                this.QuantityReserved += diff;

                                if (this.QuantityShortFalled > 0)
                                {
                                    this.QuantityShortFalled += diff;
                                    if (this.QuantityShortFalled < 0)
                                    {
                                        this.QuantityShortFalled = 0;
                                    }
                                }

                                if (this.QuantityRequestsShipping > this.QuantityReserved)
                                {
                                    this.QuantityRequestsShipping = this.QuantityReserved;
                                }

                                var shipmentCorrection = leftToShip + diff;
                                if (this.ExistQuantityPendingShipment && shipmentCorrection < 0)
                                {
                                    this.DecreasePendingShipmentQuantity(derivation, 0 - shipmentCorrection);
                                }
                            }

                            this.ReservedFromNonSerialisedInventoryItem.OnDerive(x => x.WithDerivation(derivation));
                        }

                        //// When first Confirmed.
                        if (!this.ExistPreviousQuantity)
                        {
                            this.SetQuantitiesWithInventoryFirstTime(derivation);
                        }
                    }

                    this.PreviousQuantity = this.QuantityOrdered;
                }

                if (this.ExistReservedFromNonSerialisedInventoryItem &&
                    (this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Cancelled) ||
                     this.SalesOrderItemState.Equals(new SalesOrderItemStates(this.Strategy.Session).Rejected)))
                {
                    if (this.ExistQuantityPendingShipment)
                    {
                        this.DecreasePendingShipmentQuantity(derivation, this.QuantityPendingShipment);
                    }

                    this.ReservedFromNonSerialisedInventoryItem.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }

        private void SetQuantitiesWithInventoryFirstTime(IDerivation derivation)
        {
            this.QuantityReserved = 0;
            this.QuantityRequestsShipping = 0;
            this.QuantityShortFalled = 0;

            this.QuantityRequestsShipping = this.QuantityOrdered > this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise ?
                this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise : this.QuantityOrdered;

            if (this.QuantityRequestsShipping < 0)
            {
                this.QuantityRequestsShipping = 0;
            }

            this.QuantityReserved = this.QuantityOrdered;
            this.QuantityShortFalled = this.QuantityOrdered - this.QuantityRequestsShipping;

            this.ReservedFromNonSerialisedInventoryItem.OnDerive(x => x.WithDerivation(derivation));
        }

        private void DecreasePendingShipmentQuantity(IDerivation derivation, decimal diff)
        {
            var pendingShipment = this.ShipToParty.AppsGetPendingCustomerShipmentForStore(this.ShipToAddress, this.SalesOrderWhereSalesOrderItem.Store, this.SalesOrderWhereSalesOrderItem.ShipmentMethod);

            if (pendingShipment != null)
            {
                foreach (ShipmentItem shipmentItem in pendingShipment.ShipmentItems)
                {
                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (orderShipment.OrderItem.Equals(this))
                        {
                            this.QuantityPendingShipment -= diff;
                            pendingShipment.AppsOnDeriveQuantityDecreased(derivation, shipmentItem, this, diff);
                            break;
                        }
                    }
                }
            }
        }

        public List<PriceComponent> GetPriceComponents()
        {
            var priceComponents = new List<PriceComponent>();

            var extent = new PriceComponents(this.Strategy.Session).Extent();
            if (this.ExistProduct)
            {
                foreach (PriceComponent priceComponent in extent)
                {
                    if (priceComponent.ExistProduct && priceComponent.Product.Equals(this.Product) && !priceComponent.ExistProductFeature &&
                        priceComponent.FromDate <= this.SalesOrderWhereSalesOrderItem.OrderDate &&
                        (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesOrderWhereSalesOrderItem.OrderDate))
                    {
                        priceComponents.Add(priceComponent);
                    }
                }

                if (priceComponents.Count == 0 && this.Product.ExistProductWhereVariant)
                {
                    extent = new PriceComponents(this.Strategy.Session).Extent();
                    foreach (PriceComponent priceComponent in extent)
                    {
                        if (priceComponent.ExistProduct && priceComponent.Product.Equals(this.Product.ProductWhereVariant) && !priceComponent.ExistProductFeature &&
                            priceComponent.FromDate <= this.SalesOrderWhereSalesOrderItem.OrderDate &&
                            (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesOrderWhereSalesOrderItem.OrderDate))
                        {
                            priceComponents.Add(priceComponent);
                        }
                    }
                }
            }

            if (this.ExistProductFeature && !this.ExistSalesOrderItemWhereOrderedWithFeature)
            {
                foreach (PriceComponent priceComponent in extent)
                {
                    if (priceComponent.ExistProductFeature && priceComponent.ProductFeature.Equals(this.ProductFeature) && !priceComponent.ExistProduct &&
                        priceComponent.FromDate <= this.SalesOrderWhereSalesOrderItem.OrderDate &&
                        (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesOrderWhereSalesOrderItem.OrderDate))
                    {
                        priceComponents.Add(priceComponent);
                    }
                }
            }

            if (this.ExistProductFeature && this.ExistSalesOrderItemWhereOrderedWithFeature)
            {
                extent = new PriceComponents(this.Strategy.Session).Extent();
                var found = false;
                foreach (PriceComponent priceComponent in extent)
                {
                    if (priceComponent.ExistProduct && priceComponent.Product.Equals(this.SalesOrderItemWhereOrderedWithFeature.Product) &&
                        priceComponent.ExistProductFeature && priceComponent.ProductFeature.Equals(this.ProductFeature) &&
                        priceComponent.FromDate <= this.SalesOrderWhereSalesOrderItem.OrderDate &&
                        (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesOrderWhereSalesOrderItem.OrderDate))
                    {
                        found = true;
                        priceComponents.Add(priceComponent);
                    }
                }

                if (!found)
                {
                    foreach (PriceComponent priceComponent in extent)
                    {
                        if (priceComponent.ExistProductFeature && priceComponent.ProductFeature.Equals(this.ProductFeature) &&
                            priceComponent.FromDate <= this.SalesOrderWhereSalesOrderItem.OrderDate &&
                            (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesOrderWhereSalesOrderItem.OrderDate))
                        {
                            priceComponents.Add(priceComponent);
                        }
                    }
                }
            }

            // Discounts and surcharges can be specified without product or product feature, these need te be added to collection of pricecomponents
            extent = new PriceComponents(this.Strategy.Session).Extent();
            foreach (PriceComponent priceComponent in extent)
            {
                if (!priceComponent.ExistProduct && !priceComponent.ExistPart && !priceComponent.ExistProductFeature &&
                    priceComponent.FromDate <= this.SalesOrderWhereSalesOrderItem.OrderDate &&
                    (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesOrderWhereSalesOrderItem.OrderDate))
                {
                    priceComponents.Add(priceComponent);
                }
            }

            return priceComponents;
        }

        public void AppsOnDeriveAddToShipping(IDerivation derivation, decimal quantity)
        {
            this.QuantityRequestsShipping += quantity;
            this.QuantityShortFalled -= quantity;
        }

        public void AppsShipNow(IDerivation derivation)
        {
            var order = (SalesOrder)this.SalesOrderWhereSalesOrderItem;
            if (order.ScheduledManually)
            {
                var quantity = this.QuantityShipNow ?? 0;

                if (this.ExistReservedFromNonSerialisedInventoryItem && quantity > this.ReservedFromNonSerialisedInventoryItem.AvailableToPromise)
                {
                    derivation.Validation.AddError(this, M.SalesOrderItem.QuantityShipNow, ErrorMessages.SalesOrderItemQuantityToShipNowNotAvailable);
                }
                else if (quantity > this.QuantityOrdered)
                {
                    derivation.Validation.AddError(this, M.SalesOrderItem.QuantityShipNow, ErrorMessages.SalesOrderItemQuantityToShipNowIsLargerThanQuantityOrdered);
                }
                else if (quantity > this.QuantityOrdered - this.QuantityShipped - this.QuantityPendingShipment + this.QuantityReturned)
                {
                    derivation.Validation.AddError(this, M.SalesOrderItem.QuantityShipNow, ErrorMessages.SalesOrderItemQuantityToShipNowIsLargerThanQuantityRemaining);
                }
                else
                {
                    if (quantity > 0)
                    {
                        this.QuantityReserved += quantity;
                        this.QuantityRequestsShipping += quantity;
                    }
                    else
                    {
                        this.DecreasePendingShipmentQuantity(derivation, 0 - quantity);
                    }

                    this.QuantityShipNow = 0;
                }
            }
        }

        public void AppsOnDeriveSubtractFromShipping(IDerivation derivation, decimal quantity)
        {
            this.QuantityRequestsShipping -= quantity;
            if (this.QuantityRequestsShipping < 0)
            {
                this.QuantityRequestsShipping = 0;
            }

            this.QuantityShortFalled = this.QuantityOrdered - this.QuantityRequestsShipping;
        }

        private bool AppsIsSubTotalItem =>
            this.ExistInvoiceItemType &&
            (this.InvoiceItemType.Equals(new InvoiceItemTypes(this.Strategy.Session).ProductItem)
            || this.InvoiceItemType.Equals(new InvoiceItemTypes(this.Strategy.Session).PartItem));
    }
}