// <copyright file="PartyRelationshipTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class PartyRelationshipTests : DomainTest
    {
        [Fact]
        public void GivenOrganisationContactRelationship_WhenRelationshipPeriodIsNotValid_ThenContactIsNotInCustomerContactUserGroup()
        {
            var party = new OrganisationBuilder(this.Session).WithName("customer").Build();

            this.Session.Derive();

            var customerRelationship = new CustomerRelationshipBuilder(this.Session)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithCustomer(party)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .Build();

            this.Session.Derive();

            Assert.Contains(customerRelationship, party.CurrentPartyRelationships);
            Assert.Empty(party.InactivePartyRelationships);

            var supplierRelationship = new SupplierRelationshipBuilder(this.Session)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithSupplier(party)
                .WithFromDate(this.Session.Now().AddYears(-1))
                .Build();

            this.Session.Derive();

            Assert.Contains(customerRelationship, party.CurrentPartyRelationships);
            Assert.Contains(supplierRelationship, party.CurrentPartyRelationships);
            Assert.Empty(party.InactivePartyRelationships);

            customerRelationship.ThroughDate = this.Session.Now().AddDays(-1);

            this.Session.Derive();

            Assert.Contains(supplierRelationship, party.CurrentPartyRelationships);
            Assert.Contains(customerRelationship, party.InactivePartyRelationships);
        }
    }
}
