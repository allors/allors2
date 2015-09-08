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
    using NUnit.Framework;

    [TestFixture]
    public class PostalAddressGeographicBoundaryTests : DomainTest
    {
        [Test]
        public void GivenGeographicBoundary_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var country = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];

            new PostalAddressBuilder(this.DatabaseSession).Build();
 
            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithAddress1("address1").Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithGeographicBoundary(country).Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithAddress1("address1").WithGeographicBoundary(country).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);        
        }

        [Test]
        public void GivenGeographicBoundary_WhenDeriving_ThenFormattedFullAddressIsSet()
        {
            var city = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var postalCode = new PostalCodeBuilder(this.DatabaseSession).WithCode("2800").Build();
            var country = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE");

            var address = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").Build();

            this.DatabaseSession.Derive(false);

            Assert.AreEqual("Haverwerf 15", address.FormattedFullAddress);

            address.Address2 = "address2";
            
            this.DatabaseSession.Derive(false);
            
            Assert.AreEqual("Haverwerf 15<br />address2", address.FormattedFullAddress);

            address.RemoveAddress2();

            address.AddGeographicBoundary(postalCode);
            address.AddGeographicBoundary(city);

            this.DatabaseSession.Derive(true);
            
            Assert.AreEqual("Haverwerf 15<br />2800 Mechelen", address.FormattedFullAddress);

            address.AddGeographicBoundary(country);

            this.DatabaseSession.Derive(true);
            
            Assert.AreEqual("Haverwerf 15<br />2800 Mechelen<br />Belgium", address.FormattedFullAddress);
        }

        [Test]
        public void GivenGeographicBoundary_WhenDeriving_ThenCountryCityAndPostalCodeAreDerived()
        {
            var city = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var postalCode = new PostalCodeBuilder(this.DatabaseSession).WithCode("2800").Build();
            var country = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE");

            var address = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithGeographicBoundary(postalCode)
                .WithGeographicBoundary(city)
                .WithGeographicBoundary(country)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(postalCode, address.PostalCode);
            Assert.AreEqual(city, address.City);
            Assert.AreEqual(country, address.Country);
        }

        [Test]
        public void GivenPostalBoundary_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var country = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var postalBoundary = new PostalBoundaryBuilder(this.DatabaseSession).WithLocality("Mechelen").WithCountry(country).Build();
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            new PostalAddressBuilder(this.DatabaseSession).Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithAddress1("address1").Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithPostalBoundary(postalBoundary).Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            new PostalAddressBuilder(this.DatabaseSession).WithAddress1("address1").WithPostalBoundary(postalBoundary).Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenPostalBoundary_WhenDeriving_ThenFormattedFullAddressIsSet()
        {
            var country = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            var postalBoundary = new PostalBoundaryBuilder(this.DatabaseSession).WithLocality("Mechelen").WithCountry(country).Build();
            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();

            var address = new PostalAddressBuilder(this.DatabaseSession).WithAddress1("Haverwerf 15").Build();

            this.DatabaseSession.Derive(false);

            Assert.AreEqual("Haverwerf 15", address.FormattedFullAddress);

            address.Address2 = "address2";

            this.DatabaseSession.Derive(false);

            Assert.AreEqual("Haverwerf 15<br />address2", address.FormattedFullAddress);

            address.RemoveAddress2();

            address.PostalBoundary = postalBoundary;

            this.DatabaseSession.Derive(true);

            Assert.AreEqual("Haverwerf 15<br />Mechelen<br />Belgium", address.FormattedFullAddress);

            address.PostalBoundary.PostalCode = "2800";

            this.DatabaseSession.Derive(true);

            Assert.AreEqual("Haverwerf 15<br />2800 Mechelen<br />Belgium", address.FormattedFullAddress);
        }

        [Test]
        public void GivenPostalBoundary_WhenDeriving_ThenCountryIsDerived()
        {
            var country = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE");
            var postalBoundary = new PostalBoundaryBuilder(this.DatabaseSession).WithLocality("locality").WithCountry(country).Build();

            var address = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Haverwerf 15")
                .WithPostalBoundary(postalBoundary)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsNull(address.PostalCode);
            Assert.IsNull(address.City);
            Assert.AreEqual(country, address.Country);
        }
    }
}
