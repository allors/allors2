namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Globalization;
    using Resources;

    public partial class SalesOrderItem
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public decimal PriceAdjustment
        {
            get
            {
                return this.TotalSurcharge - this.TotalDiscount;
            }
        }

        public decimal PriceAdjustmentAsPercentage
        {
            get
            {
                return decimal.Round(((this.TotalSurcharge - this.TotalDiscount) / this.TotalBasePrice) * 100, 2);
            }
        }

        public string GetActualUnitBasePriceAsCurrencyString
        {
            get
            {
                return this.ExistActualUnitPrice ? DecimalExtensions.AsCurrencyString(this.ActualUnitPrice, this.SalesOrderWhereSalesOrderItem.CurrencyFormat) : string.Empty;
            }
        }

        public string GetExtraDiscountAmountAsCurrencyString
        {
            get
            {
                if (this.ExistDiscountAdjustment)
                {
                    if (this.DiscountAdjustment.ExistAmount)
                    {
                        return this.DiscountAdjustment.Amount.AsCurrencyString(this.SalesOrderWhereSalesOrderItem.CurrencyFormat);
                    }
                }

                return string.Empty;
            }
        }

        public string GetExtraDiscountPercentage
        {
            get
            {
                if (this.ExistDiscountAdjustment)
                {
                    if (this.DiscountAdjustment.Percentage.HasValue)
                    {
                        return this.DiscountAdjustment.Percentage.Value.ToString(new CultureInfo(this.SalesOrderWhereSalesOrderItem.Locale.Name));
                    }
                }

                return string.Empty;
            }
        }

        public string GetDiscountAsPercentageString
        {
            get
            {
                if (this.ExistTotalDiscountAsPercentage)
                {
                    return this.TotalDiscountAsPercentage.ToString(new CultureInfo(this.SalesOrderWhereSalesOrderItem.Locale.Name));
                }

                return string.Empty;
            }
        }

        public string GetSurchargeAsPercentageString
        {
            get
            {
                if (this.ExistTotalSurchargeAsPercentage)
                {
                    return this.TotalSurchargeAsPercentage.ToString(new CultureInfo(this.SalesOrderWhereSalesOrderItem.Locale.Name));
                }

                return string.Empty;
            }
        }

        public string GetUnitBasePriceAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.UnitBasePrice, this.SalesOrderWhereSalesOrderItem.CurrencyFormat);
            }
        }

        public string GetPriceAdjustmentAsCurrencyString
        {
            get
            {
                return this.PriceAdjustment.AsCurrencyString(this.SalesOrderWhereSalesOrderItem.CurrencyFormat);
            }
        }

        public string GetPriceAdjustmentAsPercentageString
        {
            get
            {
                return this.PriceAdjustmentAsPercentage.ToString("##.##");
            }
        }

        public string GetNothingAsCurrencyString
        {
            get
            {
                const decimal Nothing = 0;
                return Nothing.AsCurrencyString(this.SalesOrderWhereSalesOrderItem.CurrencyFormat);
            }
        }

        public string GetTotalExVatAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalExVat, this.SalesOrderWhereSalesOrderItem.CurrencyFormat);
            }
        }

        public Party ItemDifferentShippingParty
        {
            get
            {
                if (this.ExistAssignedShipToParty && !this.SalesOrderWhereSalesOrderItem.ShipToCustomer.Equals(this.AssignedShipToParty))
                {
                    return this.AssignedShipToParty;
                }

                return null;
            }
        }

        public PostalAddress ItemDifferentShippingAddress
        {
            get
            {
                if (this.ExistAssignedShipToAddress && !this.SalesOrderWhereSalesOrderItem.ShipToAddress.Equals(this.AssignedShipToAddress))
                {
                    return this.AssignedShipToAddress;
                }

                return null;
            }
        }

        public bool IsValid
        {
            get
            {
                if (this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Cancelled) ||
                    this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Rejected) ||
                    this.QuantityOrdered == 0 ||
                    (this.ExistCalculatedUnitPrice && this.CalculatedUnitPrice == 0))
                {
                    return false;
                }

                return true;
            }
        }

        public void SetActualDiscountAmount(decimal amount)
        {
            if (!this.ExistDiscountAdjustment)
            {
                this.DiscountAdjustment = new DiscountAdjustmentBuilder(this.Strategy.Session).Build();
            }

            this.DiscountAdjustment.Amount = amount;
            this.DiscountAdjustment.RemovePercentage();
        }

        public void SetActualDiscountPercentage(decimal percentage)
        {
            if (!this.ExistDiscountAdjustment)
            {
                this.DiscountAdjustment = new DiscountAdjustmentBuilder(this.Strategy.Session).Build();
            }

            this.DiscountAdjustment.Percentage = percentage;
            this.DiscountAdjustment.RemoveAmount();
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            

            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).Created;
            }

            if (!this.ExistQuantityOrdered)
            {
                this.QuantityOrdered = 0;
            }

            if (!this.ExistQuantityPicked)
            {
                this.QuantityPicked = 0;
            }

            if (!this.ExistQuantityRequestsShipping)
            {
                this.QuantityRequestsShipping = 0;
            }

            if (!this.ExistQuantityReserved)
            {
                this.QuantityReserved = 0;
            }

            if (!this.ExistQuantityReturned)
            {
                this.QuantityReturned = 0;
            }

            if (!this.ExistQuantityShipped)
            {
                this.QuantityShipped = 0;
            }

            if (!this.ExistQuantityPendingShipment)
            {
                this.QuantityPendingShipment = 0;
            }

            if (!this.ExistQuantityShortFalled)
            {
                this.QuantityShortFalled = 0;
            }

            if (!this.ExistQuantityShipNow)
            {
                this.QuantityShipNow = 0;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistSalesOrderWhereSalesOrderItem)
            {
                derivation.AddDependency(this.SalesOrderWhereSalesOrderItem, this);
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
                derivation.Log.AddError(this, SalesOrderItems.Meta.Product, ErrorMessages.SalesOrderItemProductChangeNotAllowed);
            }
            else
            {
                this.PreviousProduct = this.Product;
            }

            if (this.ExistSalesOrderItemWhereOrderedWithFeature && !this.ExistProductFeature)
            {
                derivation.Log.AssertExists(this, SalesOrderItems.Meta.ProductFeature);
            }

            if (this.ExistProduct && this.ExistQuantityOrdered && this.QuantityOrdered < this.QuantityShipped)
            {
                derivation.Log.AddError(this, SalesOrderItems.Meta.QuantityOrdered, ErrorMessages.SalesOrderItemLessThanAlreadeyShipped);
            }

            if (!this.ExistAssignedShipToAddress && this.ExistAssignedShipToParty)
            {
                this.AssignedShipToAddress = this.AssignedShipToParty.ShippingAddress;
            }

            derivation.Log.AssertAtLeastOne(this, SalesOrderItems.Meta.Product, SalesOrderItems.Meta.ProductFeature);
            derivation.Log.AssertExistsAtMostOne(this, SalesOrderItems.Meta.Product, SalesOrderItems.Meta.ProductFeature);
            derivation.Log.AssertExistsAtMostOne(this, SalesOrderItems.Meta.ActualUnitPrice, SalesOrderItems.Meta.DiscountAdjustment, SalesOrderItems.Meta.SurchargeAdjustment);
            derivation.Log.AssertExistsAtMostOne(this, SalesOrderItems.Meta.RequiredMarkupPercentage, SalesOrderItems.Meta.RequiredProfitMargin, SalesOrderItems.Meta.DiscountAdjustment, SalesOrderItems.Meta.SurchargeAdjustment);

            this.DerivePrices(derivation, 0 , 0);
            this.DeriveDeliveryDate(derivation);
            this.DeriveVatRegime(derivation);
            this.DeriveIsValidOrderItem(derivation);

            if (this.QuantityShipNow != 0)
            {
                this.AppsShipNow(derivation);
                this.ReservedFromInventoryItem.OnDerive(x => x.WithDerivation(derivation));
                this.SalesOrderWhereSalesOrderItem.OnDerive(x => x.WithDerivation(derivation));
            }

            this.DeriveCurrentObjectState(derivation);
        }

        public void AppsOnDeriveIsValidOrderItem(IDerivation derivation)
        {
            if (this.ExistSalesOrderWhereSalesOrderItem)
            {
                this.SalesOrderWhereSalesOrderItem.RemoveValidOrderItem(this);

                if (this.IsValid)
                {
                    this.SalesOrderWhereSalesOrderItem.AddValidOrderItem(this);
                }
            }
        }

        public void AppsOnDeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistOrderWhereValidOrderItem)
            {
                var order = this.SalesOrderWhereSalesOrderItem;

                if (order.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).InProcess))
                {
                    if (this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Created))
                    {
                        this.Confirm();
                    }
                }

                if (order.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).Finished))
                {
                    this.Finish();
                }

                if (order.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).Cancelled))
                {
                    this.Cancel();
                }

                if (order.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).Rejected))
                {
                    this.Reject();
                }
            }

            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {
                var currentStatus = new SalesOrderItemStatusBuilder(this.Strategy.Session).WithSalesOrderItemObjectState(this.CurrentObjectState).Build();
                this.AddOrderItemStatus(currentStatus);
                this.CurrentOrderItemStatus = currentStatus;
            }

            if (this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).InProcess))
            {
                this.DeriveReservedFromInventoryItem(derivation);
                this.DeriveQuantities(derivation);

                this.PreviousQuantity = this.QuantityOrdered;
                this.PreviousReservedFromInventoryItem = this.ReservedFromInventoryItem;
                this.PreviousProduct = this.Product;
            }

            if (this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Cancelled) ||
                this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Rejected))
            {
                this.DeriveQuantities(derivation);
            }

            if (this.ExistCurrentObjectState)
            {
                this.CurrentObjectState.Process(this);
            }
        }

        public void AppsCancel(OrderItemCancel method)
        {
            this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).Cancelled;
        }

        public void AppsConfirm(OrderItemConfirm method)
        {
            this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).InProcess;
        }

        public void AppsReject(OrderItemReject method)
        {
            this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).Rejected;
        }

        public void AppsApprove(OrderItemApprove method)
        {
            this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).InProcess;
        }

        public void AppsContinue(SalesOrderItemContinue method)
        {
            this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).InProcess;
        }

        public void AppsFinish(OrderItemFinish method)
        {
            this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).Finished;
        }

        public void AppsOnDeriveDeliveryDate(IDerivation derivation)
        {
            if (this.AssignedDeliveryDate.HasValue)
            {
                this.DeliveryDate = this.AssignedDeliveryDate.Value;
            }
            else if (this.ExistSalesOrderWhereSalesOrderItem && this.SalesOrderWhereSalesOrderItem.ExistDeliveryDate)
            {
                this.DeliveryDate = this.SalesOrderWhereSalesOrderItem.DeliveryDate;
            }
        }
        
        public void AppsOnDeriveShipTo(IDerivation derivation)
        {
            if (this.ExistSalesOrderWhereSalesOrderItem)
            {
                this.ShipToAddress = this.ExistAssignedShipToAddress ? this.AssignedShipToAddress : this.SalesOrderWhereSalesOrderItem.ShipToAddress;
                this.ShipToParty = this.ExistAssignedShipToParty ? this.AssignedShipToParty : this.SalesOrderWhereSalesOrderItem.ShipToCustomer;
            }
        }

        public void AppsOnDeriveReservedFromInventoryItem(IDerivation derivation)
        {
            if (this.ExistProduct)
            {
                var good = this.Product as Good;
                if (good != null)
                {
                    if (good.ExistFinishedGood)
                    {
                        if (!this.ExistReservedFromInventoryItem || !this.ReservedFromInventoryItem.Part.Equals(good.FinishedGood))
                        {
                            var inventoryItems = good.FinishedGood.InventoryItemsWherePart;
                            inventoryItems.Filter.AddEquals(InventoryItems.Meta.Facility, SalesOrderWhereSalesOrderItem.TakenByInternalOrganisation.DefaultFacility);
                            this.ReservedFromInventoryItem = inventoryItems.First as NonSerializedInventoryItem;
                        }
                    }
                    else
                    {
                        if (!this.ExistReservedFromInventoryItem || !this.ReservedFromInventoryItem.Good.Equals(good))
                        {
                            var inventoryItems = good.InventoryItemsWhereGood;
                            inventoryItems.Filter.AddEquals(InventoryItems.Meta.Facility, SalesOrderWhereSalesOrderItem.TakenByInternalOrganisation.DefaultFacility);
                            this.ReservedFromInventoryItem = inventoryItems.First as NonSerializedInventoryItem;
                        }
                    }
                }
            }
        }

        public void AppsOnDeriveQuantities(IDerivation derivation)
        {
            if (this.SalesOrderWhereSalesOrderItem.ScheduledManually)
            {
                this.AppsOnDeriveQuantitiesmanualShipment(derivation);
            }
            else
            {
                this.AppsOnDeriveQuantitiesAutomaticShipment(derivation);
            }
        }

        public void AppsOnDeriveQuantitiesmanualShipment(IDerivation derivation)
        {
            foreach (SalesOrderItem item in this.OrderedWithFeatures)
            {
                this.QuantityOrdered = item.QuantityOrdered;
            }

            if (this.ExistReservedFromInventoryItem &&
                this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).InProcess) &&
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

                        this.ReservedFromInventoryItem.OnDerive(x => x.WithDerivation(derivation));
                    }
                }

                this.PreviousQuantity = this.QuantityOrdered;
            }

            if (this.ExistReservedFromInventoryItem &&
                (this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Cancelled) ||
                this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Rejected)))
            {
                if (this.ExistQuantityPendingShipment)
                {
                    this.DecreasePendingShipmentQuantity(derivation, this.QuantityPendingShipment);
                }

                this.ReservedFromInventoryItem.OnDerive(x => x.WithDerivation(derivation));
            }
        }

        public void AppsOnDeriveQuantitiesAutomaticShipment(IDerivation derivation)
        {
            foreach (SalesOrderItem item in this.OrderedWithFeatures)
            {
                this.QuantityOrdered = item.QuantityOrdered;
            }

            if (this.ExistReservedFromInventoryItem && this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).InProcess))
            {
                if (this.ExistPreviousReservedFromInventoryItem && !this.ReservedFromInventoryItem.Equals(this.PreviousReservedFromInventoryItem))
                {
                    this.PreviousReservedFromInventoryItem.OnDerive(x => x.WithDerivation(derivation));

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

                            if (diff > this.ReservedFromInventoryItem.AvailableToPromise)
                            {
                                this.QuantityRequestsShipping += this.ReservedFromInventoryItem.AvailableToPromise;
                                this.QuantityShortFalled += diff - this.ReservedFromInventoryItem.AvailableToPromise;
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

                        this.ReservedFromInventoryItem.OnDerive(x => x.WithDerivation(derivation));
                    }

                    //// When first Confirmed.
                    if (!this.ExistPreviousQuantity)
                    {
                        this.SetQuantitiesWithInventoryFirstTime(derivation);
                    }
                }

                this.PreviousQuantity = this.QuantityOrdered;
            }

            if (this.ExistReservedFromInventoryItem &&
                (this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Cancelled) ||
                this.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Rejected)))
            {
                if (this.ExistQuantityPendingShipment)
                {
                    this.DecreasePendingShipmentQuantity(derivation, this.QuantityPendingShipment);
                }

                this.ReservedFromInventoryItem.OnDerive(x => x.WithDerivation(derivation));
            }
        }

        private void SetQuantitiesWithInventoryFirstTime(IDerivation derivation)
        {
            this.QuantityReserved = 0;
            this.QuantityRequestsShipping = 0;
            this.QuantityShortFalled = 0;

            this.QuantityRequestsShipping = this.QuantityOrdered > this.ReservedFromInventoryItem.AvailableToPromise ?
                this.ReservedFromInventoryItem.AvailableToPromise : this.QuantityOrdered;

            if (this.QuantityRequestsShipping < 0)
            {
                this.QuantityRequestsShipping = 0;
            }

            this.QuantityReserved = this.QuantityOrdered;
            this.QuantityShortFalled = this.QuantityOrdered - this.QuantityRequestsShipping;

            this.ReservedFromInventoryItem.OnDerive(x => x.WithDerivation(derivation));
        }

        private void DecreasePendingShipmentQuantity(IDerivation derivation, decimal diff)
        {
            var pendingShipment = this.ShipToParty.GetPendingCustomerShipmentForStore(this.ShipToAddress, this.SalesOrderWhereSalesOrderItem.Store, this.SalesOrderWhereSalesOrderItem.ShipmentMethod);

            if (pendingShipment != null)
            {
                foreach (ShipmentItem shipmentItem in pendingShipment.ShipmentItems)
                {
                    foreach (OrderShipment orderShipment in shipmentItem.OrderShipmentsWhereShipmentItem)
                    {
                        if (orderShipment.SalesOrderItem.Equals(this))
                        {
                            this.QuantityPendingShipment -= diff;
                            pendingShipment.DeriveQuantityDecreased(derivation, shipmentItem, this, diff);
                            break;
                        }
                    }
                }
            }
        }

        public void AppsOnDeriveCurrentOrderStatus(IDerivation derivation)
        {
            if (this.ExistCurrentShipmentStatus && this.CurrentShipmentStatus.SalesOrderItemObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).PartiallyShipped))
            {
                this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).PartiallyShipped;
                this.DeriveCurrentObjectState(derivation);
            }

            if (this.ExistCurrentShipmentStatus && this.CurrentShipmentStatus.SalesOrderItemObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Shipped))
            {
                this.CurrentObjectState = new SalesOrderItemObjectStates(this.Strategy.Session).Completed;
                this.DeriveCurrentObjectState(derivation);
            }
        }

        public void AppsOnDeriveCurrentShipmentStatus(IDerivation derivation)
        {
            if (this.QuantityShipped > 0)
            {
                if (this.QuantityShipped < this.QuantityOrdered)
                {
                    if (!this.ExistCurrentShipmentStatus || !this.CurrentShipmentStatus.SalesOrderItemObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).PartiallyShipped))
                    {
                        this.CurrentShipmentStatus = new SalesOrderItemStatusBuilder(this.Strategy.Session).WithSalesOrderItemObjectState(new SalesOrderItemObjectStates(this.Strategy.Session).PartiallyShipped).Build();
                    }
                }
                else
                {
                    if (!this.ExistCurrentShipmentStatus || !this.CurrentShipmentStatus.SalesOrderItemObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Shipped))
                    {
                        this.CurrentShipmentStatus = new SalesOrderItemStatusBuilder(this.Strategy.Session).WithSalesOrderItemObjectState(new SalesOrderItemObjectStates(this.Strategy.Session).Shipped).Build();
                    }
                }

                this.AddShipmentStatus(this.CurrentShipmentStatus);
            }

            this.DeriveCurrentOrderStatus(derivation);
        }
        
        public void AppsCalculatePurchasePrice(IDerivation derivation)
        {
            this.UnitPurchasePrice = 0;

            if (this.ExistProduct &&
                this.Product.ExistSupplierOfferingsWhereProduct &&
                this.Product.SupplierOfferingsWhereProduct.Count == 1 &&
                this.Product.SupplierOfferingsWhereProduct.First.ExistProductPurchasePrices)
            {
                ProductPurchasePrice productPurchasePrice = null;

                var prices = this.Product.SupplierOfferingsWhereProduct.First.ProductPurchasePrices;
                foreach (ProductPurchasePrice purchasePrice in prices)
                {
                    if (purchasePrice.FromDate <= this.SalesOrderWhereSalesOrderItem.OrderDate &&
                        (!purchasePrice.ExistThroughDate || purchasePrice.ThroughDate >= this.SalesOrderWhereSalesOrderItem.OrderDate))
                    {
                        productPurchasePrice = purchasePrice;
                    }
                }

                if (productPurchasePrice == null)
                {
                    var index = this.Product.SupplierOfferingsWhereProduct.First.ProductPurchasePrices.Count;
                    var lastKownPrice = this.Product.SupplierOfferingsWhereProduct.First.ProductPurchasePrices[index - 1];
                    productPurchasePrice = lastKownPrice;
                }

                if (productPurchasePrice != null)
                {
                    this.UnitPurchasePrice = productPurchasePrice.Price;
                    if (!productPurchasePrice.UnitOfMeasure.Equals(this.Product.UnitOfMeasure))
                    {
                        foreach (UnitOfMeasureConversion unitOfMeasureConversion in productPurchasePrice.UnitOfMeasure.UnitOfMeasureConversions)
                        {
                            if (unitOfMeasureConversion.ToUnitOfMeasure.Equals(this.Product.UnitOfMeasure))
                            {
                                this.UnitPurchasePrice = decimal.Round(this.UnitPurchasePrice * (1 / unitOfMeasureConversion.ConversionFactor), 2);
                            }
                        }
                    }
                }
            }
        }

        public void AppsCalculateUnitPrice(IDerivation derivation)
        {
            if (this.RequiredMarkupPercentage.HasValue && this.UnitPurchasePrice > 0)
            {
                this.ActualUnitPrice = decimal.Round((1 + (this.RequiredMarkupPercentage.Value / 100)) * this.UnitPurchasePrice, 2);
            }

            if (this.RequiredProfitMargin.HasValue && this.UnitPurchasePrice > 0)
            {
                this.ActualUnitPrice = decimal.Round(this.UnitPurchasePrice / (1 - (this.RequiredProfitMargin.Value / 100)), 2);
            }
        }

        public void AppsOnDerivePrices(IDerivation derivation, decimal quantityOrdered, decimal totalBasePrice)
        {
            this.RemoveCurrentPriceComponents();

            this.UnitBasePrice = 0;
            this.UnitDiscount = 0;
            this.UnitSurcharge = 0;
            this.CalculatedUnitPrice = 0;
            decimal discountAdjustmentAmount = 0;
            decimal surchargeAdjustmentAmount = 0;

            var internalOrganisation = this.ExistSalesOrderWhereSalesOrderItem ? this.SalesOrderWhereSalesOrderItem.TakenByInternalOrganisation : Singleton.Instance(this.strategy.Session).DefaultInternalOrganisation;
            var customer = this.SalesOrderWhereSalesOrderItem.BillToCustomer;
            var salesOrder = this.SalesOrderWhereSalesOrderItem;

            var priceComponents = this.GetPriceComponents(internalOrganisation);

            PartyRevenueHistory partyRevenueHistory = null;
            Dictionary<ProductCategory, PartyProductCategoryRevenueHistory> partyProductCategoryRevenueHistoryByProductCategory = null;
            Extent<PartyPackageRevenueHistory> partyPackageRevenuesHistories = null;

            if (customer != null)
            {
                var partyRevenueHistories = customer.PartyRevenueHistoriesWhereParty;
                partyRevenueHistories.Filter.AddEquals(PartyRevenueHistories.Meta.InternalOrganisation, internalOrganisation);
                partyRevenueHistory = partyRevenueHistories.First;

                partyProductCategoryRevenueHistoryByProductCategory = PartyProductCategoryRevenueHistories.PartyProductCategoryRevenueHistoryByProductCategory(internalOrganisation, customer);

                partyPackageRevenuesHistories = customer.PartyPackageRevenueHistoriesWhereParty;
                partyPackageRevenuesHistories.Filter.AddEquals(PartyPackageRevenueHistories.Meta.InternalOrganisation, internalOrganisation);
            }

            foreach (var priceComponent in priceComponents)
            {
                if (priceComponent.Strategy.Class.Equals(BasePrice.Meta.ObjectType))
                {
                    if (PriceComponents.IsEligible(new PriceComponents.IsEligibleParams
                    {
                        PriceComponent = priceComponent, 
                        Customer = customer, 
                        Product = this.Product, 
                        SalesOrder = salesOrder, 
                        QuantityOrdered = quantityOrdered, 
                        ValueOrdered = totalBasePrice,
                        PartyPackageRevenueHistoryList = partyPackageRevenuesHistories, 
                        PartyProductCategoryRevenueHistoryByProductCategory = partyProductCategoryRevenueHistoryByProductCategory, 
                        PartyRevenueHistory = partyRevenueHistory
                    }))
                    {
                        if (priceComponent.ExistPrice)
                        {
                            if (this.UnitBasePrice == 0 || priceComponent.Price < this.UnitBasePrice)
                            {
                                this.UnitBasePrice = priceComponent.Price.HasValue ? priceComponent.Price.Value : 0;

                                this.RemoveCurrentPriceComponents();
                                this.AddCurrentPriceComponent(priceComponent);
                            }
                        }
                    }
                }
            }

            ////SafeGuard
            if (this.ExistProduct && !this.ExistActualUnitPrice)
            {
                var invalid = true;
                foreach (BasePrice basePrice in this.CurrentPriceComponents)
                {
                    if (basePrice.Price > 0)
                    {
                        invalid = false;
                    }
                }

                if (invalid)
                {
                    this.QuantityOrdered = 0;
                }
            }

            if (!this.ExistActualUnitPrice)
            {
                //var priceComponents = this.GetPriceComponents(internalOrganisation);

                var revenueBreakDiscount = 0M;
                var revenueBreakSurcharge = 0M;

                foreach (var priceComponent in priceComponents)
                {
                    if (priceComponent.Strategy.Class.Equals(DiscountComponents.Meta.ObjectType) || priceComponent.Strategy.Class.Equals(SurchargeComponents.Meta.ObjectType))
                    {
                        if (PriceComponents.IsEligible(new PriceComponents.IsEligibleParams
                        {
                            PriceComponent = priceComponent, 
                            Customer = customer, 
                            Product = this.Product, 
                            SalesOrder = salesOrder, 
                            QuantityOrdered = quantityOrdered, 
                            ValueOrdered = totalBasePrice, 
                            PartyPackageRevenueHistoryList = partyPackageRevenuesHistories, 
                            PartyProductCategoryRevenueHistoryByProductCategory = partyProductCategoryRevenueHistoryByProductCategory, 
                            PartyRevenueHistory = partyRevenueHistory
                        }))
                        {
                            this.AddCurrentPriceComponent(priceComponent);

                            if (priceComponent.Strategy.Class.Equals(DiscountComponents.Meta.ObjectType))
                            {
                                var discountComponent = (DiscountComponent)priceComponent;
                                decimal discount;

                                if (discountComponent.Price.HasValue)
                                {
                                    discount = discountComponent.Price.Value;
                                    this.UnitDiscount += discount;
                                }
                                else
                                {
                                    var percentage = discountComponent.Percentage.HasValue ? discountComponent.Percentage.Value : 0;
                                    discount = decimal.Round((this.UnitBasePrice * percentage) / 100, 2);
                                    this.UnitDiscount += discount;
                                }

                                ////Revenuebreaks on quantity and value are mutually exclusive.
                                if (priceComponent.ExistRevenueQuantityBreak || priceComponent.ExistRevenueValueBreak)
                                {
                                    if (revenueBreakDiscount == 0)
                                    {
                                        revenueBreakDiscount = discount;
                                    }
                                    else
                                    {
                                        ////Apply highest of the two. Revert the other one. 
                                        if (discount > revenueBreakDiscount)
                                        {
                                            this.UnitDiscount -= revenueBreakDiscount;
                                        }
                                        else
                                        {
                                            this.UnitDiscount -= discount;
                                        }
                                    }
                                }
                            }

                            if (priceComponent.Strategy.Class.Equals(SurchargeComponents.Meta.ObjectType))
                            {
                                var surchargeComponent = (SurchargeComponent)priceComponent;
                                decimal surcharge;

                                if (surchargeComponent.Price.HasValue)
                                {
                                    surcharge = surchargeComponent.Price.Value;
                                    this.UnitSurcharge += surcharge;
                                }
                                else
                                {
                                    var percentage = surchargeComponent.Percentage.HasValue ? surchargeComponent.Percentage.Value : 0;
                                    surcharge = decimal.Round((this.UnitBasePrice * percentage) / 100, 2);
                                    this.UnitSurcharge += surcharge;
                                }

                                ////Revenuebreaks on quantity and value are mutually exclusive.
                                if (priceComponent.ExistRevenueQuantityBreak || priceComponent.ExistRevenueValueBreak)
                                {
                                    if (revenueBreakSurcharge == 0)
                                    {
                                        revenueBreakSurcharge = surcharge;
                                    }
                                    else
                                    {
                                        ////Apply highest of the two. Revert the other one. 
                                        if (surcharge > revenueBreakSurcharge)
                                        {
                                            this.UnitDiscount -= revenueBreakSurcharge;
                                        }
                                        else
                                        {
                                            this.UnitDiscount -= surcharge;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                var adjustmentBase = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;

                if (this.ExistDiscountAdjustment)
                {
                    if (this.DiscountAdjustment.Percentage.HasValue)
                    {
                        discountAdjustmentAmount = decimal.Round((adjustmentBase * this.DiscountAdjustment.Percentage.Value) / 100, 2);
                    }
                    else
                    {
                        discountAdjustmentAmount = this.DiscountAdjustment.Amount.HasValue ? this.DiscountAdjustment.Amount.Value : 0;
                    }

                    this.UnitDiscount += discountAdjustmentAmount;
                }

                if (this.ExistSurchargeAdjustment)
                {
                    if (this.SurchargeAdjustment.Percentage.HasValue)
                    {
                        surchargeAdjustmentAmount = decimal.Round((adjustmentBase * this.SurchargeAdjustment.Percentage.Value) / 100, 2);
                    }
                    else
                    {
                        surchargeAdjustmentAmount = this.SurchargeAdjustment.Amount.HasValue ? this.SurchargeAdjustment.Amount.Value : 0;
                    }

                    this.UnitSurcharge += surchargeAdjustmentAmount;
                }
            }

            var price = this.ActualUnitPrice.HasValue ? this.ActualUnitPrice.Value : this.UnitBasePrice;

            decimal vat = 0;
            if (this.ExistDerivedVatRate)
            {
                var vatRate = this.DerivedVatRate.Rate;
                var vatBase = price - this.UnitDiscount + this.UnitSurcharge;
                vat = decimal.Round((vatBase * vatRate) / 100, 2);
            }

            this.UnitVat = vat;
            this.TotalBasePrice = price * this.QuantityOrdered;
            this.TotalDiscount = this.UnitDiscount * this.QuantityOrdered;
            this.TotalSurcharge = this.UnitSurcharge * this.QuantityOrdered;
            this.TotalOrderAdjustment = (0 - discountAdjustmentAmount + surchargeAdjustmentAmount) * this.QuantityOrdered;

            if (this.TotalBasePrice > 0)
            {
                this.TotalDiscountAsPercentage = decimal.Round((this.TotalDiscount / this.TotalBasePrice) * 100, 2);
                this.TotalSurchargeAsPercentage = decimal.Round((this.TotalSurcharge / this.TotalBasePrice) * 100, 2);
            }

            if (this.ActualUnitPrice.HasValue)
            {
                this.CalculatedUnitPrice = this.ActualUnitPrice.Value;
            }
            else
            {
                this.CalculatedUnitPrice = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;
            }

            this.TotalExVat = this.CalculatedUnitPrice * this.QuantityOrdered;
            this.TotalVat = this.UnitVat * this.QuantityOrdered;
            this.TotalIncVat = this.TotalExVat + this.TotalVat;

            foreach (SalesOrderItem featureItem in this.OrderedWithFeatures)
            {
                this.CalculatedUnitPrice += featureItem.CalculatedUnitPrice;
                this.TotalBasePrice += featureItem.TotalBasePrice;
                this.TotalDiscount += featureItem.TotalDiscount;
                this.TotalSurcharge += featureItem.TotalSurcharge;
                this.TotalExVat += featureItem.TotalExVat;
                this.TotalVat += featureItem.TotalVat;
                this.TotalIncVat += featureItem.TotalIncVat;
            }

            var toCurrency = this.SalesOrderWhereSalesOrderItem.CustomerCurrency;
            var fromCurrency = internalOrganisation.PreferredCurrency;

            if (fromCurrency.Equals(toCurrency))
            {
                this.TotalBasePriceCustomerCurrency = this.TotalBasePrice;
                this.TotalDiscountCustomerCurrency = this.TotalDiscount;
                this.TotalSurchargeCustomerCurrency = this.TotalSurcharge;
                this.TotalExVatCustomerCurrency = this.TotalExVat;
                this.TotalVatCustomerCurrency = this.TotalVat;
                this.TotalIncVatCustomerCurrency = this.TotalIncVat;
            }
            else
            {
                this.TotalBasePriceCustomerCurrency = Currencies.ConvertCurrency(this.TotalBasePrice, fromCurrency, toCurrency);
                this.TotalDiscountCustomerCurrency = Currencies.ConvertCurrency(this.TotalDiscount, fromCurrency, toCurrency);
                this.TotalSurchargeCustomerCurrency = Currencies.ConvertCurrency(this.TotalSurcharge, fromCurrency, toCurrency);
                this.TotalExVatCustomerCurrency = Currencies.ConvertCurrency(this.TotalExVat, fromCurrency, toCurrency);
                this.TotalVatCustomerCurrency = Currencies.ConvertCurrency(this.TotalVat, fromCurrency, toCurrency);
                this.TotalIncVatCustomerCurrency = Currencies.ConvertCurrency(this.TotalIncVat, fromCurrency, toCurrency);
            }

            this.DeriveMarkupAndProfitMargin(derivation);
        }

        private List<PriceComponent> GetPriceComponents(InternalOrganisation internalOrganisation)
        {
            var priceComponents = new List<PriceComponent>();

            var extent = internalOrganisation.PriceComponentsWhereSpecifiedFor;
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
                    extent = internalOrganisation.PriceComponentsWhereSpecifiedFor;
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
                extent = internalOrganisation.PriceComponentsWhereSpecifiedFor;
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
            extent = internalOrganisation.PriceComponentsWhereSpecifiedFor;
            foreach (PriceComponent priceComponent in extent)
            {
                if (!priceComponent.ExistProduct && !priceComponent.ExistProductFeature &&
                    priceComponent.FromDate <= this.SalesOrderWhereSalesOrderItem.OrderDate &&
                    (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesOrderWhereSalesOrderItem.OrderDate))
                {
                    priceComponents.Add(priceComponent);
                }
            }

            return priceComponents;
        }

        public void AppsOnDeriveMarkupAndProfitMargin(IDerivation derivation)
        {
            this.InitialMarkupPercentage = 0;
            this.MaintainedMarkupPercentage = 0;
            this.InitialProfitMargin = 0;
            this.MaintainedProfitMargin = 0;

            ////internet wiki page on markup business
            if (this.ExistUnitPurchasePrice && this.UnitPurchasePrice != 0 && this.CalculatedUnitPrice != 0 && this.UnitBasePrice != 0)
            {
                this.InitialMarkupPercentage = decimal.Round(((this.UnitBasePrice / this.UnitPurchasePrice) - 1) * 100, 2);
                this.MaintainedMarkupPercentage = decimal.Round(((this.CalculatedUnitPrice / this.UnitPurchasePrice) - 1) * 100, 2);

                this.InitialProfitMargin = decimal.Round(((this.UnitBasePrice - this.UnitPurchasePrice) / this.UnitBasePrice) * 100, 2);
                this.MaintainedProfitMargin = decimal.Round(((this.CalculatedUnitPrice - this.UnitPurchasePrice) / this.CalculatedUnitPrice) * 100, 2);
            }
        }

        public void AppsOnDeriveOnShip(IDerivation derivation)
        {
             this.QuantityPendingShipment += this.QuantityRequestsShipping;
            this.QuantityRequestsShipping = 0;
        }

        public void AppsOnDeriveOnShipped(IDerivation derivation, decimal quantity)
        {
            this.QuantityPicked -= quantity;
            this.QuantityPendingShipment -= quantity;
            this.QuantityShipped += quantity;

            this.DeriveCurrentShipmentStatus(derivation);
        }

        public void AppsOnDeriveOnPicked(IDerivation derivation, decimal quantity)
        {
            this.QuantityPicked += quantity;
            this.QuantityReserved -= quantity;
        }

        public void AppsOnDeriveAddToShipping(IDerivation derivation, decimal quantity)
        {
            this.QuantityRequestsShipping += quantity;
            this.QuantityShortFalled -= quantity;
        }

        public void AppsShipNow(IDerivation derivation)
        {            
            if (this.SalesOrderWhereSalesOrderItem.ScheduledManually)
            {
                var quantity = this.QuantityShipNow.HasValue ? this.QuantityShipNow.Value : 0;

                if (quantity > this.ReservedFromInventoryItem.AvailableToPromise)
                {
                    derivation.Log.AddError(this, SalesOrderItems.Meta.QuantityShipNow, ErrorMessages.SalesOrderItemQuantityToShipNowNotAvailable);
                }
                else if (quantity > this.QuantityOrdered)
                {
                    derivation.Log.AddError(this, SalesOrderItems.Meta.QuantityShipNow, ErrorMessages.SalesOrderItemQuantityToShipNowIsLargerThanQuantityOrdered);
                }
                else if (quantity > this.QuantityOrdered - this.QuantityShipped - this.QuantityPendingShipment + this.QuantityReturned)
                {
                    derivation.Log.AddError(this, SalesOrderItems.Meta.QuantityShipNow, ErrorMessages.SalesOrderItemQuantityToShipNowIsLargerThanQuantityRemaining);
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

        public void AppsOnDeriveSalesRep(IDerivation derivation)
        {
            this.SalesRep = null;
            var customer = this.ShipToParty as Organisation;
            if (customer != null)
            {
                if (this.ExistProduct)
                {
                    this.SalesRep = SalesRepRelationships.SalesRep(customer, Product.PrimaryProductCategory, this.SalesOrderWhereSalesOrderItem.OrderDate);
                }
                else
                {
                    this.SalesRep = SalesRepRelationships.SalesRep(customer, null, this.SalesOrderWhereSalesOrderItem.OrderDate);
                }
            }
        }

        public void AppsOnDeriveVatRegime(IDerivation derivation)
        {
            if (this.ExistSalesOrderWhereSalesOrderItem)
            {
                this.VatRegime = this.ExistAssignedVatRegime ? this.AssignedVatRegime : this.SalesOrderWhereSalesOrderItem.VatRegime;

                this.DeriveVatRate(derivation);
            }
        }

        public void AppsOnDeriveVatRate(IDerivation derivation)
        {
            if (!this.ExistDerivedVatRate && this.ExistVatRegime && this.VatRegime.ExistVatRate)
            {
                this.DerivedVatRate = this.VatRegime.VatRate;
            }

            if (!this.ExistDerivedVatRate &&  (this.ExistProduct || this.ExistProductFeature))
            {
                this.DerivedVatRate = this.ExistProduct ? this.Product.VatRate : this.ProductFeature.VatRate;
            }
        }

        public void AppsOnDeriveCurrentPaymentStatus(IDerivation derivation)
        {
            SalesOrderItemObjectState state = null;
            foreach (OrderShipment orderShipment in this.OrderShipmentsWhereSalesOrderItem)
            {
                foreach (SalesInvoiceItem invoiceItem in orderShipment.ShipmentItem.InvoiceItems)
                {
                    if (state == null && invoiceItem.SalesInvoiceWhereSalesInvoiceItem.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Paid))
                    {
                        state = new SalesOrderItemObjectStates(this.Strategy.Session).Paid;
                    }
                    else
                    {
                        if (state != null && state.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Paid))
                        {
                            state = new SalesOrderItemObjectStates(this.Strategy.Session).PartiallyPaid;
                        }
                    }
                }
            }

            if (state != null)
            {
                if (!this.ExistCurrentPaymentStatus || !this.CurrentPaymentStatus.SalesOrderItemObjectState.Equals(state))
                {
                    this.CurrentPaymentStatus = new SalesOrderItemStatusBuilder(this.Strategy.Session).WithSalesOrderItemObjectState(state).Build();
                    this.AddPaymentStatus(this.CurrentPaymentStatus);
                }
            }
        }
    }
}