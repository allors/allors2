//------------------------------------------------------------------------------------------------- 
// <copyright file="OwnCreditCardTests.cs" company="Allors bvba">
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
    using System;
    using Xunit;

    public class OwnCreditCardTests : DomainTest
    {
        [Fact]
        public void GivenOwnCreditCard_WhenDeriving_ThenCreditCardRelationMustExist()
        {
            var creditCard = new CreditCardBuilder(this.Session)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(DateTime.UtcNow.Year + 1)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build())
                .Build();

            this.Session.Commit();

            var builder = new OwnCreditCardBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithCreditCard(creditCard);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnCreditCard_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var creditCard = new CreditCardBuilder(this.Session)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(DateTime.UtcNow.Year + 1)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build())
                .Build();

            var paymentMethod = new OwnCreditCardBuilder(this.Session)
                .WithCreditCard(creditCard)
                .Build();

            this.InternalOrganisation.AddPaymentMethod(paymentMethod);

            this.Session.Derive();

            Assert.True(paymentMethod.IsActive);
        }

        [Fact]
        public void GivenOwnCreditCardForSingleton_WhenDeriving_ThenExpiredCardIsBlocked()
        {
            var creditCard = new CreditCardBuilder(this.Session)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(DateTime.UtcNow.Year + 1)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build())
                .Build();

            var paymentMethod = new OwnCreditCardBuilder(this.Session)
                .WithCreditCard(creditCard)
                .Build();

            this.InternalOrganisation.AddPaymentMethod(paymentMethod);

            this.Session.Derive();
            Assert.True(paymentMethod.IsActive);

            creditCard.ExpirationYear = DateTime.UtcNow.Year;
            creditCard.ExpirationMonth = DateTime.UtcNow.Month;

            this.Session.Derive();
            Assert.False(paymentMethod.IsActive);
        }

        [Fact]
        public void GivenOwnCreditCard_WhenDeriving_ThenGeneralLedgerAccountAndJournalAtMostOne()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session).WithDescription("journal").Build();

            var creditCard = new CreditCardBuilder(this.Session)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(DateTime.UtcNow.Year + 1)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build())
                .Build();

            var collectionMethod = new OwnCreditCardBuilder(this.Session)
                .WithCreditCard(creditCard)
                .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                .Build();

            this.InternalOrganisation.AddActiveCollectionMethod(collectionMethod);

            this.Session.Commit();

            var internalOrganisation = this.InternalOrganisation;
            internalOrganisation.DoAccounting = true;
            internalOrganisation.DefaultCollectionMethod = collectionMethod;

            Assert.False(this.Session.Derive(false).HasErrors);

            collectionMethod.Journal = journal;

            Assert.True(this.Session.Derive(false).HasErrors);

            collectionMethod.RemoveGeneralLedgerAccount();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnCreditCardForSingletonThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            this.InternalOrganisation.DoAccounting = true;

            this.Session.Derive();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.Session)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.Session)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.Session).WithDescription("journal").Build();

            var creditCard = new CreditCardBuilder(this.Session)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(DateTime.UtcNow.Year + 1)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.Session).WithName("Visa").Build())
                .Build();

            var paymentMethod = new OwnCreditCardBuilder(this.Session)
                .WithCreditCard(creditCard)
                .Build();

            this.InternalOrganisation.AddPaymentMethod(paymentMethod);

            Assert.True(this.Session.Derive(false).HasErrors);

            paymentMethod.Journal = journal;

            Assert.False(this.Session.Derive(false).HasErrors);

            paymentMethod.RemoveJournal();
            paymentMethod.GeneralLedgerAccount = internalOrganisationGlAccount;

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
