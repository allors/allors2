// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerShipments.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Meta;

    public partial class CustomerShipments
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.CustomerShipmentState);
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);

            var created = new CustomerShipmentStates(this.Session).Created;
            var picked = new CustomerShipmentStates(this.Session).Picked;
            var packed = new CustomerShipmentStates(this.Session).Packed;
            var shipped = new CustomerShipmentStates(this.Session).Shipped;
            var delivered = new CustomerShipmentStates(this.Session).Delivered;
            var cancelled = new CustomerShipmentStates(this.Session).Cancelled;
            var onHold = new CustomerShipmentStates(this.Session).OnHold;

            var hold = this.Meta.Hold;
            var @continue = this.Meta.Continue;
            var ship = this.Meta.Ship;

            config.Deny(this.ObjectType, shipped, hold, @continue);
            config.Deny(this.ObjectType, onHold, ship, hold, @continue);
            config.Deny(this.ObjectType, created, hold, @continue);
            config.Deny(this.ObjectType, picked, @continue);
            config.Deny(this.ObjectType, packed, @continue);

            config.Deny(this.ObjectType, cancelled, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, shipped, Operations.Execute, Operations.Write);
            config.Deny(this.ObjectType, delivered, Operations.Execute, Operations.Write);
        }
    }
}