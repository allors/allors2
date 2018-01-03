// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TermTypes.cs" company="Allors bvba">
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

    public partial class SalesTermTypes
    {
        private static readonly Guid PercentageCancellationChargeId = new Guid("D81EE27F-EC44-4f66-BF45-9F5E0F95EA9B");
        private static readonly Guid DaysCancellationWithoutPenaltyId = new Guid("8475143A-D6CF-4ff7-BDB7-27168DC1C288");
        private static readonly Guid PercentagePenaltyNonPerformanceId = new Guid("87C6AD9D-E2CC-4946-A022-FCDD820832B8");
        private static readonly Guid DaysWithinWhichDeliveraryMustOccurId = new Guid("FEC52109-5169-4e52-9DA3-13C052B34076");
        private static readonly Guid NonReturnableSalesItemId = new Guid("B9A57CAF-3C48-463a-B627-3D0E127E5AF2");
        private static readonly Guid PaymentNetDaysId = new Guid("23AB7C88-C7B0-4a8e-916E-02DFD3CD261A");
        private static readonly Guid LateFeeId = new Guid("4D8E9C5E-F4F6-4e62-8009-D247D4C60753");
        private static readonly Guid CollectionAgencyPenaltyId = new Guid("56C719DE-1B5D-4c2a-8AE3-F205F9852C79");
        private static readonly Guid ExwId = new Guid("08F45D13-4354-494E-889E-BD84F73749D8");
        private static readonly Guid FcaId = new Guid("689D7B46-6DE5-4276-AF1B-F9A8A3DEB7CF");
        private static readonly Guid CptId = new Guid("CAF35B5B-7156-45D0-95E3-26632D0D4BF7");
        private static readonly Guid CipId = new Guid("D93EF678-15FE-4794-8C94-7384DD84ACBB");
        private static readonly Guid DatId = new Guid("8412EB47-C674-49C3-B2F5-F93B1E2C28FD");
        private static readonly Guid DapId = new Guid("2E050969-A3B8-481A-B37F-ABC11C50A9DD");
        private static readonly Guid DdpId = new Guid("D9CB935C-539E-403E-A368-15564038591B");
        private static readonly Guid FasId = new Guid("DC851808-BA41-46C4-AB32-18219E52939D");
        private static readonly Guid FobId = new Guid("5005D64C-2D6E-411F-9CF7-FBE3BE0DA79E");
        private static readonly Guid CfrId = new Guid("14C74444-F935-4AF2-9108-8CCEBD1920B2");
        private static readonly Guid CifId = new Guid("13C6ACE8-A928-45D4-8F2D-7F2320E38EE0");

        private UniquelyIdentifiableSticky<SalesTermType> cache;

        public SalesTermType PercentageCancellationCharge => this.Cache[PercentageCancellationChargeId];

        public SalesTermType DaysCancellationWithoutPenalty => this.Cache[DaysCancellationWithoutPenaltyId];

        public SalesTermType PercentagePenaltyNonPerformance => this.Cache[PercentagePenaltyNonPerformanceId];

        public SalesTermType DaysWithinWhichDeliveraryMustOccur => this.Cache[DaysWithinWhichDeliveraryMustOccurId];

        public SalesTermType NonReturnableSalesItem => this.Cache[NonReturnableSalesItemId];

        public SalesTermType PaymentNetDays => this.Cache[PaymentNetDaysId];

        public SalesTermType LateFee => this.Cache[LateFeeId];

        public SalesTermType CollectionAgencyPenalty => this.Cache[CollectionAgencyPenaltyId];

        public SalesTermType Exw => this.Cache[ExwId];

        public SalesTermType Fca => this.Cache[FcaId];

        public SalesTermType Cpt => this.Cache[CptId];

        public SalesTermType Dat => this.Cache[DatId];

        public SalesTermType Dap => this.Cache[DapId];

        public SalesTermType Ddp => this.Cache[DdpId];

        public SalesTermType Cip => this.Cache[CipId];

        public SalesTermType Fas => this.Cache[FasId];

        public SalesTermType Fob => this.Cache[FobId];

        public SalesTermType Cfr => this.Cache[CfrId];

        public SalesTermType Cif => this.Cache[CifId];

        private UniquelyIdentifiableSticky<SalesTermType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<SalesTermType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var belgianLocale = new Locales(this.Session).DutchNetherlands;

            new SalesTermTypeBuilder(this.Session)
                .WithName("Percentage Cancellation Charge")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Percentage Cancellation Charge").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Procent annulerings toeslag").WithLocale(belgianLocale).Build())
                .WithUniqueId(PercentageCancellationChargeId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Days Cancellation Without Penalty")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Days Cancellation Without Penalty").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aantal dagen annulatie zonder kost").WithLocale(belgianLocale).Build())
                .WithUniqueId(DaysCancellationWithoutPenaltyId)
                .Build();
            
            new SalesTermTypeBuilder(this.Session)
                .WithName("Percentage Penalty Non Performance")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Percentage Penalty Non Performance").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Percentage toeslag slechte (non performance)").WithLocale(belgianLocale).Build())
                .WithUniqueId(PercentagePenaltyNonPerformanceId)
                .Build();
            
            new SalesTermTypeBuilder(this.Session)
                .WithName("Days Within Which Deliverary Must Occur")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Days Within Which Deliverary Must Occur").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Aantal dagen waarbinnen levering moet gebeuren").WithLocale(belgianLocale).Build())
                .WithUniqueId(DaysWithinWhichDeliveraryMustOccurId)
                .Build();
            
            new SalesTermTypeBuilder(this.Session)
                .WithName("Non returnable sales item")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Non returnable sales item").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Niet retourneerbaat item").WithLocale(belgianLocale).Build())
                .WithUniqueId(NonReturnableSalesItemId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Payment-net days")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Payment-net days").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Betaaltermijn").WithLocale(belgianLocale).Build())
                .WithUniqueId(PaymentNetDaysId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Penalty for late fee")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Penalty for late fee").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boete late betaling").WithLocale(belgianLocale).Build())
                .WithUniqueId(LateFeeId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Penalty for collection agency")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Penalty for collection agency").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Boete incassobureau").WithLocale(belgianLocale).Build())
                .WithUniqueId(CollectionAgencyPenaltyId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm EXW")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ex Works").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Af fabriek").WithLocale(belgianLocale).Build())
                .WithUniqueId(ExwId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm FCA")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Free Carrier").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrachtvrij tot vervoerder").WithLocale(belgianLocale).Build())
                .WithUniqueId(FcaId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm CPT")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Carriage Paid To").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrachtvrij tot").WithLocale(belgianLocale).Build())
                .WithUniqueId(CptId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm CIP")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Carriage and Insurance Paid To").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrachtvrij inclusief verzekering tot").WithLocale(belgianLocale).Build())
                .WithUniqueId(CipId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm DAT")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Delivered At Terminal").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Franco terminal").WithLocale(belgianLocale).Build())
                .WithUniqueId(DatId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm DAP")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Delivered At Place").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Franco ter plaatse").WithLocale(belgianLocale).Build())
                .WithUniqueId(DapId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm DDP")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Delivered Duty Paid").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Franco inclusief rechten").WithLocale(belgianLocale).Build())
                .WithUniqueId(DdpId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm FAS")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Free Alongside Ship").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrij langszij schip").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm FOB")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Free On Board").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrij aan boord").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm CFR")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Cost and Freight").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kostprijs en vracht").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new SalesTermTypeBuilder(this.Session)
                .WithName("Incoterm CIF")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Cost, Insurance and Freight").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kostprijs, verzekering en vracht").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();
        }
    }
}
