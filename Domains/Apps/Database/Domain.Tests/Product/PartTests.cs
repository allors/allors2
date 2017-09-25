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
            var builder = new FinishedGoodBuilder(this.DatabaseSession);
            var finishedGood = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("FinishedGood");
            finishedGood = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenFinishedGood_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var finishedGood = new FinishedGoodBuilder(this.DatabaseSession)
                .WithName("FinishedGood")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            Assert.Equal(new InventoryItemKinds(this.DatabaseSession).NonSerialised, finishedGood.InventoryItemKind);
        }

        [Fact]
        public void GivenNewFinishedGood_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var finishedGood = new FinishedGoodBuilder(this.DatabaseSession)
                .WithName("FinishedGood")
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialised)
                .Build();

            this.DatabaseSession.Derive();
            
            Assert.Equal(1, finishedGood.InventoryItemsWherePart.Count);
            Assert.Equal(new Warehouses(this.DatabaseSession).FindBy(M.Warehouse.Name, "facility"), finishedGood.InventoryItemsWherePart.First.Facility);
        }

        [Fact]
        public void GivenRawMaterial_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new RawMaterialBuilder(this.DatabaseSession);
            var deliverableBasedService = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("RawMaterial");
            deliverableBasedService = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenRawMaterial_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var rawMaterial = new RawMaterialBuilder(this.DatabaseSession)
                .WithName("rawMaterial")
                .Build();

            Assert.Equal(new InventoryItemKinds(this.DatabaseSession).NonSerialised, rawMaterial.InventoryItemKind);
        }

        [Fact]
        public void GivenNewRawMaterial_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var rawMaterial = new RawMaterialBuilder(this.DatabaseSession)
                .WithName("RawMaterial")
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, rawMaterial.InventoryItemsWherePart.Count);
            Assert.Equal(new Warehouses(this.DatabaseSession).FindBy(M.Warehouse.Name, "facility"), rawMaterial.InventoryItemsWherePart.First.Facility);
        }

        [Fact]
        public void GivenSubAssembly_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new SubAssemblyBuilder(this.DatabaseSession);
            var subAssembly = builder.Build();

            Assert.True(this.DatabaseSession.Derive(false).HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("SubAssembly");
            subAssembly = builder.Build();

            Assert.False(this.DatabaseSession.Derive(false).HasErrors);
        }

        [Fact]
        public void GivenSubAssembly_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var subAssembly = new SubAssemblyBuilder(this.DatabaseSession)
                .WithName("subAssembly")
                .Build();

            Assert.Equal(new InventoryItemKinds(this.DatabaseSession).NonSerialised, subAssembly.InventoryItemKind);
        }

        [Fact]
        public void GivenNewSubAssembly_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var subAssembly = new SubAssemblyBuilder(this.DatabaseSession)
                .WithName("SubAssembly")
                .Build();

            this.DatabaseSession.Derive();

            Assert.Equal(1, subAssembly.InventoryItemsWherePart.Count);
            Assert.Equal(new Warehouses(this.DatabaseSession).FindBy(M.Warehouse.Name, "facility"), subAssembly.InventoryItemsWherePart.First.Facility);
        }
    }
}
