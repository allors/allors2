// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VarianceReasons.cs" company="Allors bvba">
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

    public partial class VarianceReasons
    {
        public static readonly Guid OrderId = new Guid("81D45DC9-03D8-480b-B210-1CC641A9CBD3");
        public static readonly Guid ShipmentId = new Guid("C768FA78-0257-4e2d-B760-EC022D89BACF");
        public static readonly Guid TheftId = new Guid("21FA1D1D-F662-4c0e-A4F6-7BD3B5298A47");
        public static readonly Guid ShrinkageId = new Guid("CF6CCC79-7EE8-4755-A9C3-EC9A83649B55");
        public static readonly Guid UnknownId = new Guid("7A438996-B2DC-4b6d-8DDD-47690B06D9B6");
        public static readonly Guid RuinedId = new Guid("6790C5D4-7CC6-43c9-9CAC-48227021E7E9");

        private UniquelyIdentifiableCache<VarianceReason> cache;

        public VarianceReason Order
        {
            get { return this.Cache.Get(OrderId); }
        }

        public VarianceReason Shipment
        {
            get { return this.Cache.Get(ShipmentId); }
        }

        public VarianceReason Theft
        {
            get { return this.Cache.Get(TheftId); }
        }

        public VarianceReason Shrinkage
        {
            get { return this.Cache.Get(ShrinkageId); }
        }

        public VarianceReason Unknown
        {
            get { return this.Cache.Get(UnknownId); }
        }

        public VarianceReason Ruined
        {
            get { return this.Cache.Get(RuinedId); }
        }

        private UniquelyIdentifiableCache<VarianceReason> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<VarianceReason>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new VarianceReasonBuilder(this.Session)
                .WithName("Order")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Order").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Bestelling").WithLocale(dutchLocale).Build())
                .WithUniqueId(OrderId)
                .Build();

            new VarianceReasonBuilder(this.Session)
                .WithName("Shipment")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Shipment").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verscheping").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShipmentId)
                .Build();
            
            new VarianceReasonBuilder(this.Session)
                .WithName("Theft")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Theft").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Diefstal").WithLocale(dutchLocale).Build())
                .WithUniqueId(TheftId)
                .Build();
            
            new VarianceReasonBuilder(this.Session)
                .WithName("Shrinkage")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Shrinkage").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Inkrimping").WithLocale(dutchLocale).Build())
                .WithUniqueId(ShrinkageId)
                .Build();
            
            new VarianceReasonBuilder(this.Session)
                .WithName("Unknown")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Unknown").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ongekend").WithLocale(dutchLocale).Build())
                .WithUniqueId(UnknownId)
                .Build();
            
            new VarianceReasonBuilder(this.Session)
                .WithName("Ruined")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ruined").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vernield").WithLocale(dutchLocale).Build())
                .WithUniqueId(RuinedId)
                .Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
