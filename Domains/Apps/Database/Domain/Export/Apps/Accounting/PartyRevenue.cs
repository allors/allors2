// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyRevenue.cs" company="Allors bvba">
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
namespace Allors.Domain
{
    using System;
    using Meta;

    public partial class PartyRevenue
    {
        internal void AppsOnDeriveRevenue()
        {
            this.RemoveRevenue();

            var year = this.Year;
            var month = this.Month;

            this.Revenue = 0;

            var toDate = DateTimeFactory.CreateDate(year, month, 01).AddMonths(1).AddMilliseconds(-1);

            var invoices = this.Party.SalesInvoicesWhereBillToCustomer;
            invoices.Filter.AddNot().AddEquals(M.SalesInvoice.SalesInvoiceState, new SalesInvoiceStates(this.Strategy.Session).WrittenOff);
            invoices.Filter.AddBetween(M.Invoice.InvoiceDate, DateTimeFactory.CreateDate(year, month, 01), toDate);

            foreach (SalesInvoice salesInvoice in invoices)
            {
                this.Revenue += salesInvoice.TotalExVat;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            derivation.AddDependency(this.Party, this);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            this.AppsOnDeriveRevenue();
        }
    }
}
