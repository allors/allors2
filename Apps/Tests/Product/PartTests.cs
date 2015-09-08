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
    using NUnit.Framework;

    [TestFixture]
    public class PartTests : DomainTest
    {
        [Test]
        public void GivenFinishedGood_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new FinishedGoodBuilder(this.DatabaseSession);
            var finishedGood = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("FinishedGood");
            finishedGood = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenFinishedGood_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var finishedGood = new FinishedGoodBuilder(this.DatabaseSession)
                .WithName("FinishedGood")
                .Build();

            Assert.AreEqual(new InventoryItemKinds(this.DatabaseSession).NonSerialized, finishedGood.InventoryItemKind);
            Assert.AreEqual(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"), finishedGood.OwnedByParty);
        }

        [Test]
        public void GivenNewFinishedGood_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var finishedGood = new FinishedGoodBuilder(this.DatabaseSession)
                .WithName("FinishedGood")
                .Build();

            this.DatabaseSession.Derive(true);
            
            Assert.AreEqual(1, finishedGood.InventoryItemsWherePart.Count);
            Assert.AreEqual(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"), finishedGood.InventoryItemsWherePart.First.Facility);
        }

        [Test]
        public void GivenRawMaterial_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new RawMaterialBuilder(this.DatabaseSession);
            var deliverableBasedService = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("RawMaterial");
            deliverableBasedService = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenRawMaterial_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var rawMaterial = new RawMaterialBuilder(this.DatabaseSession)
                .WithName("rawMaterial")
                .Build();

            Assert.AreEqual(new InventoryItemKinds(this.DatabaseSession).NonSerialized, rawMaterial.InventoryItemKind);
            Assert.AreEqual(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"), rawMaterial.OwnedByParty);
        }

        [Test]
        public void GivenNewRawMaterial_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var rawMaterial = new RawMaterialBuilder(this.DatabaseSession)
                .WithName("RawMaterial")
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, rawMaterial.InventoryItemsWherePart.Count);
            Assert.AreEqual(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"), rawMaterial.InventoryItemsWherePart.First.Facility);
        }

        [Test]
        public void GivenSubAssembly_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new SubAssemblyBuilder(this.DatabaseSession);
            var subAssembly = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithName("SubAssembly");
            subAssembly = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenSubAssembly_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var subAssembly = new SubAssemblyBuilder(this.DatabaseSession)
                .WithName("subAssembly")
                .Build();

            Assert.AreEqual(new InventoryItemKinds(this.DatabaseSession).NonSerialized, subAssembly.InventoryItemKind);
            Assert.AreEqual(new InternalOrganisations(this.DatabaseSession).FindBy(InternalOrganisations.Meta.Name, "internalOrganisation"), subAssembly.OwnedByParty);
        }

        [Test]
        public void GivenNewSubAssembly_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var subAssembly = new SubAssemblyBuilder(this.DatabaseSession)
                .WithName("SubAssembly")
                .Build();

            this.DatabaseSession.Derive(true);

            Assert.AreEqual(1, subAssembly.InventoryItemsWherePart.Count);
            Assert.AreEqual(new Warehouses(this.DatabaseSession).FindBy(Warehouses.Meta.Name, "facility"), subAssembly.InventoryItemsWherePart.First.Facility);
        }
    }
}
