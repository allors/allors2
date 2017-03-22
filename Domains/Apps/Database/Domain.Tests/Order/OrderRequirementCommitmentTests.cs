//------------------------------------------------------------------------------------------------- 
// <copyright file="OrderRequirementCommitmentTests.cs" company="Allors bvba">
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

    
    public class OrderRequirementCommitmentTests : DomainTest
    {
        [Fact]
        public void GivenOrderRequirementCommitment_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var shipToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("shipToCustomer").Build();
            var billToCustomer = new OrganisationBuilder(this.DatabaseSession).WithName("billToCustomer").Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(billToCustomer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession)
                .WithCustomer(shipToCustomer)
                .WithInternalOrganisation(Singleton.Instance(this.DatabaseSession).DefaultInternalOrganisation)
                .Build();

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();
            var good = new GoodBuilder(this.DatabaseSession)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("Gizmo").WithLocale(Singleton.Instance(this.DatabaseSession).DefaultLocale).Build())
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            this.DatabaseSession.Derive(true);

            var salesOrder = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(shipToCustomer)
                .WithBillToCustomer(billToCustomer)
                .WithVatRegime(new VatRegimes(this.DatabaseSession).Export)
                .Build();

            var goodOrderItem = new SalesOrderItemBuilder(this.DatabaseSession).WithProduct(good).WithQuantityOrdered(1).Build();
            salesOrder.AddSalesOrderItem(goodOrderItem);

            var customerRequirement = new CustomerRequirementBuilder(this.DatabaseSession).WithDescription("100 gizmo's").Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var builder = new OrderRequirementCommitmentBuilder(this.DatabaseSession);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithOrderItem(goodOrderItem);
            builder.Build();

            Assert.True(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithRequirement(customerRequirement);
            var tsts = builder.Build();

            Assert.False(this.DatabaseSession.Derive().HasErrors);
        }
    }
}