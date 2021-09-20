// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BillToModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Collections.Generic;

    public class BillToModel
    {
        public BillToModel(ProductQuote quote, Dictionary<string, byte[]> imageByImageName)
        {
            var contactMechanism = quote.FullfillContactMechanism;

            if (contactMechanism is PostalAddress postalAddress)
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

            if (contactMechanism is ElectronicAddress electronicAddress)
            {
                this.Address = electronicAddress.ElectronicAddressString;
            }
        }

        public string Address { get; }

        public string City { get; }

        public string State { get; }

        public string Country { get; }

        public string PostalCode { get; }
    }
}
