// <copyright file="PurchaseShipmentSequences.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class PurchaseShipmentSequences
    {
        public static readonly Guid EnforcedSequenceId = new Guid("d0ec0fb6-d915-471e-9cfd-d5651b664d0a");
        public static readonly Guid RestartOnFiscalYearId = new Guid("db3c986a-b37a-4c89-a3b5-6cc12438bbb2");

        private UniquelyIdentifiableSticky<PurchaseShipmentSequence> cache;

        public PurchaseShipmentSequence EnforcedSequence => this.Cache[EnforcedSequenceId];

        public PurchaseShipmentSequence RestartOnFiscalYear => this.Cache[RestartOnFiscalYearId];

        private UniquelyIdentifiableSticky<PurchaseShipmentSequence> Cache => this.cache ??= new UniquelyIdentifiableSticky<PurchaseShipmentSequence>(this.Session);

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
