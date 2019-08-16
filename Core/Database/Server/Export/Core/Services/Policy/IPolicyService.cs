// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPolicyService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
