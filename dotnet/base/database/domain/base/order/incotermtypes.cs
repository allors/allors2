// <copyright file="IncoTermTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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

        private UniquelyIdentifiableSticky<IncoTermType> Cache => this.cache ??= new UniquelyIdentifiableSticky<IncoTermType>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            var merge = this.Cache.Merger().Action();
            var localisedName = new LocalisedTextAccessor(this.Meta.LocalisedNames);

            merge(ExwId, v =>
            {
                v.Name = "Incoterm EXW (Ex Works)";
                localisedName.Set(v, dutchLocale, "Incoterm EXW (Af fabriek)");
                v.IsActive = true;
            });

            merge(FcaId, v =>
            {
                v.Name = "Incoterm FCA (Free Carrier)";
                localisedName.Set(v, dutchLocale, "Incoterm FCA (Vrachtvrij tot vervoerder)");
                v.IsActive = true;
            });

            merge(CptId, v =>
            {
                v.Name = "Incoterm CPT (Carriage Paid To)";
                localisedName.Set(v, dutchLocale, "Incoterm CPT (Vrachtvrij tot)");
                v.IsActive = true;
            });

            merge(CipId, v =>
            {
                v.Name = "Incoterm CIP (Carriage and Insurance Paid To))";
                localisedName.Set(v, dutchLocale, "Incoterm CIP (Vrachtvrij inclusief verzekering tot)");
                v.IsActive = true;
            });

            merge(DatId, v =>
            {
                v.Name = "Incoterm DAT (Delivered At Terminal)";
                localisedName.Set(v, dutchLocale, "Incoterm DAT (Franco terminal)");
                v.IsActive = true;
            });

            merge(DapId, v =>
            {
                v.Name = "Incoterm DAP (Delivered At Place)";
                localisedName.Set(v, dutchLocale, "Incoterm DAP (Franco ter plaatse)");
                v.IsActive = true;
            });

            merge(DdpId, v =>
            {
                v.Name = "Incoterm DDP (Delivered Duty Paid)";
                localisedName.Set(v, dutchLocale, "Incoterm DDP (Franco inclusief rechten)");
                v.IsActive = true;
            });

            merge(FasId, v =>
            {
                v.Name = "Incoterm FAS (Free Alongside Ship)";
                localisedName.Set(v, dutchLocale, "Incoterm FAS (Vrij langszij schip)");
                v.IsActive = true;
            });

            merge(FobId, v =>
            {
                v.Name = "Incoterm FOB (Free On Board)";
                localisedName.Set(v, dutchLocale, "Incoterm FOB (Vrij aan boord)");
                v.IsActive = true;
            });

            merge(CfrId, v =>
            {
                v.Name = "Incoterm CFR (Cost and Freight)";
                localisedName.Set(v, dutchLocale, "Incoterm CFR (Kostprijs en vracht)");
                v.IsActive = true;
            });

            merge(CifId, v =>
            {
                v.Name = "Incoterm CIF (Cost, Insurance and Freight)";
                localisedName.Set(v, dutchLocale, "Incoterm CIF (Kostprijs, verzekering en vracht)");
                v.IsActive = true;
            });
        }
    }
}
