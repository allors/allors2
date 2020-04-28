// <copyright file="QuoteTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System.Linq;
    using Allors.Domain.Derivations.Default;
    using Allors.Domain.TestPopulation;
    using Allors.Meta;

    using Xunit;

    public class QuoteTests : DomainTest
    {
        [Fact]
        public void GivenProductQuote_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var party = new PersonBuilder(this.Session).WithLastName("party").Build();

            this.Session.Commit();

            var builder = new ProductQuoteBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithReceiver(party);
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
            var party = new PersonBuilder(this.Session).WithLastName("party").Build();

            this.Session.Commit();

            var builder = new ProposalBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithReceiver(party);
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
            var party = new PersonBuilder(this.Session).WithLastName("party").Build();

            this.Session.Commit();

            var builder = new StatementOfWorkBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithReceiver(party);
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
            var party = new PersonBuilder(this.Session).WithLastName("party").Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            var quote = new ProductQuoteBuilder(this.Session)
                .WithReceiver(party)
                .WithFullfillContactMechanism(new WebAddressBuilder(this.Session).WithElectronicAddressString("test").Build())
                .Build();

            var item1 = new QuoteItemBuilder(this.Session).WithProduct(good).WithQuantity(1).WithAssignedUnitPrice(1000).Build();
            var item2 = new QuoteItemBuilder(this.Session).WithProduct(good).WithQuantity(3).WithAssignedUnitPrice(100).Build();

            quote.AddQuoteItem(item1);
            quote.AddQuoteItem(item2);

            this.Session.Derive();

            Assert.Equal(1300, quote.TotalExVat);
        }
    }
}
