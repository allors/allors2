//------------------------------------------------------------------------------------------------- 
// <copyright file="NeededSkillTests.cs" company="Allors bvba">
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
    using NUnit.Framework;

    [TestFixture]
    public class NeededSkillTests : DomainTest
    {
        [Test]
        public void GivenNeededSkill_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var projectManagement = new Skills(this.DatabaseSession).ProjectManagement;
            var expert = new SkillLevels(this.DatabaseSession).Expert;

            var builder = new NeededSkillBuilder(this.DatabaseSession);
            var neededSkill = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSkill(projectManagement);
            neededSkill = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithSkillLevel(expert);
            neededSkill = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
