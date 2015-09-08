// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepPartyRevenue.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class SalesRepPartyRevenue
    {
        public string RevenueAsCurrencyString()
        {
            return DecimalExtensions.AsCurrencyString(this.Revenue, this.InternalOrganisation.CurrencyFormat);
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistSalesRep)
            {
                derivation.AddDependency(this.SalesRep, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            
            this.SalesRepName = this.SalesRep.PartyName;

            this.AppsOnDeriveRevenue(derivation);
        }

        public void AppsOnDeriveRevenue(IDerivation derivation)
        {
            this.Revenue = 0;

            var toDate = DateTimeFactory.CreateDate(this.Year, this.Month, 01).AddMonths(1);

            var invoices = this.Party.SalesInvoicesWhereBillToCustomer;
            invoices.Filter.AddEquals(SalesInvoices.Meta.BilledFromInternalOrganisation, this.InternalOrganisation);
            invoices.Filter.AddNot().AddEquals(SalesInvoices.Meta.CurrentObjectState, new SalesInvoiceObjectStates(this.Strategy.Session).WrittenOff);
            invoices.Filter.AddBetween(SalesInvoices.Meta.InvoiceDate, DateTimeFactory.CreateDate(this.Year, this.Month, 01), toDate);

            foreach (SalesInvoice salesInvoice in invoices)
            {
                foreach (SalesInvoiceItem salesInvoiceItem in salesInvoice.SalesInvoiceItems)
                {
                    if (salesInvoiceItem.ExistSalesRep && salesInvoiceItem.SalesRep.Equals(this.SalesRep))
                    {
                        this.Revenue += salesInvoiceItem.TotalExVat;
                    }
                }
            }

            if (this.ExistSalesRep)
            {
                var salesRepRevenue = SalesRepRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                salesRepRevenue.OnDerive(x => x.WithDerivation(derivation));
            }
        }
    }
}