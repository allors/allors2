// <copyright file="OrderRequirementCommitmentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Allors.Meta;

    using Xunit;

    public class OrderRequirementCommitmentTests : DomainTest
    {
        [Fact]
        public void GivenOrderRequirementCommitment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var shipToCustomer = new OrganisationBuilder(this.Session).WithName("shipToCustomer").Build();
            var billToCustomer = new OrganisationBuilder(this.Session).WithName("billToCustomer").Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(billToCustomer)

                .Build();

            new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(shipToCustomer)

                .Build();

            var good = new Goods(this.Session).FindBy(M.Good.Name, "good1");

            this.Session.Derive();

            var salesOrder = new SalesOrderBuilder(this.Session)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithAssignedVatRegime(new VatRegimes(this.Session).ZeroRated)
                .Build();

            var goodOrderItem = new SalesOrderItemBuilder(this.Session)
                .WithInvoiceItemType(new InvoiceItemTypes(this.Session).ProductItem)
                .WithProduct(good)
                .WithAssignedUnitPrice(1)
                .WithQuantityOrdered(1)
                .Build();

            salesOrder.AddSalesOrderItem(goodOrderItem);

            var customerRequirement = new RequirementBuilder(this.Session).WithDescription("100 gizmo's").Build();

            this.Session.Derive();
            this.Session.Commit();

            var builder = new OrderRequirementCommitmentBuilder(this.Session);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithOrderItem(goodOrderItem);
            builder.Build();

            Assert.True(this.Session.Derive(false).HasErrors);

            this.Session.Rollback();

            builder.WithRequirement(customerRequirement);
            var tsts = builder.Build();

            Assert.False(this.Session.Derive(false).HasErrors);
        }
    }
}
