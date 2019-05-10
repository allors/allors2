// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepeatingSalesInvoice.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    using Allors.Meta;

    using Resources;

    public partial class RepeatingSalesInvoice
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistSource)
            {
                derivation.AddDependency(this.Source, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            if (!this.Frequency.Equals(new TimeFrequencies(this.Strategy.Session).Month) && !this.Frequency.Equals(new TimeFrequencies(this.Strategy.Session).Week))
            {
                derivation.Validation.AddError(this, M.RepeatingSalesInvoice.Frequency, ErrorMessages.FrequencyNotSupported);
            }

            if (this.Frequency.Equals(new TimeFrequencies(this.Strategy.Session).Week) && !this.ExistDayOfWeek)
            {
                derivation.Validation.AssertExists(this, M.RepeatingSalesInvoice.DayOfWeek);
            }

            if (this.Frequency.Equals(new TimeFrequencies(this.Strategy.Session).Month) && this.ExistDayOfWeek)
            {
                derivation.Validation.AssertNotExists(this, M.RepeatingSalesInvoice.DayOfWeek);
            }

            if (this.Frequency.Equals(new TimeFrequencies(this.Strategy.Session).Week) && this.ExistDayOfWeek && this.ExistNextExecutionDate)
            {
                if (!this.NextExecutionDate.DayOfWeek.ToString().Equals(this.DayOfWeek.Name))
                {
                    derivation.Validation.AddError(this, M.RepeatingSalesInvoice.DayOfWeek, ErrorMessages.DateDayOfWeek);
                }
            }
        }

        public void Repeat()
        {
            var now = this.Strategy.Session.Now().Date;
            var monthly = new TimeFrequencies(this.Strategy.Session).Month;
            var weekly = new TimeFrequencies(this.Strategy.Session).Week;

            if (this.Frequency.Equals(monthly))
            {
                var nextDate = now.AddMonths(1).Date;
                this.Repeat(now, nextDate);
            }

            if (this.Frequency.Equals(weekly))
            {
                var nextDate = now.AddDays(7).Date;
                this.Repeat(now, nextDate);
            }
        }

        private void Repeat(DateTime now, DateTime nextDate)
        {
            if (!this.ExistFinalExecutionDate || nextDate <= this.FinalExecutionDate.Value.Date)
            {
                this.NextExecutionDate = nextDate.Date;
            }

            var nextInvoice = this.Source.AppsCopy(new SalesInvoiceCopy(this.Source));
            this.AddSalesInvoice(nextInvoice);

            this.PreviousExecutionDate = now.Date;
        }
    }
}
