// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkEffortAssignmentRate.cs" company="Allors bvba">
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

using Allors.Meta;
using Resources;

namespace Allors.Domain
{
    public partial class WorkEffortAssignmentRate
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistFrequency)
            {
                this.Frequency = new TimeFrequencies(this.Strategy.Session).Hour;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (ServiceEntry serviceEntry in this.WorkEffort.ServiceEntriesWhereWorkEffort)
            {
                 derivation.AddDependency(serviceEntry, this);   
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistWorkEffort && this.ExistWorkEffortPartyAssignment)
            {
                this.WorkEffort = this.WorkEffortPartyAssignment.Assignment;
            }

            if (this.ExistRateType)
            {
                var extent = this.WorkEffort.WorkEffortAssignmentRatesWhereWorkEffort;
                extent.Filter.AddEquals(M.WorkEffortAssignmentRate.RateType, this.RateType);
                if (extent.Count > 1)
                {
                    derivation.Validation.AddError(this, this.Meta.RateType, ErrorMessages.WorkEffortRateError);
                }
            }
        }

        public void AppsDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            method.SecurityTokens = this.WorkEffort?.SecurityTokens.ToArray();
            method.DeniedPermissions = this.WorkEffort?.DeniedPermissions.ToArray();
        }
    }
}