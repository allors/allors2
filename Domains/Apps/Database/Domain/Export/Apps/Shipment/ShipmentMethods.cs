// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentMethods.cs" company="Allors bvba">
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

    public partial class ShipmentMethods
    {
        private static readonly Guid GroundId = new Guid("8E534609-12C4-4701-A868-D7B3B3D4D6D9");
        private static readonly Guid RailId = new Guid("5F13CAFD-022F-46aa-BA89-7992DC52F69B");
        private static readonly Guid FirstClassAirId = new Guid("D2A20D3A-7790-485c-910A-A09854E37FED");
        private static readonly Guid BoatId = new Guid("CD6A439A-445A-4f8c-8DE2-654A0C504F48");
        private static readonly Guid ExWorksId = new Guid("5E628193-2E06-4C9B-BE86-D9BEB91B7D64");

        private UniquelyIdentifiableSticky<ShipmentMethod> cache;

        public ShipmentMethod Ground => this.Cache[GroundId];

        public ShipmentMethod Rail => this.Cache[RailId];

        public ShipmentMethod FirstClassAir => this.Cache[FirstClassAirId];

        public ShipmentMethod Boat => this.Cache[BoatId];

        public ShipmentMethod ExWorks => this.Cache[ExWorksId];

        private UniquelyIdentifiableSticky<ShipmentMethod> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<ShipmentMethod>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new ShipmentMethodBuilder(this.Session)
                .WithName("Ground")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Weg").WithLocale(dutchLocale).Build())
                .WithUniqueId(GroundId)
                .WithIsActive(true)
                .Build();

            new ShipmentMethodBuilder(this.Session)
                .WithName("Rail")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Spoor").WithLocale(dutchLocale).Build())
                .WithUniqueId(RailId)
                .WithIsActive(true)
                .Build();
            
            new ShipmentMethodBuilder(this.Session)
                .WithName("Air")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vliegtuig").WithLocale(dutchLocale).Build())
                .WithUniqueId(FirstClassAirId)
                .WithIsActive(true)
                .Build();
            
            new ShipmentMethodBuilder(this.Session)
                .WithName("Boat")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boot").WithLocale(dutchLocale).Build())
                .WithUniqueId(BoatId)
                .WithIsActive(true)
                .Build();

            new ShipmentMethodBuilder(this.Session)
                .WithName("Ex works")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Af fabriek").WithLocale(dutchLocale).Build())
                .WithUniqueId(ExWorksId)
                .WithIsActive(true)
                .Build();
        }
    }
}
