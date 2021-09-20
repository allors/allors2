// <copyright file="SerialisedItemCharacteristic.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class SerialisedItemCharacteristic
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.SerialisedItemCharacteristicType.ExistUnitOfMeasure)
            {
                this.Sync();
            }
        }

        private void Sync()
        {
            var existingLocalisedtexts = this.LocalisedValues.ToDictionary(d => d.Locale);

            foreach (Locale locale in this.Strategy.Session.GetSingleton().AdditionalLocales)
            {
                if (existingLocalisedtexts.TryGetValue(locale, out var localisedText))
                {
                    localisedText.Text = this.Value;
                    existingLocalisedtexts.Remove(locale);
                }
                else
                {
                    localisedText = new LocalisedTextBuilder(this.Strategy.Session)
                        .WithLocale(locale)
                        .Build();

                    this.AddLocalisedValue(localisedText);
                }
            }

            foreach (var localisedText in existingLocalisedtexts.Values)
            {
                this.RemoveLocalisedValue(localisedText);
            }
        }
    }
}
