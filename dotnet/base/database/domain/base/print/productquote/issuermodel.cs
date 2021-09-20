// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IssuerModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Collections.Generic;
    using System.Linq;

    public class IssuerModel
    {
        public IssuerModel(Quote quote, Dictionary<string, byte[]> imageByImageName)
        {
            if (quote.Issuer is Organisation issuer)
            {
                this.Name = issuer.PartyName;
                this.Email = issuer.GeneralEmail?.ElectronicAddressString;
                this.Website = issuer.InternetAddress?.ElectronicAddressString;
                this.TaxId = issuer.TaxNumber;

                var phoneNumbers = issuer?.CurrentPartyContactMechanisms.Where(v => v.ContactMechanism.GetType().Name == typeof(TelecommunicationsNumber).Name).Select(v => v.ContactMechanism).ToArray();
                if (phoneNumbers.Length > 0)
                {
                    this.Telephone = phoneNumbers[0].ToString();
                }

                if (phoneNumbers.Length > 1)
                {
                    this.Telephone2 = phoneNumbers[1].ToString();
                }

                if (issuer.GeneralCorrespondence is PostalAddress generalAddress)
                {
                    this.Address = generalAddress.Address1;
                    if (!string.IsNullOrWhiteSpace(generalAddress.Address2))
                    {
                        this.Address = $"\n{generalAddress.Address2}";
                    }

                    if (!string.IsNullOrWhiteSpace(generalAddress.Address3))
                    {
                        this.Address = $"\n{generalAddress.Address3}";
                    }

                    this.City = generalAddress.Locality;
                    this.State = generalAddress.Region;
                    this.PostalCode = generalAddress.PostalCode;
                    this.Country = generalAddress.Country?.Name;
                }

                var bankAccount = issuer.BankAccounts.FirstOrDefault(v => v.ExistIban);
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

        public string Address { get; }

        public string City { get; }

        public string State { get; }

        public string Country { get; }

        public string PostalCode { get; }

        public string Telephone { get; }

        public string Telephone2 { get; }

        public string Email { get; }

        public string Website { get; }

        public string Bank { get; }

        public string BankAccount { get; }

        public string IBAN { get; }

        public string Swift { get; }

        public string TaxId { get; }
    }
}
