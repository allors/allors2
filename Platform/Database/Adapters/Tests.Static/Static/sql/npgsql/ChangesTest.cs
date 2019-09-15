// <copyright file="ChangesTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Npgsql
{
    using System;
    using Adapters;
    using Xunit;

    [Collection(Fixture.Collection)]
    public class ChangesTest : Adapters.ChangesTest, IDisposable
    {
        private readonly Adapters.Profile profile;

        public ChangesTest(Fixture fixture) => this.profile = new Profile(fixture.Server);

        protected override IProfile Profile => this.profile;

        public override void Dispose() => this.profile.Dispose();
    }
}
