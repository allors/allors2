//------------------------------------------------------------------------------------------------- 
// <copyright file="CostCenterCategoryTests.cs" company="Allors bvba">
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
    public class CostCenterCategoryTests : DomainTest
    {
        [Test]
        public void GivenCostCenterCategory_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new CostCenterCategoryBuilder(this.DatabaseSession);
            builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("CostCenterCategory");
            builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenCostCenterCategory_WhenDeriving_ThenPostBuildRelationsMustExist()
        {
            var costCenterCategory = new CostCenterCategoryBuilder(this.DatabaseSession)
                .WithDescription("CostCenterCategory")
                .Build();

            Assert.IsTrue(costCenterCategory.ExistUniqueId);
        }
    }
}