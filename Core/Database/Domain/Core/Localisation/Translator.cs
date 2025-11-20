// <copyright file="Translator.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public partial class Translator<T>
        where T : IObject
    {
        private readonly Locale locale;
        private readonly Locale defaultLocale;

        private readonly Func<T, string> value;
        private readonly Func<T, LocalisedText[]> localisedValues;

        public Translator(Locale locale, Func<T, string> value, Func<T, LocalisedText[]> localisedValues)
        {
            this.locale = locale;
            this.defaultLocale = locale?.Strategy.Session.GetSingleton().DefaultLocale;

            this.value = value;
            this.localisedValues = localisedValues;
        }

        public string Translate(T source)
        {
            if (source == null)
            {
                return null;
            }

            if (this.defaultLocale == null || this.locale == null || this.defaultLocale.Equals(this.locale))
            {
                return this.value(source);
            }

            var localisedValue = this.localisedValues(source).FirstOrDefault(v => v.Locale.Equals(this.locale));
            return localisedValue != null ? localisedValue.Text : this.value(source);
        }
    }
}
