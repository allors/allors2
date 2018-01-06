// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitsOfMeasure.cs" company="Allors bvba">
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

    public partial class UnitsOfMeasure
    {
        private static readonly Guid PackId = new Guid("C4EC577A-D682-433c-BD70-84538BE83209");
        private static readonly Guid PairId = new Guid("62CB31EB-CD70-4836-B20F-1088C6CA9DCB");
        private static readonly Guid PieceId = new Guid("F4BBDB52-3441-4768-92D4-729C6C5D6F1B");
        private static readonly Guid LengthCmId = new Guid("BD1D2F1F-329E-41c4-A27B-0BA0CD3960E2");
        private static readonly Guid CentimeterId = new Guid("7D81FFC7-E77D-4a00-916D-49F2B1CCA12E");
        private static readonly Guid MeterId = new Guid("2598BA8D-CF49-47f5-98E2-E65795C4178E");
        private static readonly Guid WidthCmId = new Guid("73C329FE-32EC-401f-A0F0-17EDE011B518");
        private static readonly Guid HeightCmId = new Guid("87519F22-9EA3-4a3f-9DC4-66A417AC08AD");

        private UniquelyIdentifiableSticky<UnitOfMeasure> cache;

        public UnitOfMeasure Pack => this.Cache[PackId];

        public UnitOfMeasure Pair => this.Cache[PairId];

        public UnitOfMeasure Piece => this.Cache[PieceId];

        public UnitOfMeasure LengthCm => this.Cache[LengthCmId];

        public UnitOfMeasure WidthCm => this.Cache[WidthCmId];

        public UnitOfMeasure HeightCm => this.Cache[HeightCmId];

        public UnitOfMeasure Centimeter => this.Cache[CentimeterId];

        public UnitOfMeasure Meter => this.Cache[MeterId];

        private UniquelyIdentifiableSticky<UnitOfMeasure> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<UnitOfMeasure>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new UnitOfMeasureBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pack").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pakket").WithLocale(dutchLocale).Build())
                .WithUniqueId(PackId)
                .Build();

            new UnitOfMeasureBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pair").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Paar").WithLocale(dutchLocale).Build())
                .WithUniqueId(PairId)
                .Build();
           
            new UnitOfMeasureBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Piece").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Stuk").WithLocale(dutchLocale).Build())
                .WithUniqueId(PieceId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm length").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm lengte").WithLocale(dutchLocale).Build())
                .WithUniqueId(LengthCmId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm width").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm breedte").WithLocale(dutchLocale).Build())
                .WithUniqueId(WidthCmId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm height").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm hoogte").WithLocale(dutchLocale).Build())
                .WithUniqueId(HeightCmId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("cm").WithLocale(dutchLocale).Build())
                .WithUniqueId(CentimeterId)
                .Build();
            
            new UnitOfMeasureBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("meter").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("meter").WithLocale(dutchLocale).Build())
                .WithUniqueId(MeterId)
                .Build();
        }
    }
}