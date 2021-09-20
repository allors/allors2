// <copyright file="IDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IChangeSet type.</summary>

namespace Allors.Workspace
{
    using System.Threading.Tasks;
    using Data;

    public interface IAsyncDatabaseClient
    {
        Task<IInvokeResult> InvokeAsync(ISession session, Method method, InvokeOptions options = null);

        Task<IInvokeResult> InvokeAsync(ISession session, Method[] methods, InvokeOptions options = null);

        Task<IPullResult> CallAsync(ISession session, Procedure procedure, params Pull[] pull);

        Task<IPullResult> PullAsync(ISession session, params Pull[] pull);

        Task<IPushResult> PushAsync(ISession session);
    }
}
