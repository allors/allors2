// <copyright file="Locale.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Globalization;

    public partial class Locale
    {
        public bool ExistCultureInfo => this.ExistName;

        public CultureInfo CultureInfo => this.ExistName ? new CultureInfo(this.Name) : null;

        public void CoreOnPostBuild(ObjectOnPostBuild method)
        {
            if (!this.ExistName && this.ExistLanguage && this.ExistCountry)
            {
                this.Name = this.Language.IsoCode + "-" + this.Country.IsoCode;
            }
        }
    }
}
