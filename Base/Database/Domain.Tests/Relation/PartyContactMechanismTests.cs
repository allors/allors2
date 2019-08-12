//------------------------------------------------------------------------------------------------- 
// <copyright file="PartyContactMechanismTests.cs" company="Allors bvba">
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


    public class PartyContactMechanismTests : DomainTest
    {
        [Fact]
        public void GivenPartyContactMechanism_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var contactMechanism = new TelecommunicationsNumberBuilder(this.Session).WithAreaCode("0495").WithContactNumber("493499").WithDescription("cellphone").Build();
            this.Session.Derive();
            this.Session.Commit();

            var builder = new PartyContactMechanismBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithContactMechanism(contactMechanism);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPartyContactMechanism_WhenPartyIsDeleted_ThenPartyContactMechanismIsDeleted()
        {
            var contactMechanism = new TelecommunicationsNumberBuilder(this.Session).WithAreaCode("0495").WithContactNumber("493499").WithDescription("cellphone").Build();
            var partyContactMechanism = new PartyContactMechanismBuilder(this.Session).WithContactMechanism(contactMechanism).Build();
            var party = new PersonBuilder(this.Session).WithLastName("party").WithPartyContactMechanism(partyContactMechanism).Build();

            this.Session.Derive();
            var countBefore = this.Session.Extent<PartyContactMechanism>().Count;

            party.Delete();
            this.Session.Derive();

            Assert.Equal(countBefore - 1, this.Session.Extent<PartyContactMechanism>().Count);

        }
    }
}
