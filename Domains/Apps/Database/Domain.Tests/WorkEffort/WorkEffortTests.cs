//------------------------------------------------------------------------------------------------- 
// <copyright file="WorkEffortTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Xunit;

    
    public class WorkEffortTests : DomainTest
    {
        [Fact]
        public void GivenCustomerShipment_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var workEffort = new ActivityBuilder(this.DatabaseSession).WithName("Activity").Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new WorkEffortObjectStates(this.DatabaseSession).NeedsAction, workEffort.CurrentObjectState);
            Assert.Equal(workEffort.LastObjectState, workEffort.CurrentObjectState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var workEffort = new ActivityBuilder(this.DatabaseSession).WithName("Activity").Build();

            this.DatabaseSession.Derive();

            Assert.Null(workEffort.PreviousObjectState);
        }

        [Fact]
        public void GivenCustomerShipment_WhenConfirmed_ThenCurrentShipmentStatusMustBeDerived()
        {
            var workEffort = new ActivityBuilder(this.DatabaseSession).WithName("Activity").Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, workEffort.AllStateVersions.Count);
            Assert.Equal(new WorkEffortObjectStates(this.DatabaseSession).NeedsAction, workEffort.CurrentStateVersion.CurrentObjectState);

            workEffort.Finish();

            this.DatabaseSession.Derive();

            Assert.Equal(2, workEffort.AllStateVersions.Count);
            Assert.Equal(new WorkEffortObjectStates(this.DatabaseSession).Completed, workEffort.CurrentStateVersion.CurrentObjectState);
        }
    }
}