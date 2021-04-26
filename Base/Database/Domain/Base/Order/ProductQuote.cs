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
                                     { "Logo1", logo },
                                     { "Logo2", logo },
                                 };

                if (this.ExistQuoteNumber)
                {
                    var session = this.Strategy.Session;
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(this.QuoteNumber, BarcodeType.CODE_128, 320, 80, pure: true);
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

            var discount = 0M;
            var discountVat = 0M;
            var discountIrpf = 0M;
            var surcharge = 0M;
            var surchargeVat = 0M;
            var surchargeIrpf = 0M;
            var fee = 0M;
            var feeVat = 0M;
            var feeIrpf = 0M;
            var shipping = 0M;
            var shippingVat = 0M;
            var shippingIrpf = 0M;
            var miscellaneous = 0M;
            var miscellaneousVat = 0M;
            var miscellaneousIrpf = 0M;

            if (this.ExistIssueDate)
            {
                this.DerivedVatRate = this.DerivedVatRegime?.VatRates.First(v => v.FromDate <= this.IssueDate && (!v.ExistThroughDate || v.ThroughDate >= this.IssueDate));
                this.DerivedIrpfRate = this.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= this.IssueDate && (!v.ExistThroughDate || v.ThroughDate >= this.IssueDate));
            }

            foreach (OrderAdjustment orderAdjustment in this.OrderAdjustments)
            {
                if (orderAdjustment.GetType().Name.Equals(typeof(DiscountAdjustment).Name))
                {
                    discount = orderAdjustment.Percentage.HasValue ?
                                    this.TotalExVat * orderAdjustment.Percentage.Value / 100 :
                                    orderAdjustment.Amount ?? 0;

                    this.TotalDiscount += discount;

                    if (this.ExistDerivedVatRegime)
                    {
                        discountVat = discount * this.DerivedVatRate.Rate / 100;
                    }

                    if (this.ExistDerivedIrpfRegime)
                    {
                        discountIrpf = discount * this.DerivedIrpfRate.Rate / 100;
                    }
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(SurchargeAdjustment).Name))
                {
                    surcharge = orderAdjustment.Percentage.HasValue ?
                                        this.TotalExVat * orderAdjustment.Percentage.Value / 100 :
                                        orderAdjustment.Amount ?? 0;

                    this.TotalSurcharge += surcharge;

                    if (this.ExistDerivedVatRegime)
                    {
                        surchargeVat = surcharge * this.DerivedVatRate.Rate / 100;
                    }

                    if (this.ExistDerivedIrpfRegime)
                    {
                        surchargeIrpf = surcharge * this.DerivedIrpfRate.Rate / 100;
                    }
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(Fee).Name))
                {
                    fee = orderAdjustment.Percentage.HasValue ?
                                this.TotalExVat * orderAdjustment.Percentage.Value / 100 :
                                orderAdjustment.Amount ?? 0;

                    this.TotalFee += fee;

                    if (this.ExistDerivedVatRegime)
                    {
                        feeVat = fee * this.DerivedVatRate.Rate / 100;
                    }

                    if (this.ExistDerivedIrpfRegime)
                    {
                        feeIrpf = fee * this.DerivedIrpfRate.Rate / 100;
                    }
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(ShippingAndHandlingCharge).Name))
                {
                    shipping = orderAdjustment.Percentage.HasValue ?
                                    this.TotalExVat * orderAdjustment.Percentage.Value / 100 :
                                    orderAdjustment.Amount ?? 0;

                    this.TotalShippingAndHandling += shipping;

                    if (this.ExistDerivedVatRegime)
                    {
                        shippingVat = shipping * this.DerivedVatRate.Rate / 100;
                    }

                    if (this.ExistDerivedIrpfRegime)
                    {
                        shippingIrpf = shipping * this.DerivedIrpfRate.Rate / 100;
                    }
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(MiscellaneousCharge).Name))
                {
                    miscellaneous = orderAdjustment.Percentage.HasValue ?
                                    this.TotalExVat * orderAdjustment.Percentage.Value / 100 :
                                    orderAdjustment.Amount ?? 0;

                    this.TotalExtraCharge += miscellaneous;

                    if (this.ExistDerivedVatRegime)
                    {
                        miscellaneousVat = miscellaneous * this.DerivedVatRate.Rate / 100;
                    }

                    if (this.ExistDerivedIrpfRegime)
                    {
                        miscellaneousIrpf = miscellaneous * this.DerivedIrpfRate.Rate / 100;
                    }
                }
            }

            var totalExtraCharge = fee + shipping + miscellaneous;
            var totalExVat = this.TotalExVat - discount + surcharge + fee + shipping + miscellaneous;
            var totalVat = this.TotalVat - discountVat + surchargeVat + feeVat + shippingVat + miscellaneousVat;
            var totalIncVat = this.TotalIncVat - discount - discountVat + surcharge + surchargeVat + fee + feeVat + shipping + shippingVat + miscellaneous + miscellaneousVat;
            var totalIrpf = this.TotalIrpf + discountIrpf - surchargeIrpf - feeIrpf - shippingIrpf - miscellaneousIrpf;
            var grandTotal = totalIncVat - totalIrpf;

            if (this.ExistIssueDate && this.ExistDerivedCurrency && this.ExistIssuer)
            {
                this.TotalBasePriceInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalBasePrice, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalDiscountInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalDiscount, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalSurchargeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalSurcharge, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalExtraChargeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalExtraCharge, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalFeeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalFee, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalShippingAndHandlingInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalShippingAndHandling, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalExVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalExVat, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalVat, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalIncVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalIncVat, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.TotalIrpfInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalIrpf, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
                this.GrandTotalInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(grandTotal, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
            }

            this.TotalBasePrice = Rounder.RoundDecimal(this.TotalBasePrice, 2);
            this.TotalDiscount = Rounder.RoundDecimal(this.TotalDiscount, 2);
            this.TotalSurcharge = Rounder.RoundDecimal(this.TotalSurcharge, 2);
            this.TotalExtraCharge = Rounder.RoundDecimal(totalExtraCharge, 2);
            this.TotalFee = Rounder.RoundDecimal(this.TotalFee, 2);
            this.TotalShippingAndHandling = Rounder.RoundDecimal(this.TotalShippingAndHandling, 2);
            this.TotalExVat = Rounder.RoundDecimal(totalExVat, 2);
            this.TotalVat = Rounder.RoundDecimal(totalVat, 2);
            this.TotalIncVat = Rounder.RoundDecimal(totalIncVat, 2);
            this.TotalIrpf = Rounder.RoundDecimal(totalIrpf, 2);
            this.GrandTotal = Rounder.RoundDecimal(grandTotal, 2);

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

            this.TotalListPriceInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(TotalListPrice, this.IssueDate, this.DerivedCurrency, this.Issuer.PreferredCurrency), 2);
            this.TotalListPrice = Rounder.RoundDecimal(this.TotalListPrice, 2);

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
                             ? quoteItem.UnitBasePrice * v.Percentage.Value / 100
                             : v.Price ?? 0);

                quoteItemDeriveRoles.UnitSurcharge = priceComponents.OfType<SurchargeComponent>().Sum(
                    v => v.Percentage.HasValue
                             ? quoteItem.UnitBasePrice * v.Percentage.Value / 100
                             : v.Price ?? 0);

                quoteItemDeriveRoles.UnitPrice = quoteItem.UnitBasePrice - quoteItem.UnitDiscount + quoteItem.UnitSurcharge;

                foreach (OrderAdjustment orderAdjustment in quoteItem.DiscountAdjustments)
                {
                    quoteItemDeriveRoles.UnitDiscount += orderAdjustment.Percentage.HasValue ?
                        quoteItem.UnitPrice * orderAdjustment.Percentage.Value / 100 :
                        orderAdjustment.Amount ?? 0;
                }

                foreach (OrderAdjustment orderAdjustment in quoteItem.SurchargeAdjustments)
                {
                    quoteItemDeriveRoles.UnitSurcharge += orderAdjustment.Percentage.HasValue ?
                        quoteItem.UnitPrice * orderAdjustment.Percentage.Value / 100 :
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

            quoteItemDeriveRoles.UnitVat = quoteItem.ExistVatRate ? quoteItem.UnitPrice * quoteItem.VatRate.Rate / 100 : 0;
            quoteItemDeriveRoles.UnitIrpf = quoteItem.ExistIrpfRate ? quoteItem.UnitPrice * quoteItem.IrpfRate.Rate / 100 : 0;

            // Calculate Totals
            quoteItemDeriveRoles.TotalBasePrice = quoteItem.UnitBasePrice * quoteItem.Quantity;
            quoteItemDeriveRoles.TotalDiscount = quoteItem.UnitDiscount * quoteItem.Quantity;
            quoteItemDeriveRoles.TotalSurcharge = quoteItem.UnitSurcharge * quoteItem.Quantity;
            quoteItemDeriveRoles.TotalPriceAdjustment = quoteItem.TotalSurcharge - quoteItem.TotalDiscount;

            if (quoteItem.TotalBasePrice > 0)
            {
                quoteItemDeriveRoles.TotalDiscountAsPercentage = Rounder.RoundDecimal(quoteItem.TotalDiscount / quoteItem.TotalBasePrice * 100, 2);
                quoteItemDeriveRoles.TotalSurchargeAsPercentage = Rounder.RoundDecimal(quoteItem.TotalSurcharge / quoteItem.TotalBasePrice * 100, 2);
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
                .WithAssignedVatRegime(this.DerivedVatRegime)
                .WithAssignedIrpfRegime(this.DerivedIrpfRegime)
                .WithShipToContactPerson(this.ContactPerson)
                .WithBillToContactPerson(this.ContactPerson)
                .WithQuote(this)
                .WithDescription(this.Description)
                .WithAssignedCurrency(this.DerivedCurrency)
                .WithLocale(this.DerivedLocale)
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
                        .WithAssignedVatRegime(quoteItem.DerivedVatRegime)
                        .WithAssignedIrpfRegime(quoteItem.DerivedIrpfRegime)
                        .WithProduct(quoteItem.Product)
                        .WithSerialisedItem(quoteItem.SerialisedItem)
                        .WithNextSerialisedItemAvailability(new SerialisedItemAvailabilities(this.Session()).Sold) 
                        .WithProductFeature(quoteItem.ProductFeature)
                        .WithQuantityOrdered(quoteItem.Quantity)
                        .WithInternalComment(quoteItem.InternalComment)
                        .Build());
            }

            return salesOrder;
        }
    }
}
