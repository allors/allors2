// <copyright file="ProductQuote.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    public partial class ProductQuote
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.ProductQuote, M.ProductQuote.QuoteState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public void BaseOrder(ProductQuoteOrder method)
        {
            this.QuoteState = new QuoteStates(this.Strategy.Session).Ordered;
            this.OrderThis();
        }

        public void BasePrint(PrintablePrint method)
        {
            if (!method.IsPrinted)
            {
                var singleton = this.Strategy.Session.GetSingleton();
                var logo = this.Issuer?.ExistLogoImage == true ?
                               this.Issuer.LogoImage.MediaContent.Data :
                               singleton.LogoImage.MediaContent.Data;

                var images = new Dictionary<string, byte[]>
                                 {
                                     { "Logo", logo },
                                 };

                if (this.ExistQuoteNumber)
                {
                    var session = this.Strategy.Session;
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(this.QuoteNumber, BarcodeType.CODE_128, 320, 80);
                    images.Add("Barcode", barcode);
                }

                var printModel = new Print.ProductQuoteModel.Model(this, images);
                this.RenderPrintDocument(this.Issuer?.ProductQuoteTemplate, printModel, images);

                this.PrintDocument.Media.InFileName = $"{this.QuoteNumber}.odt";
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var session = this.Session();

            // SalesOrderItem Derivations and Validations
            foreach (QuoteItem quoteItem in this.QuoteItems)
            {
                var isSubTotalItem = quoteItem.ExistInvoiceItemType && (quoteItem.InvoiceItemType.IsProductItem || quoteItem.InvoiceItemType.IsPartItem);
                if (isSubTotalItem)
                {
                    if (quoteItem.Quantity == 0)
                    {
                        derivation.Validation.AddError(quoteItem, M.QuoteItem.Quantity, "Quantity is Required");
                    }
                }
                else
                {
                    if (quoteItem.UnitPrice == 0)
                    {
                        derivation.Validation.AddError(quoteItem, M.QuoteItem.UnitPrice, "Price is Required");
                    }
                }
            }

            var validQuoteItems = this.QuoteItems.Where(v => v.IsValid).ToArray();
            this.ValidQuoteItems = validQuoteItems;

            var currentPriceComponents = new PriceComponents(session).CurrentPriceComponents(this.IssueDate);

            var quantityOrderedByProduct = this.ValidQuoteItems
                .Where(v => v.ExistProduct)
                .GroupBy(v => v.Product)
                .ToDictionary(v => v.Key, v => v.Sum(w => w.Quantity));

            // First run to calculate price
            foreach (QuoteItem quoteItem in this.ValidQuoteItems)
            {
                decimal quantityOrdered = 0;

                if (quoteItem.ExistProduct)
                {
                    quantityOrderedByProduct.TryGetValue(quoteItem.Product, out quantityOrdered);
                }

                foreach (QuoteItem featureItem in quoteItem.QuotedWithFeatures)
                {
                    featureItem.Quantity = quoteItem.Quantity;
                    this.CalculatePrices(derivation, featureItem, currentPriceComponents, quantityOrdered, 0);
                }

                this.CalculatePrices(derivation, quoteItem, currentPriceComponents, quantityOrdered, 0);
            }

            var totalBasePriceByProduct = this.QuoteItems
                .Where(v => v.ExistProduct)
                .GroupBy(v => v.Product)
                .ToDictionary(v => v.Key, v => v.Sum(w => w.TotalBasePrice));

            // Second run to calculate price (because of order value break)
            foreach (QuoteItem quoteItem in this.ValidQuoteItems)
            {
                decimal quantityOrdered = 0;
                decimal totalBasePrice = 0;

                if (quoteItem.ExistProduct)
                {
                    quantityOrderedByProduct.TryGetValue(quoteItem.Product, out quantityOrdered);
                    totalBasePriceByProduct.TryGetValue(quoteItem.Product, out totalBasePrice);
                }

                foreach (QuoteItem featureItem in quoteItem.QuotedWithFeatures)
                {
                    this.CalculatePrices(derivation, featureItem, currentPriceComponents, quantityOrdered, totalBasePrice);
                }

                this.CalculatePrices(derivation, quoteItem, currentPriceComponents, quantityOrdered, totalBasePrice);
            }

            // Calculate Totals
            if (this.ExistQuoteItems)
            {
                this.TotalBasePrice = 0;
                this.TotalDiscount = 0;
                this.TotalSurcharge = 0;
                this.TotalExVat = 0;
                this.TotalFee = 0;
                this.TotalShippingAndHandling = 0;
                this.TotalVat = 0;
                this.TotalIncVat = 0;
                this.TotalListPrice = 0;

                foreach (QuoteItem quoteItem in this.ValidQuoteItems)
                {
                    if (!quoteItem.ExistQuoteItemWhereQuotedWithFeature)
                    {
                        this.TotalBasePrice += quoteItem.TotalBasePrice;
                        this.TotalDiscount += quoteItem.TotalDiscount;
                        this.TotalSurcharge += quoteItem.TotalSurcharge;
                        this.TotalExVat += quoteItem.TotalExVat;
                        this.TotalVat += quoteItem.TotalVat;
                        this.TotalIncVat += quoteItem.TotalIncVat;
                        this.TotalListPrice += quoteItem.UnitPrice;
                    }
                }

                if (this.ExistDiscountAdjustment)
                {
                    var discount = this.DiscountAdjustment.Percentage.HasValue ?
                                           Math.Round(this.TotalExVat * this.DiscountAdjustment.Percentage.Value / 100, 2) :
                                           this.DiscountAdjustment.Amount ?? 0;

                    this.TotalDiscount += discount;
                    this.TotalExVat -= discount;

                    if (this.ExistVatRegime)
                    {
                        var vat = Math.Round(discount * this.VatRegime.VatRate.Rate / 100, 2);

                        this.TotalVat -= vat;
                        this.TotalIncVat -= discount + vat;
                    }
                }

                if (this.ExistSurchargeAdjustment)
                {
                    var surcharge = this.SurchargeAdjustment.Percentage.HasValue ?
                                            Math.Round(this.TotalExVat * this.SurchargeAdjustment.Percentage.Value / 100, 2) :
                                            this.SurchargeAdjustment.Amount ?? 0;

                    this.TotalSurcharge += surcharge;
                    this.TotalExVat += surcharge;

                    if (this.ExistVatRegime)
                    {
                        var vat = Math.Round(surcharge * this.VatRegime.VatRate.Rate / 100, 2);
                        this.TotalVat += vat;
                        this.TotalIncVat += surcharge + vat;
                    }
                }

                if (this.ExistFee)
                {
                    var fee = this.Fee.Percentage.HasValue ?
                                      Math.Round(this.TotalExVat * this.Fee.Percentage.Value / 100, 2) :
                                      this.Fee.Amount ?? 0;

                    this.TotalFee += fee;
                    this.TotalExVat += fee;

                    if (this.Fee.ExistVatRate)
                    {
                        var vat1 = Math.Round(fee * this.Fee.VatRate.Rate / 100, 2);
                        this.TotalVat += vat1;
                        this.TotalIncVat += fee + vat1;
                    }
                }

                if (this.ExistShippingAndHandlingCharge)
                {
                    var shipping = this.ShippingAndHandlingCharge.Percentage.HasValue ?
                                           Math.Round(this.TotalExVat * this.ShippingAndHandlingCharge.Percentage.Value / 100, 2) :
                                           this.ShippingAndHandlingCharge.Amount ?? 0;

                    this.TotalShippingAndHandling += shipping;
                    this.TotalExVat += shipping;

                    if (this.ShippingAndHandlingCharge.ExistVatRate)
                    {
                        var vat2 = Math.Round(shipping * this.ShippingAndHandlingCharge.VatRate.Rate / 100, 2);
                        this.TotalVat += vat2;
                        this.TotalIncVat += shipping + vat2;
                    }
                }

                //// Only take into account items for which there is data at the item level.
                //// Skip negative sales.
                decimal totalUnitBasePrice = 0;
                decimal totalListPrice = 0;

                foreach (QuoteItem item1 in this.ValidQuoteItems)
                {
                    if (item1.TotalExVat > 0)
                    {
                        totalUnitBasePrice += item1.UnitBasePrice;
                        totalListPrice += item1.UnitPrice;
                    }
                }
            }

            if (this.QuoteState.IsSent
                && (!this.ExistLastQuoteState || !this.LastQuoteState.IsSent)
                && this.Issuer.SerialisedItemAssignedOn == new SerialisedItemAssignedOns(this.Session()).ProductQuoteSend)
            {
                foreach (QuoteItem item in this.ValidQuoteItems.Where(v => v.ExistSerialisedItem))
                {
                    item.SerialisedItem.SerialisedItemState = new SerialisedItemStates(this.Strategy.Session).Assigned;
                }
            }

            this.DeriveWorkflow();

            this.Sync(this.Strategy.Session);

            this.ResetPrintDocument();
        }

        public void CalculatePrices(
            IDerivation derivation,
            QuoteItem quoteItem,
            PriceComponent[] currentPriceComponents,
            decimal quantityOrdered,
            decimal totalBasePrice)
        {
            var quoteItemDeriveRoles = (QuoteItemDerivedRoles)quoteItem;

            var currentGenericOrProductOrFeaturePriceComponents = Array.Empty<PriceComponent>();
            if (quoteItem.ExistProduct)
            {
                currentGenericOrProductOrFeaturePriceComponents = quoteItem.Product.GetPriceComponents(currentPriceComponents);
            }
            else if (quoteItem.ExistProductFeature)
            {
                currentGenericOrProductOrFeaturePriceComponents = quoteItem.ProductFeature.GetPriceComponents(quoteItem.QuoteItemWhereQuotedWithFeature.Product, currentPriceComponents);
            }

            var priceComponents = currentGenericOrProductOrFeaturePriceComponents.Where(
                v => PriceComponents.BaseIsApplicable(
                    new PriceComponents.IsApplicable
                    {
                        PriceComponent = v,
                        Customer = this.Receiver,
                        Product = quoteItem.Product,
                        QuantityOrdered = quantityOrdered,
                        ValueOrdered = totalBasePrice,
                    })).ToArray();

            var unitBasePrice = priceComponents.OfType<BasePrice>().Min(v => v.Price);
            if (!unitBasePrice.HasValue)
            {
                unitBasePrice = 0;
            }

            // Calculate Unit Price (with Discounts and Surcharges)
            if (quoteItem.AssignedUnitPrice.HasValue)
            {
                quoteItemDeriveRoles.UnitBasePrice = unitBasePrice ?? quoteItem.AssignedUnitPrice.Value;
                quoteItemDeriveRoles.UnitDiscount = 0;
                quoteItemDeriveRoles.UnitSurcharge = 0;
                quoteItemDeriveRoles.UnitPrice = quoteItem.AssignedUnitPrice.Value;
            }
            else
            {
                quoteItemDeriveRoles.UnitBasePrice = unitBasePrice.Value;

                quoteItemDeriveRoles.UnitDiscount = priceComponents.OfType<DiscountComponent>().Sum(
                    v => v.Percentage.HasValue
                             ? Math.Round(quoteItem.UnitBasePrice * v.Percentage.Value / 100, 2)
                             : v.Price ?? 0);

                quoteItemDeriveRoles.UnitSurcharge = priceComponents.OfType<SurchargeComponent>().Sum(
                    v => v.Percentage.HasValue
                             ? Math.Round(quoteItem.UnitBasePrice * v.Percentage.Value / 100, 2)
                             : v.Price ?? 0);

                quoteItemDeriveRoles.UnitPrice = quoteItem.UnitBasePrice - quoteItem.UnitDiscount + quoteItem.UnitSurcharge;

                if (quoteItem.ExistDiscountAdjustment)
                {
                    quoteItemDeriveRoles.UnitDiscount += quoteItem.DiscountAdjustment.Percentage.HasValue ?
                        Math.Round(quoteItem.UnitPrice * quoteItem.DiscountAdjustment.Percentage.Value / 100, 2) :
                        quoteItem.DiscountAdjustment.Amount ?? 0;
                }

                if (quoteItem.ExistSurchargeAdjustment)
                {
                    quoteItemDeriveRoles.UnitSurcharge += quoteItem.SurchargeAdjustment.Percentage.HasValue ?
                        Math.Round(quoteItem.UnitPrice * quoteItem.SurchargeAdjustment.Percentage.Value / 100, 2) :
                        quoteItem.SurchargeAdjustment.Amount ?? 0;
                }

                quoteItemDeriveRoles.UnitPrice = quoteItem.UnitBasePrice - quoteItem.UnitDiscount + quoteItem.UnitSurcharge;
            }

            foreach (QuoteItem featureItem in quoteItem.QuotedWithFeatures)
            {
                quoteItemDeriveRoles.UnitBasePrice += featureItem.UnitBasePrice;
                quoteItemDeriveRoles.UnitPrice += featureItem.UnitPrice;
                quoteItemDeriveRoles.UnitDiscount += featureItem.UnitDiscount;
                quoteItemDeriveRoles.UnitSurcharge += featureItem.UnitSurcharge;
            }

            quoteItemDeriveRoles.UnitVat = quoteItem.ExistVatRate ? Math.Round(quoteItem.UnitPrice * quoteItem.VatRate.Rate / 100, 2) : 0;

            // Calculate Totals
            quoteItemDeriveRoles.TotalBasePrice = quoteItem.UnitBasePrice * quoteItem.Quantity;
            quoteItemDeriveRoles.TotalDiscount = quoteItem.UnitDiscount * quoteItem.Quantity;
            quoteItemDeriveRoles.TotalSurcharge = quoteItem.UnitSurcharge * quoteItem.Quantity;
            quoteItemDeriveRoles.TotalPriceAdjustment = quoteItem.TotalSurcharge - quoteItem.TotalDiscount;

            if (quoteItem.TotalBasePrice > 0)
            {
                quoteItemDeriveRoles.TotalDiscountAsPercentage = Math.Round(quoteItem.TotalDiscount / quoteItem.TotalBasePrice * 100, 2);
                quoteItemDeriveRoles.TotalSurchargeAsPercentage = Math.Round(quoteItem.TotalSurcharge / quoteItem.TotalBasePrice * 100, 2);
            }
            else
            {
                quoteItemDeriveRoles.TotalDiscountAsPercentage = 0;
                quoteItemDeriveRoles.TotalSurchargeAsPercentage = 0;
            }

            quoteItemDeriveRoles.TotalExVat = quoteItem.UnitPrice * quoteItem.Quantity;
            quoteItemDeriveRoles.TotalVat = quoteItem.UnitVat * quoteItem.Quantity;
            quoteItemDeriveRoles.TotalIncVat = quoteItem.TotalExVat + quoteItem.TotalVat;
        }

        private void Sync(ISession session)
        {
            // session.Prefetch(this.SyncPrefetch, this);
            foreach (QuoteItem quoteItem in this.QuoteItems)
            {
                quoteItem.Sync(this);
            }
        }

        private void DeriveWorkflow()
        {
            this.WorkItemDescription = $"ProductQuote: {this.QuoteNumber} [{this.Issuer?.PartyName}]";

            var openTasks = this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();

            if (this.QuoteState.IsCreated)
            {
                if (!openTasks.OfType<ProductQuoteApproval>().Any())
                {
                    new ProductQuoteApprovalBuilder(this.Session()).WithProductQuote(this).Build();
                }
            }
        }

        private SalesOrder OrderThis()
        {
            var salesOrder = new SalesOrderBuilder(this.Strategy.Session)
                .WithTakenBy(this.Issuer)
                .WithBillToCustomer(this.Receiver)
                .WithDescription(this.Description)
                .WithShipToContactPerson(this.ContactPerson)
                .WithBillToContactPerson(this.ContactPerson)
                .WithQuote(this)
                .Build();

            var quoteItems = this.ValidQuoteItems
                .Where(i => i.QuoteItemState.Equals(new QuoteItemStates(this.Strategy.Session).Sent))
                .ToArray();

            foreach (var quoteItem in quoteItems)
            {
                quoteItem.QuoteItemState = new QuoteItemStates(this.Strategy.Session).Ordered;

                salesOrder.AddSalesOrderItem(
                    new SalesOrderItemBuilder(this.Strategy.Session)
                        .WithInvoiceItemType(quoteItem.InvoiceItemType)
                        .WithInternalComment(quoteItem.InternalComment)
                        .WithAssignedDeliveryDate(quoteItem.EstimatedDeliveryDate)
                        .WithAssignedUnitPrice(quoteItem.UnitPrice)
                        .WithProduct(quoteItem.Product)
                        .WithSerialisedItem(quoteItem.SerialisedItem)
                        .WithProductFeature(quoteItem.ProductFeature)
                        .WithQuantityOrdered(quoteItem.Quantity).Build());
            }

            return salesOrder;
        }
    }
}
