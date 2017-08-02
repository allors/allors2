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
            var contactMechanism = new TelecommunicationsNumberBuilder(this.DatabaseSession).WithAreaCode("0495").WithContactNumber("493499").WithDescription("cellphone").Build();
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var builder = new PartyContactMechanismBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithContactMechanism(contactMechanism);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPartyContactMechanism_WhenPartyIsDeleted_ThenPartyContactMechanismIsDeleted()
        {
            var contactMechanism = new TelecommunicationsNumberBuilder(this.DatabaseSession).WithAreaCode("0495").WithContactNumber("493499").WithDescription("cellphone").Build();
            var partyContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession).WithContactMechanism(contactMechanism).Build();
            var party = new PersonBuilder(this.DatabaseSession).WithLastName("party").WithPartyContactMechanism(partyContactMechanism).WithPersonRole(new PersonRoles(this.DatabaseSession).Contact).Build();

            this.DatabaseSession.Derive();
            var countBefore = this.DatabaseSession.Extent<PartyContactMechanism>().Count;

            party.Delete();
            this.DatabaseSession.Derive();

            Assert.Equal(countBefore - 1, this.DatabaseSession.Extent<PartyContactMechanism>().Count);

        }
    }
}
