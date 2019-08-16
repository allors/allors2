
// <copyright file="DebugTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using Allors.Adapters;

namespace Allors.Adapters.Object.SqlClient.Snapshot
{
    using Adapters;

    using Allors.Adapters.Object.SqlClient.Caching;
    using Allors.Adapters.Object.SqlClient.Debug;

    using Xunit;

    public class DebugTests : SqlClient.DebugTests, IDisposable
    {
        private readonly Profile profile;

        private readonly DebugConnectionFactory connectionFactory;
        private readonly DefaultCacheFactory cacheFactory;

        protected override IProfile Profile => this.profile;

        public DebugTests()
        {
            this.connectionFactory = new DebugConnectionFactory();
            this.cacheFactory = new DefaultCacheFactory();
            this.profile = new Profile(this.connectionFactory, this.cacheFactory);
        }

        public void Dispose() => this.profile.Dispose();
    }
}
