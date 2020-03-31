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

            Assert.Equal(1300, quote.TotalIncVat);
        }

        [Fact]
        public void GivenSettingSerialisedItemAssignedOnProductQuoteSend_WhenSendingQuote_ThenSerialisedItemStateIsChanged()
        {
            this.InternalOrganisation.SerialisedItemAssignedOn = new SerialisedItemAssignedOns(this.Session).ProductQuoteSend;

            this.Session.Derive();

            var quote = new ProductQuoteBuilder(this.Session).WithSerializedDefaults(this.InternalOrganisation).Build();

            var serialisedItem = quote.QuoteItems.First().SerialisedItem;
            serialisedItem.SerialisedItemState = new SerialisedItemStates(this.Session).Good;

            this.Session.Derive();

            quote.Approve();
            this.Session.Derive();

            Assert.NotEqual(new SerialisedItemStates(this.Session).Assigned, serialisedItem.SerialisedItemState);

            quote.Send();
            this.Session.Derive();

            Assert.Equal(new SerialisedItemStates(this.Session).Assigned, serialisedItem.SerialisedItemState);
        }

        [Fact]
        public void GivenSettingSerialisedItemAssignedOnSalesOrderConfirm_WhenSendingQuote_ThenSerialisedItemStateIsNotChanged()
        {
            this.InternalOrganisation.SerialisedItemAssignedOn = new SerialisedItemAssignedOns(this.Session).SalesOrderPost;

            this.Session.Derive();

            var quote = new ProductQuoteBuilder(this.Session).WithSerializedDefaults(this.InternalOrganisation).Build();

            var serialisedItem = quote.QuoteItems.First().SerialisedItem;
            serialisedItem.SerialisedItemState = new SerialisedItemStates(this.Session).Good;

            this.Session.Derive();

            quote.Approve();
            this.Session.Derive();

            Assert.NotEqual(new SerialisedItemStates(this.Session).Assigned, serialisedItem.SerialisedItemState);

            quote.Send();
            this.Session.Derive();

            Assert.NotEqual(new SerialisedItemStates(this.Session).Assigned, serialisedItem.SerialisedItemState);
        }

        [Fact]
        public void GivenSettingSerialisedItemAssignedOnProductQuoteSend_WhenSendingSalesOrder_ThenSerialisedItemStateIsNotChanged()
        {
            this.InternalOrganisation.SerialisedItemAssignedOn = new SerialisedItemAssignedOns(this.Session).ProductQuoteSend;
            this.InternalOrganisation.SerialisedItemSoldOn = new SerialisedItemSoldOns(this.Session).SalesOrderPost;

            this.Session.Derive();

            var quote = new ProductQuoteBuilder(this.Session).WithSerializedDefaults(this.InternalOrganisation).Build();

            var serialisedItem = quote.QuoteItems.First().SerialisedItem;
            serialisedItem.SerialisedItemState = new SerialisedItemStates(this.Session).Good;

            this.Session.Derive();

            quote.Approve();
            this.Session.Derive();

            Assert.NotEqual(new SerialisedItemStates(this.Session).Assigned, serialisedItem.SerialisedItemState);

            quote.Send();
            this.Session.Derive();

            Assert.Equal(new SerialisedItemStates(this.Session).Assigned, serialisedItem.SerialisedItemState);

            quote.Order();

            this.Session.Derive();

            var salesOrder = quote.SalesOrderWhereQuote;
            salesOrder.SalesOrderItems.First.NewSerialisedItemState = new SerialisedItemStates(this.Session).Sold;

            salesOrder.SetReadyForPosting();
            this.Session.Derive();

            salesOrder.Post();
            this.Session.Derive();

            salesOrder.Accept();
            this.Session.Derive();

            Assert.Equal(new SerialisedItemStates(this.Session).Sold, serialisedItem.SerialisedItemState);

            var derivation = new Derivation(this.Session);
            derivation.Mark(quote);
            derivation.Derive();

            Assert.Equal(new SerialisedItemStates(this.Session).Sold, serialisedItem.SerialisedItemState);
        }

        [Fact]
        public void GivenSettingSerialisedItemAssignedOnSalesOrderConfirm_WhenSendingSalesOrder_ThenSerialisedItemStateIsChanged()
        {
            this.InternalOrganisation.SerialisedItemAssignedOn = new SerialisedItemAssignedOns(this.Session).SalesOrderPost;

            this.Session.Derive();

            var quote = new ProductQuoteBuilder(this.Session).WithSerializedDefaults(this.InternalOrganisation).Build();

            var serialisedItem = quote.QuoteItems.First().SerialisedItem;
            serialisedItem.SerialisedItemState = new SerialisedItemStates(this.Session).Good;

            this.Session.Derive();

            quote.Approve();
            this.Session.Derive();

            Assert.NotEqual(new SerialisedItemStates(this.Session).Assigned, serialisedItem.SerialisedItemState);

            quote.Send();
            this.Session.Derive();

            Assert.NotEqual(new SerialisedItemStates(this.Session).Assigned, serialisedItem.SerialisedItemState);
        }
    }
}
