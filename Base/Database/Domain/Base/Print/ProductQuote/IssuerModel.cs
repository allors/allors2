// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BilledFromModel.cs" company="Allors bvba">
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

namespace Allors.Domain.Print.ProductQuoteModel
{
    using System.Linq;

    public class IssuerModel
    {
        public IssuerModel(Organisation Issuer)
        {
            if (Issuer != null)
            {
                this.Name = Issuer.PartyName;
                this.Email = Issuer.GeneralEmail?.ElectronicAddressString;
                this.Website = Issuer.InternetAddress?.ElectronicAddressString;
                this.TaxId = Issuer.TaxNumber;

                var phone = Issuer.BillingInquiriesPhone ?? Issuer.GeneralPhoneNumber;
                if (phone != null)
                {
                    this.Telephone = $"{phone.CountryCode} {phone.AreaCode} {phone.ContactNumber}";
                }

                if (Issuer.GeneralCorrespondence is PostalAddress generalAddress)
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

                var bankAccount = Issuer.BankAccounts.FirstOrDefault(v => v.ExistIban);
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
        public string Email { get; }
        public string Website { get; }
        public string Bank { get; }
        public string BankAccount { get; }
        public string IBAN { get; }
        public string Swift { get; }
        public string TaxId { get; }
    }
}
