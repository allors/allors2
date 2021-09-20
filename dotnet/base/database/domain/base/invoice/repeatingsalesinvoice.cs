// <copyright file="RepeatingSalesInvoice.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    using Allors.Meta;

    using Resources;

    public partial class RepeatingSalesInvoice
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistSource)
                {
                    iteration.AddDependency(this.Source, this);
                    iteration.Mark(this.Source);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
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

            var nextInvoice = this.Source.BaseCopy(new SalesInvoiceCopy(this.Source));
            this.AddSalesInvoice(nextInvoice);

            this.PreviousExecutionDate = now.Date;
        }
    }
}
