// <copyright file="InvoiceSequences.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class InvoiceSequences
    {
        private static readonly Guid EnforcedSequenceId = new Guid("54FF2FC1-9A4F-4d46-8BEA-866F4FBB448C");
        private static readonly Guid RestartOnFiscalYearId = new Guid("2A2027B5-30D2-42a1-BE8B-FEF343C742D1");

        private UniquelyIdentifiableSticky<InvoiceSequence> cache;

        public InvoiceSequence EnforcedSequence => this.Cache[EnforcedSequenceId];

        public InvoiceSequence RestartOnFiscalYear => this.Cache[RestartOnFiscalYearId];

        private UniquelyIdentifiableSticky<InvoiceSequence> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<InvoiceSequence>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new InvoiceSequenceBuilder(this.Session)
                .WithName("Enforced Sequence (no gaps)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aansluitend genummerd").WithLocale(dutchLocale).Build())
                .WithUniqueId(EnforcedSequenceId)
                .WithIsActive(true)
                .Build();

            new InvoiceSequenceBuilder(this.Session)
                .WithName("Restart each fiscal year (no gaps, reset to '1' each year")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Herstart elk fiscaal jaar (aansluitend, begint elk jaar met nummer '1'").WithLocale(dutchLocale).Build())
                .WithUniqueId(RestartOnFiscalYearId)
                .WithIsActive(true)
                .Build();
        }
    }
}
