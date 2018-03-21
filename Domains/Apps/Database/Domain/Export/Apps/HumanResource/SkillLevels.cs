// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkillLevels.cs" company="Allors bvba">
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

    public partial class SkillLevels
    {
        private static readonly Guid BeginnerId = new Guid("F8A94F72-E5F2-49c5-B738-6B0F5EFC7BAF");
        private static readonly Guid IntermediateId = new Guid("BD859561-9488-4db4-82C4-621D0F4AA1B4");
        private static readonly Guid AdvancedId = new Guid("C4FD3054-20F4-40e8-B6A3-E91734D75C13");
        private static readonly Guid ExpertId = new Guid("E204AA8A-C61E-44f6-906B-FE45AB15D4B0");

        private UniquelyIdentifiableSticky<SkillLevel> cache;

        public SkillLevel Beginner => this.Cache[BeginnerId];

        public SkillLevel Intermediate => this.Cache[IntermediateId];

        public SkillLevel Advanced => this.Cache[AdvancedId];

        public SkillLevel Expert => this.Cache[ExpertId];

        private UniquelyIdentifiableSticky<SkillLevel> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<SkillLevel>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var dutchLocale = new Locales(this.Session).DutchNetherlands;
            
            new SkillLevelBuilder(this.Session)
                .WithName("Beginner")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Starter").WithLocale(dutchLocale).Build())
                .WithUniqueId(BeginnerId)
                .Build();

            new SkillLevelBuilder(this.Session)
                .WithName("Intermediate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Intermediate").WithLocale(dutchLocale).Build())
                .WithUniqueId(IntermediateId)
                .Build();
            
            new SkillLevelBuilder(this.Session)
                .WithName("Advanced")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ervaren").WithLocale(dutchLocale).Build())
                .WithUniqueId(AdvancedId)
                .Build();
            
            new SkillLevelBuilder(this.Session)
                .WithName("Expert")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Expert").WithLocale(dutchLocale).Build())
                .WithUniqueId(ExpertId)
                .Build();
        }
    }
}
