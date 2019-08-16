// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RateTypes.cs" company="Allors bvba">
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

    public partial class RateTypes
    {
        private static readonly Guid StandardRateId = new Guid("FE2C3012-7FBC-4c10-B76E-F0DA4754020A");
        private static readonly Guid OvertimeRateId = new Guid("DE4D0A4C-EDDC-460c-BF78-A45A9B881F48");
        private static readonly Guid WeekendRateId = new Guid("2AA92139-E634-444e-9997-89B5F598812F");
        //private static readonly Guid RegularPayId = new Guid("9C1051EA-DDAF-4e21-ACA7-8ABB9A06073D");
        //private static readonly Guid OvertimePayId = new Guid("2422BDBB-C30C-452a-B02F-ED1EE6839B56");
        //private static readonly Guid AveragePayRateId = new Guid("68D502D8-FB6F-4f63-BD33-18AF3D5D1F75");
        //private static readonly Guid HighestPayRateId = new Guid("5D09F716-5C8F-4c30-91C2-87AFB55BB371");
        //private static readonly Guid LowestPayRateId = new Guid("C7353288-278A-44a0-AF7E-9AC9B584B984");

        private UniquelyIdentifiableSticky<RateType> cache;

        public RateType StandardRate => this.Cache[StandardRateId];

        public RateType OvertimeRate => this.Cache[OvertimeRateId];

        public RateType WeekendRate => this.Cache[WeekendRateId];

        //public RateType RegularPay => this.Cache[RegularPayId];

        //public RateType OvertimePay => this.Cache[OvertimePayId];

        //public RateType AveragePayRate => this.Cache[AveragePayRateId];

        //public RateType HighestPayRate => this.Cache[HighestPayRateId];

        //public RateType LowestPayRate => this.Cache[LowestPayRateId];

        private UniquelyIdentifiableSticky<RateType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<RateType>(this.Session));

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new RateTypeBuilder(this.Session)
                .WithName("Standard Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Standaard tarief").WithLocale(dutchLocale).Build())
                .WithUniqueId(StandardRateId)
                .WithIsActive(true)
                .Build();

            new RateTypeBuilder(this.Session)
                .WithName("Overtime Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overuren tarief").WithLocale(dutchLocale).Build())
                .WithUniqueId(OvertimeRateId)
                .WithIsActive(true)
                .Build();

            new RateTypeBuilder(this.Session)
                .WithName("Weekend Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Weekend tarief").WithLocale(dutchLocale).Build())
                .WithUniqueId(WeekendRateId)
                .WithIsActive(true)
                .Build();

            //new RateTypeBuilder(this.Session)
            //    .WithName("Cost")
            //    .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kost").WithLocale(dutchLocale).Build())
            //    .WithUniqueId(CostId)
            //    .WithIsActive(true)
            //    .Build();

            //new RateTypeBuilder(this.Session)
            //    .WithName("Regular Pay")
            //    .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gewone bezoldiging").WithLocale(dutchLocale).Build())
            //    .WithUniqueId(RegularPayId)
            //    .WithIsActive(true)
            //    .Build();

            //new RateTypeBuilder(this.Session)
            //    .WithName("Overtime Pay")
            //    .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Betaling overuren").WithLocale(dutchLocale).Build())
            //    .WithUniqueId(OvertimePayId)
            //    .WithIsActive(true)
            //    .Build();

            //new RateTypeBuilder(this.Session)
            //    .WithName("Average Pay Rate")
            //    .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gemiddeld bezoldiging").WithLocale(dutchLocale).Build())
            //    .WithUniqueId(AveragePayRateId)
            //    .WithIsActive(true)
            //    .Build();

            //new RateTypeBuilder(this.Session)
            //    .WithName("Highest Pay Rate")
            //    .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoogste bezoldiging").WithLocale(dutchLocale).Build())
            //    .WithUniqueId(HighestPayRateId)
            //    .WithIsActive(true)
            //    .Build();

            //new RateTypeBuilder(this.Session)
            //    .WithName("Lowest Pay Rate")
            //    .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Laagste bezoldiging").WithLocale(dutchLocale).Build())
            //    .WithUniqueId(LowestPayRateId)
            //    .WithIsActive(true)
            //    .Build();
        }
    }
}
