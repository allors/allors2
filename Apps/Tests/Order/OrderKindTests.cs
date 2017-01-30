// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderKindTests.cs" company="Allors bvba">
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
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    

    

    using NUnit.Framework;

    [TestFixture]
    public class OrderKindTests : DomainTest
    {
        [Test]
        public void GivenOrderKind_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrderKindBuilder(this.DatabaseSession);
            var orderKind = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("orderkind");
            orderKind = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenOrderKind_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var orderKind = new OrderKindBuilder(this.DatabaseSession)
                .WithDescription("Pre order summer collections")
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(orderKind.ScheduleManually);
        }
    }
}
