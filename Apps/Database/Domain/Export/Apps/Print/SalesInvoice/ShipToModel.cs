// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShipToModel.cs" company="Allors bvba">
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

namespace Allors.Domain.Print.SalesInvoiceModel
{
    public class ShipToModel
    {
        public ShipToModel(SalesInvoice invoice)
        {
            var shipTo = invoice.ShipToCustomer ?? invoice.BillToCustomer;
            var shipToOrganisation = shipTo as Organisation;

            if (shipTo != null)
            {
                this.Name = shipTo.PartyName;
                this.TaxId = shipToOrganisation?.TaxNumber;
            }

            this.Contact = invoice.ShipToContactPerson?.PartyName;

            var shipToAddress = invoice.ShipToAddress ??
                                invoice.ShipToCustomer?.ShippingAddress ??
                                invoice.ShipToCustomer?.GeneralCorrespondence ??
                                invoice.BillToContactMechanism as PostalAddress ??
                                invoice.BillToCustomer?.ShippingAddress ??
                                invoice.BillToCustomer?.GeneralCorrespondence;

            if (shipToAddress is PostalAddress postalAddress)
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

                if (postalAddress.ExistCity)
                {
                    this.City = postalAddress.City.Name;
                    this.State = postalAddress.City.State?.Name;
                }
                else if (postalAddress.ExistPostalBoundary)
                {
                    var postalBoundary = postalAddress.PostalBoundary;

                    this.City = postalBoundary.Locality;
                    this.State = postalBoundary.Region;
                    this.PostalCode = postalBoundary.PostalCode;
                    this.Country = postalBoundary.Country.Name;
                }

                if (this.PostalCode == null)
                {
                    this.PostalCode = postalAddress.PostalCode?.Code;
                }

                if (this.Country == null)
                {
                    this.Country = postalAddress.Country?.Name;
                }
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
