
// <copyright file="TakenViaModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.PurchaseOrderModel
{
    public class TakenViaModel
    {
        public TakenViaModel(PurchaseOrder order)
        {
            var supplier = order.TakenViaSupplier;
            var contactPerson = order.BillToContactPerson;
            var contactMechanisam = order.BillToContactMechanism;

            var billTo = supplier;
            var billToOrganisation = billTo as Organisation;
            if (billTo != null)
            {
                this.Name = billTo.PartyName;
                this.TaxId = billToOrganisation?.TaxNumber;
            }

            this.Contact = contactPerson?.PartyName;

            if (contactMechanisam is PostalAddress postalAddress)
            {
                this.Address = postalAddress.Address1;
                if (!string.IsNullOrWhiteSpace(postalAddress.Address2))
                {
                    this.Address = $"\n{postalAddress.Address2}";
                }

                if (!string.IsNullOrWhiteSpace(postalAddress.Address3))
                {
                    this.Address = $"\n{postalAddress.Address3}";
                }

                this.City = postalAddress.Locality;
                this.State = postalAddress.Region;
                this.PostalCode = postalAddress.PostalCode;
                this.Country = postalAddress.Country?.Name;
            }

            if (contactMechanisam is ElectronicAddress electronicAddress)
            {
                this.Address = electronicAddress.ElectronicAddressString;
            }
        }

        public string Name { get; }

        public string Address { get; }

        public string City { get; }

        public string State { get; }

        public string Country { get; }

        public string PostalCode { get; }

        public string TaxId { get; }

        public string Contact { get; }
    }
}
