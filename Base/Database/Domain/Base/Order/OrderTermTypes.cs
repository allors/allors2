// <copyright file="OrderTermTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class OrderTermTypes
    {
        private static readonly Guid PercentageCancellationChargeId = new Guid("D81EE27F-EC44-4f66-BF45-9F5E0F95EA9B");
        private static readonly Guid DaysCancellationWithoutPenaltyId = new Guid("8475143A-D6CF-4ff7-BDB7-27168DC1C288");
        private static readonly Guid PercentagePenaltyNonPerformanceId = new Guid("87C6AD9D-E2CC-4946-A022-FCDD820832B8");
        private static readonly Guid DaysWithinWhichDeliveraryMustOccurId = new Guid("FEC52109-5169-4e52-9DA3-13C052B34076");
        private static readonly Guid NonReturnableSalesItemId = new Guid("B9A57CAF-3C48-463a-B627-3D0E127E5AF2");

        private UniquelyIdentifiableSticky<OrderTermType> cache;

        public OrderTermType PercentageCancellationCharge => this.Cache[PercentageCancellationChargeId];

        public OrderTermType DaysCancellationWithoutPenalty => this.Cache[DaysCancellationWithoutPenaltyId];

        public OrderTermType PercentagePenaltyNonPerformance => this.Cache[PercentagePenaltyNonPerformanceId];

        public OrderTermType DaysWithinWhichDeliveraryMustOccur => this.Cache[DaysWithinWhichDeliveraryMustOccurId];

        public OrderTermType NonReturnableSalesItem => this.Cache[NonReturnableSalesItemId];

        private UniquelyIdentifiableSticky<OrderTermType> Cache => this.cache ??= new UniquelyIdentifiableSticky<OrderTermType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var belgianLocale = new Locales(this.Session).DutchNetherlands;

            new OrderTermTypeBuilder(this.Session)
                .WithName("Percentage Cancellation Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Procent annulerings toeslag").WithLocale(belgianLocale).Build())
                .WithUniqueId(PercentageCancellationChargeId)
                .WithIsActive(true)
                .Build();

            new OrderTermTypeBuilder(this.Session)
                .WithName("Days Cancellation Without Penalty")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aantal dagen annulatie zonder kost").WithLocale(belgianLocale).Build())
                .WithUniqueId(DaysCancellationWithoutPenaltyId)
                .WithIsActive(true)
                .Build();

            new OrderTermTypeBuilder(this.Session)
                .WithName("Percentage Penalty Non Performance")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Percentage toeslag slechte (non performance)").WithLocale(belgianLocale).Build())
                .WithUniqueId(PercentagePenaltyNonPerformanceId)
                .WithIsActive(true)
                .Build();

            new OrderTermTypeBuilder(this.Session)
                .WithName("Days Within Which Deliverary Must Occur")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aantal dagen waarbinnen levering moet gebeuren").WithLocale(belgianLocale).Build())
                .WithUniqueId(DaysWithinWhichDeliveraryMustOccurId)
                .WithIsActive(true)
                .Build();

            new OrderTermTypeBuilder(this.Session)
                .WithName("Non returnable sales item")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Niet retourneerbaat item").WithLocale(belgianLocale).Build())
                .WithUniqueId(NonReturnableSalesItemId)
                .WithIsActive(true)
                .Build();
        }
    }
}
