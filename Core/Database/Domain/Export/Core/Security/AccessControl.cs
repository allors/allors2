// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Linq;

    public partial class AccessControl
    {
        public void CoreOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, Meta.Subjects, Meta.SubjectGroups);

            this.EffectiveUsers = this.SubjectGroups.SelectMany(v => v.Members).Union(this.Subjects).ToArray();
            this.EffectivePermissions = this.Role?.Permissions;

            // Invalidate cache
            this.CacheId = Guid.NewGuid();
        }
    }
}
