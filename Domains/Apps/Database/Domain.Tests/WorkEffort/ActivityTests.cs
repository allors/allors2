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
    using Xunit;

    
    public class ActivityTests : DomainTest
    {
        [Fact]
        public void GivenActivity_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ActivityBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("Description");
            var activity = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);

            Assert.Equal(activity.CurrentWorkEffortStatus.WorkEffortObjectState, new WorkEffortObjectStates(this.DatabaseSession).NeedsAction);
            Assert.Equal(activity.CurrentObjectState, new WorkEffortObjectStates(this.DatabaseSession).NeedsAction);
            Assert.Equal(activity.CurrentObjectState, activity.LastObjectState);
        }
    }
}