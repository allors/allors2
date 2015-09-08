//------------------------------------------------------------------------------------------------- 
// <copyright file="PartSpecificationTests.cs" company="Allors bvba">
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
    public class PartSpecificationTests : DomainTest
    {
        [Test]
        public void GivenConstraintSpecification_WhenBuild_ThenLastObjectStateEqualsCurrencObjectState()
        {
            var specification = new ConstraintSpecificationBuilder(this.DatabaseSession).WithDescription("specification").Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(new PartSpecificationObjectStates(this.DatabaseSession).Created, specification.CurrentObjectState);
            Assert.AreEqual(specification.LastObjectState, specification.CurrentObjectState);
        }

        [Test]
        public void GivenConstraintSpecification_WhenBuild_ThenPreviousObjectStateIsNull()
        {
            var specification = new ConstraintSpecificationBuilder(this.DatabaseSession).WithDescription("specification").Build();

            this.DatabaseSession.Derive(true);

            Assert.IsNull(specification.PreviousObjectState);
        }

        [Test]
        public void GivenConstraintSpecification_WhenConfirmed_ThenCurrentSpecificationStatusMustBeDerived()
        {
            var specification = new ConstraintSpecificationBuilder(this.DatabaseSession).WithDescription("specification").Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, specification.PartSpecificationStatuses.Count);
            Assert.AreEqual(new PartSpecificationObjectStates(this.DatabaseSession).Created, specification.CurrentPartSpecificationStatus.PartSpecificationObjectState);

            specification.Approve();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(2, specification.PartSpecificationStatuses.Count);
            Assert.AreEqual(new PartSpecificationObjectStates(this.DatabaseSession).Approved, specification.CurrentPartSpecificationStatus.PartSpecificationObjectState);
        }
    }
}
