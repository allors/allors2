// <copyright file="FiscalYearInvoiceNumber.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class FiscalYearInvoiceNumber
    {
        public int DeriveNextSalesInvoiceNumber()
        {
            this.NextSalesInvoiceNumber = this.ExistNextSalesInvoiceNumber ? this.NextSalesInvoiceNumber : 1;
            int salesInvoiceNumber = this.NextSalesInvoiceNumber;
            this.NextSalesInvoiceNumber++;

            return salesInvoiceNumber;
        }

        public int DeriveNextCreditNoteNumber()
        {
            this.NextCreditNoteNumber = this.ExistNextCreditNoteNumber ? this.NextCreditNoteNumber : 1;
            int creditNoteNumber = this.NextCreditNoteNumber;
            this.NextCreditNoteNumber++;

            return creditNoteNumber;
        }
    }
}
