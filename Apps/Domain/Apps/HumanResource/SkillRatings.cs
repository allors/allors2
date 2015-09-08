// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkillRatings.cs" company="Allors bvba">
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

    public partial class SkillRatings
    {
        public static readonly Guid PoorId = new Guid("5D2D23C7-95AA-49ed-8B2A-9A3E4D91BC3D");
        public static readonly Guid FairId = new Guid("583BCA0A-2A5E-40c1-936C-D8F16A4DAAC5");
        public static readonly Guid GoodId = new Guid("374DEE3A-82FA-4bee-B66B-F48CA1B0CBD7");
        public static readonly Guid ExcellentId = new Guid("52029ECD-1752-4b40-A39D-54B0C1CB8297");

        private UniquelyIdentifiableCache<SkillRating> cache;

        public SkillRating Poor
        {
            get { return this.Cache.Get(PoorId); }
        }

        public SkillRating Fair
        {
            get { return this.Cache.Get(FairId); }
        }

        public SkillRating Good
        {
            get { return this.Cache.Get(GoodId); }
        }

        public SkillRating Excellent
        {
            get { return this.Cache.Get(ExcellentId); }
        }

        private UniquelyIdentifiableCache<SkillRating> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<SkillRating>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new SkillRatingBuilder(this.Session)
                .WithName("Poor")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Poor").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Slecht").WithLocale(dutchLocale).Build())
                .WithUniqueId(PoorId)
                .Build();

            new SkillRatingBuilder(this.Session)
                .WithName("Fair")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fair").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Matig").WithLocale(dutchLocale).Build())
                .WithUniqueId(FairId)
                .Build();
            
            new SkillRatingBuilder(this.Session)
                .WithName("Good")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Good").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Goed").WithLocale(dutchLocale).Build())
                .WithUniqueId(GoodId)
                .Build();
            
            new SkillRatingBuilder(this.Session)
                .WithName("Excellent")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Excellent").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Uitstekend").WithLocale(dutchLocale).Build())
                .WithUniqueId(ExcellentId)
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
