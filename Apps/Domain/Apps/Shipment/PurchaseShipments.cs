// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseShipments.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    public partial class PurchaseShipments
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, PurchaseShipmentObjectStates.Meta.ObjectType);
        }

        protected override void AppsSecure(Domain.Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            config.GrantOperations(this.ObjectType, full);

            config.GrantCustomer(this.ObjectType, Meta.CurrentShipmentStatus, Operations.Read);
            config.GrantCustomer(this.ObjectType, Meta.ShipmentStatuses, Operations.Read);

            config.GrantSales(this.ObjectType, Meta.CurrentShipmentStatus, Operations.Read);
            config.GrantSales(this.ObjectType, Meta.ShipmentStatuses, Operations.Read);
        }
    }
}