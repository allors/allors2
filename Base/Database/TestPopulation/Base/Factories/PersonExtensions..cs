// <copyright file="PersonExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.TestPopulation
{
    using Allors.Domain;
    using Bogus;
    using Person = Allors.Domain.Person;

    public static class PersonExtensions
    {
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
    }
}
