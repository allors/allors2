// <copyright file="RequestForQuoteBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    public static partial class RequestForQuoteBuilderExtensions
    {
        public static RequestForQuoteBuilder WithSerializedDefaults(this RequestForQuoteBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var requestItem = new RequestItemBuilder(@this.Session).WithSerializedDefaults(internalOrganisation).Build();

            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithRecipient(internalOrganisation);
            @this.WithRequestDate(@this.Session.Now().AddDays(-2));
            @this.WithRequiredResponseDate(@this.Session.Now().AddDays(2));
            @this.WithRequestItem(requestItem);
            @this.WithEmailAddress(faker.Internet.Email());
            @this.WithTelephoneNumber(faker.Phone.PhoneNumber());

            return @this;
        }

        public static RequestForQuoteBuilder WithNonSerializedDefaults(this RequestForQuoteBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var requestItem = new RequestItemBuilder(@this.Session).WithNonSerializedDefaults(internalOrganisation).Build();

            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithRecipient(internalOrganisation);
            @this.WithRequestDate(@this.Session.Now().AddDays(-2));
            @this.WithRequiredResponseDate(@this.Session.Now().AddDays(2));
            @this.WithRequestItem(requestItem);
            @this.WithEmailAddress(faker.Internet.Email());
            @this.WithTelephoneNumber(faker.Phone.PhoneNumber());

            return @this;
        }
    }
}
