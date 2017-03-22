//------------------------------------------------------------------------------------------------- 
// <copyright file="ConstraintSpecificationTests.cs" company="Allors bvba">
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

    
    public class ConstraintSpecificationTests : DomainTest
    {
        [Fact]
        public void GivenConstraintSpecification_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ConstraintSpecificationBuilder(this.DatabaseSession);
            var specification = builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("Description");
            specification = builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);

            Assert.Equal(specification.CurrentPartSpecificationStatus.PartSpecificationObjectState, new PartSpecificationObjectStates(this.DatabaseSession).Created);
            Assert.Equal(specification.CurrentObjectState, new PartSpecificationObjectStates(this.DatabaseSession).Created);
            Assert.Equal(specification.CurrentObjectState, specification.LastObjectState);
        }
    }
}