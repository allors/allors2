// <copyright file="PushToWorkspaceTracker.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>

namespace Allors.Workspace.Adapters
{
    using System.Collections.Generic;

    public sealed class PushToWorkspaceTracker
    {
        public ISet<Strategy> Created { get; set; }

        public ISet<WorkspaceOriginState> Changed { get; set; }

        public void OnCreated(Strategy strategy) => (this.Created ??= new HashSet<Strategy>()).Add(strategy);

        public void OnChanged(WorkspaceOriginState state) => (this.Changed ??= new HashSet<WorkspaceOriginState>()).Add(state);
    }
}
