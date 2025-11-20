// <copyright file="UserGroup.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class UserGroup
    {
        public void CoreOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (changeSet.HasChangedRole(this, this.Meta.Members))
            {
                foreach (AccessControl accessControl in this.AccessControlsWhereSubjectGroup)
                {
                    iteration.AddDependency(accessControl, this);
                    iteration.Mark(accessControl);
                }
            }
        }
    }
}
