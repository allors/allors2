// <copyright file="SalesInvoiceItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class SalesInvoiceItemBuilderExtensions
    {
        public static SalesInvoiceItemBuilder WithDefaults(this SalesInvoiceItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();
            var invoiceItemTypes = @this.Session.Extent<InvoiceItemType>().ToList();
            var otherInvoiceItemTypes = invoiceItemTypes.Except(new List<InvoiceItemType>
            {
                invoiceItemTypes.Where(v => v.UniqueId.Equals(InvoiceItemTypes.ProductItemId) || v.UniqueId.Equals(InvoiceItemTypes.PartItemId)).FirstOrDefault(),
            }).ToList();

            @this.WithDescription(faker.Lorem.Sentences(2));
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithInvoiceItemType(faker.Random.ListItem(otherInvoiceItemTypes));
            @this.WithMessage(faker.Lorem.Sentence());
            @this.WithQuantity(1);
            @this.WithAssignedUnitPrice(faker.Random.UInt());

            return @this;
        }

        public static SalesInvoiceItemBuilder WithGSEDefaults(this SalesInvoiceItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();
            var serializedProduct = new UnifiedGoodBuilder(@this.Session).WithSerialisedDefaults(internalOrganisation).Build();
            var invoiceItemType = @this.Session.Extent<InvoiceItemType>().Where(v => v.UniqueId.Equals(InvoiceItemTypes.ProductItemId)).FirstOrDefault();

            @this.WithDescription(faker.Lorem.Sentences(2));
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithInvoiceItemType(invoiceItemType);
            @this.WithProduct(serializedProduct);
            @this.WithSerialisedItem(serializedProduct.SerialisedItems.First);
            @this.WithMessage(faker.Lorem.Sentence());
            @this.WithQuantity(1);
            @this.WithAssignedUnitPrice(faker.Random.UInt());

            return @this;
        }
    }
}
