// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesInvoice.cs" company="Allors bvba">
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

namespace Allors.Domain.Print
{
    class SalesInvoiceBillFromPrint
    {
        public string Number;
        public string Name;
        public string Address1;
        public string Address2;
        public string Address3;
        public string City;
        public string State;
        public string Country;
        public string PostalCode;
        public string Telephone;
        public string Email;
        public string Website;
        public string Bank;
        public string BankAccount;
        public string IBAN;
        public string Swift;
        public string TaxId;

        public SalesInvoiceBillFromPrint(InternalOrganisation billFrom)
        {
            this.Number = billFrom.Id.ToString();
            this.Name = billFrom.PartyName;
            this.Telephone = billFrom.OrderInquiriesPhone?.Description ?? billFrom.GeneralPhoneNumber?.Description;
            this.Email = billFrom.GeneralEmail?.Description;
            this.Website = billFrom.InternetAddress?.ElectronicAddressString;

            var organisation = (Organisation)billFrom;
            this.TaxId = organisation.TaxNumber;

            if (billFrom.GeneralCorrespondence is PostalAddress generalAddress)
            {
                this.Address1 = generalAddress.Address1;
                this.Address2 = generalAddress.Address2;
                this.Address3 = generalAddress.Address3;

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

            if (billFrom.ExistBankAccounts)
            {
                var bankAccount = billFrom.BankAccounts.First;
                var bank = bankAccount.Bank;

                this.Bank = bank?.Name;
                this.IBAN = bankAccount.Iban;
                this.Swift = bank?.SwiftCode ?? bank?.Bic;
            }
        }
    }
}
