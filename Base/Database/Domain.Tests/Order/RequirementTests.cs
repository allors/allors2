//------------------------------------------------------------------------------------------------- 
// <copyright file="RequirementTests.cs" company="Allors bvba">
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


    public class RequirementTests : DomainTest
    {
        [Fact]
        public void GivenCustomerRequirement_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var requirement = new RequirementBuilder(this.Session).WithDescription("CustomerRequirement").Build();

            this.Session.Derive();

            Assert.Equal(new RequirementStates(this.Session).Active, requirement.RequirementState);
            Assert.Equal(requirement.LastRequirementState, requirement.RequirementState);
        }

        [Fact]
        public void GivenCustomerRequirement_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var requirement = new RequirementBuilder(this.Session).WithDescription("CustomerRequirement").Build();

            this.Session.Derive();

            Assert.Null(requirement.PreviousRequirementState);
        }

        [Fact]
        public void GivenCustomerRequirement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new RequirementBuilder(this.Session);
            var customerRequirement = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("CustomerRequirement");
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}