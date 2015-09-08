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
        public static readonly Guid PurchaseShipmentTemplateEnId = new Guid("AF818B3F-E621-4391-A4A4-8B381223651D");
        public static readonly Guid PurchaseShipmentTemplateNlId = new Guid("E4EDB920-8577-43C6-A6CD-1635A700C439");

        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, Domain.TemplatePurposes.Meta.ObjectType);
            setup.AddDependency(this.ObjectType, PurchaseShipmentObjectStates.Meta.ObjectType);
        }

        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            var englishLocale = new Locales(this.Session).EnglishGreatBritain;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new StringTemplateBuilder(this.Session)
                .WithName("PurchaseShipment " + englishLocale.Name)
                .WithBody(PurchaseShipmentTemplateEn)
                .WithUniqueId(PurchaseShipmentTemplateEnId)
                .WithLocale(englishLocale)
                .WithTemplatePurpose(new Domain.TemplatePurposes(this.Session).PurchaseShipment)
                .Build();

            new StringTemplateBuilder(this.Session)
                .WithName("PurchaseShipment " + dutchLocale.Name)
                .WithBody(PurchaseShipmentTemplateNl)
                .WithUniqueId(PurchaseShipmentTemplateNlId)
                .WithLocale(dutchLocale)
                .WithTemplatePurpose(new Domain.TemplatePurposes(this.Session).PurchaseShipment)
                .Build();
        }

        protected override void AppsSecure(Domain.Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);

            config.GrantOperations(this.ObjectType, full);

            config.GrantCustomer(this.ObjectType, Meta.CurrentShipmentStatus, Operation.Read);
            config.GrantCustomer(this.ObjectType, Meta.ShipmentStatuses, Operation.Read);

            config.GrantSales(this.ObjectType, Meta.CurrentShipmentStatus, Operation.Read);
            config.GrantSales(this.ObjectType, Meta.ShipmentStatuses, Operation.Read);
        }
    }
}