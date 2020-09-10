// <copyright file="PurchaseOrderItemBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    public static partial class PurchaseOrderItemBuilderExtensions
    {
        public static PurchaseOrderItemBuilder WithSerializedPartDefaults(this PurchaseOrderItemBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var serializedPart = new UnifiedGoodBuilder(@this.Session).WithSerialisedDefaults(internalOrganisation).Build();
            var serializedItem = new SerialisedItemBuilder(@this.Session).WithDefaults(internalOrganisation).Build();
            serializedPart.AddSerialisedItem(serializedItem);

            @this.Session.Derive();
            @this.Session.Commit();

            @this.WithDescription(faker.Lorem.Sentences(2));
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithInvoiceItemType(new InvoiceItemTypes(@this.Session).ProductItem);
            @this.WithPart(serializedPart);
            @this.WithAssignedUnitPrice(faker.Random.UInt(5, 10));
            @this.WithSerialisedItem(serializedItem);
            @this.WithQuantityOrdered(1);
            @this.WithAssignedDeliveryDate(@this.Session.Now().AddDays(5));
            @this.WithShippingInstruction(faker.Lorem.Sentences(3));
            @this.WithMessage(faker.Lorem.Sentence());

            return @this;
        }

        public static PurchaseOrderItemBuilder WithNonSerializedPartDefaults(this PurchaseOrderItemBuilder @this, Organisation internalOrganisation, Party supplier)
        {
            var faker = @this.Session.Faker();

            var nonSerializedPart = new NonUnifiedPartBuilder(@this.Session).WithNonSerialisedDefaults(internalOrganisation).Build();

            new SupplierOfferingBuilder(@this.Session)
                .WithPart(nonSerializedPart)
                .WithSupplier(supplier)
                .WithFromDate(@this.Session.Now().AddMinutes(-1))
                .WithUnitOfMeasure(new UnitsOfMeasure(@this.Session).Piece)
                .WithPrice(faker.Random.UInt(5, 10))
                .Build();

            @this.Session.Derive();
            @this.Session.Commit();

            @this.WithDescription(faker.Lorem.Sentences(2));
            @this.WithInvoiceItemType(new InvoiceItemTypes(@this.Session).PartItem);
            @this.WithPart(nonSerializedPart);
            @this.WithAssignedUnitPrice(faker.Random.UInt(5, 10));
            @this.WithQuantityOrdered(faker.Random.UInt(5, 15));
            @this.WithAssignedDeliveryDate(@this.Session.Now().AddDays(5));
            @this.WithShippingInstruction(faker.Lorem.Sentences(3));
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithMessage(faker.Lorem.Sentence());

            return @this;
        }
    }
}
