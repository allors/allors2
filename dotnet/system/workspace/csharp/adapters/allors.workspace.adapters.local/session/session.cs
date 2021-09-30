// <copyright file="LocalSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Local
{
    using System;
    using System.Threading.Tasks;
    using Meta;

    public class Session : Adapters.Session
    {
        internal Session(Adapters.Workspace workspace, ISessionServices sessionServices) : base(workspace, sessionServices) => this.Services.OnInit(this);

        public new Workspace Workspace => (Workspace)base.Workspace;
        
        public override T Create<T>(IClass @class)
        {
            var workspaceId = this.Workspace.DatabaseConnection.NextId();
            var strategy = new Strategy(this, @class, workspaceId);
            this.AddStrategy(strategy);

            if (@class.Origin != Origin.Session)
            {
                this.PushToWorkspaceTracker.OnCreated(strategy);
                if (@class.Origin == Origin.Database)
                {
                    this.PushToDatabaseTracker.OnCreated(strategy);
                }
            }

            this.ChangeSetTracker.OnCreated(strategy);

            return (T)strategy.Object;
        }

        private void InstantiateDatabaseStrategy(long id)
        {
            var databaseRecord = this.Workspace.DatabaseConnection.GetRecord(id);
            var strategy = new Strategy(this, (DatabaseRecord)databaseRecord);
            this.AddStrategy(strategy);

            this.ChangeSetTracker.OnInstantiated(strategy);
        }

        protected override Adapters.Strategy InstantiateWorkspaceStrategy(long id)
        {
            if (!this.Workspace.WorkspaceClassByWorkspaceId.TryGetValue(id, out var @class))
            {
                return null;
            }

            var strategy = new Strategy(this, @class, id);
            this.AddStrategy(strategy);

            this.ChangeSetTracker.OnInstantiated(strategy);

            return strategy;
        }

        internal void OnPulled(Pull pull)
        {
            var syncObjects = this.Workspace.DatabaseConnection.ObjectsToSync(pull);
            this.Workspace.DatabaseConnection.Sync(syncObjects, pull.AccessControl);

            foreach (var databaseObject in pull.DatabaseObjects)
            {
                if (this.StrategyByWorkspaceId.TryGetValue(databaseObject.Id, out var strategy))
                {
                    strategy.DatabaseOriginState.OnPulled(pull);
                }
                else
                {
                    this.InstantiateDatabaseStrategy(databaseObject.Id);
                }
            }
        }
    }
}
