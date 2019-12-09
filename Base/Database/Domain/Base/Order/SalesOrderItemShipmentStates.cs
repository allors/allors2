// <copyright file="SalesOrderItemShipmentStates.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SalesOrderItemShipmentStates
    {
        internal static readonly Guid NotShippedId = new Guid("4DDA233D-EE4A-4716-80C2-C2FD4307C3DC");
        internal static readonly Guid PartiallyShippedId = new Guid("E0FF4A01-CF9B-4dc7-ACF6-145F38F48AD1");
        internal static readonly Guid ShippedId = new Guid("E91BAA87-DF5F-4a6c-B380-B683AD17AE18");

        private UniquelyIdentifiableSticky<SalesOrderItemShipmentState> cache;

        public SalesOrderItemShipmentState NotShipped => this.Cache[NotShippedId];

        public SalesOrderItemShipmentState PartiallyShipped => this.Cache[PartiallyShippedId];

        public SalesOrderItemShipmentState Shipped => this.Cache[ShippedId];

        private UniquelyIdentifiableSticky<SalesOrderItemShipmentState> Cache => this.cache ??= new UniquelyIdentifiableSticky<SalesOrderItemShipmentState>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(NotShippedId, v => v.Name = "Not Shipped");
            merge(PartiallyShippedId, v => v.Name = "Partially Shipped");
            merge(ShippedId, v => v.Name = "Shipped");
        }
    }
}
