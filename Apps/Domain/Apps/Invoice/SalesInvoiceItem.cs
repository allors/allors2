namespace Allors.Domain
{
    using System.Collections.Generic;

    using Meta;

    public partial class SalesInvoiceItem
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public int? PaymentNetDays
        {
            get
            {
                int? invoicePaymentNetDays = null;
                if (this.ExistInvoiceTerms)
                {
                    foreach (AgreementTerm term in this.InvoiceTerms)
                    {
                        if (term.TermType.Equals(new TermTypes(this.Strategy.Session).PaymentNetDays))
                        {
                            int netDays;
                            if (int.TryParse(term.TermValue, out netDays))
                            {
                                invoicePaymentNetDays = netDays;
                            }

                            return invoicePaymentNetDays;
                        }
                    }
                }

                return this.SalesInvoiceWhereSalesInvoiceItem.PaymentNetDays;
            }
        }

        public decimal PriceAdjustment => this.TotalSurcharge - this.TotalDiscount;

        public decimal PriceAdjustmentAsPercentage => decimal.Round(((this.TotalSurcharge - this.TotalDiscount) / this.TotalBasePrice) * 100, 2);

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
                this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).ReadyForPosting;
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

            if (this.ExistSalesOrderItem)
            {
                derivation.AddDependency(this.SalesOrderItem, this);
            }

            if (this.ExistShipmentItemWhereInvoiceItem)
            {
                derivation.AddDependency(this.ShipmentItemWhereInvoiceItem, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveAmountPaid(derivation);

            derivation.Validation.AssertAtLeastOne(this, this.Meta.Product, this.Meta.ProductFeature, this.Meta.TimeEntries);
            derivation.Validation.AssertExistsAtMostOne(this, this.Meta.Product, this.Meta.ProductFeature, this.Meta.TimeEntries);
        }

        public void AppsWriteOff(IDerivation derivation)
        {
            this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).WrittenOff;
        }

        public void AppsCancel(IDerivation derivation)
        {
            this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).Cancelled;
        }

        public void AppsPaymentReceived(IDerivation derivation)
        {
            if (this.AmountPaid < this.TotalIncVat)
            {
                this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).PartiallyPaid;
            }
            else
            {
                this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).Paid;                
            }

            if (this.ExistCurrentObjectState)
            {
                var currentStatus = new SalesInvoiceItemStatusBuilder(this.Strategy.Session).WithSalesInvoiceItemObjectState(this.CurrentObjectState).Build();
                this.AddInvoiceItemStatus(currentStatus);
                this.CurrentInvoiceItemStatus = currentStatus;
            }
        }

        public void AppsOnDeriveCurrentPaymentStatus(IDerivation derivation)
        {
            if (!this.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).Cancelled) &&
                !this.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).WrittenOff))
            {
                if (this.ExistAmountPaid && this.AmountPaid > 0 && this.AmountPaid < this.TotalIncVat)
                {
                    if (!this.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).PartiallyPaid))
                    {
                        this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).PartiallyPaid;
                        this.AppsOnDeriveCurrentObjectState(derivation);
                    }
                }

                if (this.ExistAmountPaid && this.AmountPaid > 0 && this.AmountPaid >= this.TotalIncVat)
                {
                    if (!this.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).Paid))
                    {
                        this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).Paid;
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

                if (invoice.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Cancelled))
                {
                    this.AppsCancel(derivation);
                }

                if (invoice.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).WrittenOff))
                {
                    this.AppsWriteOff(derivation);
                }
            }

            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.LastObjectState))
            {
                var currentStatus = new SalesInvoiceItemStatusBuilder(this.Strategy.Session).WithSalesInvoiceItemObjectState(this.CurrentObjectState).Build();
                this.AddInvoiceItemStatus(currentStatus);
                this.CurrentInvoiceItemStatus = currentStatus;
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

            var internalOrganisation = this.SalesInvoiceWhereSalesInvoiceItem.BilledFromInternalOrganisation;
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
                var partyRevenueHistories = customer.PartyRevenueHistoriesWhereParty;
                partyRevenueHistories.Filter.AddEquals(M.PartyRevenueHistory.InternalOrganisation, internalOrganisation);
                var partyRevenueHistory = partyRevenueHistories.First;

                var partyProductCategoryRevenueHistoryByProductCategory = PartyProductCategoryRevenueHistories.PartyProductCategoryRevenueHistoryByProductCategory(internalOrganisation, customer);

                var partyPackageRevenuesHistories = customer.PartyPackageRevenueHistoriesWhereParty;
                partyPackageRevenuesHistories.Filter.AddEquals(M.PartyPackageRevenueHistory.InternalOrganisation, internalOrganisation);

                var priceComponents = this.GetPriceComponents(internalOrganisation);

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
                                                                   PartyPackageRevenueHistoryList = partyPackageRevenuesHistories, 
                                                                   PartyRevenueHistory = partyRevenueHistory, 
                                                                   PartyProductCategoryRevenueHistoryByProductCategory = partyProductCategoryRevenueHistoryByProductCategory
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
                        discountAdjustmentAmount = decimal.Round((adjustmentBase * this.DiscountAdjustment.Percentage.Value) / 100, 2);
                    }
                    else
                    {
                        discountAdjustmentAmount = this.DiscountAdjustment.Amount.HasValue? this.DiscountAdjustment.Amount.Value : 0;
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
            this.TotalBasePrice = price * this.Quantity;
            this.TotalDiscount = this.UnitDiscount * this.Quantity;
            this.TotalSurcharge = this.UnitSurcharge * this.Quantity;
            this.TotalInvoiceAdjustment = (0 - discountAdjustmentAmount + surchargeAdjustmentAmount) * this.Quantity;

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

            this.TotalVat = this.UnitVat * this.Quantity;
            this.TotalExVat = this.CalculatedUnitPrice * this.Quantity;
            this.TotalIncVat = this.TotalExVat + this.TotalVat;

            var toCurrency = this.SalesInvoiceWhereSalesInvoiceItem.CustomerCurrency;
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

            this.AppsOnDeriveMarkupAndProfitMargin(derivation);
        }

        private IEnumerable<PriceComponent> GetPriceComponents(InternalOrganisation internalOrganisation)
        {
            var priceComponents = new List<PriceComponent>();

            if (priceComponents.Count == 0)
            {
                var extent = internalOrganisation.PriceComponentsWhereSpecifiedFor;
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
                        extent = internalOrganisation.PriceComponentsWhereSpecifiedFor;
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
                extent = internalOrganisation.PriceComponentsWhereSpecifiedFor;
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

            if (this.ExistProduct &&
                this.ExistQuantity && this.Quantity > 0 &&
                this.Product.ExistSupplierOfferingsWhereProduct &&
                this.Product.SupplierOfferingsWhereProduct.Count == 1 &&
                this.Product.SupplierOfferingsWhereProduct.First.ExistProductPurchasePrices)
            {
                ProductPurchasePrice productPurchasePrice = null;
                var prices = this.Product.SupplierOfferingsWhereProduct.First.ProductPurchasePrices;
                foreach (ProductPurchasePrice purchasePrice in prices)
                {
                    if (purchasePrice.FromDate <= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate &&
                        (!purchasePrice.ExistThroughDate || purchasePrice.ThroughDate >= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate))
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

                    ////internet wiki page on markup business
                    if (this.UnitPurchasePrice != 0 && this.TotalExVat != 0 && this.UnitBasePrice != 0)
                    {
                        this.InitialMarkupPercentage = decimal.Round(((this.UnitBasePrice / this.UnitPurchasePrice) - 1) * 100, 2);
                        this.MaintainedMarkupPercentage = decimal.Round(((this.CalculatedUnitPrice / this.UnitPurchasePrice) - 1) * 100, 2);

                        this.InitialProfitMargin = decimal.Round(((this.UnitBasePrice - this.UnitPurchasePrice) / this.UnitBasePrice) * 100, 2);
                        this.MaintainedProfitMargin = decimal.Round(((this.CalculatedUnitPrice - this.UnitPurchasePrice) / this.CalculatedUnitPrice) * 100, 2);
                    }
                }
            }
        }

        public void AppsOnDeriveSalesRep(IDerivation derivation)
        {
            if (this.SalesOrderItem != null)
            {
                this.SalesRep = this.SalesOrderItem.SalesRep;
            }
            else
            {
                var customer = this.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer as Organisation;
                if (customer != null)
                {
                    if (this.ExistProduct)
                    {
                        this.SalesRep = SalesRepRelationships.SalesRep(customer, this.Product.PrimaryProductCategory, this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate);
                    }
                    else
                    {
                        this.SalesRep = SalesRepRelationships.SalesRep(customer, null, this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate);
                    }
                }                
            }
        }
    }
}