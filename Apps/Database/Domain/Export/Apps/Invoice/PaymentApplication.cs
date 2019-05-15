// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentApplication.cs" company="Allors bvba">
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
    using Meta;
    using Resources;

    public partial class PaymentApplication
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
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

        public void AppsOnDerive(ObjectOnDerive method)
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