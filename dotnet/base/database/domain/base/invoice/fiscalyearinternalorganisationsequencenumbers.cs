// <copyright file="FiscalYearInternalOrganisationSequenceNumbers.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    // TODO: Why not use Counters?
    public partial class FiscalYearInternalOrganisationSequenceNumbers
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseOrderNumberCounter)
            {
                this.PurchaseOrderNumberCounter = new CounterBuilder(this.Session()).Build();
            }

            if (!this.ExistPurchaseInvoiceNumberCounter)
            {
                this.PurchaseInvoiceNumberCounter = new CounterBuilder(this.Session()).Build();
            }

            if (!this.ExistRequestNumberCounter)
            {
                this.RequestNumberCounter = new CounterBuilder(this.Session()).Build();
            }

            if (!this.ExistQuoteNumberCounter)
            {
                this.QuoteNumberCounter = new CounterBuilder(this.Session()).Build();
            }

            if (!this.ExistIncomingShipmentNumberCounter)
            {
                this.IncomingShipmentNumberCounter = new CounterBuilder(this.Session()).Build();
            }

            if (!this.ExistWorkEffortNumberCounter)
            {
                this.WorkEffortNumberCounter = new CounterBuilder(this.Session()).Build();
            }
        }

        public string NextPurchaseOrderNumber(int year)
        {
            var number = this.PurchaseOrderNumberCounter.NextValue();
            return string.Concat(this.ExistPurchaseOrderNumberPrefix ? this.PurchaseOrderNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }

        public string NextPurchaseInvoiceNumber(int year)
        {
            var number = this.PurchaseInvoiceNumberCounter.NextValue();
            return string.Concat(this.ExistPurchaseInvoiceNumberPrefix ? this.PurchaseInvoiceNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }

        public string NextRequestNumber(int year)
        {
            var number = this.RequestNumberCounter.NextValue();
            return string.Concat(this.ExistRequestNumberPrefix ? this.RequestNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }

        public string NextQuoteNumber(int year)
        {
            var number = this.QuoteNumberCounter.NextValue();
            return string.Concat(this.ExistQuoteNumberPrefix ? this.QuoteNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }

        public string NextIncomingShipmentNumber(int year)
        {
            var number = this.IncomingShipmentNumberCounter.NextValue();
            return string.Concat(this.ExistIncomingShipmentNumberPrefix ? this.IncomingShipmentNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }

        public string NextWorkEffortNumber(int year)
        {
            var number = this.WorkEffortNumberCounter.NextValue();
            return string.Concat(this.ExistWorkEffortNumberPrefix ? this.WorkEffortNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }
    }
}
