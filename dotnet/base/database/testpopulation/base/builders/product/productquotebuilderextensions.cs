// <copyright file="ProductQuoteBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    using System.Linq;

    public static partial class ProductQuoteBuilderExtensions
    {
        public static ProductQuoteBuilder WithSerializedDefaults(this ProductQuoteBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var quoteItem = new QuoteItemBuilder(@this.Session).WithSerializedDefaults(internalOrganisation).Build();
            var customer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);

            @this.WithContactPerson(customer.CurrentContacts.FirstOrDefault());
            @this.WithFullfillContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithIssuer(internalOrganisation);
            @this.WithIssueDate(@this.Session.Now().AddDays(-2));
            @this.WithValidFromDate(@this.Session.Now().AddDays(-2));
            @this.WithValidThroughDate(@this.Session.Now().AddDays(2));
            @this.WithRequiredResponseDate(@this.Session.Now().AddDays(2));
            @this.WithReceiver(customer);
            @this.WithQuoteItem(quoteItem);

            return @this;
        }
    }
}
