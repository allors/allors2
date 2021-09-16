// <copyright file="WorkEffortFixedAssetAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Resources;

    public partial class WorkEffortFixedAssetAssignment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistFixedAsset)
                {
                    iteration.AddDependency(this.FixedAsset, this);
                    iteration.Mark(this.FixedAsset);
                }
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
