// <copyright file="BillToModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.SalesInvoiceModel
{
    public class BillToModel
    {
        public BillToModel(SalesInvoice invoice)
        {
            var customer = invoice.BillToCustomer ?? invoice.BillToCustomer;
            var contactPerson = invoice.BillToContactPerson ?? invoice.BillToContactPerson;
            var contactMechanisam = invoice.DerivedBillToContactMechanism ?? invoice.DerivedBillToContactMechanism;

            var billTo = customer;
            var billToOrganisation = billTo as Organisation;
            if (billTo != null)
            {
                this.Name = billTo.PartyName;
                this.TaxId = billToOrganisation?.TaxNumber;
            }

            this.Contact = contactPerson?.PartyName;

            if (contactMechanisam is PostalAddress postalAddress)
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

            if (contactMechanisam is ElectronicAddress electronicAddress)
            {
                this.Address = electronicAddress.ElectronicAddressString.Split('\n');
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
