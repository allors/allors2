// --------------------------------------------------------------------------------------------------------------------
// <copyright file="localeTests.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Domain
{
    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using NUnit.Framework;

    [TestFixture]
    public class LocaleTests : DomainTest
    {
        [Test]
        public void GivenLocale_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new LocaleBuilder(this.Session);
            builder.Build();

            Assert.IsTrue(this.Session.Derive().HasErrors);

            this.Session.Rollback();

            builder.WithLanguage(new Languages(this.Session).FindBy(M.Language.IsoCode, "en"));
            builder.Build();

            Assert.IsTrue(this.Session.Derive().HasErrors);

            this.Session.Rollback();

            builder.WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"));
            builder.Build();

            Assert.IsFalse(this.Session.Derive().HasErrors);
        }

        [Test]
        public void GivenLocale_WhenDeriving_ThenNameIsSet()
        {
            var locale = new LocaleBuilder(this.Session)
                .WithLanguage(new Languages(this.Session).FindBy(M.Language.IsoCode, "en"))
                .WithCountry(new Countries(this.Session).FindBy(M.Country.IsoCode, "BE"))
                .Build();

            this.Session.Derive(true);

            Assert.AreEqual("en-BE", locale.Name);
        }

        [Test]
        public void GivenLocaleWhenValidatingThenRequiredRelationsMustExist()
        {
            var dutch = new Languages(this.Session).LanguageByCode["nl"];
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];

            var builder = new LocaleBuilder(this.Session);
            builder.Build();
            
            Assert.IsTrue(this.Session.Derive().HasErrors);

            builder.WithLanguage(dutch).Build();

            Assert.IsTrue(this.Session.Derive().HasErrors);

            builder.WithCountry(netherlands).Build();

            Assert.IsFalse(this.Session.Derive().HasErrors);
        }

        [Test]
        public void GivenLocaleWhenValidatingThenNameIsSet()
        {
            var locale = new Locales(this.Session).FindBy(M.Locale.Name, Locales.DutchNetherlandsName);

            Assert.AreEqual("nl-NL", locale.Name);
        }
    }
}