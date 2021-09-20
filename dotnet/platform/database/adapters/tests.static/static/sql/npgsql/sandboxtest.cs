// <copyright file="SandboxTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql
{
    using Xunit;
    using System;
    using Adapters;

    [Collection(Fixture.Collection)]
    public class SandboxTest : Adapters.SandboxTest, IDisposable
    {
        private readonly Profile profile;

        public SandboxTest(Fixture fixture) => this.profile = new Profile(fixture.PgServer);

        protected override IProfile Profile => this.profile;

        public override void Dispose() => this.profile.Dispose();
    }
}
