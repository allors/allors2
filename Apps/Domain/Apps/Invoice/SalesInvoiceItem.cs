namespace Allors.Domain
{
    using System.Collections.Generic;

    public partial class SalesInvoiceItem
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

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

        public string GetActualUnitBasePriceAsCurrencyString
        {
            get
            {
                return this.ExistActualUnitPrice ? this.ActualUnitPrice.AsCurrencyString(this.SalesInvoiceWhereSalesInvoiceItem.CurrencyFormat) : string.Empty;
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
                        return this.DiscountAdjustment.Amount.AsCurrencyString(this.SalesInvoiceWhereSalesInvoiceItem.CurrencyFormat);
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
                        return this.DiscountAdjustment.Percentage.Value.ToString(this.SalesInvoiceWhereSalesInvoiceItem.Locale);
                    }
                }

                return string.Empty;
            }
        }

        public string GetDiscountAsPercentageString
        {
            get
            {
                if (this.TotalDiscountAsPercentage.HasValue)
                {
                    return this.TotalDiscountAsPercentage.Value.ToString(this.SalesInvoiceWhereSalesInvoiceItem.Locale);
                }

                return string.Empty;
            }
        }

        public string GetSurchargeAsPercentageString
        {
            get
            {
                if (this.TotalSurchargeAsPercentage.HasValue)
                {
                    return this.TotalSurchargeAsPercentage.Value.ToString(this.SalesInvoiceWhereSalesInvoiceItem.Locale);
                }

                return string.Empty;
            }
        }

        public string GetUnitBasePriceAsCurrencyString
        {
            get
            {
                return this.UnitBasePrice.AsCurrencyString(this.SalesInvoiceWhereSalesInvoiceItem.CurrencyFormat);
            }
        }

        public string GetCalculatedUnitPriceAsCurrencyString
        {
            get
            {
                return this.CalculatedUnitPrice.AsCurrencyString(this.SalesInvoiceWhereSalesInvoiceItem.CurrencyFormat);
            }
        }

        public string GetPriceAdjustmentAsCurrencyString
        {
            get
            {
                return this.PriceAdjustment.AsCurrencyString(this.SalesInvoiceWhereSalesInvoiceItem.CurrencyFormat);
            }
        }

        public string GetPriceAdjustmentAsPercentageString
        {
            get
            {
                return this.PriceAdjustmentAsPercentage.ToString("##.##");
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

        public string GetTotalExVatAsCurrencyString
        {
            get
            {
                return this.TotalExVat.AsCurrencyString(this.SalesInvoiceWhereSalesInvoiceItem.CurrencyFormat);
            }
        }

        public string GetTotalIncVatAsCurrencyString
        {
            get
            {
                return this.TotalIncVat.AsCurrencyString(this.SalesInvoiceWhereSalesInvoiceItem.CurrencyFormat);
            }
        }

        public string GetNothingAsCurrencyString
        {
            get
            {
                const decimal Nothing = 0;
                return Nothing.AsCurrencyString(this.SalesInvoiceWhereSalesInvoiceItem.CurrencyFormat);
            }
        }

        public string VatRateAsString
        {
            get
            {
                return this.ExistDerivedVatRate && this.DerivedVatRate.ExistRate ? this.DerivedVatRate.Rate.ToString("##.##") : string.Empty;
            }
        }

        // TODO:

        // public SalesOrderItem SalesOrderItem
        // {
        // get
        // {
        // if (this.ExistShipmentItemWhereInvoiceItem)
        // {
        // return this.ShipmentItemWhereInvoiceItem.OrderShipmentsWhereShipmentItem[0].SalesOrderItem;
        // }

        // return null;
        // }
        // }
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

            if (!this.ExistQuantity)
            {
                this.Quantity = 0;
            }

            if (!this.ExistAmountPaid)
            {
                this.AmountPaid = 0;
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
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveAmountPaid(derivation);

            derivation.Log.AssertAtLeastOne(this, SalesInvoiceItems.Meta.Product, SalesInvoiceItems.Meta.ProductFeature, SalesInvoiceItems.Meta.TimeEntries);
            derivation.Log.AssertExistsAtMostOne(this, SalesInvoiceItems.Meta.Product, SalesInvoiceItems.Meta.ProductFeature, SalesInvoiceItems.Meta.TimeEntries);
        }

        private static decimal PriceInCurrency(decimal price, Currency fromCurrency, Currency toCurrency)
        {
            if (!fromCurrency.Equals(toCurrency))
            {
                foreach (UnitOfMeasureConversion unitOfMeasureConversion in fromCurrency.UnitOfMeasureConversions)
                {
                    if (unitOfMeasureConversion.ToUnitOfMeasure.Equals(toCurrency))
                    {
                        return decimal.Round(price * unitOfMeasureConversion.ConversionFactor, 2);
                    }
                }
            }

            return price;
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
                this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).PartiallyPaid;                
            }

            if (this.ExistCurrentObjectState)
            {
                var currentStatus = new SalesInvoiceItemStatusBuilder(this.Strategy.Session).WithSalesInvoiceItemObjectState(this.CurrentObjectState).Build();
                this.AddInvoiceItemStatus(currentStatus);
                this.CurrentInvoiceItemStatus = currentStatus;
                
                this.CurrentObjectState.Process(this);
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
                        this.DeriveCurrentObjectState(derivation);
                    }
                }

                if (this.ExistAmountPaid && this.AmountPaid > 0 && this.AmountPaid >= this.TotalIncVat)
                {
                    if (!this.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).Paid))
                    {
                        this.CurrentObjectState = new SalesInvoiceItemObjectStates(this.Strategy.Session).Paid;
                        this.DeriveCurrentObjectState(derivation);
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
                    this.Cancel(derivation);
                }

                if (invoice.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).WrittenOff))
                {
                    this.WriteOff(derivation);
                }
            }

            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {
                var currentStatus = new SalesInvoiceItemStatusBuilder(this.Strategy.Session).WithSalesInvoiceItemObjectState(this.CurrentObjectState).Build();
                this.AddInvoiceItemStatus(currentStatus);
                this.CurrentInvoiceItemStatus = currentStatus;
            }

            if (this.ExistCurrentObjectState)
            {
                this.CurrentObjectState.Process(this);
            }
        }

        public void AppsOnDeriveAmountPaid(IDerivation derivation)
        {
            this.AmountPaid = 0;
            foreach (PaymentApplication paymentApplication in this.PaymentApplicationsWhereInvoiceItem)
            {
                this.AmountPaid += paymentApplication.AmountApplied;
            }
        }

        public void AppsOnDeriveVatRegime(IDerivation derivation)
        {
            this.VatRegime = this.ExistAssignedVatRegime ? this.AssignedVatRegime : this.SalesInvoiceWhereSalesInvoiceItem.VatRegime;
            
            this.DeriveVatRate(derivation);
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
                baseprices = Product.BasePrices; 
            }

            if (this.ExistProductFeature && this.ProductFeature.ExistBasePrices)
            {
                baseprices = ProductFeature.BasePrices;                 
            }

            foreach (BasePrice priceComponent in baseprices)
            {
                if (priceComponent.FromDate <= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate &&
                    (!priceComponent.ExistThroughDate || priceComponent.ThroughDate >= this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate))
                {
                    if (priceComponent.Strategy.Class.Equals(BasePrices.Meta.ObjectType))
                    {
                        if (PriceComponents.IsEligible(new PriceComponents.IsEligibleParams
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
                                if (this.UnitBasePrice == 0 || priceComponent.Price < this.UnitBasePrice)
                                {
                                    this.UnitBasePrice = priceComponent.Price;

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
                partyRevenueHistories.Filter.AddEquals(PartyRevenueHistories.Meta.InternalOrganisation, internalOrganisation);
                var partyRevenueHistory = partyRevenueHistories.First;

                var partyProductCategoryRevenueHistoryByProductCategory = PartyProductCategoryRevenueHistories.PartyProductCategoryRevenueHistoryByProductCategory(internalOrganisation, customer);

                var partyPackageRevenuesHistories = customer.PartyPackageRevenueHistoriesWhereParty;
                partyPackageRevenuesHistories.Filter.AddEquals(PartyPackageRevenueHistories.Meta.InternalOrganisation, internalOrganisation);

                var priceComponents = this.GetPriceComponents(internalOrganisation);

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
                                                               SalesInvoice = salesInvoice, 
                                                               QuantityOrdered = quantityInvoiced, 
                                                               ValueOrdered = totalBasePrice, 
                                                               PartyPackageRevenueHistoryList = partyPackageRevenuesHistories, 
                                                               PartyRevenueHistory = partyRevenueHistory, 
                                                               PartyProductCategoryRevenueHistoryByProductCategory = partyProductCategoryRevenueHistoryByProductCategory
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
                this.TotalBasePriceCustomerCurrency = Domain.Currencies.ConvertCurrency(this.TotalBasePrice, fromCurrency, toCurrency);
                this.TotalDiscountCustomerCurrency = Domain.Currencies.ConvertCurrency(this.TotalDiscount, fromCurrency, toCurrency);
                this.TotalSurchargeCustomerCurrency = Domain.Currencies.ConvertCurrency(this.TotalSurcharge, fromCurrency, toCurrency);
                this.TotalExVatCustomerCurrency = Domain.Currencies.ConvertCurrency(this.TotalExVat, fromCurrency, toCurrency);
                this.TotalVatCustomerCurrency = Domain.Currencies.ConvertCurrency(this.TotalVat, fromCurrency, toCurrency);
                this.TotalIncVatCustomerCurrency = Domain.Currencies.ConvertCurrency(this.TotalIncVat, fromCurrency, toCurrency);
            }

            this.DeriveMarkupAndProfitMargin(derivation);
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
                        this.SalesRep = SalesRepRelationships.SalesRep(customer, Product.PrimaryProductCategory, this.SalesInvoiceWhereSalesInvoiceItem.InvoiceDate);
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