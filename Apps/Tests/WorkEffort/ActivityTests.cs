// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivityTests.cs" company="Allors bvba">
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
//   Defines the MediaTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using NUnit.Framework;

    [TestFixture]
    public class ActivityTests : DomainTest
    {
        [Test]
        public void GivenActivity_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ActivityBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("Description");
            var activity = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);

            Assert.AreEqual(activity.CurrentWorkEffortStatus.WorkEffortObjectState, new WorkEffortObjectStates(this.DatabaseSession).NeedsAction);
            Assert.AreEqual(activity.CurrentObjectState, new WorkEffortObjectStates(this.DatabaseSession).NeedsAction);
            Assert.AreEqual(activity.CurrentObjectState, activity.LastObjectState);
        }
    }
}