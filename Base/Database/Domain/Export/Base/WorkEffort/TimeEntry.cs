// <copyright file="TimeEntry.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public partial class TimeEntry
    {
        private int DecimalScale => this.Meta.AmountOfTime.Scale ?? 2;

        public decimal ActualHours
        {
            get
            {
                var frequencies = new TimeFrequencies(this.Strategy.Session);

                var through = this.ExistThroughDate ? this.ThroughDate : this.strategy.Session.Now();
                var minutes = (decimal)(through - this.FromDate).Value.TotalMinutes;
                var hours = (decimal)frequencies.Minute.ConvertToFrequency((decimal)minutes, frequencies.Hour);

                return Math.Round(hours, this.DecimalScale);
            }
        }

        public void BaseOnBuild(ObjectOnBuild method)
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

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistWorkEffort)
            {
                derivation.AddDependency(this.WorkEffort, this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExists(this, this.Meta.TimeSheetWhereTimeEntry);
            derivation.Validation.AssertAtLeastOne(this, this.Meta.WorkEffort, this.Meta.EngagementItem);

            if (this.ExistTimeSheetWhereTimeEntry)
            {
                this.Worker = this.TimeSheetWhereTimeEntry.Worker;
            }

            var billingRate = 0M;
            if (this.AssignedBillingRate.HasValue)
            {
                billingRate = this.AssignedBillingRate.Value;
            }
            else
            {
                if (this.ExistWorkEffort)
                {
                    var workEffortAssignmentRate = this.WorkEffort.WorkEffortAssignmentRatesWhereWorkEffort.FirstOrDefault(v => v.RateType.Equals(this.RateType) && v.Frequency.Equals(this.BillingFrequency));
                    if (workEffortAssignmentRate != null)
                    {
                        billingRate = workEffortAssignmentRate.Rate;
                    }
                }

                if (billingRate == 0 && this.ExistWorkEffort && this.WorkEffort.ExistCustomer)
                {
                    var partyRate = this.WorkEffort.Customer.PartyRates.FirstOrDefault(v => v.RateType.Equals(this.RateType)
                                                                               && v.Frequency.Equals(this.BillingFrequency)
                                                                               && v.FromDate <= this.FromDate && (!v.ExistThroughDate || v.ThroughDate >= this.FromDate));
                    if (partyRate != null)
                    {
                        billingRate = partyRate.Rate;
                    }
                }

                if (billingRate == 0 && this.ExistWorker && this.ExistRateType)
                {
                    var partyRate = this.Worker.PartyRates.FirstOrDefault(v => v.RateType.Equals(this.RateType)
                                                                               && v.Frequency.Equals(this.BillingFrequency)
                                                                               && v.FromDate <= this.FromDate && (!v.ExistThroughDate || v.ThroughDate >= this.FromDate));
                    if (partyRate != null)
                    {
                        billingRate = partyRate.Rate;
                    }
                }
            }

            this.BillingRate = billingRate;

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

            var minutes = 0M;
            if (this.ThroughDate != null)
            {
                var timeSpan = this.ThroughDate - this.FromDate;
                minutes = (decimal)timeSpan.Value.TotalMinutes;
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
            else if (this.ExistAssignedAmountOfTime)
            {
                minutes = (decimal)this.TimeFrequency.ConvertToFrequency((decimal)this.AssignedAmountOfTime, frequencies.Minute);

                var timeSpan = TimeSpan.FromMinutes((double)minutes);
                this.ThroughDate = new DateTime(this.FromDate.Ticks, this.FromDate.Kind) + timeSpan;

                this.AmountOfTime = this.AssignedAmountOfTime;
            }

            if (this.ExistBillingRate && this.ExistBillingFrequency)
            {
                var billableMinutes = 0M;
                if (this.BillableAmountOfTime.HasValue)
                {
                    billableMinutes = (decimal)this.TimeFrequency.ConvertToFrequency((decimal)this.BillableAmountOfTime, frequencies.Minute);
                }
                else
                {
                    billableMinutes = minutes;
                }

                var billableTimeInTimeEntryRateFrequency = Math.Round((decimal)frequencies.Minute.ConvertToFrequency(billableMinutes, this.BillingFrequency), 2);
                this.BillingAmount = Math.Round((decimal)(this.BillingRate * billableTimeInTimeEntryRateFrequency), 2);
            }
        }

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            method.SecurityTokens = this.WorkEffort?.SecurityTokens.ToArray();
            method.DeniedPermissions = this.WorkEffort?.DeniedPermissions.ToArray();
        }
    }
}
