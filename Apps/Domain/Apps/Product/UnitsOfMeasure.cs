// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitsOfMeasure.cs" company="Allors bvba">
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

    public partial class UnitsOfMeasure
    {
        public static readonly Guid PackId = new Guid("C4EC577A-D682-433c-BD70-84538BE83209");
        public static readonly Guid PairId = new Guid("62CB31EB-CD70-4836-B20F-1088C6CA9DCB");
        public static readonly Guid PiecesId = new Guid("DDC5822F-9D7E-4729-B73F-8D033384B3FB");
        public static readonly Guid PieceId = new Guid("F4BBDB52-3441-4768-92D4-729C6C5D6F1B");
        public static readonly Guid LengthCmId = new Guid("BD1D2F1F-329E-41c4-A27B-0BA0CD3960E2");
        public static readonly Guid CentimeterId = new Guid("7D81FFC7-E77D-4a00-916D-49F2B1CCA12E");
        public static readonly Guid MeterId = new Guid("2598BA8D-CF49-47f5-98E2-E65795C4178E");
        public static readonly Guid WidthCmId = new Guid("73C329FE-32EC-401f-A0F0-17EDE011B518");
        public static readonly Guid HeightCmId = new Guid("87519F22-9EA3-4a3f-9DC4-66A417AC08AD");

        private UniquelyIdentifiableCache<UnitOfMeasure> cache;

        public UnitOfMeasure Pack
        {
            get { return this.Cache.Get(PackId); }
        }

        public UnitOfMeasure Pair
        {
            get { return this.Cache.Get(PairId); }
        }

        public UnitOfMeasure Pieces
        {
            get { return this.Cache.Get(PiecesId); }
        }

        public UnitOfMeasure Piece
        {
            get { return this.Cache.Get(PieceId); }
        }

        public UnitOfMeasure LengthCm
        {
            get { return this.Cache.Get(LengthCmId); }
        }

        public UnitOfMeasure WidthCm
        {
            get { return this.Cache.Get(WidthCmId); }
        }

        public UnitOfMeasure HeightCm
        {
            get { return this.Cache.Get(HeightCmId); }
        }

        public UnitOfMeasure Centimeter
        {
            get { return this.Cache.Get(CentimeterId); }
        }

        public UnitOfMeasure Meter
        {
            get { return this.Cache.Get(MeterId); }
        }

        private UniquelyIdentifiableCache<UnitOfMeasure> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<UnitOfMeasure>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new UnitOfMeasureBuilder(this.Session)
                .WithName("Pack")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pack").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pakket").WithLocale(dutchLocale).Build())
                .WithUniqueId(PackId)
                .Build();

            new UnitOfMeasureBuilder(this.Session)
                .WithName("Pair")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pair").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Paar").WithLocale(dutchLocale).Build())
                .WithUniqueId(PairId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithName("Pieces")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pieces").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Stuks").WithLocale(dutchLocale).Build())
                .WithUniqueId(PiecesId)
                .Build();
           
            new UnitOfMeasureBuilder(this.Session)
                .WithName("Piece")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Piece").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Stuk").WithLocale(dutchLocale).Build())
                .WithUniqueId(PieceId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithName("cm length")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm length").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm lengte").WithLocale(dutchLocale).Build())
                .WithUniqueId(LengthCmId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithName("cm width")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm width").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm breedte").WithLocale(dutchLocale).Build())
                .WithUniqueId(WidthCmId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithName("cm height")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm height").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm hoogte").WithLocale(dutchLocale).Build())
                .WithUniqueId(HeightCmId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithName("cm")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm").WithLocale(dutchLocale).Build())
                .WithUniqueId(CentimeterId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithName("meter")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("meter").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("meter").WithLocale(dutchLocale).Build())
                .WithUniqueId(MeterId)
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