// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalOrganisationTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
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
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class InternalOrganisationTests : DomainTest
    {
        private OwnBankAccount ownBankAccount;
        private Currency euro;
        private PartyContactMechanism billingAddress;

        [SetUp]
        public override void Init()
        {
            base.Init();

            var belgium = new Countries(this.DatabaseSession).CountryByIsoCode["BE"];
            this.euro = belgium.Currency;

            var bank = new BankBuilder(this.DatabaseSession).WithCountry(belgium).WithName("ING België").WithBic("BBRUBEBB").Build();

            this.ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession)
                .WithDescription("BE23 3300 6167 6391")
                .WithBankAccount(new BankAccountBuilder(this.DatabaseSession).WithBank(bank).WithCurrency(euro).WithIban("BE23 3300 6167 6391").WithNameOnAccount("Koen").Build())
                .Build();

            this.billingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(new WebAddressBuilder(this.DatabaseSession).WithElectronicAddressString("billfrom").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();
        }

        [Test]
        public void GivenInternalOrganisation_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var builder = new InternalOrganisationBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPaymentMethod(this.ownBankAccount);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("Organisation");
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPartyContactMechanism(this.billingAddress);
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenDoAccountingIsFalse()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(internalOrganisation.DoAccounting);
        }

        [Test]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenFiscalYearStartMonthIsJanuary()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, internalOrganisation.FiscalYearStartMonth);
        }

        [Test]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenFiscalYearStartDayIsFirstDayOfMonth()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, internalOrganisation.FiscalYearStartDay);
        }

        [Test]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenInvoiceSequenceIsEqualRestartOnFiscalYear()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new InvoiceSequences(this.DatabaseSession).FindBy(UniquelyIdentifiables.Meta.UniqueId, InvoiceSequences.RestartOnFiscalYearId), internalOrganisation.InvoiceSequence);
        }

        [Test]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenLocaleIsEqualDefaultInternalOrganisationLocale()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.Locale, internalOrganisation.Locale);
        }

        [Test]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenPreferredCurrencyIsEqualInternalOrganisationPreferredCurrency()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation.PreferredCurrency, internalOrganisation.PreferredCurrency);
        }

        [Test]
        public void GivenInternalOrganisation_WhenBuild_ThenUserGroupForInternalOrganisationIsCreated()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            var name = string.Format("{0} for {1})", new Roles(this.DatabaseSession).Administrator.Name, internalOrganisation.Name);

            Assert.IsNotNull(new UserGroups(this.DatabaseSession).FindBy(UserGroups.Meta.Name, name));
        }

        [Test]
        public void GivenInternalOrganisation_WhenOperationsRoleIsUsed_ThenOperationsUserGroupIsDerived()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            var name = string.Format("{0} for {1})", new Roles(this.DatabaseSession).Administrator.Name, internalOrganisation.Name);
            var userGroup = new UserGroups(this.DatabaseSession).FindBy(UserGroups.Meta.Name, name);
            Assert.IsNotNull(userGroup);
        }

        [Test]
        public void GivenInternalOrganisation_WhenOperationsRoleIsNoLongerUsed_ThenInternalOrganisationOperationsUserGroupIsRemoved()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            var name = string.Format("{0} for {1})", new Roles(this.DatabaseSession).Administrator.Name, internalOrganisation.Name);
            var userGroup = new UserGroups(this.DatabaseSession).FindBy(UserGroups.Meta.Name, name);
            Assert.IsNotNull(userGroup);

            internalOrganisation.RemoveEmployeeRole(new Roles(this.DatabaseSession).Administrator);

            this.DatabaseSession.Derive(true);
            
            Assert.IsNotNull(userGroup);
        }

        [Test]
        public void GivenInternalOrganisation_WhenPreferredCurrencyIsChanged_ThenValidationErrorIsTrhown()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);
            Assert.IsNotNull(organisation.PreviousCurrency);
               
            organisation.PreferredCurrency = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "GBP");

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            organisation.PreferredCurrency = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenInternalOrganisationWithDefaultFiscalYearStartMonthAndNotExistActualAccountingPeriod_WhenStartingNewFiscalYear_ThenAccountingPeriodsAreCreated()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            organisation.StartNewFiscalYear();

            var fromDate = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 01, 01).Date;
            var month = organisation.ActualAccountingPeriod;

            Assert.AreEqual(1, month.PeriodNumber);
            Assert.AreEqual(new TimeFrequencies(this.DatabaseSession).Month, month.TimeFrequency);
            Assert.AreEqual(fromDate, month.FromDate);
            Assert.AreEqual(fromDate.AddMonths(1).AddSeconds(-1).Date, month.ThroughDate);
            Assert.IsTrue(month.ExistParent);

            var trimester = month.Parent;

            Assert.AreEqual(1, trimester.PeriodNumber);
            Assert.AreEqual(new TimeFrequencies(this.DatabaseSession).Trimester, trimester.TimeFrequency);
            Assert.AreEqual(fromDate, trimester.FromDate);
            Assert.AreEqual(fromDate.AddMonths(3).AddSeconds(-1).Date, trimester.ThroughDate);
            Assert.IsTrue(trimester.ExistParent);

            var semester = trimester.Parent;

            Assert.AreEqual(1, semester.PeriodNumber);
            Assert.AreEqual(new TimeFrequencies(this.DatabaseSession).Semester, semester.TimeFrequency);
            Assert.AreEqual(fromDate, semester.FromDate);
            Assert.AreEqual(fromDate.AddMonths(6).AddSeconds(-1).Date, semester.ThroughDate);
            Assert.IsTrue(semester.ExistParent);

            var year = semester.Parent;

            Assert.AreEqual(1, year.PeriodNumber);
            Assert.AreEqual(new TimeFrequencies(this.DatabaseSession).Year, year.TimeFrequency);
            Assert.AreEqual(fromDate, year.FromDate);
            Assert.AreEqual(fromDate.AddMonths(12).AddSeconds(-1).Date, year.ThroughDate);
            Assert.IsFalse(year.ExistParent);
        }

        [Test]
        public void GivenInternalOrganisationWithCustomFiscalYearStartMonthAndNotExistActualAccountingPeriod_WhenStartingNewFiscalYear_ThenAccountingPeriodsAreCreated()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithFiscalYearStartMonth(05)
                .WithFiscalYearStartDay(15)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            organisation.StartNewFiscalYear();

            var fromDate = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 05, 15).Date;
            var month = organisation.ActualAccountingPeriod;

            Assert.AreEqual(1, month.PeriodNumber);
            Assert.AreEqual(new TimeFrequencies(this.DatabaseSession).Month, month.TimeFrequency);
            Assert.AreEqual(fromDate, month.FromDate);
            Assert.AreEqual(fromDate.AddMonths(1).AddSeconds(-1).Date, month.ThroughDate);
            Assert.IsTrue(month.ExistParent);

            var trimester = month.Parent;

            Assert.AreEqual(1, trimester.PeriodNumber);
            Assert.AreEqual(new TimeFrequencies(this.DatabaseSession).Trimester, trimester.TimeFrequency);
            Assert.AreEqual(fromDate, trimester.FromDate);
            Assert.AreEqual(fromDate.AddMonths(3).AddSeconds(-1).Date, trimester.ThroughDate);
            Assert.IsTrue(trimester.ExistParent);

            var semester = trimester.Parent;

            Assert.AreEqual(1, semester.PeriodNumber);
            Assert.AreEqual(new TimeFrequencies(this.DatabaseSession).Semester, semester.TimeFrequency);
            Assert.AreEqual(fromDate, semester.FromDate);
            Assert.AreEqual(fromDate.AddMonths(6).AddSeconds(-1).Date, semester.ThroughDate);
            Assert.IsTrue(semester.ExistParent);

            var year = semester.Parent;

            Assert.AreEqual(1, year.PeriodNumber);
            Assert.AreEqual(new TimeFrequencies(this.DatabaseSession).Year, year.TimeFrequency);
            Assert.AreEqual(fromDate, year.FromDate);
            Assert.AreEqual(fromDate.AddMonths(12).AddSeconds(-1).Date, year.ThroughDate);
            Assert.IsFalse(year.ExistParent);

            Assert.IsTrue(organisation.ExistActualAccountingPeriod);
            Assert.Contains(organisation.ActualAccountingPeriod, organisation.AccountingPeriods);
            Assert.Contains(trimester, organisation.AccountingPeriods);
            Assert.Contains(semester, organisation.AccountingPeriods);
            Assert.Contains(year, organisation.AccountingPeriods);
        }

        [Test]
        public void GivenInternalOrganisationWithActiveActualAccountingPeriod_WhenStartingNewFiscalYear_ThenNothingHappens()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithFiscalYearStartMonth(05)
                .WithFiscalYearStartDay(15)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            organisation.StartNewFiscalYear();

            Assert.AreEqual(4, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.StartNewFiscalYear();

            Assert.AreEqual(4, this.DatabaseSession.Extent<AccountingPeriod>().Count);
        }

        [Test]
        public void GivenInternalOrganisation_WhenDefaultPaymentMethodIsSet_ThenPaymentMethodIsAddedToCollectionPaymentMethods()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, organisation.PaymentMethods.Count);
            Assert.AreEqual(ownBankAccount, organisation.PaymentMethods.First);
        }

        [Test]
        public void GivenInternalOrganisationWithoutDefaultPaymentMethod_WhenSinglePaymentMethodIsAdded_ThenDefaultPaymentMethodIsSet()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(ownBankAccount, organisation.DefaultPaymentMethod);
        }

        [Test]
        public void GivenInternalOrganisationWithSinglePaymentMethod_WhenPaymentMethodIsRemoved_ThenDefaultPaymentMethodIsAdded()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithLocale(new Locales(this.DatabaseSession).EnglishGreatBritain)
                .WithPreferredCurrency(this.euro)
                .WithPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive(true);

            organisation.RemovePaymentMethod(ownBankAccount);

            this.DatabaseSession.Derive();

            Assert.IsTrue(organisation.ExistDefaultPaymentMethod);
            Assert.AreEqual(1, organisation.PaymentMethods.Count);
        }
        
        private void InstantiateObjects(ISession session)
        {
            this.ownBankAccount = (OwnBankAccount)session.Instantiate(this.ownBankAccount);
            this.euro = (Currency)session.Instantiate(this.euro);
            this.billingAddress = (PartyContactMechanism)session.Instantiate(this.billingAddress);
        }
    }
}
