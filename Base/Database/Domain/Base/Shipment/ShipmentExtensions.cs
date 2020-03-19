// <copyright file="ShipmentExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public static partial class ShipmentExtensions
    {
        public static void BaseOnDerive(this Shipment @this, ObjectOnDerive method)
        {
            var session = @this.Strategy.Session;
            @this.AddSecurityToken(new SecurityTokens(session).DefaultSecurityToken);
        }
    }
}
