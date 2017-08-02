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
            var requirement = new CustomerRequirementBuilder(this.DatabaseSession).WithDescription("CustomerRequirement").Build();

            this.DatabaseSession.Derive();

            Assert.Equal(new RequirementObjectStates(this.DatabaseSession).Active, requirement.CurrentObjectState);
            Assert.Equal(requirement.LastObjectState, requirement.CurrentObjectState);
        }

        [Fact]
        public void GivenCustomerRequirement_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var requirement = new CustomerRequirementBuilder(this.DatabaseSession).WithDescription("CustomerRequirement").Build();

            this.DatabaseSession.Derive();

            Assert.Null(requirement.PreviousObjectState);
        }

        [Fact]
        public void GivenCustomerRequirement_WhenConfirmed_ThenCurrentRequirementStatusMustBeDerived()
        {
            var requirement = new CustomerRequirementBuilder(this.DatabaseSession).WithDescription("CustomerRequirement").Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, requirement.RequirementStatuses.Count);
            Assert.Equal(new RequirementObjectStates(this.DatabaseSession).Active, requirement.CurrentRequirementStatus.RequirementObjectState);

            requirement.Close();

            this.DatabaseSession.Derive();

            Assert.Equal(2, requirement.RequirementStatuses.Count);
            Assert.Equal(new RequirementObjectStates(this.DatabaseSession).Closed, requirement.CurrentRequirementStatus.RequirementObjectState);
        }

        [Fact]
        public void GivenCustomerRequirement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new CustomerRequirementBuilder(this.DatabaseSession);
            var customerRequirement = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("CustomerRequirement");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenInternalRequirement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new InternalRequirementBuilder(this.DatabaseSession);
            var internalRequirement = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("InternalRequirement");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenProductRequirement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new ProductRequirementBuilder(this.DatabaseSession);
            var productRequirement = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("ProductRequirement");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenProjectRequirement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new ProjectRequirementBuilder(this.DatabaseSession);
            var projectRequirement = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("ProjectRequirement");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenResourceRequirement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new ResourceRequirementBuilder(this.DatabaseSession);
            var resourceRequirement = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("ResourceRequirement");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenWorkRequirement_WhenDeriving_ThenDescriptionIsRequired()
        {
            var builder = new WorkRequirementBuilder(this.DatabaseSession);
            var workRequirement = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("WorkRequirement");
            builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }
    }
}