// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeEntry.cs" company="Allors bvba">
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

    public partial class TimeEntry
    {
        public decimal ActualHours
        {
            get
            {
                var units = new UnitsOfMeasure(this.strategy.Session);
                decimal hours = 0.0M;

                if (this.UnitOfMeasure.Equals(units.Day))
                {
                    hours = Math.Round((this.AmountOfTime ?? 0.0M) * 24.0M, 17);
                }
                else if (this.UnitOfMeasure.Equals(units.Hour))
                {
                    hours = this.AmountOfTime ?? 0.0M;
                }
                else if (this.UnitOfMeasure.Equals(units.Minute))
                {
                    hours = Math.Round((this.AmountOfTime ?? 0.0M) / 60.0M, 17);
                }

                return hours;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistUnitOfMeasure)
            {
                this.UnitOfMeasure = new UnitsOfMeasure(this.strategy.Session).Hour;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistWorkEffort)
            {
                derivation.AddDependency(this.WorkEffort, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExists(this, this.Meta.TimeSheetWhereTimeEntry);
            derivation.Validation.AssertAtLeastOne(this, this.Meta.WorkEffort, this.Meta.EngagementItem);
            derivation.Validation.AssertAtLeastOne(this, this.Meta.ThroughDate, this.Meta.AmountOfTime);

            this.ValidateUnitOfMeasure(derivation);
            this.DeriveAmountOfTimeOrThroughDate();
        }

        private void ValidateUnitOfMeasure(IDerivation derivation)
        {
            var units = new UnitsOfMeasure(this.strategy.Session);
            const string error = "The Unit of Measure must be Day, Hour, or Minute.";

            if (!this.UnitOfMeasure.Equals(units.Day) && !this.UnitOfMeasure.Equals(units.Hour) && !this.UnitOfMeasure.Equals(units.Minute))
            {
                derivation.Validation.AddError(this, this.Meta.UnitOfMeasure, error);
                this.UnitOfMeasure = units.Hour;
            }
        }

        private void DeriveAmountOfTimeOrThroughDate()
        {
            var units = new UnitsOfMeasure(this.strategy.Session);

            if (this.ThroughDate != null)
            {
                var timeSpan = this.ThroughDate - this.FromDate;
                var amount = (decimal)timeSpan.Value.TotalMinutes;

                if (this.UnitOfMeasure.Equals(units.Day))
                {
                    this.AmountOfTime = amount / 1440.0M;
                }
                else if (this.UnitOfMeasure.Equals(units.Hour))
                {
                    this.AmountOfTime = amount / 60.0M;
                }
                else if (this.UnitOfMeasure.Equals(units.Minute))
                {
                    this.AmountOfTime = amount;
                }
                else
                {
                    this.RemoveAmountOfTime();
                    return;
                }
            }
            else if (this.AmountOfTime != null)
            {
                var amount = (decimal)this.AmountOfTime;
                int days = 0;
                int hours = 0;
                int minutes = 0;
                int seconds = 0;
                int millis = 0;

                if (this.UnitOfMeasure.Equals(units.Day))
                {
                    days = (int)amount;
                    amount -= (decimal)days;

                    hours = (int)(amount * 24);
                    amount -= (decimal)hours;

                    minutes = (int)(amount * 60);
                    amount -= (decimal)minutes;
                }
                else if (this.UnitOfMeasure.Equals(units.Hour))
                {
                    if (amount > 24.0M)
                    {
                        days = (int)(amount / 24);
                        amount -= ((decimal)days * 24.0M);
                    }

                    hours = (int)(amount);
                    amount -= (decimal)hours;

                    minutes = (int)(amount * 60);
                    amount -= (decimal)minutes;
                }
                else if (this.UnitOfMeasure.Equals(units.Minute))
                {
                    if (amount > 1440.0M)
                    {
                        days = (int)(amount / 1440);
                        amount -= ((decimal)days * 24);
                    }

                    if (amount > 60.0M)
                    {
                        hours = (int)(amount / 60M);
                        amount -= ((decimal)hours * 60M);
                    }

                    minutes = (int)amount;
                    amount -= (decimal)minutes;
                }
                else
                {
                    this.RemoveThroughDate();
                    return;
                }

                seconds = (int)(amount * 60);
                amount -= (decimal)seconds;

                millis = (int)amount * 1000;

                var timeSpan = new TimeSpan(days, hours, minutes, seconds, millis);
                this.ThroughDate = new DateTime(this.FromDate.Ticks, this.FromDate.Kind) + timeSpan;
            }
        }
    }
}