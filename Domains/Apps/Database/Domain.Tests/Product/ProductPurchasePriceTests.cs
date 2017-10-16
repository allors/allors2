//------------------------------------------------------------------------------------------------- 
// <copyright file="ProductPurchasePriceTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using Meta;
    using Xunit;

    
    public class ProductPurchasePriceTests : DomainTest
    {
        [Fact]
        public void GivenProductPurchasePrice_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ProductPurchasePriceBuilder(this.Session);
            var purchasePrice = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithPrice(1);
            purchasePrice = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCurrency(new Currencies(this.Session).FindBy(M.Currency.IsoCode, "EUR"));
            purchasePrice = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFromDate(DateTime.UtcNow);
            purchasePrice = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece);
            purchasePrice = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
