// <copyright file="FacilityBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain.TestPopulation
{
    using System.Linq;

    public static partial class FacilityBuilderExtensions
    {
        public static FacilityBuilder WithDefaults(this FacilityBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();
            var customer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);
            var postalAddress = new PostalAddressBuilder(@this.Session).WithDefaults().Build();

            @this.WithName(faker.Name.FullName());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithLatitude(faker.Address.Latitude());
            @this.WithLongitude(faker.Address.Longitude());
            @this.WithOwner(internalOrganisation);
            @this.WithSquareFootage(faker.Random.Decimal(100, 10000));
            @this.WithFacilityType(faker.Random.ListItem(@this.Session.Extent<FacilityType>()));
            @this.WithFacilityContactMechanism(customer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());

            return @this;
        }
    }
}
