// <copyright file="v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Local
{
    using System;
    using System.Threading.Tasks;

    public class AsyncDatabaseClient : IAsyncDatabaseClient
    {
        public Task<IInvokeResult> InvokeAsync(ISession session, Method method, InvokeOptions options = null) =>
            this.InvokeAsync(session, new[] { method }, options);

        public Task<IInvokeResult> InvokeAsync(ISession session, Method[] methods, InvokeOptions options = null)
        {
            var result = new Invoke((Session)session);
            result.Execute(methods, options);
            return Task.FromResult<IInvokeResult>(result);
        }

        public Task<IPullResult> CallAsync(ISession session, Data.Procedure procedure, params Data.Pull[] pull)
        {
            var result = new Pull((Session)session);

            result.Execute(procedure);
            result.Execute(pull);

            ((Session)session).OnPulled(result);

            return Task.FromResult<IPullResult>(result);
        }

        public Task<IPullResult> PullAsync(ISession session, params Data.Pull[] pulls)
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

            var result = new Pull((Session)session);
            result.Execute(pulls);

            ((Session)session).OnPulled(result);

            return Task.FromResult<IPullResult>(result);
        }

        public Task<IPushResult> PushAsync(ISession session)
        {
            var localSession = (Session)session;
            var databaseTracker = localSession.PushToDatabaseTracker;

            var result = new Push(localSession);

            result.Execute(databaseTracker);

            if (result.HasErrors)
            {
                return Task.FromResult<IPushResult>(result);
            }

            databaseTracker.Changed = null;

            if (result.ObjectByNewId?.Count > 0)
            {
                foreach (var kvp in result.ObjectByNewId)
                {
                    var workspaceId = kvp.Key;
                    var databaseId = kvp.Value.Id;

                    localSession.OnDatabasePushResponseNew(workspaceId, databaseId);
                }
            }

            databaseTracker.Created = null;

            foreach (var @object in result.Objects)
            {
                var strategy = localSession.GetStrategy(@object.Id);
                strategy.OnDatabasePushed();
            }

            return Task.FromResult<IPushResult>(result);
        }
    }
}
