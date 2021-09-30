// <copyright file="VatRates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class VatRates
    {
        public static readonly Guid ZeroRated0Id = new Guid("0D1BB08D-E2C3-4417-8587-EB5738A5FBBF");
        public static readonly Guid Exempt0Id = new Guid("61e17ad1-8457-4eb9-a8b9-6696065a5dbe");
        public static readonly Guid Intracommunity0Id = new Guid("875bd768-375a-4a8f-8494-aeb5d6843426");
        public static readonly Guid Spain4Id = new Guid("3c240520-2549-46ae-ab8b-3be525be6bf5");
        public static readonly Guid Spain7Id = new Guid("0e2fe4dd-d7e8-43e8-8b30-1fc15cd5e675");
        public static readonly Guid Spain10Id = new Guid("dba36f12-db21-40a6-93ec-7e9d85561459");
        public static readonly Guid Spain21Id = new Guid("528ffde3-5740-46d8-ab0f-03c78626fbcd");
        public static readonly Guid Dutch21Id = new Guid("7CDDE391-1BB5-4329-A224-E7C26E1EE73E");
        public static readonly Guid Dutch9Id = new Guid("5276f5bb-c2fb-4d96-a8b3-e0d8caffa91b");
        public static readonly Guid Belgium6Id = new Guid("9D70146B-A18E-4A69-A134-0619AAB9FE52");
        public static readonly Guid Belgium12Id = new Guid("2D5E377F-A78C-4F38-8249-5A0F46F5DDAB");
        public static readonly Guid Belgium21Id = new Guid("220dbc32-1cf8-4b29-8585-784e55c1abc5");
        public static readonly Guid BelgiumServiceB2B0Id = new Guid("7e865270-fd83-4eb0-81da-aaca6d7c2d3a");

        private UniquelyIdentifiableSticky<VatRate> cache;

        public VatRate ZeroRated0 => this.Cache[ZeroRated0Id];

        public VatRate Exempt0 => this.Cache[Exempt0Id];

        public VatRate Intracommunity0 => this.Cache[Intracommunity0Id];

        public VatRate Dutch9 => this.Cache[Dutch9Id];

        public VatRate Dutch21 => this.Cache[Dutch21Id];

        public VatRate Spain4 => this.Cache[Spain4Id];

        public VatRate Spain7 => this.Cache[Spain7Id];

        public VatRate Spain10 => this.Cache[Spain10Id];

        public VatRate Spain21 => this.Cache[Spain21Id];

        public VatRate Belgium6 => this.Cache[Belgium6Id];

        public VatRate Belgium12 => this.Cache[Belgium12Id];

        public VatRate Belgium21 => this.Cache[Belgium21Id];

        public VatRate BelgiumServiceB2B0 => this.Cache[BelgiumServiceB2B0Id];

        private UniquelyIdentifiableSticky<VatRate> Cache => this.cache ??= new UniquelyIdentifiableSticky<VatRate>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(ZeroRated0Id, v =>
            {
                v.FromDate = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 0;
            });

            merge(Exempt0Id, v =>
            {
                v.FromDate = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 0;
            });

            merge(Intracommunity0Id, v =>
            {
                v.FromDate = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 0;
            });

            merge(Spain4Id, v =>
            {
                v.FromDate = new DateTime(2012, 09, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 4;
            });

            merge(Spain7Id, v =>
            {
                v.FromDate = new DateTime(2020, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 7;
            });

            merge(Spain10Id, v =>
            {
                v.FromDate = new DateTime(2012, 09, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 10;
            });

            merge(Spain21Id, v =>
            {
                v.FromDate = new DateTime(2012, 09, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 21;
            });

            merge(Dutch21Id, v =>
            {
                v.FromDate = new DateTime(2012, 10, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 21;
            });

            merge(Dutch9Id, v =>
            {
                v.FromDate = new DateTime(2019, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 9;
            });

            merge(Belgium6Id, v =>
            {
                v.FromDate = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 6;
            });

            merge(Belgium12Id, v =>
            {
                v.FromDate = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 12;
            });

            merge(Belgium21Id, v =>
            {
                v.FromDate = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 21;
            });

            merge(BelgiumServiceB2B0Id, v =>
            {
                v.FromDate = new DateTime(2000, 01, 01, 0, 0, 0, DateTimeKind.Utc);
                v.Rate = 0;
            });
        }
    }
}
