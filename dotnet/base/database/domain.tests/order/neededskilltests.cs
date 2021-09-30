// <copyright file="NeededSkillTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;

    public class NeededSkillTests : DomainTest
    {
        [Fact]
        public void GivenNeededSkill_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var projectManagement = new Skills(this.Session).ProjectManagement;
            var expert = new SkillLevels(this.Session).Expert;

            var builder = new NeededSkillBuilder(this.Session);
            var neededSkill = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithSkill(projectManagement);
            neededSkill = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithSkillLevel(expert);
            neededSkill = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
