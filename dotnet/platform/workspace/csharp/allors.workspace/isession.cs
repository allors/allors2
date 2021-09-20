// <copyright file="Session.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Allors.Protocol.Remote.Push;
    using Allors.Workspace.Meta;

    public interface ISession
    {
        bool HasChanges { get; }

        IWorkspace Workspace { get; }

        ISessionObject Create(IClass @class);

        ISessionObject Get(long id);

        PushRequest PushRequest();

        void PushResponse(PushResponse pushResponse);

        void Reset();

        void Refresh();

        IEnumerable<ISessionObject> GetAssociation(ISessionObject @object, IAssociationType associationType);
    }
}
