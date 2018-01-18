// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncoTermTypes.cs" company="Allors bvba">
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

    public partial class IncoTermTypes {
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
                .WithName("Incoterm EXW (Ex Works)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm EXW (Af fabriek)").WithLocale(belgianLocale).Build())
                .WithUniqueId(ExwId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm FCA (Free Carrier)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm FCA (Vrachtvrij tot vervoerder)").WithLocale(belgianLocale).Build())
                .WithUniqueId(FcaId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm CPT (Carriage Paid To)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm CPT (Vrachtvrij tot)").WithLocale(belgianLocale).Build())
                .WithUniqueId(CptId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm CIP (Carriage and Insurance Paid To)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm CIP (Vrachtvrij inclusief verzekering tot)").WithLocale(belgianLocale).Build())
                .WithUniqueId(CipId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm DAT (Delivered At Terminal)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm DAT (Franco terminal)").WithLocale(belgianLocale).Build())
                .WithUniqueId(DatId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm DAP (Delivered At Place)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm DAP (Franco ter plaatse)").WithLocale(belgianLocale).Build())
                .WithUniqueId(DapId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm DDP (Delivered Duty Paid)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm DDP (Franco inclusief rechten)").WithLocale(belgianLocale).Build())
                .WithUniqueId(DdpId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm FAS (Free Alongside Ship)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm FAS (Vrij langszij schip)").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm FOB (Free On Board)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm FOB (Vrij aan boord)").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm CFR (Cost and Freight)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm CFR (Kostprijs en vracht)").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();

            new IncoTermTypeBuilder(this.Session)
                .WithName("Incoterm CIF (Cost, Insurance and Freight)")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Incoterm CIF (Kostprijs, verzekering en vracht)").WithLocale(belgianLocale).Build())
                .WithUniqueId(CifId)
                .Build();
        }
    }
}
