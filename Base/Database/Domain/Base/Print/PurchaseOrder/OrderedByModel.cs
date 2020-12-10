// <copyright file="OrderedByModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.PurchaseOrderModel
{
    using System.Linq;

    public class OrderedByModel
    {
        public OrderedByModel(Organisation orderedBy)
        {
            if (orderedBy != null)
            {
                this.Name = orderedBy.PartyName;
                this.Email = orderedBy.GeneralEmail?.ElectronicAddressString;
                this.Website = orderedBy.InternetAddress?.ElectronicAddressString;
                this.TaxId = orderedBy.TaxNumber;

                var phone = orderedBy.BillingInquiriesPhone ?? orderedBy.GeneralPhoneNumber;
                if (phone != null)
                {
                    this.Telephone = $"{phone.CountryCode} {phone.AreaCode} {phone.ContactNumber}";
                }

                if (orderedBy.GeneralCorrespondence is PostalAddress generalAddress)
                {
                    var address = generalAddress.Address1;
                    if (!string.IsNullOrWhiteSpace(generalAddress.Address2))
                    {
                        address += $"\n{generalAddress.Address2}";
                    }

                    if (!string.IsNullOrWhiteSpace(generalAddress.Address3))
                    {
                        address += $"\n{generalAddress.Address3}";
                    }

                    this.Address = address.Split('\n');
                    this.City = generalAddress.Locality;
                    this.State = generalAddress.Region;
                    this.PostalCode = generalAddress.PostalCode;
                    this.Country = generalAddress.Country?.Name;
                }

                var bankAccount = orderedBy.BankAccounts.FirstOrDefault(v => v.ExistIban);
                if (bankAccount != null)
                {
                    this.IBAN = bankAccount.Iban;

                    var bank = bankAccount.Bank;
                    this.Bank = bank?.Name;
                    this.Swift = bank?.SwiftCode ?? bank?.Bic;
                }
            }
        }

        public string Name { get; }

        public string[] Address { get; }

        public string City { get; }

        public string State { get; }

        public string Country { get; }

        public string PostalCode { get; }

        public string Telephone { get; }

        public string Email { get; }

        public string Website { get; }

        public string Bank { get; }

        public string BankAccount { get; }

        public string IBAN { get; }

        public string Swift { get; }

        public string TaxId { get; }
    }
}
