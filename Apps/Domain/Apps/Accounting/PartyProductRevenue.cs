// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyProductRevenue.cs" company="Allors bvba">
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

    public partial class PartyProductRevenue
    {
        public string RevenueAsCurrencyString()
        {
            return DecimalExtensions.AsCurrencyString(this.Revenue, this.InternalOrganisation.CurrencyFormat);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.PartyProductName = string.Concat(this.Party.PartyName, "/", this.Product.Name);

            this.AppsOnDeriveRevenue(derivation);
        }

        public void AppsOnDeriveRevenue(IDerivation derivation)
        {
            this.Revenue = 0;
            this.Quantity = 0;

            var toDate = DateTimeFactory.CreateDate(this.Year, this.Month, 01).AddMonths(1);

            var invoices = this.Party.SalesInvoicesWhereBillToCustomer;
            invoices.Filter.AddEquals(SalesInvoices.Meta.BilledFromInternalOrganisation, this.InternalOrganisation);https://teamcity.inxin.net/win32/userStatus.html?small=1#
            invoices.Filter.AddNot().AddEquals(SalesInvoices.Meta.CurrentObjectState, new SalesInvoiceObjectStates(this.Strategy.Session).WrittenOff);
            invoices.Filter.AddBetween(SalesInvoices.Meta.InvoiceDate, DateTimeFactory.CreateDate(this.Year, this.Month, 01), toDate);

            foreach (SalesInvoice salesInvoice in invoices)
            {
                foreach (SalesInvoiceItem salesInvoiceItem in salesInvoice.SalesInvoiceItems)
                {
                    if (salesInvoiceItem.ExistProduct && salesInvoiceItem.Product.Equals(this.Product))
                    {
                        this.Revenue += salesInvoiceItem.TotalExVat;
                        this.Quantity += salesInvoiceItem.Quantity;
                    }
                }
            }

            var months = ((DateTime.UtcNow.Year - this.Year) * 12) + DateTime.UtcNow.Month - this.Month;
            if (months <= 12)
            {
                var histories = this.Party.PartyProductRevenueHistoriesWhereParty;
                histories.Filter.AddEquals(PartyProductRevenueHistories.Meta.InternalOrganisation, this.InternalOrganisation);
                histories.Filter.AddEquals(PartyProductRevenueHistories.Meta.Product, this.Product);
                var history = histories.First ?? new PartyProductRevenueHistoryBuilder(this.Strategy.Session)
                                                     .WithCurrency(this.Currency)
                                                     .WithInternalOrganisation(this.InternalOrganisation)
                                                     .WithParty(this.Party)
                                                     .WithProduct(this.Product)
                                                     .WithRevenue(0)
                                                     .WithQuantity(0)
                                                     .Build();

                history.AppsOnDeriveHistory();
            }

            if (this.ExistParty)
            {
                var partyRevenue = PartyRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                partyRevenue.OnDerive(x => x.WithDerivation(derivation));
            }

            if (this.ExistProduct)
            {
                var productRevenue = ProductRevenues.AppsFindOrCreateAsDependable(this.Strategy.Session, this);
                productRevenue.OnDerive(x => x.WithDerivation(derivation));

                foreach (ProductCategory productCategory in this.Product.ProductCategories)
                {
                    var partyProductCategoryRevenues = this.Party.PartyProductCategoryRevenuesWhereParty;
                    partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.InternalOrganisation, this.InternalOrganisation);
                    partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.Year, this.Year);
                    partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.Month, this.Month);
                    partyProductCategoryRevenues.Filter.AddEquals(PartyProductCategoryRevenues.Meta.ProductCategory, productCategory);
                    var partyProductCategoryRevenue = partyProductCategoryRevenues.First
                                                      ?? new PartyProductCategoryRevenueBuilder(this.Strategy.Session)
                                                                .WithInternalOrganisation(this.InternalOrganisation)
                                                                .WithParty(this.Party)
                                                                .WithProductCategory(productCategory)
                                                                .WithYear(this.Year)
                                                                .WithMonth(this.Month)
                                                                .WithCurrency(this.Currency)
                                                                .WithRevenue(0M)
                                                                .Build();
                    partyProductCategoryRevenue.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }
    }
}
