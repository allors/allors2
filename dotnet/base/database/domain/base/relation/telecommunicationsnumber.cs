// <copyright file="TelecommunicationsNumber.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class TelecommunicationsNumber
    {
        public bool IsPostalAddress => false;

        public override string ToString()
        {
            var numberString = string.Empty;
            if (this.ExistCountryCode || this.ExistAreaCode)
            {
                if (this.ExistCountryCode && this.ExistAreaCode)
                {
                    numberString = this.CountryCode + ' ' + this.AreaCode;
                }
                else if (this.ExistCountryCode)
                {
                    numberString = this.CountryCode;
                }
                else
                {
                    numberString = this.AreaCode;
                }
            }

            if (string.IsNullOrEmpty(numberString) && this.ExistContactNumber)
            {
                return this.ContactNumber;
            }
            else
            {
                numberString += ' ' + this.ContactNumber;
                return numberString;
            }
        }
    }
}
