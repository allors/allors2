// <copyright file="UnitTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql.ReadCommitted
{
    using Xunit;

    [Collection(Fixture.Collection)]
    public class UnitTest : SqlClient.UnitTest
    {
        private readonly Profile profile;

        public UnitTest(Fixture fixture) => this.profile = new Profile(fixture.Server);

        protected override IProfile Profile => this.profile;

        public override void Dispose() => this.profile.Dispose();
    }
}
