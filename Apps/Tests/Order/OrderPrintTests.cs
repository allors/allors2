//------------------------------------------------------------------------------------------------- 
// <copyright file="OrderPrintTests.cs" company="Allors bvba">
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
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    [TestFixture]
    public class OrderPrintTests : DomainTest
    {
        [Test]
        public void GivenOrder_WhenDeriving_ThenPrintoutIsCreated()
        {
            var euro = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR");
            var poundSterling = new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "GBP");

            var euroToPoundStirling = new UnitOfMeasureConversionBuilder(this.DatabaseSession)
                .WithConversionFactor(0.8553M)
                .WithToUnitOfMeasure(poundSterling)
                .WithStartDate(DateTime.UtcNow)
                .Build();

            euro.AddUnitOfMeasureConversion(euroToPoundStirling);

            var englishLocale = new Locales(this.DatabaseSession).EnglishGreatBritain;

            var mechelen = new CityBuilder(this.DatabaseSession).WithName("Mechelen").Build();
            var postalCode = new PostalCodeBuilder(this.DatabaseSession).WithCode("2800").Build();
            var belgie = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "BE");

            var postalAddress = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("Kleine Nieuwedijkstraat 2")
                .WithGeographicBoundary(mechelen)
                .WithGeographicBoundary(postalCode)
                .WithGeographicBoundary(belgie)
                .Build();

            var phone = new TelecommunicationsNumberBuilder(this.DatabaseSession)
                .WithCountryCode("+32")
                .WithContactNumber("3301 3301")
                .Build();

            var billingAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(postalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var headQuartersAddress = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(postalAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var generalPhoneNumber = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(phone)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).GeneralPhoneNumber)
                .WithUseAsDefault(true)
                .Build();

            var logo = new MediaBuilder(this.DatabaseSession)
                    .WithContent(GetEmbeddedResource("Tests.Resources.logo.png"))
                    .WithUniqueId(new Guid("9c41233f-9480-4df9-9421-63ed61f80623"))
                    .WithMediaType(new MediaTypes(this.DatabaseSession).Png)
                    .Build();

            var fortis = new BankBuilder(this.DatabaseSession).WithName("Fortis België").WithBic("GEBABEBB").WithCountry(belgie).Build();
            var bankaccount = new BankAccountBuilder(this.DatabaseSession).WithBank(fortis).WithIban("BE23 3300 6167 6391").WithCurrency(euro).WithNameOnAccount("Koen").Build();
            var ownBankAccount = new OwnBankAccountBuilder(this.DatabaseSession).WithDescription("own account").WithBankAccount(bankaccount).Build();

            var internalOrganisation = new InternalOrganisationBuilder(this.DatabaseSession)
                .WithName("Allors bvba")
                .WithPartyContactMechanism(billingAddress)
                .WithPartyContactMechanism(headQuartersAddress)
                .WithPartyContactMechanism(generalPhoneNumber)
                .WithDefaultPaymentMethod(ownBankAccount)
                .WithBankAccount(bankaccount)
                .WithLogoImage(logo)
                .WithTaxNumber("11111111")
                .WithLocale(englishLocale)
                .WithPreferredCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .Build();

            var facility = new WarehouseBuilder(this.DatabaseSession).WithName("facility").WithOwner(internalOrganisation).Build();
            new StoreBuilder(this.DatabaseSession)
                .WithName("store")
                .WithDefaultFacility(facility)
                .WithOwner(internalOrganisation)
                .WithDefaultShipmentMethod(new ShipmentMethods(this.DatabaseSession).Ground)
                .WithDefaultCarrier(new Carriers(this.DatabaseSession).Fedex)
                .WithSalesOrderTemplate(new StringTemplates(this.DatabaseSession).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesOrders.SalesOrderTemplateEnId))
                .WithSalesOrderTemplate(new StringTemplates(this.DatabaseSession).FindBy(UniquelyIdentifiables.Meta.UniqueId, SalesOrders.SalesOrderTemplateNlId))
                .Build();

            internalOrganisation.DefaultFacility = facility;

            var vatRate21 = new VatRateBuilder(this.DatabaseSession).WithRate(21).Build();

            ProductFeature feature1 = new ColourBuilder(this.DatabaseSession)
                .WithName("white")
                .WithVatRate(vatRate21)
                .WithLocalisedName(new LocalisedTextBuilder(this.DatabaseSession).WithText("White").WithLocale(englishLocale).Build())
                .Build();

            var category = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("gizmo").Build();

            var good = new GoodBuilder(this.DatabaseSession)
                .WithSku("10101")
                .WithVatRate(vatRate21)
                .WithName("good")
                .WithProductCategory(category)
                .WithInventoryItemKind(new InventoryItemKinds(this.DatabaseSession).NonSerialized)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            var supplier = new OrganisationBuilder(this.DatabaseSession).WithName("supplier").Build();

            new SupplierRelationshipBuilder(this.DatabaseSession)
                .WithSupplier(supplier)
                .WithInternalOrganisation(internalOrganisation)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            var goodPurchasePrice = new ProductPurchasePriceBuilder(this.DatabaseSession)
                .WithCurrency(euro)
                .WithFromDate(DateTime.UtcNow)
                .WithPrice(7)
                .WithUnitOfMeasure(new UnitsOfMeasure(this.DatabaseSession).Piece)
                .Build();

            new SupplierOfferingBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithSupplier(supplier)
                .WithProductPurchasePrice(goodPurchasePrice)
                .WithFromDate(DateTime.UtcNow)
                .Build();

            new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(internalOrganisation)
                .WithDescription("good baseprice")
                .WithProduct(good)
                .WithPrice(10234.3M)
                .WithFromDate(DateTimeFactory.CreateDate(2011, 2, 22))
                .Build();

            new BasePriceBuilder(this.DatabaseSession)
                .WithSpecifiedFor(internalOrganisation)
                .WithDescription("productfeature baseprice")
                .WithProductFeature(feature1)
                .WithPrice(2.5M)
                .WithFromDate(DateTimeFactory.CreateDate(2011, 2, 22))
                .Build();

            new DiscountComponentBuilder(this.DatabaseSession)
                .WithSpecifiedFor(internalOrganisation)
                .WithDescription("discount good for product category")
                .WithProductCategory(category)
                .WithProduct(good)
                .WithPercentage(5)
                .WithFromDate(DateTimeFactory.CreateDate(2011, 2, 22))
                .Build();

            this.DatabaseSession.Derive(true);
            this.DatabaseSession.Commit();

            var burbank = new CityBuilder(this.DatabaseSession).WithName("Burbank").Build();
            var postalCode2 = new PostalCodeBuilder(this.DatabaseSession).WithCode("CA 91505").Build();
            var usa = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "US");

            var amsterdam = new CityBuilder(this.DatabaseSession).WithName("Amsterdam").Build();
            var postalCode3 = new PostalCodeBuilder(this.DatabaseSession).WithCode("1001 AA").Build();
            var netherlands = new Countries(this.DatabaseSession).FindBy(Countries.Meta.IsoCode, "NL");

            var customerAddress = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("4000 Warner Blvd")
                .WithGeographicBoundary(burbank)
                .WithGeographicBoundary(postalCode2)
                .WithGeographicBoundary(usa)
                .Build();

            var orderItem2Address = new PostalAddressBuilder(this.DatabaseSession)
                .WithAddress1("PO box 1")
                .WithGeographicBoundary(amsterdam)
                .WithGeographicBoundary(postalCode3)
                .WithGeographicBoundary(netherlands)
                .Build();

            var billingContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(new EmailAddressBuilder(this.DatabaseSession).WithElectronicAddressString("invoices@acme.com").Build())
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).BillingAddress)
                .WithUseAsDefault(true)
                .Build();

            var shippingContactMechanism = new PartyContactMechanismBuilder(this.DatabaseSession)
                .WithContactMechanism(customerAddress)
                .WithContactPurpose(new ContactMechanismPurposes(this.DatabaseSession).ShippingAddress)
                .WithUseAsDefault(true)
                .Build();

            var customer = new OrganisationBuilder(this.DatabaseSession)
                .WithName("ACME")
                .WithPartyContactMechanism(billingContactMechanism)
                .WithPartyContactMechanism(shippingContactMechanism)
                .WithTaxNumber("22222222")
                .WithLocale(englishLocale)
                .WithPreferredCurrency(new Currencies(this.DatabaseSession).FindBy(Currencies.Meta.IsoCode, "EUR"))
                .Build();

            new CustomerRelationshipBuilder(this.DatabaseSession).WithCustomer(customer).WithInternalOrganisation(internalOrganisation).Build();

            var shippingAndHandling = new ShippingAndHandlingChargeBuilder(this.DatabaseSession).WithAmount(7.5M).WithVatRate(vatRate21).Build();

            var order = new SalesOrderBuilder(this.DatabaseSession)
                .WithShipToCustomer(customer)
                .WithBillToCustomer(customer)
                .WithTakenByInternalOrganisation(internalOrganisation)
                .WithShippingAndHandlingCharge(shippingAndHandling)
                .WithOrderDate(DateTimeFactory.CreateDate(2011, 2, 22))
                .WithMessage("Thanks for your order.")
                .Build();

            var item1 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithMessage("Orderitem message")
                .WithQuantityOrdered(3)
                .Build();

            order.AddSalesOrderItem(item1);

            this.DatabaseSession.Derive(true);

            var item2 = new SalesOrderItemBuilder(this.DatabaseSession)
                .WithProduct(good)
                .WithAssignedShipToAddress(orderItem2Address)
                .WithQuantityOrdered(3)
                .Build();

            order.AddSalesOrderItem(item2);

            this.DatabaseSession.Derive(true);

            Assert.IsTrue(order.ExistPrintContent);
            this.AssertXml(ExpectedOrderLayout.salesOrderEnglisch, order.PrintContent);
        }

        private void AssertXml(string expectedXml, string actualXml)
        {
            Assert.AreEqual(this.ToComparableXml(expectedXml), this.ToComparableXml(actualXml));
        }

        private string ToComparableXml(string xml)
        {
            var strictXml = new XmlStrictValidation(xml);
            strictXml.XmlDocument.Normalize();

            var stripWhiteSpace = new Regex(@">(\n|\s)*<");
            return stripWhiteSpace.Replace(strictXml.XmlDocument.InnerXml, "><");
        }

        private byte[] GetEmbeddedResource(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                var content = new byte[stream.Length];
                stream.Read(content, 0, content.Length);
                return content;
            }

            return null;
        }
    }
}
