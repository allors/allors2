// <copyright file="IWorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Allors.Workspace.Meta;
    using Security;

    public interface IWorkspaceObject
    {
        IAccessControlList AccessControlList { get; }

        IClass Class { get; }

        long Id { get; }

        IWorkspaceRole[] Roles { get; }

        long Version { get; }

        IWorkspace Workspace { get; }
    }
}
