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
    using Meta;
    using Xunit;

    
    public class PostalAddressGeographicBoundaryTests : DomainTest
    {
        [Fact]
        public void GivenGeographicBoundary_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var country = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];

            new PostalAddressBuilder(this.DatabaseSession).Build();
 
            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithAddress1("address1").Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(country).Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithAddress1("address1").WithGeographicBoundary(country).Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);        
        }

        [Fact]
        public void GivenGeographicBoundary_WhenDeriving_ThenCountryCityAndPostalCodeAreDerived()
        {
            var city = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var postalCode = new PostalCodeBuilder(this.DatabaseSession).WithCode("2800").Build();
            var country = new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE");

            var address = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithGeographicBoundary(postalCode)
                .WithGeographicBoundary(city)
                .WithGeographicBoundary(country)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(postalCode, address.PostalCode);
            Assert.Equal(city, address.City);
            Assert.Equal(country, address.Country);
        }

        [Fact]
        public void GivenPostalBoundary_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var country = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var postalBoundary = new PostalBoundaryBuilder(this.DatabaseSession).WithLocality("Mechelen").WithCountry(country).Build();
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            new PostalAddressBuilder(this.DatabaseSession).Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithAddress1("address1").Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithPostalBoundary(postalBoundary).Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithAddress1("address1").WithPostalBoundary(postalBoundary).Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenPostalBoundary_WhenDeriving_ThenCountryIsDerived()
        {
            var country = new Countries(this.DatabaseSession).FindBy(M.Country.IsoCode, "BE");
            var postalBoundary = new PostalBoundaryBuilder(this.DatabaseSession).WithLocality("locality").WithCountry(country).Build();

            var address = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(postalBoundary)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Null(address.PostalCode);
            Assert.Null(address.City);
            Assert.Equal(country, address.Country);
        }
    }
}
