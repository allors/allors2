// <copyright file="BankTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class BankTests : DomainTest
    {
        [Fact]
        public void GivenBank_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];

            var builder = new BankBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCountry(netherlands);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBic("RABONL2U");
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Rabo");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenBankWithBic_WhenDeriving_ThenFirstfourCharactersMustBeAlphabetic()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            bank.Bic = "RAB1NL2U";

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenBankWithBic_WhenDeriving_ThenCharacters5And6MustBeValidCountryCode()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            bank.Bic = "RABONN2U";

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenBankWithBic_WhenDeriving_ThenStringLengthMustBeEightOrEleven()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2").Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            bank.Bic = "RABONL2UAAAA";

            Assert.True(this.Session.Derive(false).HasErrors);

            bank.Bic = "RABONL2UAAA";

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
