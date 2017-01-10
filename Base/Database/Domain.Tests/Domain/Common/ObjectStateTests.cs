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

namespace Domain
{
    using Allors;
    using Allors.Domain;

    using NUnit.Framework;

    using Should;

    [TestFixture]
    public class ObjectStateTests : DomainTest
    {
        [Test]
        public void Transitions()
        {
            var initial = new OrderObjectStates(this.Session).Initial;
            var confirmed = new OrderObjectStates(this.Session).Confirmed;
            var cancelled = new OrderObjectStates(this.Session).Cancelled;

            var order = new OrderBuilder(this.Session).Build();

            this.Session.Derive(true);

            order.CurrentObjectState.ShouldBeNull();
            order.LastObjectState.ShouldBeNull();
            order.PreviousObjectState.ShouldBeNull();

            order.Amount = 10;
            order.CurrentObjectState = initial;

            this.Session.Derive(true);

            order.CurrentObjectState.ShouldEqual(initial);
            order.LastObjectState.ShouldEqual(initial);
            order.PreviousObjectState.ShouldBeNull();

            order.CurrentObjectState = confirmed;

            this.Session.Derive(true);

            order.CurrentObjectState.ShouldEqual(confirmed);
            order.LastObjectState.ShouldEqual(confirmed);
            order.PreviousObjectState.ShouldEqual(initial);

            order.Amount = 0;

            this.Session.Derive(true);

            order.CurrentObjectState.ShouldEqual(cancelled);
            order.LastObjectState.ShouldEqual(cancelled);
            order.PreviousObjectState.ShouldEqual(confirmed);
        }
    }
}
