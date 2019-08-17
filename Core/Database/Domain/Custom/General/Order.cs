// <copyright file="Order.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the HomeAddress type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Order
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.Order, M.Order.OrderState),
                new TransitionalConfiguration(M.Order, M.Order.ShipmentState),
                new TransitionalConfiguration(M.Order, M.Order.PaymentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void CustomOnDerive(ObjectOnDerive method)
        {
            if (this.ExistAmount && this.Amount == -1)
            {
                this.OrderState = new OrderStates(this.Strategy.Session).Cancelled;
            }
        }
    }
}
