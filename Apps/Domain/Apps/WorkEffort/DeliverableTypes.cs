// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeliverableTypes.cs" company="Allors bvba">
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

    public partial class DeliverableTypes
    {
        public static readonly Guid ProjectPlanId = new Guid("AFAC9785-9DA9-4a74-BC04-113EEB52949D");
        public static readonly Guid PresentationId = new Guid("1A8FCF9F-EF83-4773-B7CC-30E1B567790B");
        public static readonly Guid ReportId = new Guid("633116FD-9843-4356-8261-637170682E2F");
        public static readonly Guid MarketAnalysisId = new Guid("6994E018-CCD5-442f-80C2-F133DC9FAD17");

        private UniquelyIdentifiableCache<DeliverableType> cache;

        public DeliverableType ProjectPlan
        {
            get { return this.Cache.Get(ProjectPlanId); }
        }

        public DeliverableType Presentation
        {
            get { return this.Cache.Get(PresentationId); }
        }

        public DeliverableType Report
        {
            get { return this.Cache.Get(ReportId); }
        }

        public DeliverableType MarketAnalysis
        {
            get { return this.Cache.Get(MarketAnalysisId); }
        }

        private UniquelyIdentifiableCache<DeliverableType> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<DeliverableType>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new DeliverableTypeBuilder(this.Session)
                .WithName("Project Plan")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Project Plan").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Projectplan").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProjectPlanId)
                .Build();
            
            new DeliverableTypeBuilder(this.Session)
                .WithName("Presentation")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Presentation").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Presentatie").WithLocale(dutchLocale).Build())
                .WithUniqueId(PresentationId)
                .Build();
            
            new DeliverableTypeBuilder(this.Session)
                .WithName("Report")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Report").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Rapport").WithLocale(dutchLocale).Build())
                .WithUniqueId(ReportId)
                .Build();
            
            new DeliverableTypeBuilder(this.Session)
                .WithName("Market Analysis")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Market Analysis").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Markt analyse").WithLocale(dutchLocale).Build())
                .WithUniqueId(MarketAnalysisId)
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
