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
    using NUnit.Framework;

    using Resources;

    [TestFixture]
    public class BankAccountTests : DomainTest
    {
        [Test]
        public void GivenBankAccount_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new BankAccountBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
            
            this.DatabaseSession.Rollback();

            builder.WithIban("NL50RABO0109546784");
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenBankAccount_WhenOwnBankAccount_ThenRequiredRelationsMustExist()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;
            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();

            this.DatabaseSession.Commit();

            var builder = new BankAccountBuilder(this.DatabaseSession).WithIban("NL50RABO0109546784");
            var bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.DatabaseSession).WithBankAccount(bankAccount).Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithBank(bank);
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.DatabaseSession).WithBankAccount(bankAccount).Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCurrency(euro);
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.DatabaseSession).WithBankAccount(bankAccount).Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithNameOnAccount("name");
            bankAccount = builder.Build();

            new OwnBankAccountBuilder(this.DatabaseSession).WithBankAccount(bankAccount).WithDescription("description").Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenBankAccount_WhenDeriving_ThenIbanMustBeUnique()
        {
            var netherlands = new Countries(this.DatabaseSession).CountryByIsoCode["NL"];
            var euro = netherlands.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(netherlands).WithName("RABOBANK GROEP").WithBic("RABONL2U").Build();
            new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("NL50RABO0109546784").Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenBankAccount_WhenValidatingIban_ThenIllegalCharactersResultInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanIllegalCharacters;

            new BankAccountBuilder(this.DatabaseSession).WithIban("-=jw").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);

            this.DatabaseSession.Rollback();

            new BankAccountBuilder(this.DatabaseSession).WithIban("TR33000610+51978645,841326").Build();

            errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Test]
        public void GivenBankAccount_WhenValidatingIban_ThenWrongStructureResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanStructuralFailure;

            new BankAccountBuilder(this.DatabaseSession).WithIban("D497888").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Test]
        public void GivenBankAccount_WhenValidatingIban_ThenWrongCheckDigitsResultInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanCheckDigitsError;

            new BankAccountBuilder(this.DatabaseSession).WithIban("TR000006100519786457841326").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);

            this.DatabaseSession.Rollback();

            new BankAccountBuilder(this.DatabaseSession).WithIban("TR010006100519786457841326").Build();

            errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);

            this.DatabaseSession.Rollback();

            new BankAccountBuilder(this.DatabaseSession).WithIban("TR990006100519786457841326").Build();

            errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Test]
        public void GivenBankAccount_WhenValidatingIban_ThenCountryWithoutIbanRulesResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanValidationUnavailable;

            new BankAccountBuilder(this.DatabaseSession).WithIban("XX330006100519786457841326").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Test]
        public void GivenBankAccount_WhenValidatingIban_ThenWronglengthResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanLengthFailure;

            new BankAccountBuilder(this.DatabaseSession).WithIban("TR3300061005196457841326").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);

            this.DatabaseSession.Rollback();

            new BankAccountBuilder(this.DatabaseSession).WithIban("TR3300061005197864578413268").Build();

            errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Test]
        public void GivenBankAccount_WhenValidatingIban_ThenWrongStuctureForCountryResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanStructuralFailure;

            new BankAccountBuilder(this.DatabaseSession).WithIban("LV80B12K0000435195001").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Test]
        public void GivenBankAccount_WhenValidatingIban_ThenInvalidIbanResultsInValidationError()
        {
            var expectedErrorMessage = ErrorMessages.IbanIncorrect;

            new BankAccountBuilder(this.DatabaseSession).WithIban("TR330006100519716457841326").Build();

            List<IDerivationError> errors = new List<IDerivationError>(this.DatabaseSession.Derive().Errors);
            Assert.AreEqual(1, errors.FindAll(e => e.Message.Equals(expectedErrorMessage)).Count);
        }

        [Test]
        public void m_Correct()
        {
            new BankAccountBuilder(this.DatabaseSession).WithIban("TR330006100519786457841326").Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
