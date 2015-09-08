// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CostOfGoodsSoldMethods.cs" company="Allors bvba">
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

    public partial class CostOfGoodsSoldMethods
    {
        public static readonly Guid FiFoId = new Guid("CC6A3F78-9063-4E4A-BBB1-08A529017696");
        public static readonly Guid LiFoId = new Guid("A4E05005-FEC2-4F81-AD67-4F9C958BF94A");
        public static readonly Guid AverageId = new Guid("857D84FF-18AE-4139-8A4F-8A4BAD78C79E");

        private UniquelyIdentifiableCache<CostOfGoodsSoldMethod> cache;

        public CostOfGoodsSoldMethod FiFo
        {
            get { return this.Cache.Get(FiFoId); }
        }

        public CostOfGoodsSoldMethod LiFo
        {
            get { return this.Cache.Get(LiFoId); }
        }

        public CostOfGoodsSoldMethod Average
        {
            get { return this.Cache.Get(AverageId); }
        }

        private UniquelyIdentifiableCache<CostOfGoodsSoldMethod> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<CostOfGoodsSoldMethod>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DebitCreditConstantBuilder(this.Session)
                .WithName("FiFo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("FiFo").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("FiFo").WithLocale(dutchLocale).Build())
                .WithUniqueId(FiFoId).Build();
            
            new DebitCreditConstantBuilder(this.Session)
                .WithName("LiFo")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("LiFo").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("LiFo").WithLocale(dutchLocale).Build())
                .WithUniqueId(LiFoId)
                .Build();

            new DebitCreditConstantBuilder(this.Session)
                .WithName("Average price")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Average price").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gemiddelde prijs").WithLocale(dutchLocale).Build())
                .WithUniqueId(AverageId)
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