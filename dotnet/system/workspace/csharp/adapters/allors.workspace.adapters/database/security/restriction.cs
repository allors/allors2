// <copyright file="LocalAccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Ranges;

    public class Revocation
    {
        public long Version { get; set; }

        public IRange<long> PermissionIds { get; set; }
    }
}
