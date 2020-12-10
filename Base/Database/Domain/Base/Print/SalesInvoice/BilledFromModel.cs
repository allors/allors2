// <copyright file="BilledFromModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.SalesInvoiceModel
{
    using System.Linq;

    public class BilledFromModel
    {
        public BilledFromModel(Organisation billedFrom)
        {
            if (billedFrom != null)
            {
                this.Name = billedFrom.PartyName;
                this.Email = billedFrom.GeneralEmail?.ElectronicAddressString;
                this.Website = billedFrom.InternetAddress?.ElectronicAddressString;
                this.TaxId = billedFrom.TaxNumber;

                var phone = billedFrom.BillingInquiriesPhone ?? billedFrom.GeneralPhoneNumber;
                if (phone != null)
                {
                    this.Telephone = $"{phone.CountryCode} {phone.AreaCode} {phone.ContactNumber}";
                }

                if (billedFrom.GeneralCorrespondence is PostalAddress generalAddress)
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

                var bankAccount = billedFrom.BankAccounts.FirstOrDefault(v => v.ExistIban);
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
