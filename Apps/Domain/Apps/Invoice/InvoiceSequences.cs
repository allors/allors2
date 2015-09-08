// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceSequences.cs" company="Allors bvba">
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

    public partial class InvoiceSequences
    {
        public static readonly Guid EnforcedSequenceId = new Guid("54FF2FC1-9A4F-4d46-8BEA-866F4FBB448C");
        public static readonly Guid RestartOnFiscalYearId = new Guid("2A2027B5-30D2-42a1-BE8B-FEF343C742D1");

        private UniquelyIdentifiableCache<InvoiceSequence> cache;

        public InvoiceSequence EnforcedSequence
        {
            get { return this.Cache.Get(EnforcedSequenceId); }
        }

        public InvoiceSequence RestartOnFiscalYear
        {
            get { return this.Cache.Get(RestartOnFiscalYearId); }
        }

        private UniquelyIdentifiableCache<InvoiceSequence> Cache
        {
            get
            {
                return this.cache ?? (this.cache = new UniquelyIdentifiableCache<InvoiceSequence>(this.Session));
            }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new InvoiceSequenceBuilder(this.Session)
                .WithName("Enforced Sequence (no gaps)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Enforced Sequence (no gaps)").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aansluitend genummerd").WithLocale(dutchLocale).Build())
                .WithUniqueId(EnforcedSequenceId)
                .Build();
            
            new InvoiceSequenceBuilder(this.Session)
                .WithName("Restart each fiscal year (no gaps, reset to '1' each year")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Restart each fiscal year (no gaps, reset to '1' each year").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Herstart elk fiscaal jaar (aansluitend, begint elk jaar met nummer '1'").WithLocale(dutchLocale).Build())
                .WithUniqueId(RestartOnFiscalYearId)
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