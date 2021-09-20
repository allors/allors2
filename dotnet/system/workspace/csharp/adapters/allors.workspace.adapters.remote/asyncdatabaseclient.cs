// <copyright file="RemoteDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Remote
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Protocol.Json.Api.Invoke;
    using Allors.Protocol.Json.Api.Pull;
    using Allors.Protocol.Json.Api.Push;
    using InvokeOptions = Allors.Workspace.InvokeOptions;
    using Data;
    using Protocol.Json;

    public class AsyncDatabaseClient : IAsyncDatabaseClient
    {
        private readonly DatabaseConnection databaseConnection;

        public AsyncDatabaseClient(DatabaseConnection databaseConnection) => this.databaseConnection = databaseConnection;

        public async Task<IInvokeResult> InvokeAsync(ISession session, Method method, InvokeOptions options = null) => await this.InvokeAsync(session, new[] { method }, options);

        public async Task<IInvokeResult> InvokeAsync(ISession session, Method[] methods, InvokeOptions options = null)
        {
            var invokeRequest = new InvokeRequest
            {
                l = methods.Select(v => new Invocation
                {
                    i = v.Object.Id,
                    v = ((Strategy)v.Object.Strategy).DatabaseOriginState.Version,
                    m = v.MethodType.Tag
                }).ToArray(),
                o = options != null
                    ? new Allors.Protocol.Json.Api.Invoke.InvokeOptions
                    {
                        c = options.ContinueOnError,
                        i = options.Isolated
                    }
                    : null
            };

            var invokeResponse = await this.databaseConnection.Invoke(invokeRequest);
            return new InvokeResult(session, invokeResponse);
        }

        public async Task<IPullResult> PullAsync(ISession session, params Pull[] pulls)
        {
            foreach (var pull in pulls)
            {
                if (pull.ObjectId < 0 || pull.Object?.Id < 0)
                {
                    throw new ArgumentException($"Id is not in the database");
                }

                if (pull.Object != null && pull.Object.Strategy.Class.Origin != Origin.Database)
                {
                    throw new ArgumentException($"Origin is not Database");
                }
            }

            var pullRequest = new PullRequest { l = pulls.Select(v => v.ToJson(this.databaseConnection.UnitConvert)).ToArray() };

            var pullResponse = await this.databaseConnection.Pull(pullRequest);
            return await ((Session)session).OnPull(pullResponse);
        }

        public async Task<IPullResult> CallAsync(ISession session, Procedure procedure, params Pull[] pull)
        {
            var pullRequest = new PullRequest
            {
                p = procedure.ToJson(this.databaseConnection.UnitConvert),
                l = pull.Select(v => v.ToJson(this.databaseConnection.UnitConvert)).ToArray()
            };

            var pullResponse = await this.databaseConnection.Pull(pullRequest);
            return await ((Session)session).OnPull(pullResponse);
        }

        public async Task<IPushResult> PushAsync(ISession session)
        {
            var localSession = (Session)session;
            var databaseTracker = localSession.PushToDatabaseTracker;

            var pushRequest = new PushRequest
            {
                n = databaseTracker.Created?.Select(v => ((DatabaseOriginState)v.DatabaseOriginState).PushNew()).ToArray(),
                o = databaseTracker.Changed?.Select(v => ((DatabaseOriginState)v.Strategy.DatabaseOriginState).PushExisting()).ToArray()
            };
            var pushResponse = await this.databaseConnection.Push(pushRequest);

            if (pushResponse.HasErrors)
            {
                return new PushResult(session, pushResponse);
            }


            if (pushResponse.n != null)
            {
                foreach (var pushResponseNewObject in pushResponse.n)
                {
                    var workspaceId = pushResponseNewObject.w;
                    var databaseId = pushResponseNewObject.d;
                    localSession.OnDatabasePushResponseNew(workspaceId, databaseId);
                }
            }

            databaseTracker.Created = null;
            databaseTracker.Changed = null;

            if (pushRequest.o != null)
            {
                foreach (var id in pushRequest.o.Select(v => v.d))
                {
                    var strategy = localSession.GetStrategy(id);
                    strategy.OnDatabasePushed();
                }
            }

            return new PushResult(session, pushResponse);
        }
    }
}
