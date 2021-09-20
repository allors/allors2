// <copyright file="EngagementTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class EngagementTests : DomainTest
    {
        [Fact]
        public void GivenEngagement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var mechelen = new CityBuilder(this.Session).WithName("Mechelen").Build();
            var billToContactMechanism = new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(mechelen).WithAddress1("Haverwerf 15").Build();
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
