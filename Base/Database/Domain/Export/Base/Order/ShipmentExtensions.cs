// <copyright file="ShipmentExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public static partial class ShipmentExtensions
    {
        public static void BaseOnBuild(this Shipment @this, ObjectOnBuild method) => @this.AddSecurityToken(@this.Strategy.Session.GetSingleton().InitialSecurityToken);

        public static void BaseOnDerive(this Shipment @this, ObjectOnDerive method)
        {
            var singleton = @this.Strategy.Session.GetSingleton();

            @this.SecurityTokens = new[]
            {
                singleton.DefaultSecurityToken,
            };

            if (@this.ExistShipFromParty && @this.ShipFromParty is InternalOrganisation from)
            {
                @this.AddSecurityToken(from.LocalAdministratorSecurityToken);
            }

            if (@this.ExistShipToParty && @this.ShipToParty is InternalOrganisation to)
            {
                @this.AddSecurityToken(to.LocalAdministratorSecurityToken);
            }
        }
    }
}
