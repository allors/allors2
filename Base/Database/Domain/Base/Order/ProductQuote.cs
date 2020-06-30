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
        public bool IsDeletable =>
              (this.QuoteState.Equals(new QuoteStates(this.Strategy.Session).Created)
                  || this.QuoteState.Equals(new QuoteStates(this.Strategy.Session).Cancelled)
                  || this.QuoteState.Equals(new QuoteStates(this.Strategy.Session).Rejected))
              && !this.ExistRequest
              && !this.ExistSalesOrderWhereQuote
              && this.QuoteItems.All(v => v.IsDeletable);

        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.ProductQuote, M.ProductQuote.QuoteState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        private bool BaseNeedsApproval => false;

        public void BaseSetReadyForProcessing(ProductQuoteSetReadyForProcessing method)
        {
            if (!method.Result.HasValue)
            {
                this.QuoteState = this.BaseNeedsApproval
                    ? new QuoteStates(this.Strategy.Session).AwaitingApproval : new QuoteStates(this.Strategy.Session).InProcess;

                method.Result = true;
            }
        }

        public void BaseOrder(ProductQuoteOrder method)
        {
            this.QuoteState = new QuoteStates(this.Strategy.Session).Ordered;

            var quoteItemStates = new QuoteItemStates(this.Session());
            foreach (QuoteItem quoteItem in this.QuoteItems)
            {
                if (Equals(quoteItem.QuoteItemState, quoteItemStates.Accepted))
                {
                    quoteItem.QuoteItemState = quoteItemStates.Ordered;
                }
            }

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

            this.ValidQuoteItems = this.QuoteItems.Where(v => v.IsValid).ToArray();

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

            // SalesOrderItem Derivations and Validations
            foreach (QuoteItem quoteItem in this.ValidQuoteItems)
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

            // Calculate Totals
            this.TotalBasePrice = 0;
            this.TotalDiscount = 0;
            this.TotalSurcharge = 0;
            this.TotalExVat = 0;
            this.TotalFee = 0;
            this.TotalShippingAndHandling = 0;
            this.TotalExtraCharge = 0;
            this.TotalVat = 0;
            this.TotalIrpf = 0;
            this.TotalIncVat = 0;
            this.TotalListPrice = 0;
            this.GrandTotal = 0;

            foreach (QuoteItem quoteItem in this.ValidQuoteItems)
            {
                if (!quoteItem.ExistQuoteItemWhereQuotedWithFeature)
                {
                    this.TotalBasePrice += quoteItem.TotalBasePrice;
                    this.TotalDiscount += quoteItem.TotalDiscount;
                    this.TotalSurcharge += quoteItem.TotalSurcharge;
                    this.TotalExVat += quoteItem.TotalExVat;
                    this.TotalVat += quoteItem.TotalVat;
                    this.TotalIrpf += quoteItem.TotalIrpf;
                    this.TotalIncVat += quoteItem.TotalIncVat;
                    this.TotalListPrice += quoteItem.TotalExVat;
                    this.GrandTotal += quoteItem.GrandTotal;
                }
            }

            foreach (OrderAdjustment orderAdjustment in this.OrderAdjustments)
            {
                if (orderAdjustment.GetType().Name.Equals(typeof(DiscountAdjustment).Name))
                {
                    var discount = orderAdjustment.Percentage.HasValue ?
                                    Math.Round(this.TotalExVat * orderAdjustment.Percentage.Value / 100, 2) :
                                    orderAdjustment.Amount ?? 0;

                    this.TotalDiscount += discount;
                    this.TotalExVat -= discount;

                    if (this.ExistVatRegime)
                    {
                        var vat = Math.Round(discount * this.VatRegime.VatRate.Rate / 100, 2);

                        this.TotalVat -= vat;
                        this.TotalIncVat -= discount + vat;
                        this.GrandTotal -= discount + vat;
                    }

                    if (this.ExistIrpfRegime)
                    {
                        var irpf = Math.Round(discount * this.IrpfRegime.IrpfRate.Rate / 100, 2);

                        this.TotalIrpf -= irpf;
                        this.GrandTotal += irpf;
                    }
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(SurchargeAdjustment).Name))
                {
                    var surcharge = orderAdjustment.Percentage.HasValue ?
                                        Math.Round(this.TotalExVat * orderAdjustment.Percentage.Value / 100, 2) :
                                        orderAdjustment.Amount ?? 0;

                    this.TotalSurcharge += surcharge;
                    this.TotalExVat += surcharge;

                    if (this.ExistVatRegime)
                    {
                        var vat = Math.Round(surcharge * this.VatRegime.VatRate.Rate / 100, 2);
                        this.TotalVat += vat;
                        this.TotalIncVat += surcharge + vat;
                        this.GrandTotal -= surcharge + vat;
                    }

                    if (this.ExistIrpfRegime)
                    {
                        var irpf = Math.Round(surcharge * this.IrpfRegime.IrpfRate.Rate / 100, 2);

                        this.TotalIrpf += irpf;
                        this.GrandTotal -= irpf;
                    }
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(Fee).Name))
                {
                    var fee = orderAdjustment.Percentage.HasValue ?
                                Math.Round(this.TotalExVat * orderAdjustment.Percentage.Value / 100, 2) :
                                orderAdjustment.Amount ?? 0;

                    this.TotalFee += fee;
                    this.TotalExtraCharge += fee;
                    this.TotalExVat += fee;

                    if (this.ExistVatRegime)
                    {
                        var vat = Math.Round(fee * this.VatRegime.VatRate.Rate / 100, 2);
                        this.TotalVat += vat;
                        this.TotalIncVat += fee + vat;
                        this.GrandTotal -= fee + vat;
                    }

                    if (this.ExistIrpfRegime)
                    {
                        var irpf = Math.Round(fee * this.IrpfRegime.IrpfRate.Rate / 100, 2);

                        this.TotalIrpf += irpf;
                        this.GrandTotal -= irpf;
                    }
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(ShippingAndHandlingCharge).Name))
                {
                    var shipping = orderAdjustment.Percentage.HasValue ?
                                    Math.Round(this.TotalExVat * orderAdjustment.Percentage.Value / 100, 2) :
                                    orderAdjustment.Amount ?? 0;

                    this.TotalShippingAndHandling += shipping;
                    this.TotalExtraCharge += shipping;
                    this.TotalExVat += shipping;

                    if (this.ExistVatRegime)
                    {
                        var vat = Math.Round(shipping * this.VatRegime.VatRate.Rate / 100, 2);
                        this.TotalVat += vat;
                        this.TotalIncVat += shipping + vat;
                        this.GrandTotal -= shipping + vat;
                    }

                    if (this.ExistIrpfRegime)
                    {
                        var irpf = Math.Round(shipping * this.IrpfRegime.IrpfRate.Rate / 100, 2);

                        this.TotalIrpf += irpf;
                        this.GrandTotal -= irpf;
                    }
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(MiscellaneousCharge).Name))
                {
                    var miscellaneous = orderAdjustment.Percentage.HasValue ?
                                    Math.Round(this.TotalExVat * orderAdjustment.Percentage.Value / 100, 2) :
                                    orderAdjustment.Amount ?? 0;

                    this.TotalExtraCharge += miscellaneous;
                    this.TotalExVat += miscellaneous;

                    if (this.ExistVatRegime)
                    {
                        var vat = Math.Round(miscellaneous * this.VatRegime.VatRate.Rate / 100, 2);
                        this.TotalVat += vat;
                        this.TotalIncVat += miscellaneous + vat;
                        this.GrandTotal -= miscellaneous + vat;
                    }

                    if (this.ExistIrpfRegime)
                    {
                        var irpf = Math.Round(miscellaneous * this.IrpfRegime.IrpfRate.Rate / 100, 2);

                        this.TotalIrpf += irpf;
                        this.GrandTotal -= irpf;
                    }
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

            this.DeriveWorkflow();

            this.Sync(this.Strategy.Session);

            this.ResetPrintDocument();
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var SetReadyPermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.SetReadyForProcessing, Operations.Execute);

            if (this.QuoteState.IsCreated)
            {
                if (this.ExistValidQuoteItems)
                {
                    this.RemoveDeniedPermission(SetReadyPermission);
                }
                else
                {
                    this.AddDeniedPermission(SetReadyPermission);
                }
            }

            var deletePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable)
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }
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

                foreach (OrderAdjustment orderAdjustment in quoteItem.DiscountAdjustments)
                {
                    quoteItemDeriveRoles.UnitDiscount += orderAdjustment.Percentage.HasValue ?
                        Math.Round(quoteItem.UnitPrice * orderAdjustment.Percentage.Value / 100, 2) :
                        orderAdjustment.Amount ?? 0;
                }

                foreach (OrderAdjustment orderAdjustment in quoteItem.SurchargeAdjustments)
                {
                    quoteItemDeriveRoles.UnitSurcharge += orderAdjustment.Percentage.HasValue ?
                        Math.Round(quoteItem.UnitPrice * orderAdjustment.Percentage.Value / 100, 2) :
                        orderAdjustment.Amount ?? 0;
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
            quoteItemDeriveRoles.UnitIrpf = quoteItem.ExistIrpfRate ? Math.Round(quoteItem.UnitPrice * quoteItem.IrpfRate.Rate / 100, 2) : 0;

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
            quoteItemDeriveRoles.TotalIrpf = quoteItem.UnitIrpf * quoteItem.Quantity;
            quoteItemDeriveRoles.GrandTotal = quoteItem.TotalIncVat - quoteItem.TotalIrpf;
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

            if (this.QuoteState.IsAwaitingApproval)
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
                .WithVatRegime(this.VatRegime)
                .WithIrpfRegime(this.IrpfRegime)
                .WithShipToContactPerson(this.ContactPerson)
                .WithBillToContactPerson(this.ContactPerson)
                .WithQuote(this)
                .WithDescription(this.Description)
                .WithCurrency(this.Currency)
                .Build();

            var quoteItems = this.ValidQuoteItems
                .Where(i => i.QuoteItemState.Equals(new QuoteItemStates(this.Strategy.Session).Ordered))
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
                        .WithAssignedVatRegime(quoteItem.AssignedVatRegime)
                        .WithAssignedIrpfRegime(quoteItem.AssignedIrpfRegime)
                        .WithProduct(quoteItem.Product)
                        .WithSerialisedItem(quoteItem.SerialisedItem)
                        .WithProductFeature(quoteItem.ProductFeature)
                        .WithQuantityOrdered(quoteItem.Quantity)
                        .WithInternalComment(quoteItem.InternalComment)
                        .Build());
            }

            return salesOrder;
        }
    }
}
