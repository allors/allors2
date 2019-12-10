// <copyright file="DeliverableTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class DeliverableTypes
    {
        private static readonly Guid ProjectPlanId = new Guid("AFAC9785-9DA9-4a74-BC04-113EEB52949D");
        private static readonly Guid PresentationId = new Guid("1A8FCF9F-EF83-4773-B7CC-30E1B567790B");
        private static readonly Guid ReportId = new Guid("633116FD-9843-4356-8261-637170682E2F");
        private static readonly Guid MarketAnalysisId = new Guid("6994E018-CCD5-442f-80C2-F133DC9FAD17");

        private UniquelyIdentifiableSticky<DeliverableType> cache;

        public DeliverableType ProjectPlan => this.Cache[ProjectPlanId];

        public DeliverableType Presentation => this.Cache[PresentationId];

        public DeliverableType Report => this.Cache[ReportId];

        public DeliverableType MarketAnalysis => this.Cache[MarketAnalysisId];

        private UniquelyIdentifiableSticky<DeliverableType> Cache => this.cache ??= new UniquelyIdentifiableSticky<DeliverableType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(ProjectPlanId, v =>
            {
                v.Name = "Project Plan";
                localisedName.Set(v, dutchLocale, "Projectplan");
                v.IsActive = true;
            });

            merge(PresentationId, v =>
            {
                v.Name = "Presentation";
                localisedName.Set(v, dutchLocale, "Presentatie");
                v.IsActive = true;
            });

            merge(ReportId, v =>
            {
                v.Name = "Report";
                localisedName.Set(v, dutchLocale, "Rapport");
                v.IsActive = true;
            });

            merge(MarketAnalysisId, v =>
            {
                v.Name = "Market Analysis";
                localisedName.Set(v, dutchLocale, "Markt analyse");
                v.IsActive = true;
            });
        }
    }
}
