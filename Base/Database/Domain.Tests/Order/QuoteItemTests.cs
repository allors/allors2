// <copyright file="QuoteItemTests.cs" company="Allors bvba">
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

    public class QuoteItemTests : DomainTest
    {
        [Fact]
        public void GivenSerialisedItem_WhenDerived_ThenSerialisedItemAvailabilityIsChanged()
        {
            var quote = new ProductQuoteBuilder(this.Session).WithSerializedDefaults(this.InternalOrganisation).Build();

            this.Session.Derive();

            var serialisedItem = quote.QuoteItems.First().SerialisedItem;

            Assert.True(serialisedItem.OnQuote);
        }
    }
}
