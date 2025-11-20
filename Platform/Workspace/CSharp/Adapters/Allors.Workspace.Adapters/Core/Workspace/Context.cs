// <copyright file="Context.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Protocol.Remote.Invoke;
    using Allors.Protocol.Remote.Pull;
    using Allors.Protocol.Remote.Push;
    using Allors.Protocol.Remote.Sync;
    using Allors.Workspace.Data;

    public class Context
    {
        private readonly IDatabase database;
        private readonly IWorkspace workspace;

        public Context(IDatabase database, IWorkspace workspace)
        {
            this.database = database;
            this.workspace = workspace;

            // TODO: remove cast
            this.Session = new Session((Workspace)this.workspace);
        }

        public Session Session { get; }

        public Task<InvokeResponse> Invoke(Method method, InvokeOptions options = null) => this.Invoke(new[] { method }, options);

        public Task<InvokeResponse> Invoke(Method[] methods, InvokeOptions options = null)
        {
            var invokeRequest = new InvokeRequest
            {
                i = methods.Select(v => new Invocation
                {
                    i = v.Object.Id.ToString(),
                    v = v.Object.Version.ToString(),
                    m = v.MethodType.IdAsString,
                }).ToArray(),
                o = options,
            };

            return this.database.Invoke(invokeRequest);
        }

        public Task<InvokeResponse> Invoke(string service, object args) => this.database.Invoke(service, args);

        public async Task<Result> Load(params Pull[] pulls)
        {
            var pullRequest = new PullRequest { p = pulls.Select(v => v.ToJson()).ToArray() };
            var pullResponse = await this.database.Pull(pullRequest);
            var syncRequest = this.workspace.Diff(pullResponse);
            if (syncRequest.Objects.Length > 0)
            {
                await this.Load(syncRequest);
            }

            var result = new Result(this.Session, pullResponse);
            return result;
        }

        public async Task<Result> Load(object args, string pullService = null)
        {
            if (args is Pull pull)
            {
                args = new PullRequest { p = new[] { pull.ToJson() } };
            }

            if (args is IEnumerable<Pull> pulls)
            {
                args = new PullRequest { p = pulls.Select(v => v.ToJson()).ToArray() };
            }

            var pullResponse = await this.database.Pull(pullService, args);
            var syncRequest = this.workspace.Diff(pullResponse);

            if (syncRequest.Objects.Length > 0)
            {
                await this.Load(syncRequest);
            }

            var result = new Result(this.Session, pullResponse);
            return result;
        }

        public async Task<PushResponse> Save()
        {
            var saveRequest = this.Session.PushRequest();
            var pushResponse = await this.database.Push(saveRequest);
            if (!pushResponse.HasErrors)
            {
                this.Session.PushResponse(pushResponse);

                var objects = saveRequest.objects.Select(v => v.i).ToArray();
                if (pushResponse.newObjects != null)
                {
                    objects = objects.Union(pushResponse.newObjects.Select(v => v.i)).ToArray();
                }

                var syncRequests = new SyncRequest
                {
                    Objects = objects,
                };

                await this.Load(syncRequests);

                this.Session.Reset();
            }

            return pushResponse;
        }

        private async Task Load(SyncRequest syncRequest)
        {
            var syncResponse = await this.database.Sync(syncRequest);
            var securityRequest = this.workspace.Sync(syncResponse);

            if (securityRequest != null)
            {
                var securityResponse = await this.database.Security(securityRequest);
                securityRequest = this.workspace.Security(securityResponse);

                if (securityRequest != null)
                {
                    securityResponse = await this.database.Security(securityRequest);
                    this.workspace.Security(securityResponse);
                }
            }
        }
    }
}
