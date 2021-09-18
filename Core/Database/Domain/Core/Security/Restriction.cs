// <copyright file="Restrictions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

    public partial class Restriction
    {
        public void CoreOnDerive(ObjectOnDerive method) => this.IsWorkspace = this.DeniedPermissions.Any(v => v.OperandType.Workspace);
    }
}
