// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceSequences.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new InvoiceSequenceBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Enforced Sequence (no gaps)").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aansluitend genummerd").WithLocale(dutchLocale).Build())
                .WithUniqueId(EnforcedSequenceId)
                .Build();
            
            new InvoiceSequenceBuilder(this.Session)
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Restart each fiscal year (no gaps, reset to '1' each year").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Herstart elk fiscaal jaar (aansluitend, begint elk jaar met nummer '1'").WithLocale(dutchLocale).Build())
                .WithUniqueId(RestartOnFiscalYearId)
                .Build();
        }
    }
}