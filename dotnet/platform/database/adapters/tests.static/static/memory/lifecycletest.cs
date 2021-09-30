// <copyright file="LifeCycleTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using Allors;
    using Adapters;
    using Xunit;

    public class LifeCycleTest : Adapters.LifeCycleTest, IDisposable
    {
        private readonly Profile profile = new Profile();

        protected override IProfile Profile => this.profile;

        [Fact]
        public override void DifferentSessions()
        {
        }

        public override void Dispose() => this.profile.Dispose();

        protected override void SwitchDatabase()
        {
        }

        protected override IDatabase CreatePopulation() => this.profile.CreateDatabase();

        protected override ISession CreateSession() => this.profile.CreateSession();
    }
}
