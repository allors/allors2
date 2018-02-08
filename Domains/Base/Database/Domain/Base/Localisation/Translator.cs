// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Translator.cs" company="Allors bvba">
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
    using System;
    using System.Linq;

    public partial class Translator<T>
        where T : IObject
    {
        private readonly Locale defaultLocale;
        private readonly Locale locale;

        private readonly Func<T, string> value;
        private readonly Func<T, LocalisedText[]> localisedValues;

        public Translator(Locale defaultLocale, Locale locale, Func<T, string> value, Func<T, LocalisedText[]> localisedValues)
        {
            this.defaultLocale = defaultLocale;
            this.locale = locale;

            this.value = value;
            this.localisedValues = localisedValues;
        }

        public string Translate(T source)
        {
            if (this.defaultLocale == null || this.defaultLocale.Equals(this.locale))
            {
                return this.value(source);
            }

            var localisedValue  = this.localisedValues(source).FirstOrDefault(v => v.Locale.Equals(this.locale));
            return localisedValue != null ? localisedValue.Text : this.value(source);
        }
    }
}