// <copyright file="CustomerModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    public class CustomerModel
    {
        public CustomerModel(Party customer)
        {
            if (customer != null)
            {
                this.Number = customer.Id.ToString();
                this.Name = customer.PartyName;

                if (customer.BillingAddress is PostalAddress postalBillingAddress)
                {
                    this.BillingAddress = postalBillingAddress.Address1;
                    if (!string.IsNullOrWhiteSpace(postalBillingAddress.Address2))
                    {
                        this.BillingAddress = $"\n{postalBillingAddress.Address2}";
                    }

                    if (!string.IsNullOrWhiteSpace(postalBillingAddress.Address3))
                    {
                        this.BillingAddress = $"\n{postalBillingAddress.Address3}";
                    }

                    this.BillingCity = postalBillingAddress.Locality;
                    this.BillingState = postalBillingAddress.Region;
                    this.BillingPostalCode = postalBillingAddress.PostalCode;
                }
                else
                {
                    this.BillingAddress = customer.BillingAddress?.ToString();
                }

                var shippingAddress = customer.ShippingAddress;
                if (shippingAddress != null)
                {
                    this.ShippingAddress = shippingAddress.Address1;
                    if (!string.IsNullOrWhiteSpace(shippingAddress.Address2))
                    {
                        this.ShippingAddress = $"\n{shippingAddress.Address2}";
                    }

                    if (!string.IsNullOrWhiteSpace(shippingAddress.Address3))
                    {
                        this.ShippingAddress = $"\n{shippingAddress.Address3}";
                    }

                    this.ShippingCity = shippingAddress.Locality;
                    this.ShippingState = shippingAddress.Region;
                    this.ShippingPostalCode = shippingAddress.PostalCode;
                }
            }
        }

        public string Number { get; }

        public string Name { get; }

        public string BillingAddress { get; }

        public string BillingCity { get; }

        public string BillingState { get; }

        public string BillingPostalCode { get; }

        public string ShippingAddress { get; }

        public string ShippingCity { get; }

        public string ShippingState { get; }

        public string ShippingPostalCode { get; }
    }
}
