// <copyright file="IRecord.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Meta;

    public interface IRecord
    {
        long Version { get; }

        object GetRole(IRoleType roleType);
    }
}
