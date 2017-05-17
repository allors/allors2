// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Locales.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Locales
    {
        public const string EnglishGreatBritainName = "en-GB";
        public const string EnglishUnitedStatesName = "en-US";
        public const string DutchNetherlandsName = "nl-NL";
        public const string DutchBelgiumName = "nl-BE";

        private Cache<string, Locale> localeByIdentifier;

        public Cache<string, Locale> LocaleByIdentifier
        {
            get
            {
                return this.localeByIdentifier
                       ?? (this.localeByIdentifier = new Cache<string, Locale>(this.Session, Meta.Name));
            }
        }

        public Locale EnglishGreatBritain => this.FindBy(this.Meta.Name, EnglishGreatBritainName);

        public Locale EnglishUnitedStates => this.FindBy(this.Meta.Name, EnglishUnitedStatesName);

        public Locale DutchNetherlands => this.FindBy(this.Meta.Name, DutchNetherlandsName);

        public Locale DutchBelgium => this.FindBy(this.Meta.Name, DutchBelgiumName);

        protected override void BasePrepare(Setup config)
        {
            config.AddDependency(this.ObjectType, M.Country);
            config.AddDependency(this.ObjectType, M.Language);
        }

        protected override void BaseSetup(Setup config)
        {
            var countries = new Countries(this.Session);
            var greatBritain = countries.CountryByIsoCode["GB"];
            var usa = countries.CountryByIsoCode["US"];
            var netherlands = countries.CountryByIsoCode["NL"];
            var belgium = countries.CountryByIsoCode["BE"];

            var languages = new Languages(this.Session);
            var english = languages.LanguageByCode["en"];
            var dutch = languages.LanguageByCode["nl"];

            new LocaleBuilder(this.Session).WithName(EnglishGreatBritainName).WithCountry(greatBritain).WithLanguage(english).Build();
            new LocaleBuilder(this.Session).WithName(EnglishUnitedStatesName).WithCountry(usa).WithLanguage(english).Build();
            new LocaleBuilder(this.Session).WithName(DutchNetherlandsName).WithCountry(netherlands).WithLanguage(dutch).Build();
            new LocaleBuilder(this.Session).WithName(DutchBelgiumName).WithCountry(belgium).WithLanguage(dutch).Build();
        }

        protected override void BaseSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}