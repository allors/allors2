// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceModel.cs" company="Allors bvba">
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
                    this.BillingAddress1 = billingAddress.Address1;
                    this.BillingAddress2 = billingAddress.Address2;
                    this.BillingAddress3 = billingAddress.Address3;

                    this.BillingCity = billingAddress.City?.Name;
                    this.BillingState = billingAddress.City?.State?.Name;

                    this.BillingPostalCode = billingAddress.PostalCode?.Code;
                }

                if (customer.ShippingAddress is PostalAddress shippingAddress)
                {
                    this.ShippingAddress1 = shippingAddress.Address1;
                    this.ShippingAddress2 = shippingAddress.Address2;
                    this.ShippingAddress3 = shippingAddress.Address3;

                    this.ShippingCity = shippingAddress.City?.Name;
                    this.ShippingState = shippingAddress.City?.State?.Name;

                    this.ShippingPostalCode = shippingAddress.PostalCode?.Code;
                }
            }
        }

        public string Number { get; }
        public string Name { get; }
        public string BillingAddress1 { get; }
        public string BillingAddress2 { get; }
        public string BillingAddress3 { get; }
        public string BillingCity { get; }
        public string BillingState { get; }
        public string BillingPostalCode { get; }
        public string ShippingAddress1 { get; }
        public string ShippingAddress2 { get; }
        public string ShippingAddress3 { get; }
        public string ShippingCity { get; }
        public string ShippingState { get; }
        public string ShippingPostalCode { get; }
    }
}
