// <copyright file="WorkEffortSequences.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class WorkEffortSequences
    {
        public static readonly Guid EnforcedSequenceId = new Guid("b1042800-5af6-4a78-9c8d-535cf480cb64");
        public static readonly Guid RestartOnFiscalYearId = new Guid("553f63dd-65d9-4a11-8f6a-70cba8a4a26f");

        private UniquelyIdentifiableSticky<WorkEffortSequence> cache;

        public WorkEffortSequence EnforcedSequence => this.Cache[EnforcedSequenceId];

        public WorkEffortSequence RestartOnFiscalYear => this.Cache[RestartOnFiscalYearId];

        private UniquelyIdentifiableSticky<WorkEffortSequence> Cache => this.cache ??= new UniquelyIdentifiableSticky<WorkEffortSequence>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(EnforcedSequenceId, v =>
            {
                v.Name = "Enforced Sequence (no gaps)";
                localisedName.Set(v, dutchLocale, "Aansluitend genummerd");
                v.IsActive = true;
            });

            merge(RestartOnFiscalYearId, v =>
            {
                v.Name = "Restart each fiscal year (no gaps, reset to '1' each year";
                localisedName.Set(v, dutchLocale, "Herstart elk fiscaal jaar (aansluitend, begint elk jaar met nummer '1'");
                v.IsActive = true;
            });
        }
    }
}
