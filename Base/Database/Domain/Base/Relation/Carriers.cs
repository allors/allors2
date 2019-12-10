// <copyright file="Carriers.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class Carriers
    {
        public static readonly Guid GlsId = new Guid("69C1CD1D-4D4E-4F2D-AF3C-4E1F740C7F62");
        public static readonly Guid UpsId = new Guid("74331516-5A0E-48F5-9166-75CC53E8B25D");
        public static readonly Guid FedexId = new Guid("D626E669-6AA9-40D1-B18B-5B06564C59A4");
        public static readonly Guid DhlId = new Guid("4901794C-B611-4DE4-8613-183B1C08E0AD");
        public static readonly Guid CustomerId = new Guid("647A4D28-1AAF-415A-B6AD-0CE7162625F1");

        private UniquelyIdentifiableSticky<Carrier> cache;

        public Carrier Gls => this.Cache[GlsId];

        public Carrier Ups => this.Cache[UpsId];

        public Carrier Fedex => this.Cache[FedexId];

        public Carrier Dhl => this.Cache[DhlId];

        public Carrier Customer => this.Cache[CustomerId];

        private UniquelyIdentifiableSticky<Carrier> Cache => this.cache ??= new UniquelyIdentifiableSticky<Carrier>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(GlsId, v => v.Name = "GLS");
            merge(UpsId, v => v.Name = "UPS");
            merge(FedexId, v => v.Name = "FEDEX");
            merge(DhlId, v => v.Name = "DHL");
            merge(CustomerId, v => v.Name = "Customer");
        }
    }
}
