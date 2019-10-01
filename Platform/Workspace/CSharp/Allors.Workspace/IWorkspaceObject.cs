// <copyright file="IWorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Security;
    using Allors.Workspace.Meta;

    public interface IWorkspaceObject
    {
        IWorkspace Workspace { get; }

        IClass Class { get; }

        long Id { get; }

        IWorkspaceRole[] Roles { get; }

        long Version { get; }

        bool IsPermitted(Permission permission);
    }
}
