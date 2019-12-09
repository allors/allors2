// <copyright file="EmploymentTerminationReasons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class EmploymentTerminationReasons
    {
        private static readonly Guid InsubordinationId = new Guid("6791B66C-FC8B-45d3-A5B0-56D15135F857");
        private static readonly Guid AcceptedNewJobId = new Guid("5DB325E1-F60F-49bc-980B-CA202D2933F5");
        private static readonly Guid NonPerformanceId = new Guid("A83EEE92-54B0-45cc-AC07-C33B0116D33B");
        private static readonly Guid MovedId = new Guid("D1BC9AE6-CD34-4164-B807-FB247B9A6278");

        private UniquelyIdentifiableSticky<EmploymentTerminationReason> cache;

        public EmploymentTerminationReason Insubordination => this.Cache[InsubordinationId];

        public EmploymentTerminationReason AcceptedNewJob => this.Cache[AcceptedNewJobId];

        public EmploymentTerminationReason NonPerformance => this.Cache[NonPerformanceId];

        public EmploymentTerminationReason Moved => this.Cache[MovedId];

        private UniquelyIdentifiableSticky<EmploymentTerminationReason> Cache => this.cache ??= new UniquelyIdentifiableSticky<EmploymentTerminationReason>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(InsubordinationId, v =>
            {
                v.Name = "Insubordination";
                localisedName.Set(v, dutchLocale, "Weigering van bevel");
                v.IsActive = true;
            });

            merge(AcceptedNewJobId, v =>
            {
                v.Name = "Accepted New Job";
                localisedName.Set(v, dutchLocale, "Nieuwe job aangenomen");
                v.IsActive = true;
            });

            merge(NonPerformanceId, v =>
            {
                v.Name = "Non Performance";
                localisedName.Set(v, dutchLocale, "Slechte performantie");
                v.IsActive = true;
            });

            merge(MovedId, v =>
            {
                v.Name = "Moved";
                localisedName.Set(v, dutchLocale, "Verhuis");
                v.IsActive = true;
            });
        }
    }
}
