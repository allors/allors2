// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerModel.cs" company="Allors bvba">
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

namespace Allors.Domain.WorkTaskPrint
{
    public class CustomerModel
    {
        public CustomerModel(Party customer)
        {
            if (customer != null)
            {
                this.Number = customer.Id.ToString();
                this.Name = customer.PartyName;

                if (customer.BillingAddress is PostalAddress billingAddress)
                {
                    this.BillingAddress = billingAddress.Address1;
                    if (!string.IsNullOrWhiteSpace(billingAddress.Address2))
                    {
                        this.BillingAddress = $"\n{billingAddress.Address2}";
                    }

                    if (!string.IsNullOrWhiteSpace(billingAddress.Address3))
                    {
                        this.BillingAddress = $"\n{billingAddress.Address3}";
                    }

                    this.BillingCity = billingAddress.City?.Name;
                    this.BillingState = billingAddress.City?.State?.Name;

                    this.BillingPostalCode = billingAddress.PostalCode?.Code;
                }

                if (customer.ShippingAddress is PostalAddress shippingAddress)
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

                    this.ShippingCity = shippingAddress.City?.Name;
                    this.ShippingState = shippingAddress.City?.State?.Name;

                    this.ShippingPostalCode = shippingAddress.PostalCode?.Code;
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
