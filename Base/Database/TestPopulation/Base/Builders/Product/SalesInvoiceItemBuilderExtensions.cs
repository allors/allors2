// <copyright file="SalesInvoiceItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    public static partial class SalesInvoiceItemBuilderExtensions
    {
        // TODO: Investigate and Change
        public static SalesInvoiceItemBuilder WithSerializedProductDefaults(this SalesInvoiceItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var serializedProduct = new UnifiedGoodBuilder(@this.Session).WithSerialisedDefaults(internalOrganisation).Build();
            var serializedItem = new SerialisedItemBuilder(@this.Session).WithDefaults(internalOrganisation).Build();

            @this.WithDescription(faker.Lorem.Sentences(2));
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithInvoiceItemType(new InvoiceItemTypes(@this.Session).ProductItem).Build();
            @this.WithProduct(serializedProduct).Build();
            @this.WithSerialisedItem(serializedItem).Build();
            @this.WithMessage(faker.Lorem.Sentence()).Build();

            return @this;
        }

        // TODO: Investigate and Change
        public static SalesInvoiceItemBuilder WithSerializedNonProductDefaults(this SalesInvoiceItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var serializedProduct = new UnifiedGoodBuilder(@this.Session).WithSerialisedDefaults(internalOrganisation).Build();
            var serializedItem = new SerialisedItemBuilder(@this.Session).WithDefaults(internalOrganisation).Build();

            @this.WithDescription(faker.Lorem.Sentences(2));
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());

            // TODO: Select a random item except ProductItem
            @this.WithInvoiceItemType(new InvoiceItemTypes(@this.Session).ProductItem).Build();
            @this.WithProduct(serializedProduct).Build();
            @this.WithSerialisedItem(serializedItem).Build();
            @this.WithMessage(faker.Lorem.Sentence()).Build();

            return @this;
        }
    }
}
