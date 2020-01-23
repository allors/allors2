// <copyright file="QuoteItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    public static partial class QuoteItemBuilderExtensions
    {
        public static QuoteItemBuilder WithSerializedDefaults(this QuoteItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var serializedProduct = new UnifiedGoodBuilder(@this.Session).WithSerialisedDefaults(internalOrganisation).Build();
            var serializedItem = new SerialisedItemBuilder(@this.Session).WithDefaults(internalOrganisation).Build();

            @this.WithDetails(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithEstimatedDeliveryDate(@this.Session.Now().AddDays(5)).Build();
            @this.WithInvoiceItemType(new InvoiceItemTypes(@this.Session).ProductItem).Build();
            @this.WithProduct(serializedProduct).Build();
            @this.WithSerialisedItem(serializedItem).Build();
            @this.WithUnitOfMeasure(new UnitsOfMeasure(@this.Session).Piece).Build();
            @this.WithQuantity(faker.Random.UShort()).Build();
            return @this;
        }
    }
}
