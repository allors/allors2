// <copyright file="OrganisationExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.TestPopulation
{
    using Allors.Domain;
    using Bogus;
    using Person = Allors.Domain.Person;

    public static class OrganisationExtensions
    {
        public static Person CreateEmployee(this Organisation @this, string password, Faker faker)
        {
            var person = new PersonBuilder(@this.Session()).WithDefaults().Build();

            new EmploymentBuilder(@this.Session())
                .WithEmployee(person)
                .WithEmployer(@this)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            new OrganisationContactRelationshipBuilder(@this.Session())
                .WithContact(person)
                .WithOrganisation(@this)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            new UserGroups(@this.Session()).Creators.AddMember(person);

            person.SetPassword(password);

            return person;
        }

        public static Person CreateAdministrator(this Organisation @this, string password, Faker faker)
        {
            var person = @this.CreateEmployee(password, faker);
            new UserGroups(@this.Session()).Administrators.AddMember(person);

            return person;
        }

        public static Organisation CreateB2BCustomer(this Organisation @this, Faker faker)
        {
            var customer = new OrganisationBuilder(@this.Session()).WithDefaults().Build();

            new CustomerRelationshipBuilder(@this.Session())
                .WithCustomer(customer)
                .WithInternalOrganisation(@this)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            new OrganisationContactRelationshipBuilder(@this.Session())
                .WithContact(new PersonBuilder(@this.Session()).WithDefaults().Build())
                .WithOrganisation(customer)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            return customer;
        }

        public static Person CreateB2CCustomer(this Organisation @this, Faker faker)
        {
            var customer = new PersonBuilder(@this.Session()).WithDefaults().Build();

            new CustomerRelationshipBuilder(@this.Session())
                .WithCustomer(customer)
                .WithInternalOrganisation(@this)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            return customer;
        }

        public static Organisation CreateSupplier(this Organisation @this, Faker faker)
        {
            var supplier = new OrganisationBuilder(@this.Session()).WithDefaults().Build();

            new SupplierRelationshipBuilder(@this.Session())
                .WithSupplier(supplier)
                .WithInternalOrganisation(@this)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            new OrganisationContactRelationshipBuilder(@this.Session())
                .WithContact(new PersonBuilder(@this.Session()).WithDefaults().Build())
                .WithOrganisation(supplier)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            return supplier;
        }

        public static Organisation CreateSubContractor(this Organisation @this, Faker faker)
        {
            var subContractor = new OrganisationBuilder(@this.Session()).WithDefaults().Build();

            new SubContractorRelationshipBuilder(@this.Session())
                .WithSubContractor(subContractor)
                .WithContractor(@this)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            new OrganisationContactRelationshipBuilder(@this.Session())
                .WithContact(new PersonBuilder(@this.Session()).WithDefaults().Build())
                .WithOrganisation(subContractor)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            return subContractor;
        }

        public static Part CreateNonSerialisedNonUnifiedPart(this Organisation @this, Faker faker)
        {
            var part = new NonUnifiedPartBuilder(@this.Session()).WithNonSerialisedDefaults(@this).Build();

            foreach (Organisation supplier in @this.CurrentSuppliers)
            {
                new SupplierOfferingBuilder(@this.Session())
                    .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                    .WithSupplier(supplier)
                    .WithPart(part)
                    .WithUnitOfMeasure(part.UnitOfMeasure)
                    .Build();
            }

            new InventoryItemTransactionBuilder(@this.Session())
                .WithPart(part)
                .WithFacility(@this.FacilitiesWhereOwner.First)
                .WithQuantity(faker.Random.Number(1000))
                .WithReason(new InventoryTransactionReasons(@this.Session()).Unknown)
                .Build();

            return part;
        }

        public static Part CreateSerialisedNonUnifiedPart(this Organisation @this, Faker faker)
        {
            var part = new NonUnifiedPartBuilder(@this.Session()).WithSerialisedDefaults(@this, faker).Build();

            foreach (Organisation supplier in @this.CurrentSuppliers)
            {
                new SupplierOfferingBuilder(@this.Session())
                    .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                    .WithSupplier(supplier)
                    .WithPart(part)
                    .WithUnitOfMeasure(part.UnitOfMeasure)
                    .Build();
            }

            return part;
        }

        public static UnifiedGood CreateUnifiedWithGoodInventoryAvailableForSale(this Organisation @this, Faker faker)
        {
            var unifiedGood = new UnifiedGoodBuilder(@this.Session()).WithSerialisedDefaults(@this).Build();
            var serialisedItem = new SerialisedItemBuilder(@this.Session()).WithForSaleDefaults(@this).Build();

            unifiedGood.AddSerialisedItem(serialisedItem);

            new InventoryItemTransactionBuilder(@this.Session())
                .WithSerialisedItem(serialisedItem)
                .WithFacility(@this.FacilitiesWhereOwner.First)
                .WithQuantity(1)
                .WithReason(new InventoryTransactionReasons(@this.Session()).IncomingShipment)
                .WithSerialisedInventoryItemState(new SerialisedInventoryItemStates(@this.Session()).Good)
                .Build();

            return unifiedGood;
        }

        /*
         * Create a PurchaseOrder without any PurchaseOrder Items
         */
        public static PurchaseOrder CreatePurchaseOrderWithoutItems(this Organisation @this)
        {
            var purchaseOrder = new PurchaseOrderBuilder(@this.Session()).WithDefaults(@this).Build();

            return purchaseOrder;
        }

        /*
         * Create PurchaseOrder with both Serialized & NonSerialized PurchaseOrder Items
         */
        public static PurchaseOrder CreatePurchaseOrderWithBothItems(this Organisation @this)
        {
            var purchaseOrder = new PurchaseOrderBuilder(@this.Session()).WithDefaults(@this).Build();

            var nonSerializedPart = new PurchaseOrderItemBuilder(@this.Session()).WithNonSerializedPartDefaults(@this, purchaseOrder).Build();
            var serializedPart = new PurchaseOrderItemBuilder(@this.Session()).WithSerializedPartDefaults(@this).Build();

            purchaseOrder.AddPurchaseOrderItem(nonSerializedPart);
            purchaseOrder.AddPurchaseOrderItem(serializedPart);

            return purchaseOrder;
        }

        /*
         * Create PurchaseOrder with Serialized PurchaseOrderItem
         */
        public static PurchaseOrder CreatePurchaseOrderWithSerializedItem(this Organisation @this)
        {
            var purchaseOrder = new PurchaseOrderBuilder(@this.Session()).WithDefaults(@this).Build();

            var serializedPart = new PurchaseOrderItemBuilder(@this.Session()).WithSerializedPartDefaults(@this).Build();

            purchaseOrder.AddPurchaseOrderItem(serializedPart);

            return purchaseOrder;
        }

        /*
         * Create PurchaseOrder with NonSerialized PurchaseOrderItem
         */
        public static PurchaseOrder CreatePurchaseOrderWithNonSerializedItem(this Organisation @this)
        {
            var purchaseOrder = new PurchaseOrderBuilder(@this.Session()).WithDefaults(@this).Build();

            var nonSerializedPart = new PurchaseOrderItemBuilder(@this.Session()).WithNonSerializedPartDefaults(@this, purchaseOrder).Build();

            purchaseOrder.AddPurchaseOrderItem(nonSerializedPart);

            return purchaseOrder;
        }
    }
}
