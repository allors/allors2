// <copyright file="IDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Local
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Allors.Domain;
    using Allors.Protocol.Data;
    using Allors.Protocol.Remote.Invoke;
    using Allors.Protocol.Remote.Pull;
    using Allors.Protocol.Remote.Push;
    using Allors.Protocol.Remote.Sync;
    using Allors.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Protocol.Remote.Security;
    using Server;
    using Task = System.Threading.Tasks.Task;

    public class LocalDatabase : IDatabase
    {
        public LocalDatabase(IServiceProvider serviceProvider) :
            this(serviceProvider.GetRequiredService<IDatabaseService>(),
                serviceProvider.GetRequiredService<ITreeService>(),
                serviceProvider.GetRequiredService<IFetchService>(),
                serviceProvider.GetRequiredService<IExtentService>(),
                serviceProvider.GetRequiredService<ILogger<LocalDatabase>>())
        {
        }

        public LocalDatabase(IDatabaseService databaseService, ITreeService treeService, IFetchService fetchService, IExtentService extentService, ILogger<LocalDatabase> logger)
        {
            this.DatabaseService = databaseService;
            this.TreeService = treeService;
            this.FetchService = fetchService;
            this.ExtentService = extentService;
            this.Logger = logger;
        }

        public IDatabaseService DatabaseService { get; }

        public IExtentService ExtentService { get; }

        public IFetchService FetchService { get; }

        public ITreeService TreeService { get; }

        public ILogger<LocalDatabase> Logger { get; set; }

        public Allors.IDatabase Database => this.DatabaseService.Database;

        public Task<InvokeResponse> Invoke(InvokeRequest request, InvokeOptions options = null)
        {
            try
            {
                using (var session = this.Database.CreateSession())
                {
                    var acls = new WorkspaceAccessControlLists(session.GetUser());
                    var responseBuilder = new InvokeResponseBuilder(session, request, acls);
                    var response = responseBuilder.Build();
                    return System.Threading.Tasks.Task.FromResult(response);
                }
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "InvokeRequest {request}", request);
                throw;
            }
        }

        public Task<InvokeResponse> Invoke(string service, object args) => throw new NotSupportedException();

        public Task<PullResponse> Pull(PullRequest request)
        {
            try
            {
                using (var session = this.Database.CreateSession())
                {
                    var acls = new WorkspaceAccessControlLists(session.GetUser());
                    var response = new PullResponseBuilder(acls, this.TreeService);

                    if (request.P != null)
                    {
                        foreach (var p in request.P)
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

                    return Task.FromResult(response.Build());
                }
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "PullRequest {request}", request);
                throw;
            }
        }

        public Task<PullResponse> Pull(string service, object args) => throw new NotSupportedException();

        public Task<PushResponse> Push(PushRequest request)
        {
            try
            {
                using (var session = this.Database.CreateSession())
                {
                    var acls = new WorkspaceAccessControlLists(session.GetUser());
                    var responseBuilder = new PushResponseBuilder(session, request, acls);
                    var response = responseBuilder.Build();
                    if (!response.HasErrors)
                    {
                        session.Commit();
                    }

                    return Task.FromResult(response);
                }
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "PushRequest {request}", request);
                throw;
            }
        }

        public Task<SyncResponse> Sync(SyncRequest request)
        {
            try
            {
                using (var session = this.Database.CreateSession())
                {
                    var acls = new WorkspaceAccessControlLists(session.GetUser());
                    var responseBuilder = new SyncResponseBuilder(session, request, acls);
                    var response = responseBuilder.Build();
                    return Task.FromResult(response);
                }
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "SyncRequest {request}", request);
                throw;
            }
        }

        public Task<SecurityResponse> Security(SecurityRequest request)
        {
            try
            {
                using (var session = this.Database.CreateSession())
                {
                    var acls = new WorkspaceAccessControlLists(session.GetUser());
                    var responseBuilder = new SecurityResponseBuilder(session, request, acls);
                    var response = responseBuilder.Build();
                    return Task.FromResult(response);
                }
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "SecurityRequest {request}", request);
                throw;
            }
        }
    }
}
