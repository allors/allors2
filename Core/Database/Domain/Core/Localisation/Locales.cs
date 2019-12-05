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

        public Sticky<string, Locale> LocaleByIdentifier => this.localeByIdentifier ??= new Sticky<string, Locale>(this.Session, this.Meta.Name);

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
            var languages = new Languages(this.Session);

            var merge = this.LocaleByIdentifier.Merger().Action();

            merge(EnglishGreatBritainName, v =>
            {
                v.Country = countries.CountryByIsoCode["GB"];
                v.Language = languages.LanguageByCode["en"];
            });

            merge(EnglishUnitedStatesName, v =>
            {
                v.Country = countries.CountryByIsoCode["US"];
                v.Language = languages.LanguageByCode["en"];
            });

            merge(DutchNetherlandsName, v =>
            {
                v.Country = countries.CountryByIsoCode["NL"];
                v.Language = languages.LanguageByCode["nl"];
            });

            merge(DutchBelgiumName, v =>
            {
                v.Country = countries.CountryByIsoCode["BE"];
                v.Language = languages.LanguageByCode["nl"];
            });
        }
    }
}
