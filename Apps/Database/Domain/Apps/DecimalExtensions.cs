// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecimalExtensions.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors
{
    using System.Globalization;

    using Allors.Domain;

    public static partial class DecimalExtensions
    {
        public static string ToString(this decimal dec, Locale locale)
        {
            var cultureInfo = locale != null ? locale.CultureInfo : null;
            return dec.ToString(cultureInfo);
        }

        public static string AsCurrencyString(this decimal dec, NumberFormatInfo numberFormatInfo)
        {
            return dec.ToString("C", numberFormatInfo);
        }

        public static string AsCurrencyString(this decimal? dec, NumberFormatInfo numberFormatInfo)
        {
            return dec.HasValue ? dec.Value.AsCurrencyString(numberFormatInfo) : null;
        }
    }
}