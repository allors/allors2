// <copyright file="IPolicyService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using Polly;

    public interface IPolicyService
    {
        Policy PullPolicy { get; }

        Policy SyncPolicy { get; }

        Policy PushPolicy { get; }

        Policy InvokePolicy { get; }
    }
}
