// <copyright file="FiscalYearStoreSequenceNumbers.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    // TODO: Why not use Counters?
    public partial class FiscalYearStoreSequenceNumbers
    {
        public string NextSalesInvoiceNumber(int year)
        {
            var number = this.SalesInvoiceNumberCounter.NextValue();
            return string.Concat(this.ExistSalesInvoiceNumberPrefix ? this.SalesInvoiceNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }

        public string NextCreditNoteNumber(int year)
        {
            var number = this.CreditNoteNumberCounter.NextValue();
            return string.Concat(this.ExistCreditNoteNumberPrefix ? this.CreditNoteNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }

        public string NextOutgoingShipmentNumber(int year)
        {
            var number = this.OutgoingShipmentNumberCounter.NextValue();
            return string.Concat(this.ExistOutgoingShipmentNumberPrefix ? this.OutgoingShipmentNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }

        public string NextSalesOrderNumber(int year)
        {
            var number = this.SalesOrderNumberCounter.NextValue();
            return string.Concat(this.ExistSalesOrderNumberPrefix ? this.SalesOrderNumberPrefix.Replace("{year}", year.ToString()) : string.Empty, number);
        }
    }
}
