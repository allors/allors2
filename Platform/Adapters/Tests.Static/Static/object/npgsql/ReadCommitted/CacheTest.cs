// <copyright file="CacheTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Xunit;

namespace Allors.Adapters.Object.Npgsql.ReadCommitted
{
    using System;

    [Collection(Fixture.Collection)]
    public class CacheTest : Adapters.CacheTest, IDisposable
    {
        private readonly Profile profile;

        public CacheTest(Fixture fixture) => this.profile = new Profile(fixture.Server);

        public override void Dispose() => this.profile.Dispose();

        protected override IDatabase CreateDatabase() => this.profile.CreateDatabase();
    }
}
