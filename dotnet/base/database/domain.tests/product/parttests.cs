// <copyright file="PartTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;
    using Xunit;

    public class PartTests : DomainTest
    {
        [Fact]
        public void GivenPart_WhenBuild_ThenPostBuildRelationsMustExist()
        {
            var finishedGood = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            Assert.Equal(new InventoryItemKinds(this.Session).NonSerialised, finishedGood.InventoryItemKind);
        }

        [Fact]
        public void GivenNewPart_WhenDeriving_ThenInventoryItemIsCreated()
        {
            var finishedGood = new NonUnifiedPartBuilder(this.Session)
                .WithProductIdentification(new PartNumberBuilder(this.Session)
                    .WithIdentification("1")
                    .WithProductIdentificationType(new ProductIdentificationTypes(this.Session).Part).Build())
                .WithInventoryItemKind(new InventoryItemKinds(this.Session).NonSerialised)
                .Build();

            this.Session.Derive();

            Assert.Single(finishedGood.InventoryItemsWherePart);
            Assert.Equal(new Facilities(this.Session).FindBy(M.Facility.FacilityType, new FacilityTypes(this.Session).Warehouse), finishedGood.InventoryItemsWherePart.First.Facility);
        }
    }
}
