// <copyright file="Country.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Country
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            if (this.ExistIsoCode)
            {
                this.EuMemberState = Countries.EuMemberStates.Contains(this.IsoCode);

                if (Countries.IbanDataByCountry.TryGetValue(this.IsoCode, out var ibanData))
                {
                    this.IbanLength = ibanData.Length;
                    this.IbanRegex = ibanData.RegexStructure;
                }
                else
                {
                    this.RemoveIbanLength();
                    this.RemoveIbanRegex();
                }
            }
            else
            {
                this.RemoveEuMemberState();
                this.RemoveIbanLength();
                this.RemoveIbanRegex();
            }

            foreach(VatRegime vatRegime in this.VatRegimesWhereCountry)
            {
                var previousCountry = vatRegime.CurrentVersion?.Country;
                if (previousCountry != null && previousCountry != vatRegime.Country)
                {
                    previousCountry.RemoveDerivedVatRegime(vatRegime);
                }

                this.AddDerivedVatRegime(vatRegime);
            }
        }
    }
}
