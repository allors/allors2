// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Allors.Protocol.Remote.Pull;
    using Allors.Protocol.Remote.Sync;
    using Protocol.Remote.Security;

    public interface IWorkspace
    {
        IObjectFactory ObjectFactory { get; }

        SyncRequest Diff(PullResponse response);

        SecurityRequest Sync(SyncResponse syncResponse);

        void Security(SecurityResponse securityResponse);

        IWorkspaceObject Get(long id);
    }
}
