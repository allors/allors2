// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountingPeriod.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    public partial class TimeFrequency
    {
        public new string ToString() => this.Name;

        public decimal? GetConvertToFactor(TimeFrequency timeFrquency)
            => this.UnitOfMeasureConversions?.FirstOrDefault(c => c.ToUnitOfMeasure.Equals(timeFrquency)).ConversionFactor;

        public decimal? ConvertToFrequency(decimal value, TimeFrequency timeFrequency)
        {
            var conversion = this.GetConvertToFactor(timeFrequency);

            if (conversion != null)
            {
                return value * (decimal)conversion;
            }

            return null;
        }

        public string GetName()
        {
            return this.Name;
        }
    }
}