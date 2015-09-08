// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankAccount.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Resources;

    public partial class BankAccount
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistOwnBankAccountsWhereBankAccount)
            {
                derivation.Log.AssertExists(this, BankAccounts.Meta.Bank);
                derivation.Log.AssertExists(this, BankAccounts.Meta.Currency);
                derivation.Log.AssertExists(this, BankAccounts.Meta.NameOnAccount);                
            }

            this.DeriveIban(derivation);
        }

        public void AppsOnDeriveIban(IDerivation derivation)
        {
            if (this.ExistIban)
            {
                var iban = Regex.Replace(this.Iban, @"\s", "").ToUpper(); // remove empty space & convert all uppercase

                if (Regex.IsMatch(iban, @"\W")) // contains chars other than (a-zA-Z0-9)
                {
                    derivation.Log.AddError(
                        this, BankAccounts.Meta.Iban, ErrorMessages.IbanIllegalCharacters);
                }

                if (!Regex.IsMatch(iban, @"^\D\D\d\d.+")) // first chars are letter letter digit digit
                {
                    derivation.Log.AddError(
                        this, BankAccounts.Meta.Iban, ErrorMessages.IbanStructuralFailure);
                }

                if (Regex.IsMatch(iban, @"^\D\D00.+|^\D\D01.+|^\D\D99.+")) // check digit are 00 or 01 or 99
                {
                    derivation.Log.AddError(
                        this, BankAccounts.Meta.Iban, ErrorMessages.IbanCheckDigitsError);
                }

                var country = new Countries(this.Strategy.Session).FindBy(Countries.Meta.IsoCode, this.Iban.Substring(0, 2));

                if (country == null || !country.ExistIbanRegex || !country.ExistIbanLength)
                {
                    derivation.Log.AddError(
                        this, BankAccounts.Meta.Iban, ErrorMessages.IbanValidationUnavailable);
                }

                if (country != null && country.ExistIbanRegex && country.ExistIbanLength)
                {
                    if (iban.Length != country.IbanLength) // fits length to country
                    {
                        derivation.Log.AddError(
                            this, BankAccounts.Meta.Iban, ErrorMessages.IbanLengthFailure);
                    }

                    if (!Regex.IsMatch(iban.Remove(0, 4), country.IbanRegex)) // check country specific structure
                    {
                        derivation.Log.AddError(
                            this, BankAccounts.Meta.Iban, ErrorMessages.IbanStructuralFailure);
                    }
                }

                if (!derivation.Log.HasErrors)
                {
                    // ******* from wikipedia.org
                    //The checksum is a basic ISO 7064 mod 97-10 calculation where the remainder must equal 1.
                    //To validate the checksum:
                    //1- Check that the total IBAN length is correct as per the country. If not, the IBAN is invalid. 
                    //2- Move the four initial characters to the end of the string. 
                    //3- Replace each letter in the string with two digits, thereby expanding the string, where A=10, B=11, ..., Z=35. 
                    //4- Interpret the string as a decimal integer and compute the remainder of that number on division by 97. 
                    //The IBAN number can only be valid if the remainder is 1.
                    var modifiedIban = iban.ToUpper().Substring(4) + iban.Substring(0, 4);
                    modifiedIban = Regex.Replace(
                        modifiedIban, @"\D", m => (m.Value[0] - 55).ToString(CultureInfo.InvariantCulture));

                    var remainer = 0;
                    while (modifiedIban.Length >= 7)
                    {
                        remainer = int.Parse(remainer + modifiedIban.Substring(0, 7)) % 97;
                        modifiedIban = modifiedIban.Substring(7);
                    }

                    remainer = int.Parse(remainer + modifiedIban) % 97;

                    if (remainer != 1)
                    {
                        derivation.Log.AddError(this, BankAccounts.Meta.Iban, ErrorMessages.IbanIncorrect);
                    }
                }
            }
        }
    }
}