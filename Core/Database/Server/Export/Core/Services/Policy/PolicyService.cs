// <copyright file="PolicyService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Allors.Services
{
    using System;
    using System.Data.Common;
    using Polly;

    public class PolicyService : IPolicyService
    {
        public PolicyService()
        {
            var defaultPolicy = Policy
                .Handle<DbException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            this.PullPolicy = defaultPolicy;
            this.SyncPolicy = defaultPolicy;
            this.PushPolicy = defaultPolicy;
            this.InvokePolicy = defaultPolicy;
        }

        public Policy PullPolicy { get; }

        public Policy SyncPolicy { get; }

        public Policy PushPolicy { get; }

        public Policy InvokePolicy { get; }
    }
}
