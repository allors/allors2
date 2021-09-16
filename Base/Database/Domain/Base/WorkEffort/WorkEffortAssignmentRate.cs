// <copyright file="WorkEffortAssignmentRate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using Resources;

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
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                foreach (ServiceEntry serviceEntry in this.WorkEffort.ServiceEntriesWhereWorkEffort)
                {
                    iteration.AddDependency(serviceEntry, this);
                    iteration.Mark(serviceEntry);
                }
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
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.WorkEffort?.SecurityTokens.ToArray();
            }

            if (method.Restrictions == null)
            {
                method.Restrictions = this.WorkEffort?.Restrictions.ToArray();
            }
        }
    }
}
