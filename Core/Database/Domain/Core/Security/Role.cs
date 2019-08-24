// <copyright file="Role.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Domain
{
    public partial class Role
    {
        public void CoreOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.HasChangedRole(this, this.Meta.Permissions))
            {
                foreach (AccessControl accessControl in this.AccessControlsWhereRole)
                {
                    derivation.AddDependency(accessControl, this);
                }
            }
        }
    }
}
