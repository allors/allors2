// <copyright file="RequirementTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

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
