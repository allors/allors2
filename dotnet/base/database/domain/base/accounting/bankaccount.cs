// <copyright file="BankAccount.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Allors.Meta;

    using Resources;

    public partial class BankAccount
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistOwnBankAccountsWhereBankAccount)
            {
                derivation.Validation.AssertExists(this, this.Meta.Bank);
                derivation.Validation.AssertExists(this, this.Meta.Currency);
                derivation.Validation.AssertExists(this, this.Meta.NameOnAccount);
            }

            this.DeriveIban(derivation);
        }

        private void DeriveIban(IDerivation derivation)
        {
            if (!string.IsNullOrEmpty(this.Iban))
            {
                var iban = Regex.Replace(this.Iban, @"\s", string.Empty).ToUpper(); // remove empty space & convert all uppercase

                if (Regex.IsMatch(iban, @"\W"))
                {
                    // contains chars other than (a-zA-Z0-9)
                    derivation.Validation.AddError(this, this.Meta.Iban, ErrorMessages.IbanIllegalCharacters);
                }

                if (!Regex.IsMatch(iban, @"^\D\D\d\d.+"))
                {
                    // first chars are letter letter digit digit
                    derivation.Validation.AddError(this, this.Meta.Iban, ErrorMessages.IbanStructuralFailure);
                }

                if (Regex.IsMatch(iban, @"^\D\D00.+|^\D\D01.+|^\D\D99.+"))
                {
                    // check digit are 00 or 01 or 99
                    derivation.Validation.AddError(this, this.Meta.Iban, ErrorMessages.IbanCheckDigitsError);
                }

                var country = new Countries(this.Strategy.Session).FindBy(M.Country.IsoCode, this.Iban.Substring(0, 2));

                if (country == null || !country.ExistIbanRegex || !country.ExistIbanLength)
                {
                    derivation.Validation.AddError(this, this.Meta.Iban, ErrorMessages.IbanValidationUnavailable);
                }

                if (country != null && country.ExistIbanRegex && country.ExistIbanLength)
                {
                    if (iban.Length != country.IbanLength)
                    {
                        // fits length to country
                        derivation.Validation.AddError(this, this.Meta.Iban, ErrorMessages.IbanLengthFailure);
                    }

                    if (!Regex.IsMatch(iban.Remove(0, 4), country.IbanRegex))
                    {
                        // check country specific structure
                        derivation.Validation.AddError(this, this.Meta.Iban, ErrorMessages.IbanStructuralFailure);
                    }
                }

                if (!derivation.Validation.HasErrors)
                {
                    // ******* from wikipedia.org
                    // The checksum is a basic ISO 7064 mod 97-10 calculation where the remainder must equal 1.
                    // To validate the checksum:
                    // 1- Check that the total IBAN length is correct as per the country. If not, the IBAN is invalid.
                    // 2- Move the four initial characters to the end of the string.
                    // 3- Replace each letter in the string with two digits, thereby expanding the string, where A=10, B=11, ..., Z=35.
                    // 4- Interpret the string as a decimal integer and compute the remainder of that number on division by 97.
                    // The IBAN number can only be valid if the remainder is 1.
                    var modifiedIban = iban.ToUpper().Substring(4) + iban.Substring(0, 4);
                    modifiedIban = Regex.Replace(modifiedIban, @"\D", m => (m.Value[0] - 55).ToString(CultureInfo.InvariantCulture));

                    var remainer = 0;
                    while (modifiedIban.Length >= 7)
                    {
                        remainer = int.Parse(remainer + modifiedIban.Substring(0, 7)) % 97;
                        modifiedIban = modifiedIban.Substring(7);
                    }

                    remainer = int.Parse(remainer + modifiedIban) % 97;

                    if (remainer != 1)
                    {
                        derivation.Validation.AddError(this, this.Meta.Iban, ErrorMessages.IbanIncorrect);
                    }
                }
            }
        }
    }
}
