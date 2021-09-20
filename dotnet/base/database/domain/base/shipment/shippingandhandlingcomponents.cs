// <copyright file="ShippingAndHandlingComponents.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ShippingAndHandlingComponents
    {
        public static bool BaseIsEligible(ShippingAndHandlingComponent shippingAndHandlingComponent, CustomerShipment customerShipment)
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

            // TODO: ???
            // if (shippingAndHandlingComponent.ExistSpecifiedFor)
            // {
            //    withSpecifiedFor = true;

            // if (customerShipment.ShipFromParty.Equals(shippingAndHandlingComponent.SpecifiedFor))
            //    {
            //        specifiedForValid = true;
            //    }
            // }
            if (shippingAndHandlingComponent.ExistGeographicBoundary)
            {
                withGeographicBoundary = true;

                if (customerShipment.ExistShipToAddress)
                {
                    foreach (GeographicBoundary geographicBoundary in customerShipment.ShipToAddress.PostalAddressBoundaries)
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
