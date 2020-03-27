// <copyright file="SerialisedItemAssignedOns.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class SerialisedItemAssignedOns
    {
        public static readonly Guid ProductQuoteSendId = new Guid("2db17a99-70ca-4fb3-b49b-dd5244217e22");
        public static readonly Guid SalesOrderConfirmId = new Guid("38bd58eb-c130-46a2-aa87-deb27fe10685");

        private UniquelyIdentifiableSticky<SerialisedItemAssignedOn> cache;

        public SerialisedItemAssignedOn ProductQuoteSend => this.Cache[ProductQuoteSendId];

        public SerialisedItemAssignedOn SalesOrderConfirm => this.Cache[SalesOrderConfirmId];

        private UniquelyIdentifiableSticky<SerialisedItemAssignedOn> Cache => this.cache ??= new UniquelyIdentifiableSticky<SerialisedItemAssignedOn>(this.Session);

        protected override void BaseSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(ProductQuoteSendId, v => v.Name = "ProductQuote Send");
            merge(SalesOrderConfirmId, v => v.Name = "SalesOrder Confirm");
        }
    }
}
