
// <copyright file="UserGroup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class UserGroup
    {
        public void CoreOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            if (derivation.HasChangedRole(this, this.Meta.Members))
            {
                foreach (AccessControl accessControl in this.AccessControlsWhereSubjectGroup)
                {
                    derivation.AddDependency(accessControl, this);
                }
            }
        }
    }
}
