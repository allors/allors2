
// <copyright file="VatRates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class VatRates
    {
        protected override void BaseSetup(Setup setup)
        {
            new VatRateBuilder(this.Session).WithRate(0).Build();
            new VatRateBuilder(this.Session).WithRate(6).Build();
            new VatRateBuilder(this.Session).WithRate(12).Build();
            new VatRateBuilder(this.Session).WithRate(21).Build();
        }
    }
}
