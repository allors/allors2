// <copyright file="SerialisedItemTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using Xunit;
    using System.Linq;
    using Allors.Domain.TestPopulation;
    using Resources;
    using System.Collections.Generic;

    public class SerialisedItemTests : DomainTest
    {
        [Fact]
        public void GivenSerializedItem_WhenAddingWithSameSerialNumber_ThenError()
        {
            var good = new UnifiedGoodBuilder(this.Session).WithSerialisedDefaults(this.InternalOrganisation).Build();
            var serialNumber = good.SerialisedItems.First.SerialNumber;

            var newItem = new SerialisedItemBuilder(this.Session).WithSerialNumber(serialNumber).Build();
            good.AddSerialisedItem(newItem);

            var expectedErrorMessage = ErrorMessages.SameSerialNumber;

            var errors = new List<string>(this.Session.Derive(false).Errors.Select(v => v.Message));
            Assert.Contains(expectedErrorMessage, errors);
        }

        [Fact]
        public void GivenSerializedItem_WhenDerived_ThenAvailabilityIsSet()
        {
            var available = new SerialisedItemAvailabilities(this.Session).Available;
            var notAvailable = new SerialisedItemAvailabilities(this.Session).NotAvailable;

            var newItem = new SerialisedItemBuilder(this.Session).WithForSaleDefaults(this.InternalOrganisation).Build();

            this.Session.Derive();

            Assert.Equal(available, newItem.SerialisedItemAvailability);

            newItem.SerialisedItemAvailability = notAvailable;

            this.Session.Derive();

            Assert.Equal(notAvailable, newItem.SerialisedItemAvailability);
        }

        [Fact]
        public void GivenSerializedItem_WhenDerived_ThenSuppliedByPartyNameIsSet()
        {
            var supplier = this.InternalOrganisation.ActiveSuppliers.First;

            var newItem = new SerialisedItemBuilder(this.Session).WithForSaleDefaults(this.InternalOrganisation).WithAssignedSuppliedBy(supplier).Build();

            this.Session.Derive();

            Assert.Equal(supplier.PartyName, newItem.SuppliedByPartyName);
        }

        [Fact]
        public void GivenSerializedItem_WhenDerived_ThenSuppliedByPartyNameIsSetFromSupplierOffering()
        {
            var supplier = this.InternalOrganisation.ActiveSuppliers.First;

            var unifiedGood = new UnifiedGoodBuilder(this.Session).WithSerialisedDefaults(this.InternalOrganisation).Build();
            this.Session.Derive();


            new SupplierOfferingBuilder(this.Session)
                .WithSupplier(supplier)
                .WithPart(unifiedGood)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Piece)
                .WithPrice(1)
                .Build();

            this.Session.Derive();

            var newItem = new SerialisedItemBuilder(this.Session).WithForSaleDefaults(this.InternalOrganisation).Build();
            unifiedGood.AddSerialisedItem(newItem);

            this.Session.Derive();

            Assert.Equal(supplier.PartyName, newItem.SuppliedByPartyName);
        }

        [Fact]
        public void GivenSerializedItem_WhenDerived_ThenOwnedByPartyNameIsSet()
        {
            var customer = this.InternalOrganisation.ActiveCustomers.First;

            var newItem = new SerialisedItemBuilder(this.Session).WithForSaleDefaults(this.InternalOrganisation).Build();
            newItem.OwnedBy = customer;

            this.Session.Derive();

            Assert.Equal(customer.PartyName, newItem.OwnedByPartyName);
        }


        [Fact]
        public void GivenSerializedItem_WhenDerived_ThenRentedByPartyNameIsSet()
        {
            var customer = this.InternalOrganisation.ActiveCustomers.First;

            var newItem = new SerialisedItemBuilder(this.Session).WithForSaleDefaults(this.InternalOrganisation).Build();
            newItem.RentedBy = customer;

            this.Session.Derive();

            Assert.Equal(customer.PartyName, newItem.RentedByPartyName);
        }

        [Fact]
        public void GivenSerializedItem_WhenDerived_ThenOwnershipByOwnershipNameIsSet()
        {
            var newItem = new SerialisedItemBuilder(this.Session).WithForSaleDefaults(this.InternalOrganisation).Build();
            newItem.Ownership = new Ownerships(this.Session).Own;

            this.Session.Derive();

            Assert.Equal(newItem.Ownership.Name, newItem.OwnershipByOwnershipName);
        }
    }
}
