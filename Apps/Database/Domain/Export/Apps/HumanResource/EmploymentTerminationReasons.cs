// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmploymentTerminationReasons.cs" company="Allors bvba">
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

        private UniquelyIdentifiableSticky<EmploymentTerminationReason> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<EmploymentTerminationReason>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new EmploymentTerminationReasonBuilder(this.Session)
                .WithName("Insubordination")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Weigering van bevel").WithLocale(dutchLocale).Build())
                .WithUniqueId(InsubordinationId)
                .WithIsActive(true)
                .Build();
            
            new EmploymentTerminationReasonBuilder(this.Session)
                .WithName("Accepted New Job")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Nieuwe job aangenomen").WithLocale(dutchLocale).Build())
                .WithUniqueId(AcceptedNewJobId)
                .WithIsActive(true)
                .Build();
            
            new EmploymentTerminationReasonBuilder(this.Session)
                .WithName("Non Performance")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Slechte performantie").WithLocale(dutchLocale).Build())
                .WithUniqueId(NonPerformanceId)
                .WithIsActive(true)
                .Build();
            
            new EmploymentTerminationReasonBuilder(this.Session)
                .WithName("Moved")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Verhuis").WithLocale(dutchLocale).Build())
                .WithUniqueId(MovedId)
                .WithIsActive(true)
                .Build();
        }
    }
}
