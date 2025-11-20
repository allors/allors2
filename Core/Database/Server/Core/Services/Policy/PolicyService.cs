// <copyright file="PolicyService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
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
