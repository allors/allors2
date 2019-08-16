
// <copyright file="WorkEffortAssignmentRate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Meta;
using Resources;

namespace Allors.Domain
{
    public partial class WorkEffortAssignmentRate
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistFrequency)
            {
                this.Frequency = new TimeFrequencies(this.Strategy.Session).Hour;
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (ServiceEntry serviceEntry in this.WorkEffort.ServiceEntriesWhereWorkEffort)
            {
                derivation.AddDependency(serviceEntry, this);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
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

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            method.SecurityTokens = this.WorkEffort?.SecurityTokens.ToArray();
            method.DeniedPermissions = this.WorkEffort?.DeniedPermissions.ToArray();
        }
    }
}
