// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerShipments.cs" company="Allors bvba">
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

using System;

namespace Allors.Domain
{
    public partial class CustomerShipments
    {
        public static readonly Guid CustomerShipmentTemplateEnId = new Guid("F4596683-A0D8-4487-9109-2209F5B340BF");
        public static readonly Guid CustomerShipmentTemplateNlId = new Guid("B8A79504-0973-4B20-BF7A-6159288F6B3D");

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, Domain.TemplatePurposes.Meta.ObjectType);
            setup.AddDependency(this.ObjectType, CustomerShipmentObjectStates.Meta.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new StringTemplateBuilder(this.Session)
                .WithName("CustomerShipment " + englishLocale.Name)
                .WithBody(CustomerShipmentTemplateEn)
                .WithUniqueId(CustomerShipmentTemplateEnId)
                .WithLocale(englishLocale)
                .WithTemplatePurpose(new TemplatePurposes(this.Session).CustomerShipment)
                .Build();

            new StringTemplateBuilder(this.Session)
                .WithName("CustomerShipment " + dutchLocale.Name)
                .WithBody(CustomerShipmentTemplateNl)
                .WithUniqueId(CustomerShipmentTemplateNlId)
                .WithLocale(dutchLocale)
                .WithTemplatePurpose(new TemplatePurposes(this.Session).CustomerShipment)
                .Build();
        }

        protected override void AppsSecure(Domain.Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            config.GrantOperations(this.ObjectType, full);

            config.GrantCustomer(this.ObjectType, Meta.CurrentShipmentStatus, Operation.Read);
            config.GrantCustomer(this.ObjectType, Meta.ShipToParty, Operation.Read);
            config.GrantCustomer(this.ObjectType, Meta.ShipmentStatuses, Operation.Read);
            config.GrantCustomer(this.ObjectType, Meta.ShipmentPackages, Operation.Read);

            config.GrantSales(this.ObjectType, Meta.CurrentShipmentStatus, Operation.Read);
            config.GrantSales(this.ObjectType, Meta.ShipToParty, Operation.Read);
            config.GrantSales(this.ObjectType, Meta.ShipmentStatuses, Operation.Read);
            config.GrantSales(this.ObjectType, Meta.ShipmentPackages, Operation.Read);

            var created = new CustomerShipmentObjectStates(Session).Created;
            var picked = new CustomerShipmentObjectStates(Session).Picked;
            var packed = new CustomerShipmentObjectStates(Session).Packed;
            var shipped = new CustomerShipmentObjectStates(Session).Shipped;
            var delivered = new CustomerShipmentObjectStates(Session).Delivered;
            var cancelled = new CustomerShipmentObjectStates(Session).Cancelled;
            var onHold = new CustomerShipmentObjectStates(Session).OnHold;

            var hold = Meta.Hold;
            var @continue = Meta.Continue;
            var ship = Meta.Ship;

            config.Deny(this.ObjectType, shipped, hold, @continue);
            config.Deny(this.ObjectType, onHold, ship, hold, @continue);
            config.Deny(this.ObjectType, created, hold, @continue);
            config.Deny(this.ObjectType, picked, @continue);
            config.Deny(this.ObjectType, packed, @continue);

            config.Deny(this.ObjectType, cancelled, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, shipped, Operation.Execute, Operation.Write);
            config.Deny(this.ObjectType, delivered, Operation.Execute, Operation.Write);
        }
    }
}