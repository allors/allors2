// <copyright file="VatRates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class VatRates
    {
        public static readonly Guid ZeroId = new Guid("0D1BB08D-E2C3-4417-8587-EB5738A5FBBF");
        public static readonly Guid SixId = new Guid("9D70146B-A18E-4A69-A134-0619AAB9FE52");
        public static readonly Guid TenId = new Guid("dba36f12-db21-40a6-93ec-7e9d85561459");
        public static readonly Guid TwelveId = new Guid("2D5E377F-A78C-4F38-8249-5A0F46F5DDAB");
        public static readonly Guid TwentyOneId = new Guid("7CDDE391-1BB5-4329-A224-E7C26E1EE73E");

        private UniquelyIdentifiableSticky<VatRate> cache;

        public VatRate Zero => this.Cache[ZeroId];

        public VatRate Ten => this.Cache[TenId];

        public VatRate Six => this.Cache[SixId];

        public VatRate Twelve => this.Cache[TwelveId];

        public VatRate TwentyOne => this.Cache[TwentyOneId];

        private UniquelyIdentifiableSticky<VatRate> Cache => this.cache ??= new UniquelyIdentifiableSticky<VatRate>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(ZeroId, v => v.Rate = 0);
            merge(SixId, v => v.Rate = 6);
            merge(TenId, v => v.Rate = 10);
            merge(TwelveId, v => v.Rate = 12);
            merge(TwentyOneId, v => v.Rate = 21);
        }
    }
}
