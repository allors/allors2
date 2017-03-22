//------------------------------------------------------------------------------------------------- 
// <copyright file="PackageQuantityBreakTests.cs" company="Allors bvba">
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

    
    public class PackageQuantityBreakTests : DomainTest
    {
        [Fact]
        public void GivenPackageQuantityBreak_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new PackageQuantityBreakBuilder(this.DatabaseSession);
            var revenueQuantityBreak = builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithFrom(10);
            revenueQuantityBreak = builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);
            builder.WithThrough(20);
            revenueQuantityBreak = builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);

            revenueQuantityBreak.RemoveFrom();

            Assert.False(this.DatabaseSession.Derive().HasErrors);
        }
    }
}
