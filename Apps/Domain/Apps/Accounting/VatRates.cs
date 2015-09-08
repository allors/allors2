// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VatRates.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    public partial class VatRates
    {
        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new VatRateBuilder(this.Session).WithRate(0).Build();
            new VatRateBuilder(this.Session).WithRate(6).Build();
            new VatRateBuilder(this.Session).WithRate(12).Build();
            new VatRateBuilder(this.Session).WithRate(21).Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}