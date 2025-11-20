// <copyright file="IWorkspaceObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Allors.Workspace.Meta;

    public interface IWorkspaceRole
    {
        IRoleType RoleType { get; }

        object Value { get; }
    }
}
