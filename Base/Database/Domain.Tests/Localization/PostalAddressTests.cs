//------------------------------------------------------------------------------------------------- 
// <copyright file="PostalAddressGeographicBoundaryTests.cs" company="Allors bvba">
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

    public class PostalAddressGeographicBoundaryTests : DomainTest
    {
        [Fact]
        public void GivenGeographicBoundary_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var country = new Countries(this.Session).CountryByIsoCode["BE"];

            new PostalAddressBuilder(this.Session).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithAddress1("address1").Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithPostalAddressBoundary(country).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithAddress1("address1").WithPostalAddressBoundary(country).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
