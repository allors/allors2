// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RateTypes.cs" company="Allors bvba">
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

    public partial class RateTypes
    {
        public static readonly Guid BillingRateId = new Guid("FE2C3012-7FBC-4c10-B76E-F0DA4754020A");
        public static readonly Guid CostId = new Guid("48C5CDA0-1A07-4484-90FC-7AA6BF1D3FF2");
        public static readonly Guid RegularPayId = new Guid("9C1051EA-DDAF-4e21-ACA7-8ABB9A06073D");
        public static readonly Guid OvertimePayId = new Guid("2422BDBB-C30C-452a-B02F-ED1EE6839B56");
        public static readonly Guid OvertimeBillingRateId = new Guid("DE4D0A4C-EDDC-460c-BF78-A45A9B881F48");
        public static readonly Guid WeekendRateId = new Guid("2AA92139-E634-444e-9997-89B5F598812F");
        public static readonly Guid AveragePayRateId = new Guid("68D502D8-FB6F-4f63-BD33-18AF3D5D1F75");
        public static readonly Guid HighestPayRateId = new Guid("5D09F716-5C8F-4c30-91C2-87AFB55BB371");
        public static readonly Guid LowestPayRateId = new Guid("C7353288-278A-44a0-AF7E-9AC9B584B984");

        private UniquelyIdentifiableCache<RateType> cache;

        public RateType BillingRate
        {
            get { return this.Cache.Get(BillingRateId); }
        }

        public RateType Cost
        {
            get { return this.Cache.Get(CostId); }
        }

        public RateType RegularPay
        {
            get { return this.Cache.Get(RegularPayId); }
        }

        public RateType OvertimePay
        {
            get { return this.Cache.Get(OvertimePayId); }
        }

        public RateType OvertimeBillingRate
        {
            get { return this.Cache.Get(OvertimeBillingRateId); }
        }

        public RateType WeekendRate
        {
            get { return this.Cache.Get(WeekendRateId); }
        }

        public RateType AveragePayRate
        {
            get { return this.Cache.Get(AveragePayRateId); }
        }

        public RateType HighestPayRate
        {
            get { return this.Cache.Get(HighestPayRateId); }
        }

        public RateType LowestPayRate
        {
            get { return this.Cache.Get(LowestPayRateId); }
        }

        private UniquelyIdentifiableCache<RateType> Cache
        {
            get { return this.cache ?? (this.cache = new UniquelyIdentifiableCache<RateType>(this.Session)); }
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new RateTypeBuilder(this.Session)
                .WithName("Billing Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Billing Rate").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Facturatie rate").WithLocale(dutchLocale).Build())
                .WithUniqueId(BillingRateId)
                .Build();
            
            new RateTypeBuilder(this.Session)
                .WithName("Cost")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Cost").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kost").WithLocale(dutchLocale).Build())
                .WithUniqueId(CostId)
                .Build();
            
            new RateTypeBuilder(this.Session)
                .WithName("Regular Pay")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Regular Pay").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gewone bezoldiging").WithLocale(dutchLocale).Build())
                .WithUniqueId(RegularPayId)
                .Build();
            
            new RateTypeBuilder(this.Session)
                .WithName("Overtime Pay")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overtime Pay").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Betaling overuren").WithLocale(dutchLocale).Build())
                .WithUniqueId(OvertimePayId)
                .Build();
            
            new RateTypeBuilder(this.Session)
                .WithName("Overtime Billing Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overtime Billing Rate").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Overuren rate").WithLocale(dutchLocale).Build())
                .WithUniqueId(OvertimeBillingRateId)
                .Build();
            
            new RateTypeBuilder(this.Session)
                .WithName("Weekend Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Weekend Rate").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Weekend rate").WithLocale(dutchLocale).Build())
                .WithUniqueId(WeekendRateId)
                .Build();
            
            new RateTypeBuilder(this.Session)
                .WithName("Average Pay Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Average Pay Rate").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Gemiddeld bezoldiging").WithLocale(dutchLocale).Build())
                .WithUniqueId(AveragePayRateId)
                .Build();
            
            new RateTypeBuilder(this.Session)
                .WithName("Highest Pay Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Highest Pay Rate").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Hoogste bezoldiging").WithLocale(dutchLocale).Build())
                .WithUniqueId(HighestPayRateId)
                .Build();
            
            new RateTypeBuilder(this.Session)
                .WithName("Lowest Pay Rate")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Lowest Pay Rate").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Laagste bezoldiging").WithLocale(dutchLocale).Build())
                .WithUniqueId(LowestPayRateId)
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
