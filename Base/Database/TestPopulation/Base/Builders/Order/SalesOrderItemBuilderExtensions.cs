// <copyright file="SalesOrderItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public static partial class SalesOrderItemBuilderExtensions
    {
        public static SalesOrderItemBuilder WithDefaults(this SalesOrderItemBuilder @this)
        {
            var faker = @this.Session.Faker();
            var invoiceItemTypes = @this.Session.Extent<InvoiceItemType>().ToList();
            var otherInvoiceItemTypes = invoiceItemTypes.Except(new List<InvoiceItemType>
            {
                invoiceItemTypes.Where(v => v.UniqueId.Equals(InvoiceItemTypes.ProductItemId) || v.UniqueId.Equals(InvoiceItemTypes.PartItemId)).FirstOrDefault(),
            }).ToList();

            var unifiedGoodExtent = @this.Session.Extent<UnifiedGood>();
            unifiedGoodExtent.Filter.AddEquals(M.UnifiedGood.InventoryItemKind.RoleType, new InventoryItemKinds(@this.Session).Serialised);
            var serializedProduct = unifiedGoodExtent.First();

            @this.WithDescription(faker.Lorem.Sentences(2));
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithInvoiceItemType(faker.Random.ListItem(otherInvoiceItemTypes));
            @this.WithProduct(serializedProduct);
            @this.WithSerialisedItem(serializedProduct.SerialisedItems.First);
            @this.WithQuantityOrdered(1);
            @this.WithAssignedUnitPrice(faker.Random.UInt());

            return @this;
        }

        public static SalesOrderItemBuilder WithGSEDefaults(this SalesOrderItemBuilder @this)
        {
            var faker = @this.Session.Faker();
            var invoiceItemType = @this.Session.Extent<InvoiceItemType>().Where(v => v.UniqueId.Equals(InvoiceItemTypes.ProductItemId)).FirstOrDefault();

            var unifiedGoodExtent = @this.Session.Extent<UnifiedGood>();
            unifiedGoodExtent.Filter.AddEquals(M.UnifiedGood.InventoryItemKind.RoleType, new InventoryItemKinds(@this.Session).Serialised);
            var serializedProduct = unifiedGoodExtent.First();

            @this.WithDescription(faker.Lorem.Sentences(2));
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithInvoiceItemType(invoiceItemType);
            @this.WithProduct(serializedProduct);
            @this.WithSerialisedItem(serializedProduct.SerialisedItems.First);
            @this.WithQuantityOrdered(1);
            @this.WithAssignedUnitPrice(faker.Random.UInt());

            return @this;
        }
    }
}
