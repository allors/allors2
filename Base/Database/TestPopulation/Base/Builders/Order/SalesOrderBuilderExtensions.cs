// <copyright file="SalesOrderBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Meta;

    public static partial class SalesOrderBuilderExtensions
    {
        /**
         *
         * All invloved parties (Buyer and Seller) must be an Organisation to use
         * WithOrganisationInternalDefaults method
         *
         **/
        public static SalesOrderBuilder WithOrganisationInternalDefaults(this SalesOrderBuilder @this, Organisation sellerOrganisation)
        {
            var faker = @this.Session.Faker();

            var internalOrganisations = @this.Session.Extent<Organisation>();
            // Organisation of type Internal Organisation
            internalOrganisations.Filter.AddEquals(M.Organisation.IsInternalOrganisation.RoleType, true);

            // Filter out the sellerOrganisation
            var shipToCustomer = internalOrganisations.Except(new List<Organisation> { sellerOrganisation }).FirstOrDefault();

            var billToCustomer = shipToCustomer;

            var endCustomer = faker.Random.ListItem(shipToCustomer.ActiveCustomers.Where(v => v.GetType().Name == "Organisation").ToList());

            var endContact = endCustomer is Person endContactPerson ? endContactPerson : endCustomer.CurrentContacts.FirstOrDefault();
            var shipToContact = shipToCustomer.CurrentContacts.FirstOrDefault();

            var salesOrderItem_NonGSE = new SalesOrderItemBuilder(@this.Session).WithDefaults(sellerOrganisation).Build();
            var salesOrderItem_GSE = new SalesOrderItemBuilder(@this.Session).WithGSEDefaults(sellerOrganisation).Build();

            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16));
            @this.WithTakenBy(sellerOrganisation);
            @this.WithTakenByContactMechanism(sellerOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithTakenByContactPerson(sellerOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToCustomer(billToCustomer);
            @this.WithBillToContactMechanism(billToCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(billToCustomer.CurrentContacts.FirstOrDefault());
            @this.WithBillToEndCustomer(endCustomer);
            @this.WithBillToEndCustomerContactMechanism(endCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToEndCustomerContactPerson(endContact);
            @this.WithShipToEndCustomer(endCustomer);
            @this.WithShipToEndCustomerAddress(endCustomer.ShippingAddress);
            @this.WithShipToEndCustomerContactPerson(endContact);
            @this.WithShipToCustomer(shipToCustomer);
            @this.WithShipToAddress(shipToCustomer.ShippingAddress);
            @this.WithShipFromAddress(sellerOrganisation.ShippingAddress);
            @this.WithShipToContactPerson(shipToContact);
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesOrderItem(salesOrderItem_NonGSE).Build();
            @this.WithSalesOrderItem(salesOrderItem_GSE).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }

        /**
         *
         * All invloved parties (Buyer and Seller) must be an Organisation to use
         * WithOrganisationExternalDefaults method
         *
         **/
        public static SalesOrderBuilder WithOrganisationExternalDefaults(this SalesOrderBuilder @this, Organisation sellerOrganisation)
        {
            var faker = @this.Session.Faker();

            var shipToCustomer = faker.Random.ListItem(sellerOrganisation.ActiveCustomers.Where(v => v.GetType().Name == "Organisation").ToList());
            var billToCustomer = shipToCustomer;

            var shipToContact = shipToCustomer is Person shipToContactPerson ? shipToContactPerson : shipToCustomer.CurrentContacts.FirstOrDefault();

            var salesOrderItem_NonGSE = new SalesOrderItemBuilder(@this.Session).WithDefaults(sellerOrganisation).Build();
            var salesOrderItem_GSE = new SalesOrderItemBuilder(@this.Session).WithGSEDefaults(sellerOrganisation).Build();

            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.Words(10));
            @this.WithTakenBy(sellerOrganisation);
            @this.WithTakenByContactMechanism(sellerOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithTakenByContactPerson(sellerOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToCustomer(billToCustomer);
            @this.WithBillToContactMechanism(billToCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(billToCustomer.CurrentContacts.FirstOrDefault());
            @this.WithShipToCustomer(shipToCustomer);
            @this.WithShipToAddress(shipToCustomer.ShippingAddress);
            @this.WithShipFromAddress(sellerOrganisation.ShippingAddress);
            @this.WithShipToContactPerson(shipToContact);
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesOrderItem(salesOrderItem_NonGSE).Build();
            @this.WithSalesOrderItem(salesOrderItem_GSE).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }

        /**
         *
         * All invloved parties (Buyer and Seller) must be an Individual to use
         * WithPersonExternalDefaults method
         *
         **/
        public static SalesOrderBuilder WithPersonInternalDefaults(this SalesOrderBuilder @this, Organisation sellerOrganisation)
        {
            var faker = @this.Session.Faker();

            var internalOrganisations = @this.Session.Extent<Organisation>();
            // Organisation of type Internal Organisation
            internalOrganisations.Filter.AddEquals(M.Organisation.IsInternalOrganisation.RoleType, true);
            // Filter out the sellerOrganisation
            var shipToCustomer = internalOrganisations.Except(new List<Organisation> { sellerOrganisation }).FirstOrDefault();

            var billToCustomer = shipToCustomer;

            var endCustomer = faker.Random.ListItem(shipToCustomer.ActiveCustomers.Where(v => v.GetType().Name == "Person").ToList());

            var shipToContact = shipToCustomer.CurrentContacts.FirstOrDefault();

            var salesOrderItem_NonGSE = new SalesOrderItemBuilder(@this.Session).WithDefaults(sellerOrganisation).Build();
            var salesOrderItem_GSE = new SalesOrderItemBuilder(@this.Session).WithGSEDefaults(sellerOrganisation).Build();

            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.Words(10));
            @this.WithTakenBy(sellerOrganisation);
            @this.WithTakenByContactMechanism(sellerOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithTakenByContactPerson(sellerOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToCustomer(billToCustomer);
            @this.WithBillToContactMechanism(billToCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithBillToContactPerson(billToCustomer.CurrentContacts.FirstOrDefault());
            @this.WithBillToEndCustomer(endCustomer);
            @this.WithBillToEndCustomerContactMechanism(endCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithShipToEndCustomer(endCustomer);
            @this.WithShipToEndCustomerAddress(endCustomer.ShippingAddress);
            @this.WithShipToCustomer(shipToCustomer);
            @this.WithShipToAddress(shipToCustomer.ShippingAddress);
            @this.WithShipFromAddress(sellerOrganisation.ShippingAddress);
            @this.WithShipToContactPerson(shipToContact);
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesOrderItem(salesOrderItem_NonGSE).Build();
            @this.WithSalesOrderItem(salesOrderItem_GSE).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }

        /**
         *
         * All invloved parties (Buyer and Seller) must be an Individual to use
         * WithPersonExternalDefaults method
         *
         **/
        public static SalesOrderBuilder WithPersonExternalDefaults(this SalesOrderBuilder @this, Organisation sellerOrganisation)
        {
            var faker = @this.Session.Faker();

            var shipToCustomer = faker.Random.ListItem(sellerOrganisation.ActiveCustomers.Where(v => v.GetType().Name == "Person").ToList());
            var billToCustomer = shipToCustomer;

            var salesOrderItem_NonGSE = new SalesOrderItemBuilder(@this.Session).WithDefaults(sellerOrganisation).Build();
            var salesOrderItem_GSE = new SalesOrderItemBuilder(@this.Session).WithGSEDefaults(sellerOrganisation).Build();

            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.Words(16));
            @this.WithTakenBy(sellerOrganisation);
            @this.WithTakenByContactMechanism(sellerOrganisation.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithTakenByContactPerson(sellerOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBillToCustomer(billToCustomer);
            @this.WithBillToContactMechanism(billToCustomer.CurrentPartyContactMechanisms.Select(v => v.ContactMechanism).FirstOrDefault());
            @this.WithShipToCustomer(shipToCustomer);
            @this.WithShipToAddress(shipToCustomer.ShippingAddress);
            @this.WithShipFromAddress(sellerOrganisation.ShippingAddress);
            @this.WithPaymentMethod(paymentMethod);
            @this.WithSalesOrderItem(salesOrderItem_NonGSE).Build();
            @this.WithSalesOrderItem(salesOrderItem_GSE).Build();
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }
    }
}
