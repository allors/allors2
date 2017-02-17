//------------------------------------------------------------------------------------------------- 
// <copyright file="BankTests.cs" company="Allors bvba">
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
    public class BankTests : DomainTest
    {
        [Test]
        public void GivenBank_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];

            var builder = new BankBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCountry(netherlands);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBic("RABONL2U");
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("Rabo");
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
        
        [Test]
        public void GivenBankWithBic_WhenDeriving_ThenFirstfourCharactersMustBeAlphabetic()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            bank.Bic = "RAB1NL2U";

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenBankWithBic_WhenDeriving_ThenCharacters5And6MustBeValidCountryCode()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            bank.Bic = "RABONN2U";

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenBankWithBic_WhenDeriving_ThenStringLengthMustBeEightOrEleven()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2").Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            bank.Bic = "RABONL2UAAAA";

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            bank.Bic = "RABONL2UAAA";

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
