// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterTest.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
