// <copyright file="WorkEffortPartyAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Resources;

    public partial class WorkEffortPartyAssignment
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistAssignmentRates)
            {
                derivation.Validation.AddError(this, this.Meta.AssignmentRates, ErrorMessages.WorkEffortRateError);
            }
        }

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.Assignment?.SecurityTokens.ToArray();
            }

            if (method.Restrictions == null)
            {
                method.Restrictions = this.Assignment?.Restrictions.ToArray();
            }
        }
    }
}
