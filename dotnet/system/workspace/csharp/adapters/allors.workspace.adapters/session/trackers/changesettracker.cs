// <copyright file="ChangeSetTracker.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>

namespace Allors.Workspace.Adapters
{
    using System.Collections.Generic;

    public sealed class ChangeSetTracker
    {
        public ISet<IStrategy> Created { get; set; }

        public ISet<IStrategy> Instantiated { get; set; }

        public ISet<DatabaseOriginState> DatabaseOriginStates { get; set; }

        public ISet<WorkspaceOriginState> WorkspaceOriginStates { get; set; }

        public ISet<SessionOriginState> SessionOriginStates { get; set; }

        public void OnCreated(Strategy strategy) => (this.Created ??= new HashSet<IStrategy>()).Add(strategy);

        public void OnInstantiated(Strategy strategy) => (this.Instantiated ??= new HashSet<IStrategy>()).Add(strategy);

        public void OnDatabaseChanged(DatabaseOriginState state) => (this.DatabaseOriginStates ??= new HashSet<DatabaseOriginState>()).Add(state);

        public void OnWorkspaceChanged(WorkspaceOriginState state) => (this.WorkspaceOriginStates ??= new HashSet<WorkspaceOriginState>()).Add(state);

        public void OnSessionChanged(SessionOriginState state) => (this.SessionOriginStates ??= new HashSet<SessionOriginState>()).Add(state);
    }
}
