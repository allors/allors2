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
        private const int Precision = 17;

        public decimal ActualHours
        {
            get
            {
                var conversion = new TimeFrequencies(this.strategy.Session).Hour.GetConversionFactor(this.TimeFrequency);

                if (conversion == null)
                {
                    return 0.0m;
                }
                else
                {
                    return Math.Round((this.AmountOfTime ?? 0.0m) * (decimal)conversion, Precision);
                }
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
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

            this.DeriveAmountOfTimeOrThroughDate();
        }

        private void DeriveAmountOfTimeOrThroughDate()
        {
            var conversion = this.TimeFrequency.GetConversionFactor(new TimeFrequencies(this.strategy.Session).Minute);

            if (this.ThroughDate != null)
            {
                if (conversion != null)
                {
                    var timeSpan = this.ThroughDate - this.FromDate;
                    var minutes = (decimal)timeSpan.Value.TotalMinutes;
                    this.AmountOfTime = Math.Round(minutes * (decimal)conversion, Precision);
                }
                else
                {
                    this.RemoveAmountOfTime();
                }
            }
            else if (this.AmountOfTime != null)
            {
                if (conversion != null)
                {
                    var minutes = Math.Round((decimal)this.AmountOfTime * (decimal)conversion, Precision);
                    var timeSpan = TimeSpan.FromMinutes((double)minutes);
                    this.ThroughDate = new DateTime(this.FromDate.Ticks, this.FromDate.Kind) + timeSpan;
                }
                else
                {
                    this.RemoveAmountOfTime();
                }
            }
        }
    }
}