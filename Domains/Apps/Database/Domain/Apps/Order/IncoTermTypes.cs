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

    public partial class IncoTermTypes
    {
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

        private UniquelyIdentifiableSticky<IncoTermType> cache;

        public IncoTermType Exw => this.Cache[ExwId];

        public IncoTermType Fca => this.Cache[FcaId];

        public IncoTermType Cpt => this.Cache[CptId];

        public IncoTermType Dat => this.Cache[DatId];

        public IncoTermType Dap => this.Cache[DapId];

        public IncoTermType Ddp => this.Cache[DdpId];

        public IncoTermType Cip => this.Cache[CipId];

        public IncoTermType Fas => this.Cache[FasId];

        public IncoTermType Fob => this.Cache[FobId];

        public IncoTermType Cfr => this.Cache[CfrId];

        public IncoTermType Cif => this.Cache[CifId];

        private UniquelyIdentifiableSticky<IncoTermType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<IncoTermType>(this.Session));

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var belgianLocale = new Locales(this.Session).DutchNetherlands;

            new IncoTermTypeBuilder(this.Session)
                .WithName("EXW")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Ex Works").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Af fabriek").WithLocale(belgianLocale).Build())
                .WithUniqueId(ExwId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("FCA")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Free Carrier").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrachtvrij tot vervoerder").WithLocale(belgianLocale).Build())
                .WithUniqueId(FcaId)
                .Build();
            
            new IncoTermTypeBuilder(this.Session)
                .WithName("CPT")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Carriage Paid To").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrachtvrij tot").WithLocale(belgianLocale).Build())
                .WithUniqueId(CptId)
                .Build();
            
            new IncoTermTypeBuilder(this.Session)
                .WithName("CIP")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Carriage and Insurance Paid To").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrachtvrij inclusief verzekering tot").WithLocale(belgianLocale).Build())
                .WithUniqueId(CipId)
                .Build();
            
            new IncoTermTypeBuilder(this.Session)
                .WithName("DAT")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Delivered At Terminal").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Franco terminal").WithLocale(belgianLocale).Build())
                .WithUniqueId(DatId)
                .Build();
            
            new IncoTermTypeBuilder(this.Session)
                .WithName("DAP")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Delivered At Place").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Franco ter plaatse").WithLocale(belgianLocale).Build())
                .WithUniqueId(DapId)
                .Build();
            
            new IncoTermTypeBuilder(this.Session)
                .WithName("DDP")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Delivered Duty Paid").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Franco inclusief rechten").WithLocale(belgianLocale).Build())
                .WithUniqueId(DdpId)
                .Build();
            
            new IncoTermTypeBuilder(this.Session)
                .WithName("FAS")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Free Alongside Ship").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrij langszij schip").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("FOB")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Free On Board").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Vrij aan boord").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("CFR")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Cost and Freight").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kostprijs en vracht").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("CIF")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Cost, Insurance and Freight").WithLocale(englishLocale).Build())
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Kostprijs, verzekering en vracht").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();
        }
    }
}
