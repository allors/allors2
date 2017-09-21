// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectStateTests.cs" company="Allors bvba">
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
    
    public class ObjectStateTests : DomainTest
    {
        [Fact]
        public void Transitions()
        {
            var initial = new OrderObjectStates(this.Session).Initial;
            var confirmed = new OrderObjectStates(this.Session).Confirmed;
            var cancelled = new OrderObjectStates(this.Session).Cancelled;

            var order = new OrderBuilder(this.Session).Build();

            this.Session.Derive(true);

            Assert.Null(order.CurrentObjectState);
            Assert.Null(order.LastObjectState);
            Assert.Null(order.PreviousObjectState);

            order.Amount = 10;
            order.CurrentObjectState = initial;

            this.Session.Derive(true);

            Assert.Equal(order.CurrentObjectState, initial);
            Assert.Equal(order.LastObjectState, initial);
            Assert.Null(order.PreviousObjectState);

            order.CurrentObjectState = confirmed;

            this.Session.Derive(true);

            Assert.Equal(order.CurrentObjectState, confirmed);
            Assert.Equal(order.LastObjectState, confirmed);
            Assert.Equal(order.PreviousObjectState, initial);

            order.Amount = -1;

            this.Session.Derive(true);

            Assert.Equal(order.CurrentObjectState, cancelled);
            Assert.Equal(order.LastObjectState, cancelled);
            Assert.Equal(order.PreviousObjectState, confirmed);
        }
    }
}
