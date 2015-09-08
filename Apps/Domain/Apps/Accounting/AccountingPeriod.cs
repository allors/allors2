// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountingPeriod.cs" company="Allors bvba">
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
    using System.Text;

    public partial class AccountingPeriod
    {
        ObjectState Transitional.CurrentObjectState
        {
            get
            {
                return this.CurrentObjectState;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistActive)
            {
                this.Active = true;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var stringBuilder = new StringBuilder();
            if (this.ExistFromDate)
            {
                stringBuilder.AppendFormat("{0:d}", this.FromDate);                
            }

            if (this.ExistThroughDate)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(" through ");
                }

                stringBuilder.AppendFormat("{0:d}", this.ThroughDate);
            }
        }

        private AccountingPeriod AppsAddNextMonth()
        {
            var allPeriods = this.InternalOrganisationWhereAccountingPeriod.AccountingPeriods;
            allPeriods.Filter.AddEquals(AccountingPeriods.Meta.TimeFrequency, new TimeFrequencies(this.Strategy.Session).Month);
            allPeriods.AddSort(AccountingPeriods.Meta.FromDate, SortDirection.Descending);

            var mostRecentMonth = allPeriods.First;

            var newMonth = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(mostRecentMonth.PeriodNumber + 1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Month)
                .Build();

            if (newMonth.PeriodNumber < 13)
            {
                newMonth.FromDate = mostRecentMonth.FromDate.AddMonths(1).Date;
                newMonth.ThroughDate = mostRecentMonth.FromDate.AddMonths(2).AddSeconds(-1).Date;
            }
            else
            {
                newMonth.FromDate = mostRecentMonth.FromDate;
                newMonth.ThroughDate = mostRecentMonth.ThroughDate;                
            }

            if (newMonth.PeriodNumber == 4 || newMonth.PeriodNumber == 7 || newMonth.PeriodNumber == 10)
            {
                newMonth.Parent = this.AppsAddNextQuarter(mostRecentMonth.Parent);
            }
            else
            {
                newMonth.Parent = mostRecentMonth.Parent;
            }

            this.InternalOrganisationWhereAccountingPeriod.AddAccountingPeriod(newMonth);
            return newMonth;
        }

        private AccountingPeriod AppsAddNextQuarter(AccountingPeriod previousPeriod)
        {
            var newQuarter = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(previousPeriod.PeriodNumber + 1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Trimester)
                .WithFromDate(previousPeriod.FromDate.AddMonths(3).Date)
                .WithThroughDate(previousPeriod.FromDate.AddMonths(6).AddSeconds(-1).Date)
                .Build();

            if (newQuarter.PeriodNumber == 3)
            {
                newQuarter.Parent = this.AppsAddNextSemester(previousPeriod.Parent);
            }
            else
            {
                newQuarter.Parent = previousPeriod.Parent;
            }

            this.InternalOrganisationWhereAccountingPeriod.AddAccountingPeriod(newQuarter);
            return newQuarter;
        }

        private AccountingPeriod AppsAddNextSemester(AccountingPeriod previousPeriod)
        {
            var newSemester = new AccountingPeriodBuilder(this.Strategy.Session)
                .WithPeriodNumber(previousPeriod.PeriodNumber + 1)
                .WithTimeFrequency(new TimeFrequencies(this.Strategy.Session).Semester)
                .WithFromDate(previousPeriod.FromDate.AddMonths(6).Date)
                .WithThroughDate(previousPeriod.FromDate.AddMonths(12).AddSeconds(-1).Date)
                .WithParent(previousPeriod.Parent)
                .Build();

            this.InternalOrganisationWhereAccountingPeriod.AddAccountingPeriod(newSemester);
            return newSemester;
        }
    }
}