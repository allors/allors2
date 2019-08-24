// <copyright file="Locales.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Locales
    {
        public const string EnglishGreatBritainName = "en-GB";
        public const string EnglishUnitedStatesName = "en-US";
        public const string DutchNetherlandsName = "nl-NL";
        public const string DutchBelgiumName = "nl-BE";

        private Sticky<string, Locale> localeByIdentifier;

        public Sticky<string, Locale> LocaleByIdentifier => this.localeByIdentifier ?? (this.localeByIdentifier = new Sticky<string, Locale>(this.Session, this.Meta.Name));

        public Locale EnglishGreatBritain => this.FindBy(this.Meta.Name, EnglishGreatBritainName);

        public Locale EnglishUnitedStates => this.FindBy(this.Meta.Name, EnglishUnitedStatesName);

        public Locale DutchNetherlands => this.FindBy(this.Meta.Name, DutchNetherlandsName);

        public Locale DutchBelgium => this.FindBy(this.Meta.Name, DutchBelgiumName);

        protected override void CorePrepare(Setup setup)
        {
            setup.AddDependency(this.ObjectType, M.Country);
            setup.AddDependency(this.ObjectType, M.Language);
        }

        protected override void CoreSetup(Setup setup)
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

        protected override void CoreSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
