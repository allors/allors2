// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmploymentTerminationReasons.cs" company="Allors bvba">
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

    public partial class EmploymentTerminationReasons
    {
        public static readonly Guid InsubordinationId = new Guid("6791B66C-FC8B-45d3-A5B0-56D15135F857");
        public static readonly Guid AcceptedNewJobId = new Guid("5DB325E1-F60F-49bc-980B-CA202D2933F5");
        public static readonly Guid NonPerformanceId = new Guid("A83EEE92-54B0-45cc-AC07-C33B0116D33B");
        public static readonly Guid MovedId = new Guid("D1BC9AE6-CD34-4164-B807-FB247B9A6278");

        private UniquelyIdentifiableCache<EmploymentTerminationReason> cache;

        public EmploymentTerminationReason Insubordination
        {
            get { return this.Cache.Get(InsubordinationId); }
        }

        public EmploymentTerminationReason AcceptedNewJob
        {
            get { return this.Cache.Get(AcceptedNewJobId); }
        }

        public EmploymentTerminationReason NonPerformance
        {
            get { return this.Cache.Get(NonPerformanceId); }
        }

        public EmploymentTerminationReason Moved
        {
            get { return this.Cache.Get(MovedId); }
        }

        private UniquelyIdentifiableCache<EmploymentTerminationReason> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<EmploymentTerminationReason>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EmploymentTerminationReasonBuilder(this.Session)
                .WithName("Insubordination")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Insubordination").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Weigering van bevel").WithLocale(dutchLocale).Build())
                .WithUniqueId(InsubordinationId)
                .Build();
            
            new EmploymentTerminationReasonBuilder(this.Session)
                .WithName("Accepted New Job")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Accepted New Job").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Nieuwe job aangenomen").WithLocale(dutchLocale).Build())
                .WithUniqueId(AcceptedNewJobId)
                .Build();
            
            new EmploymentTerminationReasonBuilder(this.Session)
                .WithName("Non Performance")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Non Performance").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Slechte performantie").WithLocale(dutchLocale).Build())
                .WithUniqueId(NonPerformanceId)
                .Build();
            
            new EmploymentTerminationReasonBuilder(this.Session)
                .WithName("Moved")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Moved").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verhuis").WithLocale(dutchLocale).Build())
                .WithUniqueId(MovedId)
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
