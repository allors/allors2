// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersioningTests.cs" company="Allors bvba">
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
    using Allors;
    using Allors.Domain;

    using Xunit;
    
    public class VersioningTests : DomainTest
    {
        [Fact]
        public void InitialNothing()
        {
            var order = new OrderBuilder(this.Session).Build();

            this.Session.Derive();

            Assert.True(order.ExistCurrentVersion);
            Assert.True(order.ExistAllVersions);
            Assert.Equal(1, order.AllVersions.Count);

            var version = order.CurrentVersion;

            Assert.Equal(order.Amount, version.Amount);
        }

        [Fact]
        public void VersionedUnitRole()
        {
            var order = new OrderBuilder(this.Session)
                .WithAmount(10m)
                .Build();

            this.Session.Derive();

            Assert.True(order.ExistCurrentVersion);
            Assert.True(order.ExistAllVersions);
            Assert.Equal(1, order.AllVersions.Count);

            var version = order.CurrentVersion;

            Assert.Equal(10m, version.Amount);
            Assert.False(version.ExistCurrentObjectState);
            Assert.False(version.ExistOrderLines);

        }

        [Fact]
        public void NonVersionedUnitRole()
        {
            var order = new OrderBuilder(this.Session)
                .WithAmount(10m)
                .Build();

            this.Session.Derive();

            var currentVersion = order.CurrentVersion;

            order.NonVersionedAmount = 20m;

            this.Session.Derive();

            Assert.True(order.ExistAllVersions);
            Assert.Equal(1, order.AllVersions.Count);
            Assert.Equal(currentVersion, order.CurrentVersion);
        }

        [Fact]
        public void InitialCompositeRole()
        {
            var initialObjectState = new OrderObjectStates(this.Session).Initial;

            var order = new OrderBuilder(this.Session)
                .WithCurrentObjectState(initialObjectState)
                .Build();

            this.Session.Derive();

            Assert.True(order.ExistCurrentVersion);
            Assert.True(order.ExistAllVersions);
            Assert.Equal(1, order.AllVersions.Count);

            var version = order.CurrentVersion;

            Assert.False(version.ExistAmount);
            Assert.Equal(initialObjectState, version.CurrentObjectState);
            Assert.False(version.ExistOrderLines);
        }

        [Fact]
        public void InitialCompositeRoles()
        {
            var orderLine = new OrderLineBuilder(this.Session).Build();

            var order = new OrderBuilder(this.Session)
                .WithOrderLine(orderLine)
                .Build();

            this.Session.Derive();

            Assert.True(order.ExistCurrentVersion);
            Assert.True(order.ExistAllVersions);
            Assert.Equal(1, order.AllVersions.Count);

            var version = order.CurrentVersion;

            Assert.False(version.ExistAmount);
            Assert.False(version.ExistCurrentObjectState);
            Assert.Equal(1, version.OrderLines.Count);
            Assert.Equal(orderLine, version.OrderLines[0]);
        }
    }
}
