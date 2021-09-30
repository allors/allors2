// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;

    public interface IWorkspaceResult
    {
        bool HasErrors { get; }

        IEnumerable<IObject> VersionErrors { get; }

        IEnumerable<IObject> MissingErrors { get; }

        IEnumerable<IObject> MergeErrors { get; }
    }
}
