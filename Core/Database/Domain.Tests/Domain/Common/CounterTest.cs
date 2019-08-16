
// <copyright file="CounterTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Tests
{
    using System;
    using System.Data;

    using Allors;
    using Allors.Domain;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    public class CounterTest : DomainTest
    {
        [Fact]
        public void Meta()
        {
            var counterBuilder = new CounterBuilder(this.Session).Build();

            Assert.True(counterBuilder.ExistUniqueId);
            Assert.NotEqual(Guid.Empty, counterBuilder.UniqueId);

            Assert.Equal(0, counterBuilder.Value);

            var secondCounterBuilder = new CounterBuilder(this.Session).Build();

            Assert.NotEqual(counterBuilder.UniqueId, secondCounterBuilder.UniqueId);
        }

        [Fact]
        public void NextValue()
        {
            var id = Guid.NewGuid();

            new CounterBuilder(this.Session).WithUniqueId(id).Build();
            this.Session.Derive(true);
            this.Session.Commit();

            Assert.Equal(1, Counters.NextValue(this.Session, id));
            Assert.Equal(2, Counters.NextValue(this.Session, id));
            Assert.Equal(3, Counters.NextValue(this.Session, id));
            Assert.Equal(4, Counters.NextValue(this.Session, id));
        }
    }
}
