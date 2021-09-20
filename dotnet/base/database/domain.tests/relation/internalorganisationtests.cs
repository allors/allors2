// <copyright file="InternalOrganisationTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Domain
{
    using Xunit;

    public class InternalOrganisationTests : DomainTest
    {
        private OwnBankAccount ownBankAccount;
        private WebAddress billingAddress;

        public InternalOrganisationTests()
        {
            this.ownBankAccount = this.Session.Extent<OwnBankAccount>().First;

            this.billingAddress = new WebAddressBuilder(this.Session).WithElectronicAddressString("billfrom").Build();

            this.Session.Derive();
            this.Session.Commit();
        }

        [Fact]
        public void GivenInternalOrganisation_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            this.InstantiateObjects(this.Session);

            var builder = new OrganisationBuilder(this.Session).WithIsInternalOrganisation(true);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("Organisation");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenDoAccountingIsFalse()
        {
            this.InstantiateObjects(this.Session);

            var internalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithDefaultCollectionMethod(this.ownBankAccount)
                .Build();

            this.Session.Derive();

            Assert.False(internalOrganisation.DoAccounting);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenFiscalYearStartMonthIsJanuary()
        {
            this.InstantiateObjects(this.Session);

            var internalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithDefaultCollectionMethod(this.ownBankAccount)
                .Build();

            this.Session.Derive();

            Assert.Equal(1, internalOrganisation.FiscalYearStartMonth);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenFiscalYearStartDayIsFirstDayOfMonth()
        {
            this.InstantiateObjects(this.Session);

            var internalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithDefaultCollectionMethod(this.ownBankAccount)
                .Build();

            this.Session.Derive();

            Assert.Equal(1, internalOrganisation.FiscalYearStartDay);
        }

        [Fact]
        public void GivenInternalOrganisation_WhenBuildWithout_ThenInvoiceSequenceIsEqualRestartOnFiscalYear()
        {
            this.InstantiateObjects(this.Session);

            var internalOrganisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithDefaultCollectionMethod(this.ownBankAccount)
                .Build();

            this.Session.Derive();

            Assert.Equal(new InvoiceSequences(this.Session).RestartOnFiscalYear, internalOrganisation.InvoiceSequence);
        }

        [Fact]
        public void GivenInternalOrganisationWithDefaultFiscalYearStartMonthAndNotExistActualAccountingPeriod_WhenStartingNewFiscalYear_ThenAccountingPeriodsAreCreated()
        {
            this.InstantiateObjects(this.Session);

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithDoAccounting(true)
                .WithName("Internal")
                .Build();

            this.Session.Derive();

            organisation.StartNewFiscalYear();

            var fromDate = DateTimeFactory.CreateDate(this.Session.Now().Year, 01, 01).Date;
            var month = organisation.ActualAccountingPeriod;

            Assert.Equal(1, month.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Month, month.Frequency);
            Assert.Equal(fromDate, month.FromDate);
            Assert.Equal(fromDate.AddMonths(1).AddSeconds(-1).Date, month.ThroughDate);
            Assert.True(month.ExistParent);

            var trimester = month.Parent;

            Assert.Equal(1, trimester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Trimester, trimester.Frequency);
            Assert.Equal(fromDate, trimester.FromDate);
            Assert.Equal(fromDate.AddMonths(3).AddSeconds(-1).Date, trimester.ThroughDate);
            Assert.True(trimester.ExistParent);

            var semester = trimester.Parent;

            Assert.Equal(1, semester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Semester, semester.Frequency);
            Assert.Equal(fromDate, semester.FromDate);
            Assert.Equal(fromDate.AddMonths(6).AddSeconds(-1).Date, semester.ThroughDate);
            Assert.True(semester.ExistParent);

            var year = semester.Parent;

            Assert.Equal(1, year.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Year, year.Frequency);
            Assert.Equal(fromDate, year.FromDate);
            Assert.Equal(fromDate.AddMonths(12).AddSeconds(-1).Date, year.ThroughDate);
            Assert.False(year.ExistParent);
        }

        [Fact]
        public void GivenInternalOrganisationWithCustomFiscalYearStartMonthAndNotExistActualAccountingPeriod_WhenStartingNewFiscalYear_ThenAccountingPeriodsAreCreated()
        {
            this.InstantiateObjects(this.Session);

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithDoAccounting(true)
                .WithName("Internal")
                .WithFiscalYearStartMonth(05)
                .WithFiscalYearStartDay(15)
                .Build();

            organisation.StartNewFiscalYear();

            var fromDate = DateTimeFactory.CreateDate(this.Session.Now().Year, 05, 15).Date;
            var month = organisation.ActualAccountingPeriod;

            Assert.Equal(1, month.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Month, month.Frequency);
            Assert.Equal(fromDate, month.FromDate);
            Assert.Equal(fromDate.AddMonths(1).AddSeconds(-1).Date, month.ThroughDate);
            Assert.True(month.ExistParent);

            var trimester = month.Parent;

            Assert.Equal(1, trimester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Trimester, trimester.Frequency);
            Assert.Equal(fromDate, trimester.FromDate);
            Assert.Equal(fromDate.AddMonths(3).AddSeconds(-1).Date, trimester.ThroughDate);
            Assert.True(trimester.ExistParent);

            var semester = trimester.Parent;

            Assert.Equal(1, semester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Semester, semester.Frequency);
            Assert.Equal(fromDate, semester.FromDate);
            Assert.Equal(fromDate.AddMonths(6).AddSeconds(-1).Date, semester.ThroughDate);
            Assert.True(semester.ExistParent);

            var year = semester.Parent;

            Assert.Equal(1, year.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Year, year.Frequency);
            Assert.Equal(fromDate, year.FromDate);
            Assert.Equal(fromDate.AddMonths(12).AddSeconds(-1).Date, year.ThroughDate);
            Assert.False(year.ExistParent);

            Assert.True(organisation.ExistActualAccountingPeriod);
        }

        [Fact]
        public void GivenInternalOrganisationWithActiveActualAccountingPeriod_WhenStartingNewFiscalYear_ThenNothingHappens()
        {
            this.InstantiateObjects(this.Session);

            var organisation = new OrganisationBuilder(this.Session)
                .WithIsInternalOrganisation(true)
                .WithName("Internal")
                .WithFiscalYearStartMonth(05)
                .WithFiscalYearStartDay(15)
                .Build();

            organisation.StartNewFiscalYear();

            Assert.Equal(4, this.Session.Extent<AccountingPeriod>().Count);

            organisation.StartNewFiscalYear();

            Assert.Equal(4, this.Session.Extent<AccountingPeriod>().Count);
        }

        [Fact]
        public void GivenInternalOrganisationWithoutDefaultCollectionMethod_WhenExistSingleCollectionMethod_ThenDefaultIsSet()
        {
            this.InternalOrganisation.RemoveDefaultCollectionMethod();

            this.Session.Derive();

            Assert.True(this.InternalOrganisation.ExistDefaultCollectionMethod);
        }

        private void InstantiateObjects(ISession session)
        {
            this.ownBankAccount = (OwnBankAccount)session.Instantiate(this.ownBankAccount);
            this.billingAddress = (WebAddress)session.Instantiate(this.billingAddress);
        }

        [Fact]
        public void GivenInternalOrganisation_ActiveCustomers_AreDerived()
        {
            var internalOrganisation = this.InternalOrganisation;

            var activeCustomersBefore = this.InternalOrganisation.ActiveCustomers.Count;

            var acme = new OrganisationBuilder(this.Session).WithName("Acme").Build();
            var nike = new OrganisationBuilder(this.Session).WithName("Nike").Build();

            var acmeRelation = new CustomerRelationshipBuilder(this.Session)
                .WithInternalOrganisation(internalOrganisation).WithCustomer(acme)
                .WithFromDate(this.Session.Now().AddDays(-10))
                .Build();

            var nikeRelation = new CustomerRelationshipBuilder(this.Session)
                .WithInternalOrganisation(internalOrganisation)
                .WithCustomer(nike)
                .Build();

            this.Session.Derive();

            Assert.True(this.InternalOrganisation.ExistCustomerRelationshipsWhereInternalOrganisation);
            Assert.True(this.InternalOrganisation.ExistActiveCustomers);
            Assert.Equal(activeCustomersBefore + 2, this.InternalOrganisation.ActiveCustomers.Count);

            // Removing will not do anything.
            ((OrganisationDerivedRoles)this.InternalOrganisation).RemoveActiveCustomers();

            this.Session.Derive();
            Assert.True(this.InternalOrganisation.ExistActiveCustomers);
            Assert.Equal(activeCustomersBefore + 2, this.InternalOrganisation.ActiveCustomers.Count);

            // Ending a RelationShip affects the ActiveCustomers
            acmeRelation.ThroughDate = this.Session.Now().AddDays(-1).Date;

            this.Session.Derive();

            Assert.True(this.InternalOrganisation.ExistCustomerRelationshipsWhereInternalOrganisation);
            Assert.True(this.InternalOrganisation.ExistActiveCustomers);
            Assert.Equal(activeCustomersBefore + 1, this.InternalOrganisation.ActiveCustomers.Count);
        }
    }
}
