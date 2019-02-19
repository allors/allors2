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
        private int DecimalScale => this.Meta.AmountOfTime.Scale ?? 2;

        public decimal ActualHours
        {
            get
            {
                var frequencies = new TimeFrequencies(this.strategy.Session);
                var minutes = (decimal)(this.ThroughDate - this.FromDate).Value.TotalMinutes;
                var hours = (decimal)frequencies.Minute.ConvertToFrequency((decimal)minutes, frequencies.Hour);

                return Math.Round(hours, DecimalScale);
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistBillingFrequency)
            {
                this.BillingFrequency = new TimeFrequencies(this.strategy.Session).Hour;
            }

            if (!this.ExistTimeFrequency)
            {
                this.TimeFrequency = new TimeFrequencies(this.strategy.Session).Hour;
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

            if (this.ExistBillingRate)
            {
                derivation.Validation.AssertExists(this, this.Meta.BillingFrequency);
            }

            if (this.ExistAmountOfTime)
            {
                derivation.Validation.AssertExists(this, this.Meta.TimeFrequency);
            }

            if (this.ExistTimeSheetWhereTimeEntry)
            {
                this.Worker = this.TimeSheetWhereTimeEntry.Worker;
            }

            // calculate AmountOfTime Or ThroughDate
            var frequencies = new TimeFrequencies(this.strategy.Session);

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

            // BillingAmount
            if (this.ExistFromDate && this.ExistThroughDate)
            {
                var entryTimeSpan = (decimal)(this.ThroughDate - this.FromDate).Value.TotalMinutes;
                var timeInTimeEntryRateFrequency = frequencies.Minute.ConvertToFrequency(entryTimeSpan, this.BillingFrequency);

                if (this.ExistBillingRate)
                {
                    this.BillingAmount = Math.Round((decimal) (this.BillingRate * timeInTimeEntryRateFrequency), 2);
                }

                if (this.BillingAmount == 0)
                {
                    foreach (WorkEffortAssignmentRate workEffortAssignmentRate in this.WorkEffort.WorkEffortAssignmentRatesWhereWorkEffort)
                    {
                        if (workEffortAssignmentRate.WorkEffortPartyAssignment.Party.Equals(this.Worker) 
                            && workEffortAssignmentRate.RateType.Equals(this.RateType))
                        {
                            var fromDate = this.FromDate;
                            if (workEffortAssignmentRate.FromDate > this.FromDate)
                            {
                                fromDate = workEffortAssignmentRate.FromDate;
                            }

                            var throughDate = this.ThroughDate;
                            if (workEffortAssignmentRate.ExistThroughDate &&
                                workEffortAssignmentRate.ThroughDate < this.ThroughDate)
                            {
                                throughDate = workEffortAssignmentRate.ThroughDate;
                            }

                            var rateTimeSpan = (decimal)(throughDate - fromDate).Value.TotalMinutes;
                            var smallestTimeSpan = rateTimeSpan <= entryTimeSpan ? rateTimeSpan : entryTimeSpan;

                            var timeInWorkEffortRateFrequency = frequencies.Minute.ConvertToFrequency(smallestTimeSpan, workEffortAssignmentRate.Frequency);
                            this.BillingAmount += Math.Round((decimal)(workEffortAssignmentRate.Rate * timeInWorkEffortRateFrequency), 2);
                        }
                    }
                }

                //if (this.BillingAmount == 0)
                //{
                //    foreach (PartyRate partyRate in this.Worker.PartyRates)
                //    {
                //        if (this.FromDate >= partyRate.FromDate.Date
                //            && (!partyRate.ExistThroughDate || this.FromDate <= partyRate.ThroughDate))
                //        {
                //            var timeInPartyRateFrequency = frequencies.Minute.ConvertToFrequency(entryTimeSpan, partyRate.Frequency);
                //            this.BillingAmount = Math.Round((decimal)(partyRate.Rate * timeInPartyRateFrequency), 2);
                //        }
                //    }
                //}

                //if (this.BillingAmount == 0)
                //{
                //    foreach (PositionFulfillment positionFulfillment in this.Worker.PositionFulfillmentsWherePerson)
                //    {
                //        if (this.FromDate >= positionFulfillment.FromDate.Date
                //            && (!positionFulfillment.ExistThroughDate || this.FromDate <= positionFulfillment.ThroughDate))
                //        {
                //            var timeInPartyRateFrequency = frequencies.Minute.ConvertToFrequency(entryTimeSpan, positionFulfillment.Frequency);
                //            this.BillingAmount = Math.Round((decimal)(positionFulfillment.Rate * timeInPartyRateFrequency), 2);
                //        }
                //    }
                //}
            }
        }
    }
}