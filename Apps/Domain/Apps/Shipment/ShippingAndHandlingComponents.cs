// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShippingAndHandlingComponents.cs" company="Allors bvba">
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
    public partial class ShippingAndHandlingComponents
    {
        protected override void AppsSecure(Domain.Security config)
        {
            base.AppsSecure(config);

            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }

        private static bool AppsIsEligible(ShippingAndHandlingComponent shippingAndHandlingComponent, CustomerShipment customerShipment)
        {
            var withSpecifiedFor = false;
            var specifiedForValid = false;
            var withGeographicBoundary = false;
            var geographicBoundaryValid = false;
            var withShipmentValueBreak = false;
            var shipmentValueBreakValid = false;
            var withShipmentMethod = false;
            var shipmentMethodValid = false;
            var withCarrier = false;
            var carrierValid = false;

            if (shippingAndHandlingComponent.ExistSpecifiedFor)
            {
                withSpecifiedFor = true;

                if (customerShipment.ShipFromParty.Equals(shippingAndHandlingComponent.SpecifiedFor))
                {
                    specifiedForValid = true;
                }
            }

            if (shippingAndHandlingComponent.ExistGeographicBoundary)
            {
                withGeographicBoundary = true;

                if (customerShipment.ExistShipToAddress)
                {
                    foreach (Domain.GeographicBoundary geographicBoundary in customerShipment.ShipToAddress.GeographicBoundaries)
                    {
                        if (geographicBoundary.Equals(shippingAndHandlingComponent.GeographicBoundary))
                        {
                            geographicBoundaryValid = true;
                        }
                    }
                }
            }

            if (shippingAndHandlingComponent.ExistShipmentMethod)
            {
                withShipmentMethod = true;

                if (customerShipment.ExistShipmentMethod && customerShipment.ShipmentMethod.Equals(shippingAndHandlingComponent.ShipmentMethod))
                {
                    shipmentMethodValid = true;
                }
            }

            if (shippingAndHandlingComponent.ExistCarrier)
            {
                withCarrier = true;

                if (customerShipment.ExistCarrier && customerShipment.Carrier.Equals(shippingAndHandlingComponent.Carrier))
                {
                    carrierValid = true;
                }
            }

            if (shippingAndHandlingComponent.ExistShipmentValue)
            {
                withShipmentValueBreak = true;

                if ((!shippingAndHandlingComponent.ShipmentValue.ExistFromAmount || shippingAndHandlingComponent.ShipmentValue.FromAmount <= customerShipment.ShipmentValue) &&
                    (!shippingAndHandlingComponent.ShipmentValue.ExistThroughAmount || shippingAndHandlingComponent.ShipmentValue.ThroughAmount >= customerShipment.ShipmentValue))
                {
                    shipmentValueBreakValid = true;
                }
            }

            if ((withGeographicBoundary && !geographicBoundaryValid) ||
                (withSpecifiedFor && !specifiedForValid) ||
                (withShipmentMethod && !shipmentMethodValid) ||
                (withCarrier && !carrierValid) ||
                (withShipmentValueBreak && !shipmentValueBreakValid))
            {
                return false;
            }

            return true;
        }
    }
}