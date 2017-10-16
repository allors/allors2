//------------------------------------------------------------------------------------------------- 
// <copyright file="PostalAddressPostalBoundaryTests.cs" company="Allors bvba">
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
    using Meta;
    using Xunit;

    
    public class PostalAddressPostalBoundaryTests : DomainTest
    {
        [Fact]
        public void GivenPostalBoundary_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var country = new Countries(this.Session).CountryByIsoCode["BE"];
            var postalBoundary = new PostalBoundaryBuilder(this.Session).WithLocality("Mechelen").WithCountry(country).Build();
            this.Session.Derive();
            this.Session.Commit();

            new PostalAddressBuilder(this.Session).Build();
 
            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithAddress1("address1").Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithPostalBoundary(postalBoundary).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            Assert.False(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            new PostalAddressBuilder(this.Session).WithAddress1("address1").WithPostalBoundary(postalBoundary).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPostalBoundary_WhenDeriving_ThenCountryAndCityAreDerived()
        {
            var country = new Countries(this.Session).FindBy(M.Country.IsoCode, "BE");
            var postalBoundary = new PostalBoundaryBuilder(this.Session).WithLocality("locality").WithCountry(country).Build();

            var address = new PostalAddressBuilder(this.Session)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(postalBoundary)
                .Build();

            this.Session.Derive();

            Assert.Equal(country, address.Country);
        }
    }
}
