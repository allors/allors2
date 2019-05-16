// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceExtensions.cs" company="Allors bvba">
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

using System;
using Allors.Meta;
using Resources;

namespace Allors.Domain
{
    public static partial class InvoiceExtensions
    {
        public static void AppsSetPaid(this Invoice @this, InvoiceSetPaid method)
        {
            var amountToPay = @this.TotalIncVat - @this.AmountPaid;
            var salesInvoice = @this as SalesInvoice;
            var puchaseInvoice = @this as PurchaseInvoice;
            var sender = @this.GetType().Name == typeof(SalesInvoice).Name ? salesInvoice.BilledFrom : puchaseInvoice.BilledTo;

            var paymentApplication = new PaymentApplicationBuilder(@this.Strategy.Session)
                .WithInvoice(@this)
                .WithAmountApplied(amountToPay)
                .Build();

            if (salesInvoice != null)
            {
                new ReceiptBuilder(@this.Strategy.Session)
                    .WithAmount(amountToPay)
                    .WithEffectiveDate(@this.Strategy.Session.Now().Date)
                    .WithPaymentApplication(paymentApplication)
                    .WithSender(sender)
                    .Build();
            }
            else if (puchaseInvoice != null)
            {
                new DisbursementBuilder(@this.Strategy.Session)
                    .WithAmount(amountToPay)
                    .WithEffectiveDate(@this.Strategy.Session.Now().Date)
                    .WithPaymentApplication(paymentApplication)
                    .WithSender(sender)
                    .Build();
            }
        }
    }
}