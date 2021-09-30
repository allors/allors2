// <copyright file="EmploymentApplicationStatuses.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class EmploymentApplicationStatuses
    {
        private static readonly Guid ReceivedId = new Guid("0BB970D8-9273-42af-AD26-58217B1D8501");
        private static readonly Guid ReviewedId = new Guid("835D288A-940D-4597-A18D-1A818749070A");
        private static readonly Guid FiledId = new Guid("25FAF6A1-07B7-4ec4-AEF1-EF5C95CBFBDB");
        private static readonly Guid RejectedId = new Guid("EDEA15DA-714D-40c4-BA10-436BD84140D7");
        private static readonly Guid NotifiedOfNonInterestedId = new Guid("E889B083-22D4-4aa8-85BE-6416AA0839F9");
        private static readonly Guid EmployedId = new Guid("2CB5ABD4-8799-46c1-9E2B-538DB076492E");

        private UniquelyIdentifiableSticky<EmploymentApplicationStatus> cache;

        public EmploymentApplicationStatus Received => this.Cache[ReceivedId];

        public EmploymentApplicationStatus Reviewed => this.Cache[ReviewedId];

        public EmploymentApplicationStatus Filed => this.Cache[FiledId];

        public EmploymentApplicationStatus Rejected => this.Cache[RejectedId];

        public EmploymentApplicationStatus NotifiedOfNonInterested => this.Cache[NotifiedOfNonInterestedId];

        public EmploymentApplicationStatus Employed => this.Cache[EmployedId];

        private UniquelyIdentifiableSticky<EmploymentApplicationStatus> Cache => this.cache ??= new UniquelyIdentifiableSticky<EmploymentApplicationStatus>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(ReceivedId, v =>
            {
                v.Name = "Received";
                localisedName.Set(v, dutchLocale, "Ontvangen");
                v.IsActive = true;
            });

            merge(ReceivedId, v =>
            {
                v.Name = "Reviewed";
                localisedName.Set(v, dutchLocale, "Gereviewed");
                v.IsActive = true;
            });

            merge(FiledId, v =>
            {
                v.Name = "Filed";
                localisedName.Set(v, dutchLocale, "Ingediend");
                v.IsActive = true;
            });

            merge(RejectedId, v =>
            {
                v.Name = "Rejected";
                localisedName.Set(v, dutchLocale, "Geweigerd");
                v.IsActive = true;
            });

            merge(NotifiedOfNonInterestedId, v =>
            {
                v.Name = "Notified Of Non Interested";
                localisedName.Set(v, dutchLocale, "Niet geïnteresseerd beantwoord");
                v.IsActive = true;
            });

            merge(EmployedId, v =>
            {
                v.Name = "Employed";
                localisedName.Set(v, dutchLocale, "Aangenomen");
                v.IsActive = true;
            });
        }
    }
}
