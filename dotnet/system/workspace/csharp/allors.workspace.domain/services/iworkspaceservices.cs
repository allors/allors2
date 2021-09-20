// <copyright file="IWorkspaceServices.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public interface IWorkspaceServices : IDisposable
    {
        void OnInit(IWorkspace workspace);

        ISessionServices CreateSessionServices();

        T Get<T>();
    }
}
