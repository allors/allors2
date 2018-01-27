//------------------------------------------------------------------------------------------------- 
// <copyright file="PartTests.cs" company="Allors bvba">
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
    using Meta;
    using Xunit;

    
    public class PartTests : DomainTest
    {
        [Fact]
        public void GivenFinishedGood_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new FinishedGoodBuilder(this.Session);
            var finishedGood = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("FinishedGood");
            finishedGood = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenFinishedGood_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithName("FinishedGood")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            Assert.Equal(new InventoryItemKinds(this.Session).NonSerialised, finishedGood.InventoryItemKind);
        }

        [Fact]
        public void GivenNewFinishedGood_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var finishedGood = new FinishedGoodBuilder(this.Session)
                .WithInternalOrganisation(this.InternalOrganisation)
                .WithName("FinishedGood")
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.Session.Derive();
            
            Assert.Equal(1, finishedGood.InventoryItemsWherePart.Count);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), finishedGood.InventoryItemsWherePart.First.Facility);
        }

        [Fact]
        public void GivenRawMaterial_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new RawMaterialBuilder(this.Session);
            var deliverableBasedService = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("RawMaterial");
            deliverableBasedService = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenRawMaterial_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var rawMaterial = new RawMaterialBuilder(this.Session)
                .WithName("rawMaterial")
                .Build();

            Assert.Equal(new InventoryItemKinds(this.Session).NonSerialised, rawMaterial.InventoryItemKind);
        }

        [Fact]
        public void GivenNewRawMaterial_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var rawMaterial = new RawMaterialBuilder(this.Session)
                .WithName("RawMaterial")
                .Build();

            this.Session.Derive();

            Assert.Equal(1, rawMaterial.InventoryItemsWherePart.Count);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), rawMaterial.InventoryItemsWherePart.First.Facility);
        }

        [Fact]
        public void GivenSubAssembly_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new SubAssemblyBuilder(this.Session);
            var subAssembly = builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithName("SubAssembly");
            subAssembly = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSubAssembly_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var subAssembly = new SubAssemblyBuilder(this.Session)
                .WithName("subAssembly")
                .Build();

            Assert.Equal(new InventoryItemKinds(this.Session).NonSerialised, subAssembly.InventoryItemKind);
        }

        [Fact]
        public void GivenNewSubAssembly_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var subAssembly = new SubAssemblyBuilder(this.Session)
                .WithName("SubAssembly")
                .Build();

            this.Session.Derive();

            Assert.Equal(1, subAssembly.InventoryItemsWherePart.Count);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), subAssembly.InventoryItemsWherePart.First.Facility);
        }
    }
}
