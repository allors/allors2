// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using Bogus;
    using Domain;
    using Domain.TestPopulation;
    using Person = Domain.Person;

    public static class OrganisationExtensions
    {
        public static Person CreateEmployee(this Organisation @this, string password, Faker faker)
        {
            var person = new PersonBuilder(@this.Session()).WithEmployeeOrCompanyContactDefaults().Build();

            new EmploymentBuilder(@this.Session())
                .WithEmployee(person)
                .WithEmployer(@this)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            new UserGroups(@this.Session()).Creators.AddMember(person);

            person.SetPassword(password);

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
                .WithContact(new PersonBuilder(@this.Session()).WithEmployeeOrCompanyContactDefaults().Build())
                .WithOrganisation(customer)
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
                .WithContact(new PersonBuilder(@this.Session()).WithEmployeeOrCompanyContactDefaults().Build())
                .WithOrganisation(supplier)
                .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                .Build();

            return supplier;
        }

        public static Part CreateNonSerialisedNonUnifiedPart(this Organisation @this, Faker faker)
        {
            var part = new NonUnifiedPartBuilder(@this.Session()).WithNonSerialisedDefaults(@this).Build();

            foreach (Organisation supplier in @this.CurrentSuppliers)
            {
                new SupplierOfferingBuilder(@this.Session())
                    .WithFromDate(faker.Date.Past(refDate: @this.Session().Now()))
                    .WithSupplier(supplier)
                    .WithPart(part);
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
                    .WithPart(part);
            }

            //new InventoryItemTransactionBuilder(@this.Session())
            //    .WithPart(part)
            //    .WithFacility(@this.FacilitiesWhereOwner.First)
            //    .WithQuantity(faker.Random.Number(1))
            //    .WithReason(new InventoryTransactionReasons(@this.Session()).Unknown)
            //    .Build();

            return part;
        }
    }
}
