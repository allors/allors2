//------------------------------------------------------------------------------------------------- 
// <copyright file="OrganisationContactRelationshipTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

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
