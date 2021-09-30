// <copyright file="CustomerReturn.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class CustomerReturn
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.CustomerReturn, M.CustomerReturn.ShipmentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistShipmentState)
            {
                this.ShipmentState = new ShipmentStates(this.Strategy.Session).Received;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistShipToAddress && this.ExistShipToParty)
            {
                this.ShipToAddress = this.ShipToParty.ShippingAddress;
            }

            if (!this.ExistShipFromAddress && this.ExistShipFromParty)
            {
                this.ShipFromAddress = this.ShipFromParty.ShippingAddress;
            }

            this.Sync(this.Session());
        }

        private void Sync(ISession session)
        {
            // session.Prefetch(this.SyncPrefetch, this);
            foreach (ShipmentItem shipmentItem in this.ShipmentItems)
            {
                shipmentItem.Sync(this);
            }
        }
    }
}
