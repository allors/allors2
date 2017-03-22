//------------------------------------------------------------------------------------------------- 
// <copyright file="CaseTests.cs" company="Allors bvba">
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

    
    public class CaseTests : DomainTest
    {
        [Fact]
        public void GivenCase_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var complaint = new CaseBuilder(this.DatabaseSession).WithDescription("Complaint").Build();

            this.DatabaseSession.Derive(true);
            
            Assert.Equal(new CaseObjectStates(this.DatabaseSession).Opened, complaint.CurrentObjectState);
            Assert.Equal(complaint.LastObjectState, complaint.CurrentObjectState);
        }

        [Fact]
        public void GivenCase_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var complaint = new CaseBuilder(this.DatabaseSession).WithDescription("Complaint").Build();

            this.DatabaseSession.Derive(true);

            Assert.Null(complaint.PreviousObjectState);
        }

        [Fact]
        public void GivenCase_WhenConfirmed_ThenCurrentCaseStatusMustBeDerived()
        {
            var complaint = new CaseBuilder(this.DatabaseSession).WithDescription("Complaint").Build();

            this.DatabaseSession.Derive(true);

            Assert.Equal(1, complaint.CaseStatuses.Count);
            Assert.Equal(new CaseObjectStates(this.DatabaseSession).Opened, complaint.CurrentCaseStatus.CaseObjectState);

            complaint.AppsClose();

            this.DatabaseSession.Derive(true);

            Assert.Equal(2, complaint.CaseStatuses.Count);
            Assert.Equal(new CaseObjectStates(this.DatabaseSession).Closed, complaint.CurrentCaseStatus.CaseObjectState);
        }
    }
}
