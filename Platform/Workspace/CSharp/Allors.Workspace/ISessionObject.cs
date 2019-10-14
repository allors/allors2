// <copyright file="SessionObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Allors.Protocol.Remote.Push;
    using Allors.Workspace.Meta;

    public interface ISessionObject
    {
        long Id { get; }

        long? NewId { get; set; }

        long? Version { get; }

        IClass ObjectType { get; }

        ISession Session { get; }

        IWorkspaceObject WorkspaceObject { get; set; }

        bool HasChanges { get; }

        bool CanRead(IRoleType roleType);

        bool CanWrite(IRoleType roleType);

        bool Exist(IRoleType roleType);

        object Get(IRoleType roleType);

        void Set(IRoleType roleType, object value);

        void Add(IRoleType roleType, ISessionObject value);

        void Remove(IRoleType roleType, ISessionObject value);

        PushRequestObject Save();

        PushRequestNewObject SaveNew();

        void Reset();
    }
}
