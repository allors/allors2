
// <copyright file="LocaleTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Xunit;

    public class LocaleTests : DomainTest
    {
        [Fact]
        public void GivenLocale_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new LocaleBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithLanguage(new Languages(this.Session).FindBy(M.Language.IsoCode, "en"));
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"));
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenLocale_WhenDeriving_ThenNameIsSet()
        {
            var locale = new LocaleBuilder(this.Session)
                .WithLanguage(new Languages(this.Session).FindBy(M.Language.IsoCode, "en"))
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            this.Session.Derive(true);

            Assert.Equal("en-BE", locale.Name);
        }

        [Fact]
        public void GivenLocaleWhenValidatingThenRequiredRelationsMustExist()
        {
            var dutch = new Languages(this.Session).LanguageByCode["nl"];
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];

            var builder = new LocaleBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithLanguage(dutch).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            builder.WithCountry(netherlands).Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenLocaleWhenValidatingThenNameIsSet()
        {
            var locale = new Locales(this.Session).FindBy(M.Locale.Name, Locales.DutchNetherlandsName);

            Assert.Equal("nl-NL", locale.Name);
        }
    }
}
