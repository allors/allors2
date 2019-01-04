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

namespace Allors.Domain.SalesInvoicePrint
{
    public class ShipToModel
    {
        public ShipToModel(SalesInvoice salesInvoice)
        {
            var shipTo = salesInvoice.ShipToCustomer ?? salesInvoice.BillToCustomer;
            var shipToOrganisation = shipTo as Organisation;

            if (shipTo != null)
            {
                this.Name = shipTo.PartyName;
                this.TaxId = shipToOrganisation?.TaxNumber;
            }

            var shipToAddress = salesInvoice.ShipToAddress ??
                                salesInvoice.ShipToCustomer?.ShippingAddress ??
                                salesInvoice.ShipToCustomer?.GeneralCorrespondence ??
                                salesInvoice.BillToContactMechanism as PostalAddress ??
                                salesInvoice.BillToCustomer?.ShippingAddress ??
                                salesInvoice.BillToCustomer?.GeneralCorrespondence;

            if (shipToAddress != null)
            {
                this.Address = shipToAddress.Address1;
                if (!string.IsNullOrWhiteSpace(shipToAddress.Address2))
                {
                    this.Address = $"\n{shipToAddress.Address2}";
                }

                if (!string.IsNullOrWhiteSpace(shipToAddress.Address3))
                {
                    this.Address = $"\n{shipToAddress.Address3}";
                }

                if (shipToAddress.ExistCity)
                {
                    this.City = shipToAddress.City.Name;
                    this.State = shipToAddress.City.State?.Name;
                }
                else if (shipToAddress.ExistPostalBoundary)
                {
                    var postalBoundary = shipToAddress.PostalBoundary;

                    this.City = postalBoundary.Locality;
                    this.State = postalBoundary.Region;
                    this.PostalCode = postalBoundary.PostalCode;
                    this.Country = postalBoundary.Country.Name;
                }

                if (this.PostalCode == null)
                {
                    this.PostalCode = shipToAddress.PostalCode?.Code;
                }

                if (this.Country == null)
                {
                    this.Country = shipToAddress.Country?.Name;
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
    }
}
