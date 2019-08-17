// <copyright file="PaymentApplication.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Domain
{
    using Meta;
    using Resources;

    public partial class PaymentApplication
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            var receipt = this.PaymentWherePaymentApplication;
            if (receipt != null)
            {
                derivation.AddDependency(receipt, this);
            }

            if (this.ExistInvoiceItem)
            {
                derivation.AddDependency(this.InvoiceItem, this);
                foreach (OrderItemBilling orderItemBilling in this.InvoiceItem.OrderItemBillingsWhereInvoiceItem)
                {
                    derivation.AddDependency(orderItemBilling.OrderItem, this);
                }
            }

            if (this.ExistInvoice)
            {
                derivation.AddDependency(this.Invoice, this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExistsAtMostOne(this, M.PaymentApplication.Invoice, M.PaymentApplication.InvoiceItem);

            if (this.ExistPaymentWherePaymentApplication && this.AmountApplied > this.PaymentWherePaymentApplication.Amount)
            {
                derivation.Validation.AddError(this, M.PaymentApplication.AmountApplied, ErrorMessages.PaymentApplicationNotLargerThanPaymentAmount);
            }

            var totalInvoiceAmountPaid = this.Invoice?.PaymentApplicationsWhereInvoice.Sum(v => v.AmountApplied);
            if (this.Invoice is SalesInvoice salesInvoice)
            {
                totalInvoiceAmountPaid += salesInvoice.AdvancePayment;
            }

            if (this.ExistInvoice && totalInvoiceAmountPaid > this.Invoice.TotalIncVat)
            {
                derivation.Validation.AddError(this, M.PaymentApplication.AmountApplied, ErrorMessages.PaymentApplicationNotLargerThanInvoiceAmount);
            }

            var totalInvoiceItemAmountPaid = this.InvoiceItem?.PaymentApplicationsWhereInvoiceItem.Sum(v => v.AmountApplied);
            if (this.ExistInvoiceItem && totalInvoiceItemAmountPaid > this.InvoiceItem.TotalIncVat)
            {
                derivation.Validation.AddError(this, M.PaymentApplication.AmountApplied, ErrorMessages.PaymentApplicationNotLargerThanInvoiceItemAmount);
            }
        }
    }
}
