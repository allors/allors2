// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoice.cs" company="Allors bvba">
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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    using Resources;

    public partial class SalesInvoice
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public bool CanEdit
        {
            get
            {
                if (this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Sent) ||
                    this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).ReadyForPosting) ||
                    this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).PartiallyPaid))
                {
                    return true;
                }

                return false;
            }
        }

        public PaymentMethod GetPaymentMethod
        {
            get
            {
                if (this.ExistBillToCustomer && this.BillToCustomer.ExistDefaultPaymentMethod)
                {
                    return this.BillToCustomer.DefaultPaymentMethod;
                }

                return this.Store.DefaultPaymentMethod;
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
                    if (this.ExistBilledFromInternalOrganisation && this.ExistBillToCustomer && this.BillToCustomer.ExistCustomerRelationshipsWhereCustomer)
                    {
                        var customerRelationships = this.BillToCustomer.CustomerRelationshipsWhereCustomer;
                        customerRelationships.Filter.AddEquals(CustomerRelationships.Meta.InternalOrganisation, this.BilledFromInternalOrganisation);
                        var customerRelationship = customerRelationships.First;

                        if (customerRelationship != null && customerRelationship.PaymentNetDays != null)
                        {
                            var paymentNetDays = customerRelationship.PaymentNetDays.Value;
                            return this.InvoiceDate.AddDays(paymentNetDays);
                        }
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

        public InvoiceItem[] InvoiceItems
        {
            get
            {
                return this.SalesInvoiceItems;
            }
        }

        public string ShortDueDateString
        {
            get { return this.DueDate.ToShortDateString(); }
        }

        public string ShortInvoiceDateString
        {
            get { return this.InvoiceDate.ToShortDateString(); }
        }

        public NumberFormatInfo CurrencyFormat
        {
            get
            {
                var cultureInfo = new CultureInfo(this.Locale.Name, false);
                var currencyFormat = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                currencyFormat.CurrencySymbol = this.CustomerCurrency.Symbol;
                return currencyFormat;
            }
        }

        public string GetTotalBasePriceAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalBasePrice, this.CurrencyFormat);
            }
        }

        public string GetTotalDiscountAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalDiscount, this.CurrencyFormat);
            }
        }

        public string GetTotalSurchargeAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalSurcharge, this.CurrencyFormat);
            }
        }

        public string GetTotalFeeAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalFee, this.CurrencyFormat);
            }
        }

        public string GetTotalShippingAndHandlingAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalShippingAndHandling, this.CurrencyFormat);
            }
        }

        public string GetTotalExVatAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalExVat, this.CurrencyFormat);
            }
        }

        public string GetTotalVatAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalVat, this.CurrencyFormat);
            }
        }

        public string GetTotalIncVatAsCurrencyString
        {
            get
            {
                return DecimalExtensions.AsCurrencyString(this.TotalIncVat, this.CurrencyFormat);
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
                        return DecimalExtensions.AsCurrencyString(this.DiscountAdjustment.Amount, this.CurrencyFormat);
                    }
                }

                return string.Empty;
            }
        }

        public string GetExtraDiscountPercentage
        {
            get
            {
                if (this.ExistDiscountAdjustment && this.DiscountAdjustment.Percentage.HasValue)
                {
                    var percentage = this.DiscountAdjustment.Percentage;

                    if (this.ExistLocale)
                    {
                        return percentage.Value.ToString(this.Locale.CultureInfo);
                    }

                    return percentage.ToString();
                }

                return null;
            }
        }

        public string GetSubTotalAsCurrencyString
        {
            get
            {
                var subtotal = this.TotalBasePrice - this.TotalDiscount + this.TotalSurcharge;
                return subtotal.AsCurrencyString(this.CurrencyFormat);
            }
        }

        public bool HasDiscount
        {
            get
            {
                return this.TotalDiscount > 0;
            }
        }

        public bool HasSurcharge
        {
            get
            {
                return this.TotalSurcharge > 0;
            }
        }

        public bool HasFee
        {
            get
            {
                return this.TotalFee > 0;
            }
        }

        public bool HasShippingAndHandling
        {
            get
            {
                return this.TotalShippingAndHandling > 0;
            }
        }

        public string GetNothingAsCurrencyString
        {
            get
            {
                return 0m.AsCurrencyString(this.CurrencyFormat);
            }
        }

        private string AppsSalesRepNames
        {
            get
            {
                var names = new StringBuilder();
                foreach (Person salesRep in this.SalesReps)
                {
                    if (names.Length > 0)
                    {
                        names.Append(", ");
                    }

                    names.Append(salesRep.PartyName);
                }

                return names.ToString();
            }
        }

        public string TotalExVatAsCurrencyString()
        {
            return this.GetTotalExVatAsCurrencyString;
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

            if (!this.ExistBilledFromInternalOrganisation)
            {
                this.BilledFromInternalOrganisation = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }

            if (!this.ExistStore && this.ExistBilledFromInternalOrganisation)
            {
                if (this.BilledFromInternalOrganisation.StoresWhereOwner.Count == 1)
                {
                    this.Store = this.BilledFromInternalOrganisation.StoresWhereOwner.First;
                }
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
                    if (this.ExistBilledFromInternalOrganisation)
                    {
                        this.CustomerCurrency = this.BilledFromInternalOrganisation.ExistPreferredCurrency ? this.BilledFromInternalOrganisation.PreferredCurrency : this.BilledFromInternalOrganisation.Locale.Country.Currency;
                    }
                }
            }

            if (!this.ExistTotalBasePrice)
            {
                this.TotalBasePrice = 0;
            }

            if (!this.ExistTotalBasePriceCustomerCurrency)
            {
                this.TotalBasePriceCustomerCurrency = 0;
            }

            if (!this.ExistTotalDiscount)
            {
                this.TotalDiscount = 0;
            }

            if (!this.ExistTotalDiscountCustomerCurrency)
            {
                this.TotalDiscountCustomerCurrency = 0;
            }

            if (!this.ExistTotalExVat)
            {
                this.TotalExVat = 0;
            }

            if (!this.ExistTotalExVatCustomerCurrency)
            {
                this.TotalExVatCustomerCurrency = 0;
            }

            if (!this.ExistTotalFee)
            {
                this.TotalFee = 0;
            }

            if (!this.ExistTotalFeeCustomerCurrency)
            {
                this.TotalFeeCustomerCurrency = 0;
            }

            if (!this.ExistTotalIncVat)
            {
                this.TotalIncVat = 0;
            }

            if (!this.ExistTotalIncVatCustomerCurrency)
            {
                this.TotalIncVatCustomerCurrency = 0;
            }

            if (!this.ExistTotalShippingAndHandling)
            {
                this.TotalShippingAndHandling = 0;
            }

            if (!this.ExistTotalShippingAndHandlingCustomerCurrency)
            {
                this.TotalShippingAndHandlingCustomerCurrency = 0;
            }

            if (!this.ExistTotalSurcharge)
            {
                this.TotalSurcharge = 0;
            }

            if (!this.ExistTotalSurchargeCustomerCurrency)
            {
                this.TotalSurchargeCustomerCurrency = 0;
            }

            if (!this.ExistTotalVat)
            {
                this.TotalVat = 0;
            }

            if (!this.ExistAmountPaid)
            {
                this.AmountPaid = 0;
            }

            if (!this.ExistTotalVatCustomerCurrency)
            {
                this.TotalVatCustomerCurrency = 0;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistBillToCustomer)
            {
                var customerRelationships = this.BillToCustomer.CustomerRelationshipsWhereCustomer;
                customerRelationships.Filter.AddEquals(CustomerRelationships.Meta.InternalOrganisation, this.BilledFromInternalOrganisation);

                foreach (CustomerRelationship customerRelationship in customerRelationships)
                {
                    if (customerRelationship.FromDate <= DateTime.UtcNow && (!customerRelationship.ExistThroughDate || customerRelationship.ThroughDate >= DateTime.UtcNow))
                    {
                        derivation.AddDependency(this, customerRelationship);
                    }
                }
            }

            if (this.ExistShipToCustomer)
            {
                var customerRelationships = this.ShipToCustomer.CustomerRelationshipsWhereCustomer;
                customerRelationships.Filter.AddEquals(CustomerRelationships.Meta.InternalOrganisation, this.BilledFromInternalOrganisation);

                foreach (CustomerRelationship customerRelationship in customerRelationships)
                {
                    if (customerRelationship.FromDate <= DateTime.UtcNow && (!customerRelationship.ExistThroughDate || customerRelationship.ThroughDate >= DateTime.UtcNow))
                    {
                        derivation.AddDependency(this, customerRelationship);
                    }
                }
            }

            foreach (SalesInvoiceItem invoiceItem in this.InvoiceItems)
            {
                derivation.AddDependency(this, invoiceItem);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistBillToContactMechanism && this.ExistBillToCustomer)
            {
                this.BillToContactMechanism = this.BillToCustomer.BillingAddress;
            }

            if (!this.ExistBilledFromContactMechanism && this.ExistBilledFromInternalOrganisation)
            {
                this.BilledFromContactMechanism = this.BilledFromInternalOrganisation.BillingAddress;
            }

            if (!this.ExistShipToAddress && this.ExistShipToCustomer)
            {
                this.ShipToAddress = this.ShipToCustomer.ShippingAddress;
            }

            this.DeriveInvoiceItems(derivation);

            this.SalesInvoiceItems = this.SalesInvoiceItems.ToArray();

            this.DeriveLocale(derivation);
            this.DeriveInvoiceTotals(derivation);
            this.DeriveCustomers(derivation);
            this.DeriveSalesReps(derivation);
            this.AppsOnDeriveAmountPaid(derivation);

            if (this.ExistBillToCustomer && this.BillToCustomer.ExistCustomerRelationshipsWhereCustomer)
            {
                var customerRelationships = this.BillToCustomer.CustomerRelationshipsWhereCustomer;
                customerRelationships.Filter.AddEquals(CustomerRelationships.Meta.InternalOrganisation, this.BilledFromInternalOrganisation);
                var customerRelationship = customerRelationships.First;

                if (customerRelationship != null)
                {
                    customerRelationship.OnDerive(x => x.WithDerivation(derivation));
                }
            }

            if (this.ExistBillToCustomer && this.ExistBilledFromInternalOrganisation)
            {
                if (!this.BilledFromInternalOrganisation.Equals(this.BillToCustomer.InternalOrganisationWhereCustomer))
                {
                    derivation.Log.AddError(this, SalesInvoices.Meta.BillToCustomer, ErrorMessages.PartyIsNotACustomer);
                }
            }

            if (this.ExistShipToCustomer && this.ExistBilledFromInternalOrganisation)
            {
                if (!this.BilledFromInternalOrganisation.Equals(this.ShipToCustomer.InternalOrganisationWhereCustomer))
                {
                    derivation.Log.AddError(this, SalesInvoices.Meta.ShipToCustomer, ErrorMessages.PartyIsNotACustomer);
                }
            }

            this.DeriveCurrentPaymentStatus(derivation);
            this.DeriveCurrentObjectState(derivation);

            this.PreviousBillToCustomer = this.BillToCustomer;
            this.PreviousShipToCustomer = this.ShipToCustomer;

            this.DeriveTemplate(derivation);
            this.AppsOnDeriveRevenues(derivation);
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            this.RemoveSecurityTokens();
            this.AddSecurityToken(Singleton.Instance(this.Strategy.Session).AdministratorSecurityToken);

            if (this.ExistBilledFromInternalOrganisation)
            {
                var securityToken = this.BilledFromInternalOrganisation.OwnerSecurityToken;

                if (!this.SecurityTokens.Contains(securityToken))
                {
                    this.AddSecurityToken(securityToken);
                }
            }

            if (this.ExistBillToCustomer && !this.SecurityTokens.Contains(this.BillToCustomer.OwnerSecurityToken))
            {
                this.AddSecurityToken(BillToCustomer.OwnerSecurityToken);
            }

            if (this.ExistPreviousBillToCustomer && !this.BillToCustomer.Equals(this.PreviousBillToCustomer))
            {
                this.RemoveSecurityToken(this.PreviousBillToCustomer.OwnerSecurityToken);
            }

            if (this.ExistShipToCustomer && !this.SecurityTokens.Contains(this.ShipToCustomer.OwnerSecurityToken))
            {
                this.AddSecurityToken(ShipToCustomer.OwnerSecurityToken);
            }

            if (this.ExistPreviousShipToCustomer && !this.ShipToCustomer.Equals(this.PreviousShipToCustomer))
            {
                this.RemoveSecurityToken(this.PreviousShipToCustomer.OwnerSecurityToken);
            }
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

            this.DeriveCurrentObjectState(derivation);
        }

        private void DeriveCurrentObjectState(IDerivation derivation)
        {
            if (this.ExistCurrentObjectState && !this.CurrentObjectState.Equals(this.PreviousObjectState))
            {
                this.CurrentObjectState.Process(this);

                var currentStatus = new SalesInvoiceStatusBuilder(this.Strategy.Session).WithSalesInvoiceObjectState(this.CurrentObjectState).Build();
                this.AddInvoiceStatus(currentStatus);
                this.CurrentInvoiceStatus = currentStatus;
            }

            if (this.CurrentObjectState.Equals(new SalesInvoiceObjectStates(this.Strategy.Session).Paid))
            {
                this.DeriveSalesOrderPaymentStatus(derivation);
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
                this.Locale = this.ExistBilledFromInternalOrganisation ? this.BilledFromInternalOrganisation.Locale : Singleton.Instance(this.Strategy.Session).DefaultLocale;
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
            this.DeriveMarkupAndProfitMargin(derivation);
        }

        private void DeriveDiscountAdjustments(IDerivation derivation)
        {
            if (this.DiscountAdjustment != null)
            {
                decimal discount = this.DiscountAdjustment.Percentage.HasValue ? decimal.Round((this.TotalExVat * this.DiscountAdjustment.Percentage.Value) / 100, 2) : this.DiscountAdjustment.Amount.HasValue ? this.DiscountAdjustment.Amount.Value : 0;

                this.TotalDiscount += discount;
                this.TotalExVat -= discount;

                if (this.ExistVatRegime)
                {
                    decimal vat = decimal.Round((discount * this.VatRegime.VatRate.Rate) / 100, 2);
                    this.TotalVat -= vat;
                    this.TotalIncVat -= discount + vat;
                }
            }
        }

        private void DeriveSurchargeAdjustments(IDerivation derivation)
        {
            if (this.ExistSurchargeAdjustment)
            {
                decimal surcharge = this.SurchargeAdjustment.Percentage.HasValue ? decimal.Round((this.TotalExVat * this.SurchargeAdjustment.Percentage.Value) / 100, 2) : this.SurchargeAdjustment.Amount.HasValue ? this.SurchargeAdjustment.Amount.Value : 0;

                this.TotalSurcharge += surcharge;
                this.TotalExVat += surcharge;

                if (this.ExistVatRegime)
                {
                    decimal vat = decimal.Round((surcharge * this.VatRegime.VatRate.Rate) / 100, 2);
                    this.TotalVat += vat;
                    this.TotalIncVat += surcharge + vat;
                }
            }
        }

        private void DeriveTotalFee(IDerivation derivation)
        {
            if (this.ExistFee)
            {
                decimal fee = this.Fee.Percentage.HasValue ? decimal.Round((this.TotalExVat * this.Fee.Percentage.Value) / 100, 2) : this.Fee.Amount.HasValue ? this.Fee.Amount.Value : 0;

                this.TotalFee += fee;
                this.TotalExVat += fee;

                if (this.Fee.ExistVatRate)
                {
                    decimal vat = decimal.Round((fee * this.Fee.VatRate.Rate) / 100, 2);
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
                    decimal.Round((this.TotalExVat * this.ShippingAndHandlingCharge.Percentage.Value) / 100, 2) : this.ShippingAndHandlingCharge.Amount.HasValue ? this.ShippingAndHandlingCharge.Amount.Value : 0;

                this.TotalShippingAndHandling += shipping;
                this.TotalExVat += shipping;

                if (this.ShippingAndHandlingCharge.ExistVatRate)
                {
                    decimal vat = decimal.Round((shipping * this.ShippingAndHandlingCharge.VatRate.Rate) / 100, 2);

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
                this.InitialMarkupPercentage = decimal.Round(((totalUnitBasePrice / totalPurchasePrice) - 1) * 100, 2);
                this.MaintainedMarkupPercentage = decimal.Round(((totalListPrice / totalPurchasePrice) - 1) * 100, 2);

                this.InitialProfitMargin = decimal.Round(((totalUnitBasePrice - totalPurchasePrice) / totalUnitBasePrice) * 100, 2);
                this.MaintainedProfitMargin = decimal.Round(((totalListPrice - totalPurchasePrice) / totalListPrice) * 100, 2);
            }
        }

        public void AppsOnDeriveTemplate(IDerivation derivation)
        {
            StringTemplate template = null;
            if (this.ExistStore && this.ExistBillToCustomer && this.BillToCustomer.ExistLocale)
            {
                var templates = this.Store.SalesInvoiceTemplates;
                templates.Filter.AddEquals(StringTemplates.Meta.Locale, this.BillToCustomer.Locale);
                template = templates.First;
            }

            if (this.ExistStore && template == null && this.BilledFromInternalOrganisation.ExistLocale)
            {
                var templates = this.Store.SalesInvoiceTemplates;
                templates.Filter.AddEquals(StringTemplates.Meta.Locale, this.BilledFromInternalOrganisation.Locale);
                template = templates.First;
            }

            if (this.ExistStore && template == null)
            {
                var templates = this.Store.SalesInvoiceTemplates;
                templates.Filter.AddEquals(StringTemplates.Meta.Locale, Singleton.Instance(this.Strategy.Session).DefaultLocale);
                template = templates.First;
            }

            if (template != null)
            {
                this.PrintContent = template.Apply(new Dictionary<string, object> { { "this", this } });
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
                        orderShipment.SalesOrderItem.DeriveCurrentPaymentStatus(derivation);
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
                salesInvoiceItem.DeriveVatRegime(derivation);
                salesInvoiceItem.DeriveSalesRep(derivation);
                salesInvoiceItem.DeriveCurrentObjectState(derivation);

                if (!salesInvoiceItem.ExistShipmentItemWhereInvoiceItem)
                {
                    invoiceFromOrder = false;
                    salesInvoiceItem.DerivePrices(derivation);
                    salesInvoiceItem.DeriveCurrentPaymentStatus(derivation);

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

                    salesInvoiceItem.DerivePrices(derivation, quantity, totalBasePrice);
                    salesInvoiceItem.DeriveCurrentPaymentStatus(derivation);
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
                storeRevenue.OnDerive(x=>x.WithDerivation(derivation));

                if (this.ExistSalesChannel)
                {
                    var salesChannelRevenue = SalesChannelRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                    salesChannelRevenue.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }
    }
}
