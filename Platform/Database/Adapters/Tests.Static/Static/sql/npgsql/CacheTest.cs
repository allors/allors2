// <copyright file="CacheTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql
{
    using System;
    using Xunit;

    [Collection(Fixture.Collection)]
    public class CacheTest : Adapters.CacheTest, IDisposable
    {
        private readonly Profile profile;

        public CacheTest(Fixture fixture) => this.profile = new Profile(fixture.PgServer);

        public override void Dispose() => this.profile.Dispose();

        protected override IDatabase CreateDatabase() => this.profile.CreateDatabase();
    }
}
