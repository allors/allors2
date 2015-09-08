// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeductionTypes.cs" company="Allors bvba">
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

    public partial class DeductionTypes
    {
        public static readonly Guid RetirementId = new Guid("2D0F0788-EB2B-4ef3-A09A-A7285DAD72CF");
        public static readonly Guid InsuranceId = new Guid("D82A5A9F-068F-4e30-88F5-5E6C81D03BAF");

        private UniquelyIdentifiableCache<DeductionType> cache;

        public DeductionType Retirement
        {
            get { return this.Cache.Get(RetirementId); }
        }

        public DeductionType Insurance
        {
            get { return this.Cache.Get(InsuranceId); }
        }

        private UniquelyIdentifiableCache<DeductionType> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<DeductionType>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DeductionTypeBuilder(this.Session)
                .WithName("Retirement")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Retirement").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Pensioen").WithLocale(dutchLocale).Build())
                .WithUniqueId(RetirementId)
                .Build();
            
            new DeductionTypeBuilder(this.Session)
                .WithName("Insurance")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Insurance").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verzekering").WithLocale(dutchLocale).Build())
                .WithUniqueId(InsuranceId)
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
