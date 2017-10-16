// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessFlows.cs" company="Allors bvba">
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
    using System;

    public partial class ProcessFlows
    {
        private static readonly Guid PayFirstId = new Guid("AB01CCC2-6480-4FC0-B20E-265AFD41FAE2");
        private static readonly Guid ShipFirstId = new Guid("E242D221-7DD6-4BD8-9A4A-E0582EEBECB0");

        private UniquelyIdentifiableSticky<ProcessFlow> cache;

        public ProcessFlow PayFirst => this.Cache[PayFirstId];

        public ProcessFlow ShipFirst => this.Cache[ShipFirstId];

        private UniquelyIdentifiableSticky<ProcessFlow> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<ProcessFlow>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new ProcessFlowBuilder(this.Session)
                .WithName("PayFirst")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("PayFirst").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("PayFirst").WithLocale(dutchLocale).Build())
                .WithUniqueId(PayFirstId)
                .Build();

            new ProcessFlowBuilder(this.Session)
                .WithName("ShipFirst")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("ShipFirst").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("ShipFirst").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShipFirstId)
                .Build();
        }
    }
}
