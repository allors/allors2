// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Locales.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Globalization;

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

        public Locale EnglishGreatBritain
        {
            get { return this.FindBy(Meta.Name, EnglishGreatBritainName); }
        }

        public Locale EnglishUnitedStates
        {
            get { return this.FindBy(Meta.Name, EnglishUnitedStatesName); }
        }

        public Locale DutchNetherlands
        {
            get { return this.FindBy(Meta.Name, DutchNetherlandsName); }
        }

        public Locale DutchBelgium
        {
            get { return this.FindBy(Meta.Name, DutchBelgiumName); }
        }

        public void Sync()
        {
            var englishNameByCountry = new Dictionary<Country, string>();
            var englishNameByCurrency = new Dictionary<Currency, string>();
            var englishNameByLanguage = new Dictionary<Language, string>();

            var countryByIsoCode = new Dictionary<string, Country>();
            foreach (Country country in new Countries(this.Session).Extent())
            {
                countryByIsoCode.Add(country.IsoCode, country);
            }

            var languageByIsoCode = new Dictionary<string, Language>();
            foreach (Language language in new Languages(this.Session).Extent())
            {
                languageByIsoCode.Add(language.IsoCode, language);
            }

            var currencyByIsoCode = new Dictionary<string, Currency>();
            foreach (Currency currency in new Currencies(this.Session).Extent())
            {
                currencyByIsoCode.Add(currency.IsoCode, currency);
            }

            var localeByName = new Dictionary<string, Locale>();
            foreach (Locale locale in new Locales(this.Session).Extent())
            {
                localeByName.Add(locale.Name, locale);
            }

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                if (cultureInfo.LCID != 127)
                {
                    var languageIsoCode = cultureInfo.TwoLetterISOLanguageName.ToLower();
                    Language language;
                    if (!languageByIsoCode.TryGetValue(languageIsoCode, out language))
                    {
                        language = new LanguageBuilder(this.Session)
                            .WithIsoCode(languageIsoCode)
                            .Build();

                        languageByIsoCode.Add(languageIsoCode, language);
                        englishNameByLanguage.Add(language, cultureInfo.Parent.EnglishName);
                    }

                    Country country = null;
                    var regionInfo = new RegionInfo(cultureInfo.LCID);

                    // Should be upper, but just in case ...
                    var countryIsoCode = regionInfo.TwoLetterISORegionName.ToUpper();

                    // sometimes a 2 letter code is a 3 digit code ...
                    if (countryIsoCode.Length == 2)
                    {
                        if (!countryByIsoCode.TryGetValue(countryIsoCode, out country))
                        {
                            country = new CountryBuilder(this.Session).WithIsoCode(countryIsoCode).Build();

                            englishNameByCountry.Add(country, regionInfo.EnglishName);
                            countryByIsoCode.Add(countryIsoCode, country);
                        }

                        var currencyIsoCode = regionInfo.ISOCurrencySymbol.ToUpper();
                        Currency currency;
                        if (!currencyByIsoCode.TryGetValue(currencyIsoCode, out currency))
                        {
                            currency = new CurrencyBuilder(this.Session).WithIsoCode(currencyIsoCode).WithSymbol(regionInfo.CurrencySymbol).Build();

                            currencyByIsoCode.Add(currencyIsoCode, currency);
                            englishNameByCurrency.Add(currency, regionInfo.CurrencyEnglishName);
                        }

                        if (country != null)
                        {
                            country.Currency = currency;
                        }
                    }

                    var localeIdentifier = cultureInfo.Name;

                    Locale locale;
                    if (!localeByName.TryGetValue(localeIdentifier, out locale))
                    {
                        if (country != null && language != null)
                        {
                            locale = new LocaleBuilder(this.Session)
                                .WithName(cultureInfo.Name)
                                .WithCountry(country)
                                .WithLanguage(language)
                                .Build();

                            localeByName.Add(localeIdentifier, locale);
                        }
                    }
                    else
                    {
                        locale.Name = cultureInfo.Name;
                        locale.Language = language;
                        locale.Country = country;
                    }
                }
            }

            var englishLocale = localeByName[EnglishGreatBritainName];

            foreach (var country in countryByIsoCode.Values)
            {
                if (englishNameByCountry.ContainsKey(country))
                {
                    var enlgishName = englishNameByCountry[country];

                    var englishCountryName = new LocalisedTextBuilder(this.Session)
                        .WithText(enlgishName)
                        .WithLocale(englishLocale)
                        .Build();

                    country.Name = enlgishName;
                    country.AddLocalisedName(englishCountryName);
                }
            }

            foreach (var currency in currencyByIsoCode.Values)
            {
                if (englishNameByCurrency.ContainsKey(currency))
                {
                    var englishName = englishNameByCurrency[currency];

                    var englishCurrencyName = new LocalisedTextBuilder(this.Session)
                        .WithText(englishName)
                        .WithLocale(englishLocale)
                        .Build();

                    currency.Name = englishName;
                    currency.AddLocalisedName(englishCurrencyName);
                }
            }

            foreach (var language in languageByIsoCode.Values)
            {
                if (englishNameByLanguage.ContainsKey(language))
                {
                    var englishName = englishNameByLanguage[language];

                    var englishLanguageName = new LocalisedTextBuilder(this.Session)
                        .WithText(englishName)
                        .WithLocale(englishLocale)
                        .Build();

                    language.Name = englishName;
                    language.AddLocalisedName(englishLanguageName);
                }
            }
        }

        protected override void BaseSetup(Setup config)
        {
            this.Sync();
        }

        protected override void BaseSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}