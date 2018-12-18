namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Meta;

    public partial class SalesInvoiceItem
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.SalesInvoiceItem, M.SalesInvoiceItem.SalesInvoiceItemState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public decimal PriceAdjustment => this.TotalSurcharge - this.TotalDiscount;

        public decimal PriceAdjustmentAsPercentage => Math.Round(((this.TotalSurcharge - this.TotalDiscount) / this.TotalBasePrice) * 100, 2);

        internal bool IsDeletable =>
            !this.ExistOrderItemBillingsWhereInvoiceItem &&
            !this.ExistShipmentItemBillingsWhereInvoiceItem &&
            !this.ExistWorkEffortBillingsWhereInvoiceItem &&
            !this.ExistServiceEntryBillingsWhereInvoiceItem;

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
            if (!this.ExistSalesInvoiceItemState)
            {
                this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).ReadyForPosting;
            }

            if (this.ExistProduct && !this.ExistInvoiceItemType)
            {
                this.InvoiceItemType = new InvoiceItemTypes(this.Strategy.Session).ProductItem;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistSalesInvoiceWhereSalesInvoiceItem)
            {
                derivation.AddDependency(this.SalesInvoiceWhereSalesInvoiceItem, this);
            }

            if (this.ExistPaymentApplicationsWhereInvoiceItem)
            {
                foreach (PaymentApplication paymentApplication in this.PaymentApplicationsWhereInvoiceItem)
                {
                    derivation.AddDependency(this, paymentApplication);
                }
            }

            foreach (OrderItemBilling orderItemBilling in this.OrderItemBillingsWhereInvoiceItem)
            {
                derivation.AddDependency(orderItemBilling.OrderItem, this);
            }

            foreach (ShipmentItemBilling shipmentItemBilling in this.ShipmentItemBillingsWhereInvoiceItem)
            {
                foreach (OrderShipment orderShipment in shipmentItemBilling.ShipmentItem.OrderShipmentsWhereShipmentItem)
                {
                    derivation.AddDependency(orderShipment.OrderItem, this);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveAmountPaid(derivation);

            derivation.Validation.AssertExistsAtMostOne(this, M.SalesInvoiceItem.Product, M.SalesInvoiceItem.ProductFeature, M.SalesInvoiceItem.SerialisedInventoryItem, M.SalesInvoiceItem.TimeEntries);

            if (this.ExistInvoiceItemType && this.Quantity == 0)
            {
                this.Quantity = 1;
            }

            if (this.ExistInvoiceItemType && this.IsSubTotalItem && this.Quantity <= 0)
            {
                derivation.Validation.AssertExists(this, this.Meta.Quantity);
            }
        }

        public void AppsWriteOff(IDerivation derivation)
        {
            this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).WrittenOff;
        }

        public void AppsCancel(IDerivation derivation)
        {
            this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).Cancelled;
        }

        public void AppsSend(IDerivation derivation)
        {
            this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).Sent;
        }

        public void AppsPaymentReceived(IDerivation derivation)
        {
            if (this.AmountPaid < this.TotalIncVat)
            {
                this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).PartiallyPaid;
            }
            else
            {
                this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).Paid;
            }
        }

        public void AppsOnDeriveCurrentPaymentStatus(IDerivation derivation)
        {
            if (!this.SalesInvoiceItemState.Equals(new SalesInvoiceItemStates(this.Strategy.Session).Cancelled) &&
                !this.SalesInvoiceItemState.Equals(new SalesInvoiceItemStates(this.Strategy.Session).WrittenOff))
            {
                if (this.ExistAmountPaid && this.AmountPaid > 0 && this.AmountPaid < this.TotalIncVat)
                {
                    if (!this.SalesInvoiceItemState.Equals(new SalesInvoiceItemStates(this.Strategy.Session).PartiallyPaid))
                    {
                        this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).PartiallyPaid;
                        this.AppsOnDeriveCurrentObjectState(derivation);
                    }
                }

                if (this.ExistAmountPaid && this.AmountPaid > 0 && this.AmountPaid >= this.TotalIncVat)
                {
                    if (!this.SalesInvoiceItemState.Equals(new SalesInvoiceItemStates(this.Strategy.Session).Paid))
                    {
                        this.SalesInvoiceItemState = new SalesInvoiceItemStates(this.Strategy.Session).Paid;
                        this.AppsOnDeriveCurrentObjectState(derivation);
                    }
                }
            }
        }

        public void AppsOnDeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistSalesInvoiceWhereSalesInvoiceItem)
            {
                var invoice = this.SalesInvoiceWhereSalesInvoiceItem;

                if (invoice.SalesInvoiceState.Equals(new SalesInvoiceStates(this.Strategy.Session).Cancelled))
                {
                    this.AppsCancel(derivation);
                }

                if (invoice.SalesInvoiceState.Equals(new SalesInvoiceStates(this.Strategy.Session).WrittenOff))
                {
                    this.AppsWriteOff(derivation);
                }

                if (invoice.SalesInvoiceState.Equals(new SalesInvoiceStates(this.Strategy.Session).Sent))
                {
                    this.AppsSend(derivation);
                }
            }
        }

        public void AppsOnDeriveAmountPaid(IDerivation derivation)
        {
            this.AmountPaid = 0;
            foreach (PaymentApplication paymentApplication in this.PaymentApplicationsWhereInvoiceItem)
            {
                this.AmountPaid += paymentApplication.AmountApplied;
                this.AppsPaymentReceived(derivation);
            }
        }

        public void AppsOnDeriveVatRegime(IDerivation derivation)
        {
            this.VatRegime = this.ExistAssignedVatRegime ? this.AssignedVatRegime : this.SalesInvoiceWhereSalesInvoiceItem.VatRegime;

            this.AppsOnDeriveVatRate(derivation);
        }

        public void AppsOnDeriveVatRate(IDerivation derivation)
        {
            if (this.ExistProduct || this.ExistProductFeature)
            {
                this.DerivedVatRate = this.ExistProduct ? this.Product.VatRate : this.ProductFeature.VatRate;
            }

            if (this.ExistVatRegime && this.VatRegime.ExistVatRate)
            {
                this.DerivedVatRate = this.VatRegime.VatRate;
            }
        }

        public void AppsOnDerivePrices(IDerivation derivation, decimal quantityInvoiced, decimal totalBasePrice)
        {
            this.UnitBasePrice = 0;
            this.UnitDiscount = 0;
            this.UnitSurcharge = 0;
            this.CalculatedUnitPrice = 0;
            decimal discountAdjustmentAmount = 0;
            decimal surchargeAdjustmentAmount = 0;

            var internalOrganisation = this.Strategy.Session.GetSingleton();
            var customer = this.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer;
            var salesInvoice = this.SalesInvoiceWhereSalesInvoiceItem;

            var baseprices = new PriceComponent[0];
            if (this.ExistProduct && this.Product.ExistBasePrices)
            {
                baseprices = this.Product.BasePrices;
            }

            if (this.ExistProductFeature && this.ProductFeature.ExistBasePrices)
            {
                baseprices = this.ProductFeature.BasePrices;
            }

            foreach (BasePrice priceComponent in baseprices)
            {
                if (priceComponent.FromDate <= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate &&
                    (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate))
                {
                    if (priceComponent.Strategy.Class.Equals(M.BasePrice.ObjectType))
                    {
                        if (PriceComponents.AppsIsEligible(new PriceComponents.IsEligibleParams
                        {
                            PriceComponent = priceComponent,
                            Customer = customer,
                            Product = this.Product,
                            SalesInvoice = salesInvoice,
                            QuantityOrdered = quantityInvoiced,
                            ValueOrdered = totalBasePrice
                        }))
                        {
                            if (priceComponent.ExistPrice)
                            {
                                if (priceComponent.Price.HasValue && (this.UnitBasePrice == 0 || priceComponent.Price < this.UnitBasePrice))
                                {
                                    this.UnitBasePrice = priceComponent.Price.Value;

                                    this.RemoveCurrentPriceComponents();
                                    this.AddCurrentPriceComponent(priceComponent);
                                }
                            }
                        }
                    }
                }
            }

            if (!this.ExistActualUnitPrice)
            {
                var priceComponents = this.GetPriceComponents();

                var revenueBreakDiscount = 0M;
                var revenueBreakSurcharge = 0M;

                foreach (var priceComponent in priceComponents)
                {
                    if (priceComponent.Strategy.Class.Equals(M.DiscountComponent.ObjectType) || priceComponent.Strategy.Class.Equals(M.SurchargeComponent.ObjectType))
                    {
                        if (PriceComponents.AppsIsEligible(new PriceComponents.IsEligibleParams
                        {
                            PriceComponent = priceComponent,
                            Customer = customer,
                            Product = this.Product,
                            SalesInvoice = salesInvoice,
                            QuantityOrdered = quantityInvoiced,
                            ValueOrdered = totalBasePrice,
                        }))
                        {
                            this.AddCurrentPriceComponent(priceComponent);

                            revenueBreakDiscount = this.SetUnitDiscount(priceComponent, revenueBreakDiscount);
                            revenueBreakSurcharge = this.SetUnitSurcharge(priceComponent, revenueBreakSurcharge);
                        }
                    }
                }

                var adjustmentBase = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;

                if (this.ExistDiscountAdjustment)
                {
                    if (this.DiscountAdjustment.Percentage.HasValue)
                    {
                        discountAdjustmentAmount = Math.Round((adjustmentBase * this.DiscountAdjustment.Percentage.Value) / 100, 2);
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
                        surchargeAdjustmentAmount = Math.Round((adjustmentBase * this.SurchargeAdjustment.Percentage.Value) / 100, 2);
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
                vat = Math.Round((vatBase * vatRate) / 100, 2);
            }

            this.UnitVat = vat;
            this.TotalBasePrice = price * this.Quantity;
            this.TotalDiscount = this.UnitDiscount * this.Quantity;
            this.TotalSurcharge = this.UnitSurcharge * this.Quantity;
            this.TotalInvoiceAdjustment = (0 - discountAdjustmentAmount + surchargeAdjustmentAmount) * this.Quantity;

            if (this.TotalBasePrice > 0)
            {
                this.TotalDiscountAsPercentage = Math.Round((this.TotalDiscount / this.TotalBasePrice) * 100, 2);
                this.TotalSurchargeAsPercentage = Math.Round((this.TotalSurcharge / this.TotalBasePrice) * 100, 2);
            }

            if (this.ActualUnitPrice.HasValue)
            {
                this.CalculatedUnitPrice = this.ActualUnitPrice.Value;
            }
            else
            {
                this.CalculatedUnitPrice = this.UnitBasePrice - this.UnitDiscount + this.UnitSurcharge;
            }

            this.TotalVat = this.UnitVat * this.Quantity;
            this.TotalExVat = this.CalculatedUnitPrice * this.Quantity;
            this.TotalIncVat = this.TotalExVat + this.TotalVat;

            var toCurrency = this.SalesInvoiceWhereSalesInvoiceItem.Currency;
            var fromCurrency = this.SalesInvoiceWhereSalesInvoiceItem.BilledFrom.PreferredCurrency;

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

            this.AppsOnDeriveMarkupAndProfitMargin(derivation);
        }

        private IEnumerable<PriceComponent> GetPriceComponents()
        {
            var priceComponents = new List<PriceComponent>();

            if (priceComponents.Count == 0)
            {
                var extent = new PriceComponents(this.strategy.Session).Extent();
                if (this.ExistProduct)
                {
                    foreach (PriceComponent priceComponent in extent)
                    {
                        if (priceComponent.ExistProduct && priceComponent.Product.Equals(this.Product) && !priceComponent.ExistProductFeature &&
                            priceComponent.FromDate <= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate &&
                            (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate))
                        {
                            priceComponents.Add(priceComponent);
                        }
                    }

                    if (priceComponents.Count == 0 && this.Product.ExistProductWhereVariant)
                    {
                        extent = new PriceComponents(this.strategy.Session).Extent();
                        foreach (PriceComponent priceComponent in extent)
                        {
                            if (priceComponent.ExistProduct && priceComponent.Product.Equals(this.Product.ProductWhereVariant) && !priceComponent.ExistProductFeature &&
                                priceComponent.FromDate <= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate &&
                                (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate))
                            {
                                priceComponents.Add(priceComponent);
                            }
                        }
                    }
                }

                if (this.ExistProductFeature)
                {
                    foreach (PriceComponent priceComponent in extent)
                    {
                        if (priceComponent.ExistProductFeature && priceComponent.ProductFeature.Equals(this.ProductFeature) && !priceComponent.ExistProduct &&
                            priceComponent.FromDate <= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate &&
                            (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate))
                        {
                            priceComponents.Add(priceComponent);
                        }
                    }
                }

                // Discounts and surcharges can be specified without product or product feature, these need te be added to collection of pricecomponents
                extent = new PriceComponents(this.strategy.Session).Extent();
                foreach (PriceComponent priceComponent in extent)
                {
                    if (!priceComponent.ExistProduct && !priceComponent.ExistProductFeature &&
                        priceComponent.FromDate <= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate &&
                        (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate))
                    {
                        priceComponents.Add(priceComponent);
                    }
                }
            }

            return priceComponents;
        }

        public void AppsOnDeriveMarkupAndProfitMargin(IDerivation derivation)
        {
            this.UnitPurchasePrice = 0;
            this.InitialMarkupPercentage = 0;
            this.MaintainedMarkupPercentage = 0;
            this.InitialProfitMargin = 0;
            this.MaintainedProfitMargin = 0;

            if (this.Product is Good good &&
                this.ExistQuantity && this.Quantity > 0 &&
                good.Part.ExistSupplierOfferingsWherePart &&
                good.Part.SupplierOfferingsWherePart.Select(v => v.Supplier).Distinct().Count() == 1)
            {
                decimal price = 0;
                UnitOfMeasure uom = null;

                foreach (SupplierOffering supplierOffering in good.Part.SupplierOfferingsWherePart)
                {
                    if (supplierOffering.FromDate <= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate &&
                        (!supplierOffering.ExistThroughDate || supplierOffering.ThroughDate >= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate))
                    {
                        price = supplierOffering.Price;
                        uom = supplierOffering.UnitOfMeasure;
                    }
                }

                if (price != 0)
                {
                    this.UnitPurchasePrice = price;
                    if (uom != null && !uom.Equals(this.Product.UnitOfMeasure))
                    {
                        foreach (UnitOfMeasureConversion unitOfMeasureConversion in uom.UnitOfMeasureConversions)
                        {
                            if (unitOfMeasureConversion.ToUnitOfMeasure.Equals(this.Product.UnitOfMeasure))
                            {
                                this.UnitPurchasePrice = Math.Round(this.UnitPurchasePrice * (1 / unitOfMeasureConversion.ConversionFactor), 2);
                            }
                        }
                    }

                    ////internet wiki page on markup business
                    if (this.UnitPurchasePrice != 0 && this.TotalExVat != 0 && this.UnitBasePrice != 0)
                    {
                        this.InitialMarkupPercentage = Math.Round(((this.UnitBasePrice / this.UnitPurchasePrice) - 1) * 100, 2);
                        this.MaintainedMarkupPercentage = Math.Round(((this.CalculatedUnitPrice / this.UnitPurchasePrice) - 1) * 100, 2);

                        this.InitialProfitMargin = Math.Round(((this.UnitBasePrice - this.UnitPurchasePrice) / this.UnitBasePrice) * 100, 2);
                        this.MaintainedProfitMargin = Math.Round(((this.CalculatedUnitPrice - this.UnitPurchasePrice) / this.CalculatedUnitPrice) * 100, 2);
                    }
                }
            }
        }

        public void AppsOnDeriveSalesRep(IDerivation derivation)
        {
            var customer = this.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer;

            this.RemoveSalesReps();

            if (this.ExistProduct)
            {
                foreach (ProductCategory productCategory in this.Product.ProductCategoriesWhereAllProduct)
                {
                    this.AddSalesRep(SalesRepRelationships.SalesRep(customer, productCategory, this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate));
                }
            }

            this.AddSalesRep(SalesRepRelationships.SalesRep(customer, null, this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate));
        }

        public void AppsDelete(DeletableDelete method)
        {
            foreach (SalesTerm salesTerm in this.SalesTerms)
            {
                salesTerm.Delete();
            }

            foreach (InvoiceVatRateItem invoiceVatRateItem in this.InvoiceVatRateItems)
            {
                invoiceVatRateItem.Delete();
            }
        }

        private bool AppsIsSubTotalItem =>
            this.InvoiceItemType.Equals(new InvoiceItemTypes(this.Strategy.Session).ProductItem)
            || this.InvoiceItemType.Equals(new InvoiceItemTypes(this.Strategy.Session).PartItem);
    }
}