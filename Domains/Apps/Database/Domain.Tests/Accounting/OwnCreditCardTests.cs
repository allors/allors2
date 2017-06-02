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
    using Meta;
    using Xunit;

    
    public class OwnCreditCardTests : DomainTest
    {
        [Fact]
        public void GivenOwnCreditCard_WhenDeriving_ThenCreditCardRelationMustExist()
        {
            var creditCard = new CreditCardBuilder(this.DatabaseSession)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build())
                .Build();

            this.DatabaseSession.Commit();

            var builder = new OwnCreditCardBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithCreditCard(creditCard);
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnCreditCard_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var creditCard = new CreditCardBuilder(this.DatabaseSession)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build())
                .Build();

            var paymentMethod = new OwnCreditCardBuilder(this.DatabaseSession)
                .WithCreditCard(creditCard)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.True(paymentMethod.IsActive);
        }

        [Fact]
        public void GivenOwnCreditCardForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenCreditorIsRequired()
        {
            var creditCard = new CreditCardBuilder(this.DatabaseSession)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build())
                .Build();

            var paymentMethod = new OwnCreditCardBuilder(this.DatabaseSession)
                .WithCreditCard(creditCard)
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;             
            
            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(paymentMethod);
            internalOrganisation.DoAccounting = false;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            internalOrganisation.DoAccounting = true;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnCreditCardForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenExpiredCardIsBlocked()
        {
            var creditCard = new CreditCardBuilder(this.DatabaseSession)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build())
                .Build();

            var paymentMethod = new OwnCreditCardBuilder(this.DatabaseSession)
                .WithCreditCard(creditCard)
                .Build();

            var internalOrganisation = Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation;
            internalOrganisation.AddPaymentMethod(paymentMethod);

            this.DatabaseSession.Derive(true);
            Assert.True(paymentMethod.IsActive);

            creditCard.ExpirationYear = DateTime.UtcNow.Year;
            creditCard.ExpirationMonth = DateTime.UtcNow.Month;

            this.DatabaseSession.Derive(true);
            Assert.False(paymentMethod.IsActive);
        }

        [Fact]
        public void GivenOwnCreditCard_WhenDeriving_ThenGeneralLedgerAccountAndJournalCannotExistBoth()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");

            var supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession).WithDescription("journal").Build();

            var creditCard = new CreditCardBuilder(this.DatabaseSession)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build())
                .Build();

            var paymentMethod = new OwnCreditCardBuilder(this.DatabaseSession)
                .WithCreditCard(creditCard)
                .WithGeneralLedgerAccount(internalOrganisationGlAccount)
                .WithCreditor(supplierRelationship)
                .Build();

            this.DatabaseSession.Commit();

            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(paymentMethod);
            internalOrganisation.DoAccounting = true;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            paymentMethod.Journal = journal;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            paymentMethod.RemoveGeneralLedgerAccount();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOwnCreditCardForInternalOrganisationThatDoesAccounting_WhenDeriving_ThenEitherGeneralLedgerAccountOrJournalMustExist()
        {
            var supplier = new OrganisationBuilder(this.DatabaseSession)
                .WithName("supplier")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .Build();

            var internalOrganisation = new InternalOrganisations(this.DatabaseSession).FindBy(M.InternalOrganisation.Name, "internalOrganisation");

            var supplierRelationship = new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var generalLedgerAccount = new GeneralLedgerAccountBuilder(this.DatabaseSession)
                .WithAccountNumber("0001")
                .WithName("GeneralLedgerAccount")
                .WithBalanceSheetAccount(true)
                .Build();

            var internalOrganisationGlAccount = new OrganisationGlAccountBuilder(this.DatabaseSession)
                .WithInternalOrganisation(internalOrganisation)
                .WithGeneralLedgerAccount(generalLedgerAccount)
                .Build();

            var journal = new JournalBuilder(this.DatabaseSession).WithDescription("journal").Build();

            var creditCard = new CreditCardBuilder(this.DatabaseSession)
                .WithCardNumber("4012888888881881")
                .WithExpirationYear(2016)
                .WithExpirationMonth(03)
                .WithNameOnCard("M.E. van Knippenberg")
                .WithCreditCardCompany(new CreditCardCompanyBuilder(this.DatabaseSession).WithName("Visa").Build())
                .Build();

            var paymentMethod = new OwnCreditCardBuilder(this.DatabaseSession)
                .WithCreditCard(creditCard)
                .WithCreditor(supplierRelationship)
                .Build();

            this.DatabaseSession.Commit();

            internalOrganisation.RemovePaymentMethods();
            internalOrganisation.AddPaymentMethod(paymentMethod);
            internalOrganisation.DoAccounting = true;

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            paymentMethod.Journal = journal;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            paymentMethod.RemoveJournal();
            paymentMethod.GeneralLedgerAccount = internalOrganisationGlAccount;

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }
    }
}
