//------------------------------------------------------------------------------------------------- 
// <copyright file="QuoteTests.cs" company="Allors bvba">
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
    using Xunit;

    public class QuoteTests : DomainTest
    {
        [Fact]
        public void GivenProductQuote_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ProductQuoteBuilder(this.Session);
            var productQuote = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("ProductQuote");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFullfillContactMechanism(new WebAddressBuilder(this.Session).WithElectronicAddressString("test").Build());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenProposal_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ProposalBuilder(this.Session);
            var requirement = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("Proposal");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFullfillContactMechanism(new WebAddressBuilder(this.Session).WithElectronicAddressString("test").Build());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenStatementOfWork_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new StatementOfWorkBuilder(this.Session);
            var statementOfWork = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("StatementOfWork");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithFullfillContactMechanism(new WebAddressBuilder(this.Session).WithElectronicAddressString("test").Build());
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenProductQuote_WhenDeriving_ThenTotalPriceIsDerivedFromItems()
        {
            var good = new GoodBuilder(this.Session)
                .WithSku("10101")
                .WithVatRate(new VatRateBuilder(this.Session).WithRate(21).Build())
                .WithName("good1")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrimaryProductCategory(this.Session.Extent<ProductCategory>().First)
                .Build();

            var quote = new ProductQuoteBuilder(this.Session)
            .WithFullfillContactMechanism(new WebAddressBuilder(this.Session).WithElectronicAddressString("test").Build())
            .Build();

            var item1 = new QuoteItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithUnitPrice(1000).Build();
            var item2 = new QuoteItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithUnitPrice(100).Build();
            
            quote.AddQuoteItem(item1);
            quote.AddQuoteItem(item2);

            this.Session.Derive();

            Assert.Equal(1300, quote.Price);
        }
    }
}