//------------------------------------------------------------------------------------------------- 
// <copyright file="EngagementTests.cs" company="Allors bvba">
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
    using Xunit;

    
    public class EngagementTests : DomainTest
    {
        [Fact]
        public void GivenEngagement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var billToContactMechanism = new PostalAddressBuilder(this.Session).WithGeographicBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session)
                .WithContactMechanism(billToContactMechanism)
                .WithContactPurpose(new ContactMechanismPurposes(this.Session).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new OrganisationBuilder(this.Session).WithName("customer").WithPartyContactMechanism(partyContactMechanism).Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new EngagementBuilder(this.Session);
            var customEngagementItem = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("Engagement");
            customEngagementItem = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBillToParty(customer);
            customEngagementItem = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
