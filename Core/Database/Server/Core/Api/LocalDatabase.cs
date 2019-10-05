// <copyright file="IDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Local
{
    using System.Threading.Tasks;
    using Allors.Domain;
    using Allors.Protocol.Data;
    using Allors.Protocol.Remote.Invoke;
    using Allors.Protocol.Remote.Pull;
    using Allors.Protocol.Remote.Push;
    using Allors.Protocol.Remote.Sync;
    using Allors.Services;
    using Protocol.Remote.Security;
    using Server;

    public class LocalDatabase : IDatabase
    {
        public LocalDatabase(IDatabaseService databaseService, ITreeService treeService, IFetchService fetchService, IExtentService extentService)
        {
            this.DatabaseService = databaseService;
            this.TreeService = treeService;
            this.FetchService = fetchService;
            this.ExtentService = extentService;
        }

        public IDatabaseService DatabaseService { get; }

        public IExtentService ExtentService { get; }

        public IFetchService FetchService { get; }

        public ITreeService TreeService { get; }

        public Allors.IDatabase Database => this.DatabaseService.Database;

        public Task<InvokeResponse> Invoke(InvokeRequest invokeRequest, InvokeOptions options = null)
        {
            using (var session = this.Database.CreateSession())
            {
                var acls = new WorkspaceAccessControlLists(session.GetUser());
                var responseBuilder = new InvokeResponseBuilder(session, invokeRequest, acls);
                var response = responseBuilder.Build();
                return System.Threading.Tasks.Task.FromResult(response);
            }
        }

        public Task<InvokeResponse> Invoke(string service, object args) => throw new System.NotSupportedException();

        public Task<PullResponse> Pull(PullRequest pullRequest)
        {
            using (var session = this.Database.CreateSession())
            {
                var acls = new WorkspaceAccessControlLists(session.GetUser());
                var response = new PullResponseBuilder(acls, this.TreeService);

                if (pullRequest.P != null)
                {
                    foreach (var p in pullRequest.P)
                    {
                        var pull = p.Load(session);

                        if (pull.Object != null)
                        {
                            var pullInstantiate = new PullInstantiate(session, pull, acls, this.FetchService);
                            pullInstantiate.Execute(response);
                        }
                        else
                        {
                            var pullExtent = new PullExtent(session, pull, acls, this.ExtentService, this.FetchService);
                            pullExtent.Execute(response);
                        }
                    }
                }

                return System.Threading.Tasks.Task.FromResult(response.Build());
            }
        }

        public Task<PullResponse> Pull(string service, object args) => throw new System.NotSupportedException();

        public Task<PushResponse> Push(PushRequest pushRequest)
        {
            using (var session = this.Database.CreateSession())
            {
                var acls = new WorkspaceAccessControlLists(session.GetUser());
                var responseBuilder = new PushResponseBuilder(session, pushRequest, acls);
                var response = responseBuilder.Build();
                if (!response.HasErrors)
                {
                    session.Commit();
                }

                return System.Threading.Tasks.Task.FromResult(response);
            }
        }

        public Task<SyncResponse> Sync(SyncRequest syncRequest)
        {
            using (var session = this.Database.CreateSession())
            {
                var acls = new WorkspaceAccessControlLists(session.GetUser());
                var responseBuilder = new SyncResponseBuilder(session, syncRequest, acls);
                var response = responseBuilder.Build();
                return System.Threading.Tasks.Task.FromResult(response);
            }
        }

        public Task<SecurityResponse> Security(SecurityRequest securityRequest)
        {
            using (var session = this.Database.CreateSession())
            {
                var acls = new WorkspaceAccessControlLists(session.GetUser());
                var responseBuilder = new SecurityResponseBuilder(session, securityRequest, acls);
                var response = responseBuilder.Build();
                return System.Threading.Tasks.Task.FromResult(response);
            }
        }
    }
}
