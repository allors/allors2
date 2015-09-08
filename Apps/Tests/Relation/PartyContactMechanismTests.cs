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
    

    
    using NUnit.Framework;

    [TestFixture]
    public class PartyContactMechanismTests : DomainTest
    {
        [Test]
        public void GivenPartyContactMechanism_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var contactMechanism = new TelecommunicationsNumberBuilder(this.DatabaseSession).WithAreaCode("0495").WithContactNumber("493499").WithDescription("cellphone").Build();
            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new PartyContactMechanismBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();
            
            builder.WithContactMechanism(contactMechanism);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
