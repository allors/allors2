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




    using Xunit;


    public class OrderKindTests : DomainTest
    {
        [Fact]
        public void GivenOrderKind_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new OrderKindBuilder(this.Session);
            var orderKind = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithDescription("orderkind");
            orderKind = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenOrderKind_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var orderKind = new OrderKindBuilder(this.Session)
                .WithDescription("Pre order summer collections")
                .Build();

            this.Session.Derive();

            Assert.False(orderKind.ScheduleManually);
        }
    }
}
