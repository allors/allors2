// <copyright file="Store.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;

    public partial class Store
    {
        public string NextSalesInvoiceNumber(int year)
        {
            if (this.InternalOrganisation.InvoiceSequence.Equals(new InvoiceSequences(this.Strategy.Session).EnforcedSequence))
            {
                return string.Concat(this.SalesInvoiceNumberPrefix, this.SalesInvoiceNumberCounter.NextValue()).Replace("{year}", year.ToString());
            }
            else
            {
                var fiscalYearStoreSequenceNumbers = this.FiscalYearsStoreSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);

                if (fiscalYearStoreSequenceNumbers == null)
                {
                    fiscalYearStoreSequenceNumbers = new FiscalYearStoreSequenceNumbersBuilder(this.Strategy.Session).WithFiscalYear(year).Build();
                    this.AddFiscalYearsStoreSequenceNumber(fiscalYearStoreSequenceNumbers);
                }

                return fiscalYearStoreSequenceNumbers.NextSalesInvoiceNumber(year);
            }
        }

        public string NextCreditNoteNumber(int year)
        {
            if (this.InternalOrganisation.InvoiceSequence.Equals(new InvoiceSequences(this.Strategy.Session).EnforcedSequence))
            {
                return string.Concat(this.CreditNoteNumberPrefix, this.CreditNoteNumberCounter.NextValue()).Replace("{year}", year.ToString());
            }
            else
            {
                var fiscalYearStoreSequenceNumbers = this.FiscalYearsStoreSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);

                if (fiscalYearStoreSequenceNumbers == null)
                {
                    fiscalYearStoreSequenceNumbers = new FiscalYearStoreSequenceNumbersBuilder(this.Strategy.Session).WithFiscalYear(year).Build();
                    this.AddFiscalYearsStoreSequenceNumber(fiscalYearStoreSequenceNumbers);
                }

                return fiscalYearStoreSequenceNumbers.NextCreditNoteNumber(year);
            }
        }

        public string NextOutgoingShipmentNumber(int year)
        {
            if (this.InternalOrganisation.CustomerShipmentSequence.Equals(new CustomerShipmentSequences(this.Strategy.Session).EnforcedSequence))
            {
                return string.Concat(this.OutgoingShipmentNumberPrefix, this.OutgoingShipmentNumberCounter.NextValue()).Replace("{year}", year.ToString());
            }
            else
            {
                var fiscalYearStoreSequenceNumbers = this.FiscalYearsStoreSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);

                if (fiscalYearStoreSequenceNumbers == null)
                {
                    fiscalYearStoreSequenceNumbers = new FiscalYearStoreSequenceNumbersBuilder(this.Strategy.Session).WithFiscalYear(year).Build();
                    this.AddFiscalYearsStoreSequenceNumber(fiscalYearStoreSequenceNumbers);
                }

                return fiscalYearStoreSequenceNumbers.NextOutgoingShipmentNumber(year);
            }
        }

        public string NextSalesOrderNumber(int year)
        {
            if (this.InternalOrganisation.InvoiceSequence.Equals(new InvoiceSequences(this.Strategy.Session).EnforcedSequence))
            {
                return string.Concat(this.SalesOrderNumberPrefix, this.SalesOrderNumberCounter.NextValue()).Replace("{year}", year.ToString());
            }
            else
            {
                var fiscalYearStoreSequenceNumbers = this.FiscalYearsStoreSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);

                if (fiscalYearStoreSequenceNumbers == null)
                {
                    fiscalYearStoreSequenceNumbers = new FiscalYearStoreSequenceNumbersBuilder(this.Strategy.Session).WithFiscalYear(year).Build();
                    this.AddFiscalYearsStoreSequenceNumber(fiscalYearStoreSequenceNumbers);
                }

                return fiscalYearStoreSequenceNumbers.NextSalesOrderNumber(year);
            }
        }

        public string NextTemporaryInvoiceNumber() => this.SalesInvoiceTemporaryCounter.NextValue().ToString();

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistAutoGenerateCustomerShipment)
            {
                this.AutoGenerateCustomerShipment = true;
            }

            if (!this.ExistAutoGenerateShipmentPackage)
            {
                this.AutoGenerateShipmentPackage = true;
            }

            if (!this.ExistBillingProcess)
            {
                this.BillingProcess = new BillingProcesses(this.Strategy.Session).BillingForShipmentItems;
            }

            if (!this.ExistSalesInvoiceTemporaryCounter)
            {
                this.SalesInvoiceTemporaryCounter = new CounterBuilder(this.Strategy.Session).Build();
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistInternalOrganisation && internalOrganisations.Length == 1)
            {
                this.InternalOrganisation = internalOrganisations.First();
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

                if (!this.ExistCollectionMethods || !this.CollectionMethods.Contains(this.DefaultCollectionMethod))
                {
                    this.AddCollectionMethod(this.DefaultCollectionMethod);
                }
            }

            if (!this.ExistDefaultFacility)
            {
                this.DefaultFacility = this.Strategy.Session.GetSingleton().Settings.DefaultFacility;
            }

            if (this.InternalOrganisation.InvoiceSequence != new InvoiceSequences(this.Session()).RestartOnFiscalYear)
            {
                if (!this.ExistSalesInvoiceNumberCounter)
                {
                    this.SalesInvoiceNumberCounter = new CounterBuilder(this.Strategy.Session).Build();
                }

                if (!this.ExistSalesOrderNumberCounter)
                {
                    this.SalesOrderNumberCounter = new CounterBuilder(this.Strategy.Session).Build();
                }

                if (!this.ExistCreditNoteNumberCounter)
                {
                    this.CreditNoteNumberCounter = new CounterBuilder(this.Strategy.Session).Build();
                }
            }

            if (this.InternalOrganisation.CustomerShipmentSequence != new CustomerShipmentSequences(this.Session()).RestartOnFiscalYear)
            {

                if (!this.ExistOutgoingShipmentNumberCounter)
                {
                    this.OutgoingShipmentNumberCounter = new CounterBuilder(this.Strategy.Session).Build();
                }
            }

            derivation.Validation.AssertExistsAtMostOne(this, M.Store.FiscalYearsStoreSequenceNumbers, M.Store.SalesInvoiceNumberCounter);
        }
    }
}
