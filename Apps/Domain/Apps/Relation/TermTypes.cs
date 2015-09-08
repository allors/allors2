// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TermTypes.cs" company="Allors bvba">
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

    public partial class TermTypes
    {
        public static readonly Guid PercentageCancellationChargeId = new Guid("D81EE27F-EC44-4f66-BF45-9F5E0F95EA9B");
        public static readonly Guid DaysCancellationWithoutPenaltyId = new Guid("8475143A-D6CF-4ff7-BDB7-27168DC1C288");
        public static readonly Guid PercentagePenaltyNonPerformanceId = new Guid("87C6AD9D-E2CC-4946-A022-FCDD820832B8");
        public static readonly Guid DaysWithinWhichDeliveraryMustOccurId = new Guid("FEC52109-5169-4e52-9DA3-13C052B34076");
        public static readonly Guid PaymentNetDaysId = new Guid("23AB7C88-C7B0-4a8e-916E-02DFD3CD261A");
        public static readonly Guid LateFeeId = new Guid("4D8E9C5E-F4F6-4e62-8009-D247D4C60753");
        public static readonly Guid CollectionAgencyPenaltyId = new Guid("56C719DE-1B5D-4c2a-8AE3-F205F9852C79");
        public static readonly Guid NonReturnableSalesItemId = new Guid("B9A57CAF-3C48-463a-B627-3D0E127E5AF2");

        private UniquelyIdentifiableCache<TermType> cache;

        public TermType PercentageCancellationCharge
        {
            get { return this.Cache.Get(PercentageCancellationChargeId); }
        }

        public TermType DaysCancellationWithoutPenalty
        {
            get { return this.Cache.Get(DaysCancellationWithoutPenaltyId); }
        }

        public TermType PercentagePenaltyNonPerformance
        {
            get { return this.Cache.Get(PercentagePenaltyNonPerformanceId); }
        }

        public TermType PaymentNetDays
        {
            get { return this.Cache.Get(PaymentNetDaysId); }
        }

        public TermType LateFee
        {
            get { return this.Cache.Get(LateFeeId); }
        }

        public TermType CollectionAgencyPenalty
        {
            get { return this.Cache.Get(CollectionAgencyPenaltyId); }
        }

        public TermType DaysWithinWhichDeliveraryMustOccur
        {
            get { return this.Cache.Get(DaysWithinWhichDeliveraryMustOccurId); }
        }

        public TermType NonReturnableSalesItem
        {
            get { return this.Cache.Get(NonReturnableSalesItemId); }
        }

        private UniquelyIdentifiableCache<TermType> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<TermType>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var belgianLocale = new Locales(this.Session).DutchNetherlands;

            new TermTypeBuilder(this.Session)
                .WithName("Percentage Cancellation Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Percentage Cancellation Charge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Procent annulerings toeslag").WithLocale(belgianLocale).Build())
                .WithUniqueId(PercentageCancellationChargeId)
                .Build();

            new TermTypeBuilder(this.Session)
                .WithName("Days Cancellation Without Penalty")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Days Cancellation Without Penalty").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aantal dagen annluatie zonder kost").WithLocale(belgianLocale).Build())
                .WithUniqueId(DaysCancellationWithoutPenaltyId)
                .Build();
            
            new TermTypeBuilder(this.Session)
                .WithName("Percentage Penalty Non Performance")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Percentage Penalty Non Performance").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Percentage toeslag slechte (non performance)").WithLocale(belgianLocale).Build())
                .WithUniqueId(PercentagePenaltyNonPerformanceId)
                .Build();
            
            new TermTypeBuilder(this.Session)
                .WithName("Days Within Which Deliverary Must Occur")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Days Within Which Deliverary Must Occur").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aantal dagen waarin levering moet geberuren").WithLocale(belgianLocale).Build())
                .WithUniqueId(DaysWithinWhichDeliveraryMustOccurId)
                .Build();
            
            new TermTypeBuilder(this.Session)
                .WithName("Payment-net days")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Payment-net days").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Betaaltermijn").WithLocale(belgianLocale).Build())
                .WithUniqueId(PaymentNetDaysId)
                .Build();
            
            new TermTypeBuilder(this.Session)
                .WithName("Penalty for late fee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Penalty for late fee").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boete late betaling").WithLocale(belgianLocale).Build())
                .WithUniqueId(LateFeeId)
                .Build();
            
            new TermTypeBuilder(this.Session)
                .WithName("Penalty for collection agency")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Penalty for collection agency").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boete incassobureau").WithLocale(belgianLocale).Build())
                .WithUniqueId(CollectionAgencyPenaltyId)
                .Build();
            
            new TermTypeBuilder(this.Session)
                .WithName("Non returnable sales item")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Non returnable sales item").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Niet retourneerbaat item").WithLocale(belgianLocale).Build())
                .WithUniqueId(NonReturnableSalesItemId)
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
