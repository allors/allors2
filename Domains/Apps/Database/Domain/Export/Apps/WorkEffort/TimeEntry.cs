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

using System.Linq;

namespace Allors.Domain
{
    using System;
    using Allors.Meta;

    public partial class TimeEntry
    {
        private int DecimalScale => this.Meta.AmountOfTime.Scale ?? 2;

        public decimal ActualHours
        {
            get
            {
                var frequencies = new TimeFrequencies(this.Strategy.Session);
                var minutes = (decimal)(this.ThroughDate - this.FromDate).Value.TotalMinutes;
                var hours = (decimal)frequencies.Minute.ConvertToFrequency((decimal)minutes, frequencies.Hour);

                return Math.Round(hours, DecimalScale);
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistBillingFrequency)
            {
                this.BillingFrequency = new TimeFrequencies(this.Strategy.Session).Hour;
            }

            if (!this.ExistTimeFrequency)
            {
                this.TimeFrequency = new TimeFrequencies(this.Strategy.Session).Hour;
            }

            if (!this.ExistIsBillable)
            {
                this.IsBillable = true;
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

            if (this.ExistTimeSheetWhereTimeEntry)
            {
                this.Worker = this.TimeSheetWhereTimeEntry.Worker;
            }

            if (this.ExistAssignedBillingRate)
            {
                this.BillingRate = this.AssignedBillingRate;
            }
            else
            {
                if (!this.ExistBillingRate && this.ExistWorkEffort)
                {
                    var workEffortAssignmentRate = this.WorkEffort.WorkEffortAssignmentRatesWhereWorkEffort.FirstOrDefault(v => v.RateType.Equals(this.RateType)
                                                                                                                                && v.Frequency.Equals(this.BillingFrequency)
                                                                                                                                && v.FromDate <= this.FromDate && (!v.ExistThroughDate || v.ThroughDate >= this.FromDate));
                    if (workEffortAssignmentRate != null)
                    {
                        this.BillingRate = workEffortAssignmentRate.Rate;
                    }
                }

                if (!this.ExistBillingRate && this.ExistWorker && this.ExistRateType)
                {
                    var partyRate = this.Worker.PartyRates.FirstOrDefault(v => v.RateType.Equals(this.RateType)
                                                                               && v.Frequency.Equals(this.BillingFrequency)
                                                                               && v.FromDate <= this.FromDate && (!v.ExistThroughDate || v.ThroughDate >= this.FromDate));
                    if (partyRate != null)
                    {
                        this.BillingRate = partyRate.Rate;
                    }
                }
            }

            if (this.ExistBillingRate)
            {
                derivation.Validation.AssertExists(this, this.Meta.BillingFrequency);
            }

            if (this.ExistAmountOfTime)
            {
                derivation.Validation.AssertExists(this, this.Meta.TimeFrequency);
            }

            // calculate AmountOfTime Or ThroughDate
            var frequencies = new TimeFrequencies(this.Strategy.Session);

            if (this.ThroughDate != null)
            {
                var timeSpan = this.ThroughDate - this.FromDate;
                var minutes = (decimal)timeSpan.Value.TotalMinutes;
                var amount = frequencies.Minute.ConvertToFrequency(minutes, this.TimeFrequency);

                if (amount == null)
                {
                    this.RemoveAmountOfTime();
                }
                else
                {
                    this.AmountOfTime = Math.Round((decimal)amount, 2);
                }
            }
            else if (this.AmountOfTime != null)
            {
                var minutes = this.TimeFrequency.ConvertToFrequency((decimal)this.AmountOfTime, frequencies.Minute);

                if (minutes == null)
                {
                    this.RemoveThroughDate();
                }
                else
                {
                    var timeSpan = TimeSpan.FromMinutes((double)minutes);
                    this.ThroughDate = new DateTime(this.FromDate.Ticks, this.FromDate.Kind) + timeSpan;
                }
            }
        }
    }
}