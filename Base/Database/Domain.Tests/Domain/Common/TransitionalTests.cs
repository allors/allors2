// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransitionalTests.cs" company="Allors bvba">
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
    
    public class TransitionalTests : DomainTest
    {
        [Fact]
        public void SingleObjectState()
        {
            var initial = new OrderStates(this.Session).Initial;
            var confirmed = new OrderStates(this.Session).Confirmed;
            var cancelled = new OrderStates(this.Session).Cancelled;

            var order = new OrderBuilder(this.Session).Build();

            this.Session.Derive(true);

            Assert.False(order.ExistOrderState);
            Assert.False(order.ExistLastOrderState);
            Assert.False(order.ExistPreviousOrderState);

            Assert.False(order.ExistObjectStates);
            Assert.False(order.ExistLastObjectStates);
            Assert.False(order.ExistPreviousObjectStates);

            order.OrderState = initial;

            this.Session.Derive(true);

            Assert.Equal(initial, order.OrderState);
            Assert.Equal(initial, order.LastOrderState);
            Assert.False(order.ExistPreviousOrderState);

            Assert.Equal(1, order.ObjectStates.Count);
            Assert.Contains(initial, order.ObjectStates);
            Assert.Equal(1, order.LastObjectStates.Count);
            Assert.Contains(initial, order.LastObjectStates);
            Assert.False(order.ExistPreviousObjectStates);
            
            order.OrderState = confirmed;

            this.Session.Derive(true);

            Assert.Equal(confirmed, order.OrderState);
            Assert.Equal(confirmed, order.LastOrderState);
            Assert.Equal(initial, order.PreviousOrderState);

            Assert.Equal(1, order.ObjectStates.Count);
            Assert.Contains(confirmed, order.ObjectStates);
            Assert.Equal(1, order.LastObjectStates.Count);
            Assert.Contains(confirmed, order.LastObjectStates);
            Assert.Equal(1, order.PreviousObjectStates.Count);
            Assert.Contains(initial, order.PreviousObjectStates);
        }

        [Fact]
        public void MultipleObjectStates()
        {
            var initial = new OrderStates(this.Session).Initial;
            var confirmed = new OrderStates(this.Session).Confirmed;
            var cancelled = new OrderStates(this.Session).Cancelled;

            var notShipped = new ShipmentStates(this.Session).NotShipped;
            var partiallyShipped = new ShipmentStates(this.Session).PartiallyShipped;
            var shipped = new ShipmentStates(this.Session).Shipped;

            var order = new OrderBuilder(this.Session).Build();

            order.OrderState = initial;

            this.Session.Derive(true);

            order.OrderState = confirmed;

            this.Session.Derive(true);

            order.ShipmentState = notShipped;

            this.Session.Derive(true);

            Assert.Equal(notShipped, order.ShipmentState);
            Assert.Equal(notShipped, order.LastShipmentState);
            Assert.False(order.ExistPreviousShipmentState);

            Assert.Equal(2, order.ObjectStates.Count);
            Assert.Contains(confirmed, order.ObjectStates);
            Assert.Contains(notShipped, order.ObjectStates);
            Assert.Equal(2, order.LastObjectStates.Count);
            Assert.Contains(confirmed, order.LastObjectStates);
            Assert.Contains(notShipped, order.LastObjectStates);
            Assert.Equal(1, order.PreviousObjectStates.Count);
            Assert.Contains(initial, order.PreviousObjectStates);

            order.ShipmentState = partiallyShipped;

            this.Session.Derive(true);

            Assert.Equal(2, order.ObjectStates.Count);
            Assert.Contains(confirmed, order.ObjectStates);
            Assert.Contains(partiallyShipped, order.ObjectStates);
            Assert.Equal(2, order.LastObjectStates.Count);
            Assert.Contains(confirmed, order.LastObjectStates);
            Assert.Contains(partiallyShipped, order.LastObjectStates);
            Assert.Equal(2, order.PreviousObjectStates.Count);
            Assert.Contains(initial, order.PreviousObjectStates);
            Assert.Contains(notShipped, order.PreviousObjectStates);
        }
    }
}
