// <copyright file="IDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Threading.Tasks;
    using Allors.Protocol.Remote.Invoke;
    using Allors.Protocol.Remote.Pull;
    using Allors.Protocol.Remote.Push;
    using Allors.Protocol.Remote.Sync;
    using Protocol.Remote.Security;

    public interface IDatabase
    {
        Task<PullResponse> Pull(PullRequest pullRequest);

        Task<PullResponse> Pull(string service, object args);

        Task<SyncResponse> Sync(SyncRequest syncRequest);

        Task<PushResponse> Push(PushRequest pushRequest);

        Task<InvokeResponse> Invoke(InvokeRequest invokeRequest, InvokeOptions options = null);

        Task<InvokeResponse> Invoke(string service, object args);

        Task<SecurityResponse> Security(SecurityRequest securityRequest);
    }
}
