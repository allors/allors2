// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartyProductRevenue.cs" company="Allors bvba">
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

    public partial class PartyProductRevenue
    {
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
            invoices.Filter.AddNot().AddEquals(M.SalesInvoice.SalesInvoiceState, new SalesInvoiceStates(this.Strategy.Session).WrittenOff);
            invoices.Filter.AddBetween(M.Invoice.InvoiceDate, DateTimeFactory.CreateDate(this.Year, this.Month, 01), toDate);

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
                    partyProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.Year, this.Year);
                    partyProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.Month, this.Month);
                    partyProductCategoryRevenues.Filter.AddEquals(M.PartyProductCategoryRevenue.ProductCategory, productCategory);
                    var partyProductCategoryRevenue = partyProductCategoryRevenues.First
                                                      ?? new PartyProductCategoryRevenueBuilder(this.Strategy.Session)
                                                                .WithParty(this.Party)
                                                                .WithProductCategory(productCategory)
                                                                .WithYear(this.Year)
                                                                .WithMonth(this.Month)
                                                                .WithCurrency(this.Currency)
                                                                .Build();
                    partyProductCategoryRevenue.OnDerive(x => x.WithDerivation(derivation));
                }
            }
        }
    }
}
