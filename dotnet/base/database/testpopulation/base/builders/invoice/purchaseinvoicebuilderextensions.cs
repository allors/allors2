// <copyright file="PurchaseInvoiceBuilderExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>

namespace Allors.Domain.TestPopulation
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static partial class PurchaseInvoiceBuilderExtensions
    {
        public static PurchaseInvoiceBuilder WithSalesInternalInvoiceDefaults(this PurchaseInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var internalOrganisations = @this.Session.Extent<Organisation>();
            var otherInternalOrganization = internalOrganisations.Except(new List<Organisation> { internalOrganisation }).FirstOrDefault();
            var endCustomer = faker.Random.ListItem(internalOrganisation.ActiveCustomers);

            var purchaseInvoiceItem_Defaullt = new PurchaseInvoiceItemBuilder(@this.Session).WithDefaults().Build();
            var purchaseInvoiceItem_Product = new PurchaseInvoiceItemBuilder(@this.Session).WithProductItemDefaults().Build();
            var purchaseInvoiceItem_Part = new PurchaseInvoiceItemBuilder(@this.Session).WithPartItemDefaults().Build();
            var purchaseInvoiceType = faker.Random.ListItem(@this.Session.Extent<PurchaseInvoiceType>());

            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper(CultureInfo.CurrentCulture));
            @this.WithBilledFrom(internalOrganisation);
            @this.WithBilledFromContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBilledTo(otherInternalOrganization);
            @this.WithBilledToContactPerson(otherInternalOrganization.CurrentContacts.FirstOrDefault());
            @this.WithBillToEndCustomer(endCustomer);
            @this.WithBillToEndCustomerContactPerson(endCustomer.CurrentContacts.FirstOrDefault());
            @this.WithShipToEndCustomer(endCustomer);
            @this.WithShipToEndCustomerContactPerson(endCustomer.CurrentContacts.FirstOrDefault());
            @this.WithShipToCustomer(otherInternalOrganization);
            @this.WithShipToCustomerContactPerson(otherInternalOrganization.CurrentContacts.FirstOrDefault());
            @this.WithPurchaseInvoiceType(purchaseInvoiceType);
            @this.WithAssignedBillToCustomerPaymentMethod(paymentMethod);
            @this.WithPurchaseInvoiceItem(purchaseInvoiceItem_Defaullt);
            @this.WithPurchaseInvoiceItem(purchaseInvoiceItem_Product);
            @this.WithPurchaseInvoiceItem(purchaseInvoiceItem_Part);
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());

            return @this;
        }

        public static PurchaseInvoiceBuilder WithSalesExternalB2BInvoiceDefaults(this PurchaseInvoiceBuilder @this, Organisation internalOrganisation)
        {
            var faker = @this.Session.Faker();

            var supplier = internalOrganisation.ActiveSuppliers.Where(v => v.GetType().Name == typeof(Organisation).Name).FirstOrDefault();

            var purchaseInvoiceItem_Defaullt = new PurchaseInvoiceItemBuilder(@this.Session).WithDefaults().Build();
            var purchaseInvoiceItem_Product = new PurchaseInvoiceItemBuilder(@this.Session).WithProductItemDefaults().Build();
            var purchaseInvoiceItem_Part = new PurchaseInvoiceItemBuilder(@this.Session).WithPartItemDefaults().Build();
            var purchaseInvoiceType = faker.Random.ListItem(@this.Session.Extent<PurchaseInvoiceType>());

            var paymentMethod = faker.Random.ListItem(@this.Session.Extent<PaymentMethod>());

            @this.WithCustomerReference(faker.Random.String(16).ToUpper(CultureInfo.CurrentCulture));
            @this.WithBilledFrom(supplier);
            @this.WithBilledFromContactPerson(supplier.CurrentContacts.FirstOrDefault());
            @this.WithDescription(faker.Lorem.Sentence());
            @this.WithComment(faker.Lorem.Sentence());
            @this.WithInternalComment(faker.Lorem.Sentence());
            @this.WithBilledTo(internalOrganisation);
            @this.WithBilledToContactPerson(internalOrganisation.CurrentContacts.FirstOrDefault());
            @this.WithPurchaseInvoiceType(purchaseInvoiceType);
            @this.WithAssignedBillToCustomerPaymentMethod(paymentMethod);
            @this.WithPurchaseInvoiceItem(purchaseInvoiceItem_Defaullt);
            @this.WithPurchaseInvoiceItem(purchaseInvoiceItem_Product);
            @this.WithPurchaseInvoiceItem(purchaseInvoiceItem_Part);
            @this.WithSalesTerm(new IncoTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new InvoiceTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithSalesTerm(new OrderTermBuilder(@this.Session).WithDefaults().Build());
            @this.WithAssignedVatRegime(faker.Random.ListItem(@this.Session.Extent<VatRegime>()));

            return @this;
        }
    }
}
