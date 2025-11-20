// <copyright file="Role.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Domain
{
    public partial class Role
    {
        public void CoreOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRole(this, this.Meta.Permissions))
            {
                foreach (AccessControl accessControl in this.AccessControlsWhereRole)
                {
                    iteration.AddDependency(accessControl, this);
                    iteration.Mark(accessControl);
                }
            }
        }
    }
}
