// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Store.cs" company="Allors bvba">
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
using System.Linq;

namespace Allors.Domain
{
    using System;
    using Meta;
    using Resources;

    public partial class Store
    {
        public string DeriveNextInvoiceNumber(int year)
        {
            int salesInvoiceNumber;
            if (this.InternalOrganisation.InvoiceSequence.Equals(new InvoiceSequences(this.Strategy.Session).EnforcedSequence))
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
        // public override void RemovePaymentMethod(PaymentMethod value)
        // {
        // if (value.Equals(this.DefaultPaymentMethod))
        // {
        // this.RemoveDefaultPaymentMethod();
        // }

        // base.RemovePaymentMethod(value);
        // }
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

            if (!this.ExistBillingProcess)
            {
                this.BillingProcess = new BillingProcesses(this.strategy.Session).BillingForShipmentItems;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Count() == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
            }

            if (!this.ExistDefaultFacility && this.ExistInternalOrganisation)
            {
                this.DefaultFacility = this.InternalOrganisation.DefaultFacility;
            }

            if (this.ExistDefaultCollectionMethod && !this.CollectionMethods.Contains(this.DefaultCollectionMethod))
            {
                this.AddCollectionMethod(this.DefaultCollectionMethod);
            }

            if (!this.ExistDefaultCollectionMethod && this.CollectionMethods.Count == 1)
            {
                this.DefaultCollectionMethod = this.CollectionMethods.First;
            }

            if (!this.ExistDefaultCollectionMethod && this.InternalOrganisation.ExistDefaultCollectionMethod)
            {
                this.DefaultCollectionMethod = this.InternalOrganisation.DefaultCollectionMethod;

                if (!this.ExistCollectionMethods|| !this.CollectionMethods.Contains(this.DefaultCollectionMethod))
                {
                    this.AddCollectionMethod(this.DefaultCollectionMethod);
                }
            }

            derivation.Validation.AssertExistsAtMostOne(this, M.Store.FiscalYearInvoiceNumbers, M.Store.SalesInvoiceCounter);
        }
    }
}