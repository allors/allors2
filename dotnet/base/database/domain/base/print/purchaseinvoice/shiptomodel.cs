// <copyright file="ShipToModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.PurchaseInvoiceModel
{
    public class ShipToModel
    {
        public ShipToModel(PurchaseInvoice invoice)
        {
            var shipTo = invoice.BilledTo;
            var shipToOrganisation = shipTo as Organisation;

            if (shipTo != null)
            {
                this.Name = shipTo.PartyName;
                this.TaxId = shipToOrganisation?.TaxNumber;
            }

            this.Contact = invoice.BilledToContactPerson?.PartyName;

            var shipToAddress = invoice.DerivedShipToCustomerAddress ??
                                invoice.BilledTo?.ShippingAddress ??
                                invoice.BilledTo?.GeneralCorrespondence;

            if (shipToAddress is PostalAddress postalAddress)
            {
                var address = postalAddress.Address1;
                if (!string.IsNullOrWhiteSpace(postalAddress.Address2))
                {
                    address += $"\n{postalAddress.Address2}";
                }

                if (!string.IsNullOrWhiteSpace(postalAddress.Address3))
                {
                    address += $"\n{postalAddress.Address3}";
                }

                this.Address = address.Split('\n');
                this.City = postalAddress.Locality;
                this.State = postalAddress.Region;
                this.PostalCode = postalAddress.PostalCode;
                this.Country = postalAddress.Country?.Name;
            }
        }

        public string Name { get; }

        public string[] Address { get; }

        public string City { get; }

        public string State { get; }

        public string Country { get; }

        public string PostalCode { get; }

        public string TaxId { get; }

        public string Contact { get; }
    }
}
