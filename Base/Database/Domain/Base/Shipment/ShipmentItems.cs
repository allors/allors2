// <copyright file="ShipmentItems.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors;
    using Allors.Meta;

    public partial class ShipmentItems
    {
        protected override void BasePrepare(Setup setup)
        {
            base.BasePrepare(setup);

            setup.AddDependency(this.ObjectType, M.ShipmentItemState);
        }

        protected override void BaseSecure(Security config)
        {
            var created = new ShipmentItemStates(this.Session).Created;
            var picked = new ShipmentItemStates(this.Session).Picked;
            var packed = new ShipmentItemStates(this.Session).Packed;
        }
    }
}
