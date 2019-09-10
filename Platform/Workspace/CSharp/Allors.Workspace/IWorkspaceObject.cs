// <copyright file="IWorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public interface IWorkspaceObject
    {
        long Id { get; }

        Dictionary<string, object> Methods { get; }

        IClass ObjectType { get; }

        Dictionary<string, object> Roles { get; }

        string UserSecurityHash { get; }

        long Version { get; }

        IWorkspace Workspace { get; }

        bool CanExecute(string methodTypeName);

        bool CanRead(string roleTypeName);

        bool CanWrite(string roleTypeName);
    }
}
