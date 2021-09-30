// <copyright file="SalesOrderShipmentState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class SalesOrderShipmentState
    {
        public bool NotShipped => Equals(this.UniqueId, SalesOrderShipmentStates.NotShippedId);

        public bool PartiallyShipped => Equals(this.UniqueId, SalesOrderShipmentStates.PartiallyShippedId);

        public bool Shipped => Equals(this.UniqueId, SalesOrderShipmentStates.ShippedId);

        public bool InProgress => Equals(this.UniqueId, SalesOrderShipmentStates.InProgressId);
    }
}
