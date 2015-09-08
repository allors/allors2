// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoreRevenue.cs" company="Allors bvba">
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

    public partial class StoreRevenue
    {
        public string RevenueAsCurrencyString()
        {
            return DecimalExtensions.AsCurrencyString(this.Revenue, this.InternalOrganisation.CurrencyFormat);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            
            this.StoreName = this.Store.Name;

            this.AppsOnDeriveRevenue(derivation);
        }

        public void AppsOnDeriveRevenue(IDerivation derivation)
        {
            this.Revenue = 0;

            var toDate = DateTimeFactory.CreateDate(this.Year, this.Month, 01).AddMonths(1);

            var invoices = this.Store.SalesInvoicesWhereStore;
            invoices.Filter.AddNot().AddEquals(SalesInvoices.Meta.CurrentObjectState, new SalesInvoiceObjectStates(this.Strategy.Session).WrittenOff);
            invoices.Filter.AddBetween(SalesInvoices.Meta.InvoiceDate, DateTimeFactory.CreateDate(this.Year, this.Month, 01), toDate);

            foreach (SalesInvoice salesInvoice in invoices)
            {
                this.Revenue += salesInvoice.TotalExVat;
            }

            var months = ((DateTime.UtcNow.Year - this.Year) * 12) + DateTime.UtcNow.Month - this.Month;
            if (months <= 12)
            {
                var histories = this.Store.StoreRevenueHistoriesWhereStore;
                histories.Filter.AddEquals(StoreRevenueHistories.Meta.InternalOrganisation, this.InternalOrganisation);
                var history = histories.First ?? new StoreRevenueHistoryBuilder(this.Strategy.Session)
                                                     .WithCurrency(this.Currency)
                                                     .WithInternalOrganisation(this.InternalOrganisation)
                                                     .WithStore(this.Store)
                                                     .Build();

                history.AppsOnDeriveRevenue();
            }

            var internalOrganisationRevenue = InternalOrganisationRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
            internalOrganisationRevenue.OnDerive(x => x.WithDerivation(derivation));
        }
    }
}
