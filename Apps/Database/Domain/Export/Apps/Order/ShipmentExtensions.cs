// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipmentExtensions.cs" company="Allors bvba">
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
    public static partial class ShipmentExtensions
    {
        public static void AppsOnBuild(this Shipment @this, ObjectOnBuild method)
        {
            @this.AddSecurityToken(@this.Strategy.Session.GetSingleton().InitialSecurityToken);
        }

        public static void AppsOnDerive(this Shipment @this, ObjectOnDerive method)
        {
            var singleton = @this.Strategy.Session.GetSingleton();

            @this.SecurityTokens = new[]
            {
                singleton.DefaultSecurityToken
            };

            var from = @this.ShipFromParty as InternalOrganisation;
            var to = @this.ShipToParty as InternalOrganisation;

            if (@this.ExistShipFromParty && from != null)
            {
                @this.AddSecurityToken(from.LocalAdministratorSecurityToken);
            }

            if (@this.ExistShipToParty && to != null)
            {
                @this.AddSecurityToken(to.LocalAdministratorSecurityToken);
            }
        }
    }
}
