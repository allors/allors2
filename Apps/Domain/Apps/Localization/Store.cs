// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Store.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    using System;

    using Resources;

    public partial class Store
    {
        public string DeriveNextInvoiceNumber(int year)
        {
            int salesInvoiceNumber;
            if (this.Owner.InvoiceSequence.Equals(new InvoiceSequences(this.Strategy.Session).EnforcedSequence))
            {
                salesInvoiceNumber = this.SalesInvoiceCounter.NextValue();
            }
            else
            {
                FiscalYearInvoiceNumber fiscalYearInvoiceNumber = null;
                foreach (FiscalYearInvoiceNumber x in this.FiscalYearInvoiceNumbers)
                {
                    if (x.FiscalYear.Equals(year))
                    {
                        fiscalYearInvoiceNumber = x;
                        break;
                    }
                }

                if (fiscalYearInvoiceNumber == null)
                {
                    fiscalYearInvoiceNumber = new FiscalYearInvoiceNumberBuilder(this.Strategy.Session).WithFiscalYear(year).Build();
                    fiscalYearInvoiceNumber.NextSalesInvoiceNumber = 1;
                    this.AddFiscalYearInvoiceNumber(fiscalYearInvoiceNumber);
                }

                salesInvoiceNumber = fiscalYearInvoiceNumber.DeriveNextSalesInvoiceNumber();
            }

            return string.Concat(this.SalesInvoiceNumberPrefix, salesInvoiceNumber);
        }

        // TODO: Cascading delete
        //public override void RemovePaymentMethod(PaymentMethod value)
        //{
        //    if (value.Equals(this.DefaultPaymentMethod))
        //    {
        //        this.RemoveDefaultPaymentMethod();
        //    }

        //    base.RemovePaymentMethod(value);
        //}

        public string DeriveNextShipmentNumber()
        {
            var shipmentNumber = this.OutgoingShipmentCounter.NextValue();
            return string.Concat(this.OutgoingShipmentNumberPrefix, shipmentNumber);
        }

        public string DeriveNextSalesOrderNumber()
        {
            var salesOrderNumber = this.SalesOrderCounter.NextValue();
            return string.Concat(this.SalesOrderNumberPrefix, salesOrderNumber);
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSalesOrderCounter)
            {
                this.SalesOrderCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistOutgoingShipmentCounter)
            {
                this.OutgoingShipmentCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistCreditLimit)
            {
                this.CreditLimit = 0;
            }
            
            if (!this.ExistShipmentThreshold)
            {
                this.ShipmentThreshold = 0;
            }

            if (!this.ExistOrderThreshold)
            {
                this.OrderThreshold = 0;
            }

            if (!this.ExistPaymentGracePeriod)
            {
                this.PaymentGracePeriod = 0;
            }

            if (new TemplatePurposes(this.Strategy.Session).SalesInvoice != null &&
                new TemplatePurposes(this.Strategy.Session).SalesInvoice.StringTemplatesWhereTemplatePurpose.Count > 0)
            {
                if (!this.ExistSalesInvoiceTemplates)
                {
                    if (this.ExistOwner && this.Owner.ExistLocale)
                    {
                        var template = this.Owner.Locale.StringTemplatesWhereLocale.First(x => x.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).SalesInvoice));
                        this.AddSalesInvoiceTemplate(template);
                    }
                    else
                    {
                        var template = Singleton.Instance(this.Strategy.Session).DefaultLocale.StringTemplatesWhereLocale.First(x => x.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).SalesInvoice));
                        this.AddSalesInvoiceTemplate(template);
                    }
                }
            }

            if (new TemplatePurposes(this.Strategy.Session).SalesOrder != null &&
                new TemplatePurposes(this.Strategy.Session).SalesOrder.StringTemplatesWhereTemplatePurpose.Count > 0)
            {
                if (!this.ExistSalesOrderTemplates)
                {
                    if (this.ExistOwner && this.Owner.ExistLocale)
                    {
                        var template = this.Owner.Locale.StringTemplatesWhereLocale.First(x => x.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).SalesOrder));
                        this.AddSalesOrderTemplate(template);
                    }
                    else
                    {
                        var template = Singleton.Instance(this.Strategy.Session).DefaultLocale.StringTemplatesWhereLocale.First(x => x.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).SalesOrder));
                        this.AddSalesOrderTemplate(template);
                    }
                }
            }

            if (new TemplatePurposes(this.Strategy.Session).CustomerShipment != null &&
                new TemplatePurposes(this.Strategy.Session).CustomerShipment.StringTemplatesWhereTemplatePurpose.Count > 0)
            {
                if (!this.ExistCustomerShipmentTemplates)
                {
                    if (this.ExistOwner && this.Owner.ExistLocale)
                    {
                        var template = this.Owner.Locale.StringTemplatesWhereLocale.First(x => x.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).CustomerShipment));
                        this.AddCustomerShipmentTemplate(template);
                    }
                    else
                    {
                        var template = Singleton.Instance(this.Strategy.Session).DefaultLocale.StringTemplatesWhereLocale.First(x => x.TemplatePurpose.Equals(new TemplatePurposes(this.Strategy.Session).CustomerShipment));
                        this.AddCustomerShipmentTemplate(template);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistOwner)
            {
                this.Owner = Domain.Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;

                if (this.ExistOwner && this.Owner.ExistDefaultFacility)
                {
                    this.DefaultFacility = this.Owner.DefaultFacility;
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

            if (this.ExistOwner)
            {
                if (!this.ExistDefaultPaymentMethod && this.Owner.ExistDefaultPaymentMethod)
                {
                    this.DefaultPaymentMethod = this.Owner.DefaultPaymentMethod;

                    if (!this.ExistPaymentMethods || !this.PaymentMethods.Contains(this.DefaultPaymentMethod))
                    {
                        this.AddPaymentMethod(this.DefaultPaymentMethod);
                    }
                }
            }

            foreach (PaymentMethod paymentMethod in this.PaymentMethods)
            {
                if (this.ExistOwner && !this.Owner.PaymentMethods.Contains(paymentMethod))
                {
                    derivation.Log.AddError(this, Stores.Meta.PaymentMethods, ErrorMessages.PaymentApplicationNotLargerThanInvoiceItemAmount);
                }
            }

            derivation.Log.AssertExistsAtMostOne(this, Stores.Meta.FiscalYearInvoiceNumbers, Stores.Meta.SalesInvoiceCounter);
        }
    }
}