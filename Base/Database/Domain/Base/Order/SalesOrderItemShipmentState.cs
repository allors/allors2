// <copyright file="SalesOrderItemShipmentState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesOrderItemShipmentState
    {
        public bool NotShipped => Equals(this.UniqueId, SalesOrderItemShipmentStates.NotShippedId);

        public bool PartiallyShipped => Equals(this.UniqueId, SalesOrderItemShipmentStates.PartiallyShippedId);

        public bool Shipped => Equals(this.UniqueId, SalesOrderItemShipmentStates.ShippedId);

        public bool InProgress => Equals(this.UniqueId, SalesOrderItemShipmentStates.InProgressId);
    }
}
