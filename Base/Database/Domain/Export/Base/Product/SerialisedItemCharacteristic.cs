// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerialisedInventoryItemCharacteristic.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System.Linq;

    using Meta;

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
                if (existingLocalisedtexts.TryGetValue(locale, out LocalisedText localisedText))
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

            foreach (LocalisedText localisedText in existingLocalisedtexts.Values)
            {
                this.RemoveLocalisedValue(localisedText);
            }
        }
    }
}