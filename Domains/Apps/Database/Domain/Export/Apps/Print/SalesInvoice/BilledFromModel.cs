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

namespace Allors.Domain.SalesInvoicePrint
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
                this.Telephone = $"{phone.CountryCode} {phone.AreaCode} {phone.ContactNumber}";

                if (billedFrom.GeneralCorrespondence is PostalAddress generalAddress)
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

                    if (generalAddress.ExistCity)
                    {
                        this.City = generalAddress.City.Name;
                        this.State = generalAddress.City.State?.Name;
                    }
                    else if (generalAddress.ExistPostalBoundary)
                    {
                        var postalBoundary = generalAddress.PostalBoundary;

                        this.City = postalBoundary.Locality;
                        this.State = postalBoundary.Region;
                        this.PostalCode = postalBoundary.PostalCode;
                        this.Country = postalBoundary.Country.Name;
                    }

                    if (this.PostalCode == null)
                    {
                        this.PostalCode = generalAddress.PostalCode?.Code;
                    }

                    if (this.Country == null)
                    {
                        this.Country = generalAddress.Country?.Name;
                    }
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
