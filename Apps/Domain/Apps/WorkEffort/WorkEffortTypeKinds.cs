// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkEffortTypeKinds.cs" company="Allors bvba">
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

    public partial class WorkEffortTypeKinds
    {
        public static readonly Guid ProductionRunId = new Guid("61480275-3EB4-45b1-9E2D-533ABB3A6C64");
        public static readonly Guid ProcessId = new Guid("4DFB282D-19A0-491b-B01D-16569E20B4F4");
        public static readonly Guid ProcessStepId = new Guid("6A73AE6F-1C74-4ad0-8784-CBEAEF3B62D5");

        private UniquelyIdentifiableCache<WorkEffortTypeKind> cache;

        public WorkEffortTypeKind ProductionRun
        {
            get { return this.Cache.Get(ProductionRunId); }
        }

        public WorkEffortTypeKind Process
        {
            get { return this.Cache.Get(ProcessId); }
        }

        public WorkEffortTypeKind ProcessStep
        {
            get { return this.Cache.Get(ProcessStepId); }
        }

        private UniquelyIdentifiableCache<WorkEffortTypeKind> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<WorkEffortTypeKind>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new WorkEffortTypeKindBuilder(this.Session)
                .WithName("Production Run")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Production Run").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Productie run").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProductionRunId)
                .Build();
            
            new WorkEffortTypeKindBuilder(this.Session)
                .WithName("Process")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Process").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verwerking").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProcessId)
                .Build();
            
            new WorkEffortTypeKindBuilder(this.Session)
                .WithName("Process Step")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Process Step").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Proces stap").WithLocale(dutchLocale).Build())
                .WithUniqueId(ProcessStepId)
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
