//------------------------------------------------------------------------------------------------- 
// <copyright file="DesiredProductFeatureTests.cs" company="Allors bvba">
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

    
    public class DesiredProductFeatureTests : DomainTest
    {
        [Fact]
        public void GivenDesiredProductFeature_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var vatRate21 = new VatRateBuilder(this.Session).WithRate(21).Build();
            var softwareFeature = new SoftwareFeatureBuilder(this.Session)
                .WithVatRate(vatRate21)
                .WithName("Tutorial")
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new DesiredProductFeatureBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();
            
            builder.WithRequired(false);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithProductFeature(softwareFeature);
            builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}