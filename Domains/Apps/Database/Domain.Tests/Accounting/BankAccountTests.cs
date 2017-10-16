//------------------------------------------------------------------------------------------------- 
// <copyright file="BankAccountTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System.Collections.Generic;

    using Resources;

    using Xunit;

    public class BankAccountTests : DomainTest
    {
        [Fact]
        public void GivenBankAccount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new BankAccountBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);
            
            this.Session.Rollback();

            builder.WithIban("NL50RABO0109546784");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenBankAccount_WhenOwnBankAccount_ThenRequiredRelationsMustExist()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;
            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            this.Session.Commit();

            var builder = new BankAccountBuilder(this.Session).WithIban("NL50RABO0109546784");
            var bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.Session).WithBankAccount(bankAccount).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithBank(bank);
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.Session).WithBankAccount(bankAccount).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCurrency(euro);
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.Session).WithBankAccount(bankAccount).Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithNameOnAccount("name");
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.Session).WithBankAccount(bankAccount).WithDescription("description").Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenBankAccount_WhenDeriving_ThenIbanMustBeUnique()
        {
            var netherlands = new Countries(this.Session).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.Session).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").Build();

            Assert.False(this.Session.Derive(false).HasErrors);

            new BankAccountBuilder(this.Session).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").Build();

            Assert.True(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenBankAccount_WhenValidatingIban_ThenIllegalCharactersResultInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanIllegalCharacters;

            new BankAccountBuilder(this.Session).WithIban("-=jw").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);

            this.Session.Rollback();

            new BankAccountBuilder(this.Session).WithIban("TR33000610+51978645,841326").Build();

            errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Fact]
        public void GivenBankAccount_WhenValidatingIban_ThenWrongStructureResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanStructuralFailure;

            new BankAccountBuilder(this.Session).WithIban("D497888").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Fact]
        public void GivenBankAccount_WhenValidatingIban_ThenWrongCheckDigitsResultInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanCheckDigitsError;

            new BankAccountBuilder(this.Session).WithIban("TR000006100519786457841326").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);

            this.Session.Rollback();

            new BankAccountBuilder(this.Session).WithIban("TR010006100519786457841326").Build();

            errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);

            this.Session.Rollback();

            new BankAccountBuilder(this.Session).WithIban("TR990006100519786457841326").Build();

            errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Fact]
        public void GivenBankAccount_WhenValidatingIban_ThenCountryWithoutIbanRulesResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanValidationUnavailable;

            new BankAccountBuilder(this.Session).WithIban("XX330006100519786457841326").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Fact]
        public void GivenBankAccount_WhenValidatingIban_ThenWronglengthResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanLengthFailure;

            new BankAccountBuilder(this.Session).WithIban("TR3300061005196457841326").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);

            this.Session.Rollback();

            new BankAccountBuilder(this.Session).WithIban("TR3300061005197864578413268").Build();

            errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Fact]
        public void GivenBankAccount_WhenValidatingIban_ThenWrongStuctureForCountryResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanStructuralFailure;

            new BankAccountBuilder(this.Session).WithIban("LV80B12K0000435195001").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Fact]
        public void GivenBankAccount_WhenValidatingIban_ThenInvalidIbanResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanIncorrect;

            new BankAccountBuilder(this.Session).WithIban("TR330006100519716457841326").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.Session.Derive(false).Errors);
            Assert.Equal(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Fact]
        public void m_Correct()
        {
            new BankAccountBuilder(this.Session).WithIban("TR330006100519786457841326").Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
