// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalOrganisation.cs" company="Allors bvba">
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
    using Meta;

    public partial class InternalOrganisation
    {
        // TODO: Cascading delete

        // public override void RemovePaymentMethod(PaymentMethod value)
        // {
        // if (value.Equals(this.DefaultPaymentMethod))
        // {
        // this.RemoveDefaultPaymentMethod();
        // }

        // base.RemovePaymentMethod(value);
        // }
        public void AppsOnDeriveCurrentContacts(IDerivation derivation)
        {
            this.RemoveCurrentContacts();
        }

        public void AppsOnDeriveInactiveContacts(IDerivation derivation)
        {
            this.RemoveInactiveContacts();
        }

        public void AppsOnDeriveCurrentOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveCurrentPartyContactMechanisms();
        }

        public void AppsOnDeriveInactiveOrganisationContactRelationships(IDerivation derivation)
        {
            this.RemoveInactivePartyContactMechanisms();
        }

        public void AppsOnDeriveCurrentPartyContactMechanisms(IDerivation derivation)
        {
            this.RemoveCurrentPartyContactMechanisms();
        }

        public void AppsOnDeriveInactivePartyContactMechanisms(IDerivation derivation)
        {
            this.RemoveInactivePartyContactMechanisms();
        }

        public int DeriveNextSubAccountNumber()
        {
            var next = this.SubAccountCounter.NextElfProefValue();
            return next;
        }

        public string DeriveNextPurchaseInvoiceNumber()
        {
            var purchaseInvoiceNumber = this.PurchaseInvoiceCounter.NextValue();
            return string.Concat(this.PurchaseInvoiceNumberPrefix, purchaseInvoiceNumber);
        }

        public string DeriveNextQuoteNumber()
        {
            var quoteNumber = this.QuoteCounter.NextValue();
            return string.Concat(this.QuoteNumberPrefix, quoteNumber);
        }
        public string DeriveNextRequestNumber()
        {
            var requestNumber = this.RequestCounter.NextValue();
            return string.Concat(this.RequestNumberPrefix, requestNumber);
        }

        public string DeriveNextShipmentNumber()
        {
            var shipmentNumber = this.IncomingShipmentCounter.NextValue();
            return string.Concat(this.IncomingShipmentNumberPrefix, shipmentNumber);
        }

        public string DeriveNextPurchaseOrderNumber()
        {
            var purchaseOrderNumber = this.PurchaseInvoiceCounter.NextValue();
            return string.Concat(this.PurchaseOrderNumberPrefix, purchaseOrderNumber);
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseInvoiceCounter)
            {
                this.PurchaseInvoiceCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistRequestCounter)
            {
                this.RequestCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistQuoteCounter)
            {
                this.QuoteCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistPurchaseOrderCounter)
            {
                this.PurchaseOrderCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistIncomingShipmentCounter)
            {
                this.IncomingShipmentCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistSubAccountCounter)
            {
                this.SubAccountCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistDoAccounting)
            {
                this.DoAccounting = false;
            }

            if (!this.ExistInvoiceSequence)
            {
                this.InvoiceSequence = new InvoiceSequences(this.Strategy.Session).RestartOnFiscalYear;
            }

            if (!this.ExistFiscalYearStartMonth)
            {
                this.FiscalYearStartMonth = 1;
            }

            if (!this.ExistFiscalYearStartDay)
            {
                this.FiscalYearStartDay = 1;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (PaymentMethod paymentMethod in this.PaymentMethods)
            {
                derivation.AddDependency(paymentMethod, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.PartyName = this.Name;

            if (this.ExistPreviousCurrency)
            {
                derivation.Validation.AssertAreEqual(this, M.InternalOrganisation.PreferredCurrency, M.InternalOrganisation.PreviousCurrency);
            }
            else
            {
                this.PreviousCurrency = this.PreferredCurrency;
            }

            this.BillingAddress = null;
            this.BillingInquiriesFax = null;
            this.BillingInquiriesPhone = null;
            this.CellPhoneNumber = null;
            this.GeneralFaxNumber = null;
            this.GeneralPhoneNumber = null;
            this.HeadQuarter = null;
            this.HomeAddress = null;
            this.InternetAddress = null;
            this.OrderAddress = null;
            this.OrderInquiriesFax = null;
            this.OrderInquiriesPhone = null;
            this.PersonalEmailAddress = null;
            this.SalesOffice = null;
            this.ShippingAddress = null;
            this.ShippingInquiriesFax = null;
            this.ShippingAddress = null;

            foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanisms)
            {
                if (partyContactMechanism.UseAsDefault)
                {
                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).BillingAddress))
                    {
                        this.BillingAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).BillingInquiriesFax))
                    {
                        this.BillingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).BillingInquiriesPhone))
                    {
                        this.BillingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).CellPhoneNumber))
                    {
                        this.CellPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).GeneralFaxNumber))
                    {
                        this.GeneralFaxNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).GeneralPhoneNumber))
                    {
                        this.GeneralPhoneNumber = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).HeadQuarters))
                    {
                        this.HeadQuarter = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).HomeAddress))
                    {
                        this.HomeAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).InternetAddress))
                    {
                        this.InternetAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).OrderAddress))
                    {
                        this.OrderAddress = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).OrderInquiriesFax))
                    {
                        this.OrderInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).OrderInquiriesPhone))
                    {
                        this.OrderInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).PersonalEmailAddress))
                    {
                        this.PersonalEmailAddress = partyContactMechanism.ContactMechanism as ElectronicAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).SalesOffice))
                    {
                        this.SalesOffice = partyContactMechanism.ContactMechanism;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).ShippingAddress))
                    {
                        this.ShippingAddress = partyContactMechanism.ContactMechanism as PostalAddress;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).ShippingInquiriesFax))
                    {
                        this.ShippingInquiriesFax = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }

                    if (partyContactMechanism.ContactPurposes.Contains(new ContactMechanismPurposes(this.strategy.Session).ShippingInquiriesPhone))
                    {
                        this.ShippingInquiriesPhone = partyContactMechanism.ContactMechanism as TelecommunicationsNumber;
                    }
                }
            }

            if (this.ExistDefaultPaymentMethod && !this.PaymentMethods.Contains(this.DefaultPaymentMethod))
            {
                this.AddPaymentMethod(this.DefaultPaymentMethod);
            }

            if (!this.ExistDefaultPaymentMethod && this.PaymentMethods.Count == 1)
            {
                this.DefaultPaymentMethod = this.PaymentMethods.First;
            }
        }

        public void AppsStartNewFiscalYear()
        {
            if (this.ExistActualAccountingPeriod && this.ActualAccountingPeriod.Active)
            {
                return;
            }

            int year = DateTime.UtcNow.Year;
            if (this.ExistActualAccountingPeriod)
            {
                year = this.ActualAccountingPeriod.FromDate.Date.Year + 1;
            }

            var fromDate = DateTimeFactory.CreateDate(year, this.FiscalYearStartMonth, this.FiscalYearStartDay).Date;

            var yearPeriod = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Year)
                .WithFromDate(fromDate)
                .WithThroughDate(fromDate.AddYears(1).AddSeconds(-1).Date)
                .Build();

            var semesterPeriod = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Semester)
                .WithFromDate(fromDate)
                .WithThroughDate(fromDate.AddMonths(6).AddSeconds(-1).Date)
                .WithParent(yearPeriod)
                .Build();

            var trimesterPeriod = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Trimester)
                .WithFromDate(fromDate)
                .WithThroughDate(fromDate.AddMonths(3).AddSeconds(-1).Date)
                .WithParent(semesterPeriod)
                .Build();

            var monthPeriod = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Month)
                .WithFromDate(fromDate)
                .WithThroughDate(fromDate.AddMonths(1).AddSeconds(-1).Date)
                .WithParent(trimesterPeriod)
                .Build();

            this.AddAccountingPeriod(yearPeriod);
            this.AddAccountingPeriod(semesterPeriod);
            this.AddAccountingPeriod(trimesterPeriod);
            this.AddAccountingPeriod(monthPeriod);

            this.ActualAccountingPeriod = monthPeriod;
        }

        private int NextValidElevenTestNumer(int previous)
        {
            var candidate = previous.ToString();
            var valid = false;

            while (!valid)
            {
                candidate = previous.ToString();
                var sum = 0;
                for (var i = candidate.Length; i > 0; i--)
                {
                    sum += int.Parse(candidate.Substring(candidate.Length - i, 1)) * i;
                }

                valid = sum % 11 == 0;

                if (!valid)
                {
                    previous++;
                }
            }

            return int.Parse(candidate);
        }

        public List<string> Roles => new List<string>() { "Internal organisation" };
    }
}