// <copyright file="RemoteSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Remote
{
    using System.Threading.Tasks;
    using Allors.Protocol.Json.Api.Pull;
    using Meta;

    public class Session : Adapters.Session
    {
        internal Session(Workspace workspace, ISessionServices sessionServices) : base(workspace, sessionServices)
        {
            this.Services.OnInit(this);
            this.DatabaseConnection = workspace.DatabaseConnection;
        }

        private DatabaseConnection DatabaseConnection { get; }

        public new Workspace Workspace => (Workspace)base.Workspace;

        public override T Create<T>(IClass @class)
        {
            var workspaceId = base.Workspace.DatabaseConnection.NextId();
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
            var databaseRecord = (DatabaseRecord)base.Workspace.DatabaseConnection.GetRecord(id);
            var strategy = new Strategy(this, databaseRecord);
            this.AddStrategy(strategy);

            this.ChangeSetTracker.OnInstantiated(strategy);
        }

        protected override Adapters.Strategy InstantiateWorkspaceStrategy(long id)
        {
            if (!base.Workspace.WorkspaceClassByWorkspaceId.TryGetValue(id, out var @class))
            {
                return null;
            }

            var strategy = new Strategy(this, @class, id);
            this.AddStrategy(strategy);

            this.ChangeSetTracker.OnInstantiated(strategy);

            return strategy;
        }

        internal async Task<IPullResult> OnPull(PullResponse pullResponse)
        {
            var pullResult = new PullResult(this, pullResponse);

            if (pullResult.HasErrors)
            {
                return pullResult;
            }

            var syncRequest = this.Workspace.DatabaseConnection.OnPullResponse(pullResponse);
            if (syncRequest.o.Length > 0)
            {
                var database = (DatabaseConnection)base.Workspace.DatabaseConnection;
                var syncResponse = await database.Sync(syncRequest);
                var accessRequest = database.OnSyncResponse(syncResponse);

                if (accessRequest != null)
                {
                    var accessResponse = await database.Access(accessRequest);
                    var permissionRequest = database.AccessResponse(accessResponse);
                    if (permissionRequest != null)
                    {
                        var permissionResponse = await database.Permission(permissionRequest);
                        database.PermissionResponse(permissionResponse);
                    }
                }
            }

            foreach (var v in pullResponse.p)
            {
                if (this.StrategyByWorkspaceId.TryGetValue(v.i, out var strategy))
                {
                    strategy.DatabaseOriginState.OnPulled(pullResult);
                }
                else
                {
                    this.InstantiateDatabaseStrategy(v.i);
                }
            }

            return pullResult;
        }
    }
}
