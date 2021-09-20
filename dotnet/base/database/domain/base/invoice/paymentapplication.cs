// <copyright file="PaymentApplication.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;
    using Resources;

    public partial class PaymentApplication
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var receipt = this.PaymentWherePaymentApplication;
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (receipt != null)
                {
                    iteration.AddDependency(receipt, this);
                    iteration.Mark(receipt);
                }

                if (this.ExistInvoiceItem)
                {
                    iteration.AddDependency(this.InvoiceItem, this);
                    iteration.Mark(this.InvoiceItem);

                    foreach (OrderItemBilling orderItemBilling in this.InvoiceItem.OrderItemBillingsWhereInvoiceItem)
                    {
                        iteration.AddDependency(orderItemBilling.OrderItem, this);
                        iteration.Mark(orderItemBilling.OrderItem);
                    }
                }

                if (this.ExistInvoice)
                {
                    iteration.AddDependency(this.Invoice, this);
                    iteration.Mark(this.Invoice);
                }
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

        public void BaseDelete(DeletableDelete method)
        {
            if (this.ExistInvoice)
            {
                this.Invoice.DerivationTrigger = Guid.NewGuid();
            }

            if (this.ExistInvoiceItem)
            {
                this.InvoiceItem.DerivationTrigger = Guid.NewGuid();
            }
        }
    }
}
