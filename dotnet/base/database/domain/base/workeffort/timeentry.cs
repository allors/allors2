// <copyright file="TimeEntry.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Resources;

    public partial class TimeEntry
    {
        public decimal ActualHours
        {
            get
            {
                var frequencies = new TimeFrequencies(this.Strategy.Session);

                var through = this.ExistThroughDate ? this.ThroughDate : this.Session().Now();
                var minutes = (decimal)(through - this.FromDate).Value.TotalMinutes;
                var hours = (decimal)frequencies.Minute.ConvertToFrequency((decimal)minutes, frequencies.Hour);

                return Rounder.RoundDecimal(hours, this.DecimalScale);
            }
        }

        private int DecimalScale => this.Meta.AmountOfTime.Scale ?? 2;

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

        public void BaseOnInit(ObjectOnInit method)
        {
            var useInternalRate = this.WorkEffort?.Customer is Organisation organisation && organisation.IsInternalOrganisation;

            if (!this.ExistRateType)
            {
                this.RateType = useInternalRate ? new RateTypes(this.Session()).InternalRate : new RateTypes(this.Session()).StandardRate;
            }

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
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistWorkEffort)
                {
                    iteration.AddDependency(this.WorkEffort, this);
                    iteration.Mark(this.WorkEffort);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExists(this, this.Meta.TimeSheetWhereTimeEntry);
            derivation.Validation.AssertAtLeastOne(this, this.Meta.WorkEffort, this.Meta.EngagementItem);

            var useInternalRate = this.WorkEffort?.Customer is Organisation organisation && organisation.IsInternalOrganisation;
            var rateType = useInternalRate ? new RateTypes(this.Session()).InternalRate : this.RateType;

            if (this.ExistTimeSheetWhereTimeEntry)
            {
                this.Worker = this.TimeSheetWhereTimeEntry.Worker;
            }

            //if (this.ExistWorker)
            //{
            //    var otherActiveTimeEntry = this.Worker.TimeEntriesWhereWorker.FirstOrDefault(v =>
            //                    v.Id != this.Id
            //                    && ((v.FromDate < this.FromDate && (!v.ExistThroughDate || v.ThroughDate > this.FromDate))
            //                        || (v.FromDate > this.FromDate && v.FromDate < this.ThroughDate)));

            //    if (otherActiveTimeEntry != null)
            //    {
            //        derivation.Validation.AddError(this, this.Meta.Worker, ErrorMessages.WorkerActiveTimeEntry.Replace("{0}", otherActiveTimeEntry.WorkEffort?.WorkEffortNumber));
            //    }
            //}

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

                if (billingRate == 0
                    && this.ExistWorkEffort
                    && this.WorkEffort.ExistCustomer
                    && (this.WorkEffort.Customer as Organisation)?.IsInternalOrganisation == false)
                {
                    var partyRate = this.WorkEffort.Customer.PartyRates.FirstOrDefault(v => v.RateType.Equals(rateType)
                                                                               && v.Frequency.Equals(this.BillingFrequency)
                                                                               && v.FromDate <= this.FromDate && (!v.ExistThroughDate || v.ThroughDate >= this.FromDate));
                    if (partyRate != null)
                    {
                        billingRate = partyRate.Rate;
                    }
                }

                if (billingRate == 0 && this.ExistWorker && this.ExistRateType)
                {
                    var partyRate = this.Worker.PartyRates.FirstOrDefault(v => v.RateType.Equals(rateType)
                                                                               && v.Frequency.Equals(this.BillingFrequency)
                                                                               && v.FromDate <= this.FromDate && (!v.ExistThroughDate || v.ThroughDate >= this.FromDate));
                    if (partyRate != null)
                    {
                        billingRate = partyRate.Rate;
                    }
                }

                if (billingRate == 0 && this.ExistWorkEffort && this.WorkEffort.ExistExecutedBy)
                {
                    var partyRate = this.WorkEffort.ExecutedBy.PartyRates.FirstOrDefault(v => v.RateType.Equals(rateType)
                                                                               && v.Frequency.Equals(this.BillingFrequency)
                                                                               && v.FromDate <= this.FromDate && (!v.ExistThroughDate || v.ThroughDate >= this.FromDate));
                    if (partyRate != null)
                    {
                        billingRate = partyRate.Rate;
                    }
                }
            }

            // rate before uplift
            var costRate = billingRate;

            if (useInternalRate && this.WorkEffort.Customer != this.WorkEffort.ExecutedBy)
            {
                billingRate = Rounder.RoundDecimal(billingRate * (1 + this.strategy.Session.GetSingleton().Settings.InternalLabourSurchargePercentage / 100), 2);
            }

            this.BillingRate = billingRate;

            if (this.ExistBillingRate)
            {
                derivation.Validation.AssertExists(this, this.Meta.BillingFrequency);
            }

            // calculate AmountOfTime Or ThroughDate
            var frequencies = new TimeFrequencies(this.Strategy.Session);
            this.AmountOfTime = null;

            if (this.ThroughDate != null)
            {
                var timeSpan = this.ThroughDate - this.FromDate;
                this.AmountOfTimeInMinutes = (decimal)timeSpan.Value.TotalMinutes;
                var amount = frequencies.Minute.ConvertToFrequency(this.AmountOfTimeInMinutes, this.TimeFrequency);

                if (amount == null)
                {
                    this.RemoveAmountOfTime();
                }
                else
                {
                    this.AmountOfTime = Rounder.RoundDecimal((decimal)amount, 2);
                }
            }
            else if (this.ExistAssignedAmountOfTime)
            {
                this.AmountOfTimeInMinutes = (decimal)this.TimeFrequency.ConvertToFrequency((decimal)this.AssignedAmountOfTime, frequencies.Minute);

                var timeSpan = TimeSpan.FromMinutes((double)this.AmountOfTimeInMinutes);
                this.ThroughDate = new DateTime(this.FromDate.Ticks, this.FromDate.Kind) + timeSpan;

                this.AmountOfTime = this.AssignedAmountOfTime;
            }
            else
            {
                var timeSpan = this.Session().Now() - this.FromDate;
                this.AmountOfTimeInMinutes = (decimal)timeSpan.TotalMinutes;
                var amount = frequencies.Minute.ConvertToFrequency(this.AmountOfTimeInMinutes, this.TimeFrequency);

                if (amount == null)
                {
                    this.RemoveAmountOfTime();
                }
                else
                {
                    this.AmountOfTime = Rounder.RoundDecimal((decimal)amount, 2);
                }
            }

            if (this.ExistBillingRate && this.ExistBillingFrequency)
            {
                if (this.BillableAmountOfTime.HasValue)
                {
                    this.BillableAmountOfTimeInMinutes = (decimal)this.TimeFrequency.ConvertToFrequency((decimal)this.BillableAmountOfTime, frequencies.Minute);
                }
                else
                {
                    this.BillableAmountOfTimeInMinutes = this.AmountOfTimeInMinutes;
                }

                var billableTimeInTimeEntryRateFrequency = Rounder.RoundDecimal((decimal)frequencies.Minute.ConvertToFrequency(this.BillableAmountOfTimeInMinutes, this.BillingFrequency), 2);

                this.BillingAmount = Rounder.RoundDecimal((decimal)(this.BillingRate * billableTimeInTimeEntryRateFrequency), 2);

                var timeSpendInTimeEntryRateFrequency = Rounder.RoundDecimal((decimal)frequencies.Minute.ConvertToFrequency(this.AmountOfTimeInMinutes, this.BillingFrequency), 2);
                this.Cost = Rounder.RoundDecimal((decimal)(costRate * timeSpendInTimeEntryRateFrequency), 2);
            }
        }

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                var workEffortSecurityTokens = this.WorkEffort?.SecurityTokens ?? Array.Empty<SecurityToken>();
                method.SecurityTokens = workEffortSecurityTokens.Append(this.Worker?.OwnerSecurityToken).ToArray();
            }

            if (method.Restrictions == null)
            {
                method.Restrictions = this.WorkEffort?.Restrictions.ToArray();
            }
        }
    }
}
