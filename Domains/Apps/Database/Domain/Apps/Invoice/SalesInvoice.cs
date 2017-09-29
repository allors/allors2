// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoice.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using Allors.Meta;

    using Resources;

    public partial class SalesInvoice
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

                return null;
            }
        }

        /// <summary>
        /// Gets the due date of an invoice, either
        /// <list type="bullet">
        ///     <item>
        ///         <description>InvoiceDate + PaymentNetDays (if exists)</description>
        ///     </item>
        ///     <item>
        ///         <description>InvoiceDate +  Store.PaymentNetDays (if exists)</description>
        ///     </item>
        /// </list>
        /// or null.
        /// </summary>
        public DateTime? DueDate
        {
            get
            {
                if (this.ExistInvoiceDate)
                {
                    if (this.ExistBillToCustomer && this.BillToCustomer.PaymentNetDays() != null)
                    {
                        var paymentNetDays = this.BillToCustomer.PaymentNetDays().Value;
                        return this.InvoiceDate.AddDays(paymentNetDays);
                    }

                    if (this.ExistStore && this.Store.ExistPaymentNetDays)
                    {
                        var storePaymentNetDays = this.Store.PaymentNetDays;
                        return this.InvoiceDate.AddDays(storePaymentNetDays);
                    }
                }

                return null;
            }
        }

        public InvoiceItem[] InvoiceItems => this.SalesInvoiceItems;

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
                this.CurrentObjectState = new SalesInvoiceObjectStates(this.Strategy.Session).ReadyForPosting;
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = DateTime.UtcNow;
            }

            if (!this.ExistInvoiceDate)
            {
                this.InvoiceDate = DateTime.UtcNow;
            }

            if (this.ExistBillToCustomer)
            {
                this.PreviousBillToCustomer = this.BillToCustomer;
            }

            if (!this.ExistStore)
            {
                this.Store = this.strategy.Session.Extent<Store>().First;
            }

            if (!this.ExistInvoiceNumber && this.ExistStore)
            {
                this.InvoiceNumber = this.Store.DeriveNextInvoiceNumber(this.InvoiceDate.Year);
            }

            if (!this.ExistSalesInvoiceType)
            {
                this.SalesInvoiceType = new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice;
            }

            if (!this.ExistCustomerCurrency)
            {
                if (this.ExistBillToCustomer &&
                    (this.BillToCustomer.ExistPreferredCurrency || this.BillToCustomer.ExistLocale))
                {
                    this.CustomerCurrency = this.BillToCustomer.ExistPreferredCurrency ? this.BillToCustomer.PreferredCurrency : this.BillToCustomer.Locale.Country.Currency;
                }
                else
                {
                    this.CustomerCurrency = Singleton.Instance(this).ExistPreferredCurrency ?
                                                Singleton.Instance(this).PreferredCurrency :
                                                Singleton.Instance(this).DefaultLocale.Country.Currency;
                }
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (SalesInvoiceItem invoiceItem in this.InvoiceItems)
            {
                derivation.AddDependency(this, invoiceItem);
            }

            if (this.ExistSalesOrder)
            {
                derivation.AddDependency(this.SalesOrder, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistBillToContactMechanism && this.ExistBillToCustomer)
            {
                this.BillToContactMechanism = this.BillToCustomer.BillingAddress;
            }

            if (!this.ExistBilledFromContactMechanism)
            {
                this.BilledFromContactMechanism = Singleton.Instance(this).InternalOrganisation.BillingAddress;
            }

            if (!this.ExistShipToAddress && this.ExistShipToCustomer)
            {
                this.ShipToAddress = this.ShipToCustomer.ShippingAddress;
            }

            this.AppsOnDeriveInvoiceItems(derivation);

            this.SalesInvoiceItems = this.SalesInvoiceItems.ToArray();

            this.AppsOnDeriveLocale(derivation);
            this.AppsOnDeriveInvoiceTotals(derivation);
            this.AppsOnDeriveCustomers(derivation);
            this.AppsOnDeriveSalesReps(derivation);
            this.AppsOnDeriveAmountPaid(derivation);

            if (this.ExistBillToCustomer && !this.BillToCustomer.IsCustomer())
            {
                derivation.Validation.AddError(this, M.SalesInvoice.BillToCustomer, ErrorMessages.PartyIsNotACustomer);
            }

            if (this.ExistShipToCustomer && !this.ShipToCustomer.IsCustomer())
            {
                derivation.Validation.AddError(this, M.SalesInvoice.ShipToCustomer, ErrorMessages.PartyIsNotACustomer);
            }

            this.DeriveCurrentPaymentStatus(derivation);
            this.DeriveCurrentObjectState(derivation);

            this.PreviousBillToCustomer = this.BillToCustomer;
            this.PreviousShipToCustomer = this.ShipToCustomer;

            this.AppsOnDeriveRevenues(derivation);
        }

        private void DeriveCurrentPaymentStatus(IDerivation derivation)
        {
            var itemsPaid = false;
            var itemsPartiallyPaid = false;
            var itemsNotPaid = false;

            foreach (SalesInvoiceItem invoiceItem in this.SalesInvoiceItems)
            {
                if (invoiceItem.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).PartiallyPaid))
                {
                    itemsPartiallyPaid = true;
                }

                if (invoiceItem.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).Paid))
                {
                    itemsPaid = true;
                }

                if (invoiceItem.CurrentObjectState.Equals(new SalesInvoiceItemObjectStates(this.Strategy.Session).Sent))
                {
                    itemsNotPaid = true;
                }
            }

            if (itemsPaid && !itemsNotPaid && !itemsPartiallyPaid && (!this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Paid)))
            {
                this.CurrentObjectState = new SalesInvoiceObjectStates(this.Strategy.Session).Paid;
            }

            if ((itemsPartiallyPaid || (itemsPaid && itemsNotPaid)) && (!this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).PartiallyPaid)))
            {
                this.CurrentObjectState = new SalesInvoiceObjectStates(this.Strategy.Session).PartiallyPaid;
            }

            if (this.ExistAmountPaid)
            {
                if (this.AmountPaid > 0 && this.AmountPaid < this.TotalIncVat
                    && (!this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).PartiallyPaid)))
                {
                    this.CurrentObjectState = new SalesInvoiceObjectStates(this.Strategy.Session).PartiallyPaid;
                }

                if (this.AmountPaid > 0 && this.AmountPaid == this.TotalIncVat
                    && (!this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Paid)))
                {
                    this.CurrentObjectState = new SalesInvoiceObjectStates(this.Strategy.Session).Paid;
                }
            }
        }

        private void DeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Paid))
            {
                this.AppsOnDeriveSalesOrderPaymentStatus(derivation);
            }
        }

        public void AppsSend(SalesInvoiceSend method)
        {
            this.CurrentObjectState = new SalesInvoiceObjectStates(this.Strategy.Session).Sent;
        }

        public void AppsWriteOff(SalesInvoiceWriteOff method)
        {
            this.CurrentObjectState = new SalesInvoiceObjectStates(this.Strategy.Session).WrittenOff;
        }

        public void AppsCancelInvoice(SalesInvoiceCancelInvoice method)
        {
            this.CurrentObjectState = new SalesInvoiceObjectStates(this.Strategy.Session).Cancelled;
        }

        public void AppsOnDeriveLocale(IDerivation derivation)
        {
            if (this.ExistBillToCustomer && this.BillToCustomer.ExistLocale)
            {
                this.Locale = this.BillToCustomer.Locale;
            }
            else
            {
                this.Locale = Singleton.Instance(this).DefaultLocale;
            }
        }

        public void AppsOnDeriveSalesReps(IDerivation derivation)
        {
            this.RemoveSalesReps();
            foreach (SalesInvoiceItem item in this.SalesInvoiceItems)
            {
                this.AddSalesRep(item.SalesRep);
            }
        }

        public void AppsOnDeriveAmountPaid(IDerivation derivation)
        {
            this.AmountPaid = 0;
            foreach (PaymentApplication paymentApplication in this.PaymentApplicationsWhereInvoice)
            {
                this.AmountPaid += paymentApplication.AmountApplied;
            }

            //// Perhaps payments are recorded at the item level.
            if (this.AmountPaid == 0)
            {
                foreach (var invoiceItem in this.InvoiceItems)
                {
                    this.AmountPaid += invoiceItem.AmountPaid;
                }
            }
        }

        public void AppsOnDeriveInvoiceTotals(IDerivation derivation)
        {
            this.TotalBasePrice = 0;
            this.TotalDiscount = 0;
            this.TotalSurcharge = 0;
            this.TotalFee = 0;
            this.TotalShippingAndHandling = 0;
            this.TotalVat = 0;
            this.TotalExVat = 0;
            this.TotalIncVat = 0;
            this.TotalPurchasePrice = 0;
            this.TotalListPrice = 0;
            this.MaintainedMarkupPercentage = 0;
            this.InitialMarkupPercentage = 0;
            this.MaintainedProfitMargin = 0;
            this.InitialProfitMargin = 0;

            foreach (SalesInvoiceItem item in this.SalesInvoiceItems)
            {
                this.TotalBasePrice += item.TotalBasePrice;
                this.TotalDiscount += item.TotalDiscount;
                this.TotalSurcharge += item.TotalSurcharge;
                this.TotalVat += item.TotalVat;
                this.TotalExVat += item.TotalExVat;
                this.TotalIncVat += item.TotalIncVat;
                this.TotalPurchasePrice += item.UnitPurchasePrice;
                this.TotalListPrice += item.CalculatedUnitPrice;
            }

            this.DeriveDiscountAdjustments(derivation);
            this.DeriveSurchargeAdjustments(derivation);
            this.DeriveTotalFee(derivation);
            this.DeriveTotalShippingAndHandling(derivation);
            this.AppsOnDeriveMarkupAndProfitMargin(derivation);
        }

        private void DeriveDiscountAdjustments(IDerivation derivation)
        {
            if (this.DiscountAdjustment != null)
            {
                decimal discount = this.DiscountAdjustment.Percentage.HasValue ? Math.Round((this.TotalExVat * this.DiscountAdjustment.Percentage.Value) / 100, 2) : this.DiscountAdjustment.Amount.HasValue ? this.DiscountAdjustment.Amount.Value : 0;

                this.TotalDiscount += discount;
                this.TotalExVat -= discount;

                if (this.ExistVatRegime)
                {
                    decimal vat = Math.Round((discount * this.VatRegime.VatRate.Rate) / 100, 2);
                    this.TotalVat -= vat;
                    this.TotalIncVat -= discount + vat;
                }
            }
        }

        private void DeriveSurchargeAdjustments(IDerivation derivation)
        {
            if (this.ExistSurchargeAdjustment)
            {
                decimal surcharge = this.SurchargeAdjustment.Percentage.HasValue ? Math.Round((this.TotalExVat * this.SurchargeAdjustment.Percentage.Value) / 100, 2) : this.SurchargeAdjustment.Amount.HasValue ? this.SurchargeAdjustment.Amount.Value : 0;

                this.TotalSurcharge += surcharge;
                this.TotalExVat += surcharge;

                if (this.ExistVatRegime)
                {
                    decimal vat = Math.Round((surcharge * this.VatRegime.VatRate.Rate) / 100, 2);
                    this.TotalVat += vat;
                    this.TotalIncVat += surcharge + vat;
                }
            }
        }

        private void DeriveTotalFee(IDerivation derivation)
        {
            if (this.ExistFee)
            {
                decimal fee = this.Fee.Percentage.HasValue ? Math.Round((this.TotalExVat * this.Fee.Percentage.Value) / 100, 2) : this.Fee.Amount.HasValue ? this.Fee.Amount.Value : 0;

                this.TotalFee += fee;
                this.TotalExVat += fee;

                if (this.Fee.ExistVatRate)
                {
                    decimal vat = Math.Round((fee * this.Fee.VatRate.Rate) / 100, 2);
                    this.TotalVat += vat;
                    this.TotalIncVat += fee + vat;
                }
            }
        }

        private void DeriveTotalShippingAndHandling(IDerivation derivation)
        {
            if (this.ExistShippingAndHandlingCharge)
            {
                decimal shipping = this.ShippingAndHandlingCharge.Percentage.HasValue ?
                    Math.Round((this.TotalExVat * this.ShippingAndHandlingCharge.Percentage.Value) / 100, 2) : this.ShippingAndHandlingCharge.Amount.HasValue ? this.ShippingAndHandlingCharge.Amount.Value : 0;

                this.TotalShippingAndHandling += shipping;
                this.TotalExVat += shipping;

                if (this.ShippingAndHandlingCharge.ExistVatRate)
                {
                    decimal vat = Math.Round((shipping * this.ShippingAndHandlingCharge.VatRate.Rate) / 100, 2);

                    this.TotalVat += vat;
                    this.TotalIncVat += shipping + vat;
                }
            }
        }

        public void AppsOnDeriveMarkupAndProfitMargin(IDerivation derivation)
        {
            //// Only take into account items for which there is data at the item level.
            //// Skip negative sales.
            decimal totalPurchasePrice = 0;
            decimal totalUnitBasePrice = 0;
            decimal totalListPrice = 0;

            foreach (SalesInvoiceItem item in this.SalesInvoiceItems)
            {
                if (item.TotalExVat > 0 && item.InitialMarkupPercentage > 0)
                {
                    totalPurchasePrice += item.UnitPurchasePrice;
                    totalUnitBasePrice += item.UnitBasePrice;
                    totalListPrice += item.CalculatedUnitPrice;
                }
            }

            if (totalPurchasePrice != 0 && totalListPrice != 0 && totalUnitBasePrice != 0)
            {
                this.InitialMarkupPercentage = Math.Round(((totalUnitBasePrice / totalPurchasePrice) - 1) * 100, 2);
                this.MaintainedMarkupPercentage = Math.Round(((totalListPrice / totalPurchasePrice) - 1) * 100, 2);

                this.InitialProfitMargin = Math.Round(((totalUnitBasePrice - totalPurchasePrice) / totalUnitBasePrice) * 100, 2);
                this.MaintainedProfitMargin = Math.Round(((totalListPrice - totalPurchasePrice) / totalListPrice) * 100, 2);
            }
        }

        public void AppsOnDeriveSalesOrderPaymentStatus(IDerivation derivation)
        {
            foreach (SalesInvoiceItem invoiceItem in this.SalesInvoiceItems)
            {
                if (invoiceItem.ExistShipmentItemWhereInvoiceItem)
                {
                    foreach (OrderShipment orderShipment in invoiceItem.ShipmentItemWhereInvoiceItem.OrderShipmentsWhereShipmentItem)
                    {
                        orderShipment.SalesOrderItem.AppsOnDeriveCurrentPaymentStatus(derivation);
                        orderShipment.SalesOrderItem.SalesOrderWhereSalesOrderItem.OnDerive(x => x.WithDerivation(derivation));
                    }
                }
            }
        }

        public void AppsOnDeriveCustomers(IDerivation derivation)
        {
            this.RemoveCustomers();
            if (this.ExistBillToCustomer && !this.Customers.Contains(this.BillToCustomer))
            {
                this.AddCustomer(this.BillToCustomer);
            }

            if (this.ExistShipToCustomer && !this.Customers.Contains(this.ShipToCustomer))
            {
                this.AddCustomer(this.ShipToCustomer);
            }
        }

        public void AppsOnDeriveInvoiceItems(IDerivation derivation)
        {
            var quantityInvoicedByProduct = new Dictionary<Product, decimal>();
            var totalBasePriceByProduct = new Dictionary<Product, decimal>();

            var invoiceFromOrder = true;
            foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
            {
                salesInvoiceItem.OnDerive(x => x.WithDerivation(derivation));
                salesInvoiceItem.AppsOnDeriveVatRegime(derivation);
                salesInvoiceItem.AppsOnDeriveSalesRep(derivation);
                salesInvoiceItem.AppsOnDeriveCurrentObjectState(derivation);

                if (!salesInvoiceItem.ExistShipmentItemWhereInvoiceItem)
                {
                    invoiceFromOrder = false;
                    salesInvoiceItem.AppsOnDerivePrices(derivation, 0, 0);
                    salesInvoiceItem.AppsOnDeriveCurrentPaymentStatus(derivation);

                    if (salesInvoiceItem.ExistProduct)
                    {
                        if (!quantityInvoicedByProduct.ContainsKey(salesInvoiceItem.Product))
                        {
                            quantityInvoicedByProduct.Add(salesInvoiceItem.Product, salesInvoiceItem.Quantity);
                            totalBasePriceByProduct.Add(salesInvoiceItem.Product, salesInvoiceItem.TotalBasePrice);
                        }
                        else
                        {
                            quantityInvoicedByProduct[salesInvoiceItem.Product] += salesInvoiceItem.Quantity;
                            totalBasePriceByProduct[salesInvoiceItem.Product] += salesInvoiceItem.TotalBasePrice;
                        }
                    }
                }
            }

            if (!invoiceFromOrder)
            {
                foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
                {
                    decimal quantity = 0;
                    decimal totalBasePrice = 0;

                    if (salesInvoiceItem.ExistProduct)
                    {
                        quantityInvoicedByProduct.TryGetValue(salesInvoiceItem.Product, out quantity);
                        totalBasePriceByProduct.TryGetValue(salesInvoiceItem.Product, out totalBasePrice);
                    }

                    salesInvoiceItem.AppsOnDerivePrices(derivation, quantity, totalBasePrice);
                    salesInvoiceItem.AppsOnDeriveCurrentPaymentStatus(derivation);
                }
            }
        }

        public void AppsOnDeriveRevenues(IDerivation derivation)
        {
            foreach (SalesInvoiceItem salesInvoiceItem in this.SalesInvoiceItems)
            {
                if (salesInvoiceItem.ExistProduct)
                {
                    var partyProductRevenue = PartyProductRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, salesInvoiceItem.Product, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem);
                    partyProductRevenue.OnDerive(x => x.WithDerivation(derivation));
                }
                else
                {
                    var partyRevenue = PartyRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem);
                    partyRevenue.OnDerive(x => x.WithDerivation(derivation));
                }

                if (salesInvoiceItem.ExistSalesRep)
                {
                    if (salesInvoiceItem.ExistProduct && salesInvoiceItem.Product.ExistPrimaryProductCategory)
                    {
                        var salesRepPartyProductCategoryRevenue = SalesRepPartyProductCategoryRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, salesInvoiceItem);
                        salesRepPartyProductCategoryRevenue.OnDerive(x => x.WithDerivation(derivation));
                    }
                    else
                    {
                        var salesRepPartyRevenue = SalesRepPartyRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, salesInvoiceItem);
                        salesRepPartyRevenue.OnDerive(x => x.WithDerivation(derivation));
                    }
                }

                if (salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.ExistBillToCustomer && salesInvoiceItem.ExistProduct)
                {
                    var partyProductRevenue = PartyProductRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, salesInvoiceItem.Product, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem);
                    partyProductRevenue.OnDerive(x => x.WithDerivation(derivation));
                }
                else
                {
                    var partyRevenue = PartyRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem);
                    partyRevenue.OnDerive(x => x.WithDerivation(derivation));
                }

                var storeRevenue = StoreRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                storeRevenue.OnDerive(x => x.WithDerivation(derivation));

                if (this.ExistSalesChannel)
                {
                    var salesChannelRevenue = SalesChannelRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                    salesChannelRevenue.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }
    }
}
