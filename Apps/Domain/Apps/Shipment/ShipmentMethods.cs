// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentMethods.cs" company="Allors bvba">
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
    using System;

    public partial class ShipmentMethods
    {
        public static readonly Guid GroundId = new Guid("8E534609-12C4-4701-A868-D7B3B3D4D6D9");
        public static readonly Guid RailId = new Guid("5F13CAFD-022F-46aa-BA89-7992DC52F69B");
        public static readonly Guid FirstClassAirId = new Guid("D2A20D3A-7790-485c-910A-A09854E37FED");
        public static readonly Guid BoatId = new Guid("CD6A439A-445A-4f8c-8DE2-654A0C504F48");
        public static readonly Guid ExWorksId = new Guid("5E628193-2E06-4C9B-BE86-D9BEB91B7D64");

        private UniquelyIdentifiableCache<ShipmentMethod> cache;

        public ShipmentMethod Ground
        {
            get { return this.Cache.Get(GroundId); }
        }

        public ShipmentMethod Rail
        {
            get { return this.Cache.Get(RailId); }
        }

        public ShipmentMethod FirstClassAir
        {
            get { return this.Cache.Get(FirstClassAirId); }
        }

        public ShipmentMethod Boat
        {
            get { return this.Cache.Get(BoatId); }
        }

        public ShipmentMethod ExWorks
        {
            get { return this.Cache.Get(ExWorksId); }
        }

        private UniquelyIdentifiableCache<ShipmentMethod> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<ShipmentMethod>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new ShipmentMethodBuilder(this.Session)
                .WithName("Ground")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ground").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Weg").WithLocale(dutchLocale).Build())
                .WithUniqueId(GroundId)
                .Build();

            new ShipmentMethodBuilder(this.Session)
                .WithName("Rail")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Rail").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Spoor").WithLocale(dutchLocale).Build())
                .WithUniqueId(RailId)
                .Build();
            
            new ShipmentMethodBuilder(this.Session)
                .WithName("Air")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Air").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vliegtuig").WithLocale(dutchLocale).Build())
                .WithUniqueId(FirstClassAirId)
                .Build();
            
            new ShipmentMethodBuilder(this.Session)
                .WithName("Boat")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boat").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boot").WithLocale(dutchLocale).Build())
                .WithUniqueId(BoatId)
                .Build();

            new ShipmentMethodBuilder(this.Session)
                .WithName("Ex works")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ex works").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Af fabriek").WithLocale(dutchLocale).Build())
                .WithUniqueId(ExWorksId)
                .Build();
        }

        protected override void AppsSecure(Domain.Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
