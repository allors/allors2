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
                .WithDefaultPaymentMethod(this.ownBankAccount)
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
                .WithDefaultPaymentMethod(this.ownBankAccount)
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
                .WithDefaultPaymentMethod(this.ownBankAccount)
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
                .WithDefaultPaymentMethod(this.ownBankAccount)
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
                .WithName("Internal")
                .Build();

            organisation.AppsStartNewFiscalYear();

            var fromDate = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 01, 01).Date;
            var month = organisation.ActualAccountingPeriod;

            Assert.Equal(1, month.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Month, month.TimeFrequency);
            Assert.Equal(fromDate, month.FromDate);
            Assert.Equal(fromDate.AddMonths(1).AddSeconds(-1).Date, month.ThroughDate);
            Assert.True(month.ExistParent);

            var trimester = month.Parent;

            Assert.Equal(1, trimester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Trimester, trimester.TimeFrequency);
            Assert.Equal(fromDate, trimester.FromDate);
            Assert.Equal(fromDate.AddMonths(3).AddSeconds(-1).Date, trimester.ThroughDate);
            Assert.True(trimester.ExistParent);

            var semester = trimester.Parent;

            Assert.Equal(1, semester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Semester, semester.TimeFrequency);
            Assert.Equal(fromDate, semester.FromDate);
            Assert.Equal(fromDate.AddMonths(6).AddSeconds(-1).Date, semester.ThroughDate);
            Assert.True(semester.ExistParent);

            var year = semester.Parent;

            Assert.Equal(1, year.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Year, year.TimeFrequency);
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
                .WithName("Internal")
                .WithFiscalYearStartMonth(05)
                .WithFiscalYearStartDay(15)
                .Build();

            organisation.AppsStartNewFiscalYear();

            var fromDate = DateTimeFactory.CreateDate(DateTime.UtcNow.Year, 05, 15).Date;
            var month = organisation.ActualAccountingPeriod;

            Assert.Equal(1, month.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Month, month.TimeFrequency);
            Assert.Equal(fromDate, month.FromDate);
            Assert.Equal(fromDate.AddMonths(1).AddSeconds(-1).Date, month.ThroughDate);
            Assert.True(month.ExistParent);

            var trimester = month.Parent;

            Assert.Equal(1, trimester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Trimester, trimester.TimeFrequency);
            Assert.Equal(fromDate, trimester.FromDate);
            Assert.Equal(fromDate.AddMonths(3).AddSeconds(-1).Date, trimester.ThroughDate);
            Assert.True(trimester.ExistParent);

            var semester = trimester.Parent;

            Assert.Equal(1, semester.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Semester, semester.TimeFrequency);
            Assert.Equal(fromDate, semester.FromDate);
            Assert.Equal(fromDate.AddMonths(6).AddSeconds(-1).Date, semester.ThroughDate);
            Assert.True(semester.ExistParent);

            var year = semester.Parent;

            Assert.Equal(1, year.PeriodNumber);
            Assert.Equal(new TimeFrequencies(this.Session).Year, year.TimeFrequency);
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

            organisation.AppsStartNewFiscalYear();

            Assert.Equal(4, this.Session.Extent<AccountingPeriod>().Count);

            organisation.AppsStartNewFiscalYear();

            Assert.Equal(4, this.Session.Extent<AccountingPeriod>().Count);
        }

        [Fact]
        public void GivenInternalOrganisationWithoutDefaultPaymentMethod_WhenSinglePaymentMethodIsAdded_ThenDefaultPaymentMethodIsSet()
        {
            var internalOrganisation = this.Session.Extent<InternalOrganisation>().First;
            internalOrganisation.RemoveDefaultPaymentMethod();
            
            this.Session.Derive();

            Assert.True(internalOrganisation.ExistDefaultPaymentMethod);
        }
        
        private void InstantiateObjects(ISession session)
        {
            this.ownBankAccount = (OwnBankAccount)session.Instantiate(this.ownBankAccount);
            this.billingAddress = (WebAddress)session.Instantiate(this.billingAddress);
        }
    }
}
