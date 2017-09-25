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
    using Meta;
    using Xunit;

    
    public class InternalOrganisationTests : DomainTest
    {
        private OwnBankAccount ownBankAccount;
        private Currency euro;
        private PartyContactMechanism billingAddress;
        
        public InternalOrganisationTests()
        {
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

            this.DatabaseSession.Derive();
            this.DatabaseSession.Commit();
        }

        [Fact]
        public void GivenInternalOrganisation_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var builder = new InternalOrganisationBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithPaymentMethod(this.ownBankAccount);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("Organisation");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenDoAccountingIsFalse()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive();

            Assert.False(internalOrganisation.DoAccounting);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenFiscalYearStartMonthIsJanuary()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, internalOrganisation.FiscalYearStartMonth);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenFiscalYearStartDayIsFirstDayOfMonth()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, internalOrganisation.FiscalYearStartDay);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenInvoiceSequenceIsEqualRestartOnFiscalYear()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithEmployeeRole(new Roles(this.DatabaseSession).Administrator)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new InvoiceSequences(this.DatabaseSession).RestartOnFiscalYear, internalOrganisation.InvoiceSequence);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenPreferredCurrencyIsChanged_ThenValidationErrorIsTrhown()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive();
            Assert.NotNull(organisation.PreviousCurrency);
               
            organisation.PreferredCurrency = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "GBP");

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            organisation.PreferredCurrency = new Currencies(this.DatabaseSession).FindBy(M.Currency.IsoCode, "EUR");

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenInternalOrganisationWithDefaultFiscalYearStartMonthAndNotExistActualAccountingPeriod_WhenStartingNewFiscalYear_ThenAccountingPeriodsAreCreated()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            organisation.AppsStartNewFiscalYear();

            var fromDate = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 01, 01).Date;
            var month = organisation.ActualAccountingPeriod;

            Assert.Equal(1, month.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Month, month.TimeFrequency);
            Assert.Equal(fromDate, month.FromDate);
            Assert.Equal(fromDate.AddMonths(1).AddSeconds(-1).Date, month.ThroughDate);
            Assert.True(month.ExistParent);

            var trimester = month.Parent;

            Assert.Equal(1, trimester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Trimester, trimester.TimeFrequency);
            Assert.Equal(fromDate, trimester.FromDate);
            Assert.Equal(fromDate.AddMonths(3).AddSeconds(-1).Date, trimester.ThroughDate);
            Assert.True(trimester.ExistParent);

            var semester = trimester.Parent;

            Assert.Equal(1, semester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Semester, semester.TimeFrequency);
            Assert.Equal(fromDate, semester.FromDate);
            Assert.Equal(fromDate.AddMonths(6).AddSeconds(-1).Date, semester.ThroughDate);
            Assert.True(semester.ExistParent);

            var year = semester.Parent;

            Assert.Equal(1, year.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Year, year.TimeFrequency);
            Assert.Equal(fromDate, year.FromDate);
            Assert.Equal(fromDate.AddMonths(12).AddSeconds(-1).Date, year.ThroughDate);
            Assert.False(year.ExistParent);
        }

        [Fact]
        public void GivenInternalOrganisationWithCustomFiscalYearStartMonthAndNotExistActualAccountingPeriod_WhenStartingNewFiscalYear_ThenAccountingPeriodsAreCreated()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithFiscalYearStartMonth(05)
                .WithFiscalYearStartDay(15)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            organisation.AppsStartNewFiscalYear();

            var fromDate = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 05, 15).Date;
            var month = organisation.ActualAccountingPeriod;

            Assert.Equal(1, month.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Month, month.TimeFrequency);
            Assert.Equal(fromDate, month.FromDate);
            Assert.Equal(fromDate.AddMonths(1).AddSeconds(-1).Date, month.ThroughDate);
            Assert.True(month.ExistParent);

            var trimester = month.Parent;

            Assert.Equal(1, trimester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Trimester, trimester.TimeFrequency);
            Assert.Equal(fromDate, trimester.FromDate);
            Assert.Equal(fromDate.AddMonths(3).AddSeconds(-1).Date, trimester.ThroughDate);
            Assert.True(trimester.ExistParent);

            var semester = trimester.Parent;

            Assert.Equal(1, semester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Semester, semester.TimeFrequency);
            Assert.Equal(fromDate, semester.FromDate);
            Assert.Equal(fromDate.AddMonths(6).AddSeconds(-1).Date, semester.ThroughDate);
            Assert.True(semester.ExistParent);

            var year = semester.Parent;

            Assert.Equal(1, year.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.DatabaseSession).Year, year.TimeFrequency);
            Assert.Equal(fromDate, year.FromDate);
            Assert.Equal(fromDate.AddMonths(12).AddSeconds(-1).Date, year.ThroughDate);
            Assert.False(year.ExistParent);

            Assert.True(organisation.ExistActualAccountingPeriod);
            Assert.Contains(organisation.ActualAccountingPeriod, organisation.AccountingPeriods);
            Assert.Contains(trimester, organisation.AccountingPeriods);
            Assert.Contains(semester, organisation.AccountingPeriods);
            Assert.Contains(year, organisation.AccountingPeriods);
        }

        [Fact]
        public void GivenInternalOrganisationWithActiveActualAccountingPeriod_WhenStartingNewFiscalYear_ThenNothingHappens()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithFiscalYearStartMonth(05)
                .WithFiscalYearStartDay(15)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            organisation.AppsStartNewFiscalYear();

            Assert.Equal(4, this.DatabaseSession.Extent<AccountingPeriod>().Count);

            organisation.AppsStartNewFiscalYear();

            Assert.Equal(4, this.DatabaseSession.Extent<AccountingPeriod>().Count);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenDefaultPaymentMethodIsSet_ThenPaymentMethodIsAddedToCollectionPaymentMethods()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithDefaultPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, organisation.PaymentMethods.Count);
            Assert.Equal(ownBankAccount, organisation.PaymentMethods.First);
        }

        [Fact]
        public void GivenInternalOrganisationWithoutDefaultPaymentMethod_WhenSinglePaymentMethodIsAdded_ThenDefaultPaymentMethodIsSet()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(ownBankAccount, organisation.DefaultPaymentMethod);
        }

        [Fact]
        public void GivenInternalOrganisationWithSinglePaymentMethod_WhenPaymentMethodIsRemoved_ThenDefaultPaymentMethodIsAdded()
        {
            this.InstantiateObjects(this.DatabaseSession);

            var organisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Internal")
                .WithPreferredCurrency(this.euro)
                .WithPaymentMethod(this.ownBankAccount)
                .WithPartyContactMechanism(this.billingAddress)
                .Build();

            this.DatabaseSession.Derive();

            organisation.RemovePaymentMethod(ownBankAccount);

            this.DatabaseSession.Derive();

            Assert.True(organisation.ExistDefaultPaymentMethod);
            Assert.Equal(1, organisation.PaymentMethods.Count);
        }
        
        private void InstantiateObjects(ISession session)
        {
            this.ownBankAccount = (OwnBankAccount)session.Instantiate(this.ownBankAccount);
            this.euro = (Currency)session.Instantiate(this.euro);
            this.billingAddress = (PartyContactMechanism)session.Instantiate(this.billingAddress);
        }
    }
}
