// <copyright file="PurchaseInvoice.cs" company="Allors bvba">
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
    using Resources;

    public partial class PurchaseInvoice
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseInvoice, M.PurchaseInvoice.PurchaseInvoiceState),
            };

        private bool IsDeletable =>
            (this.PurchaseInvoiceState.Equals(new PurchaseInvoiceStates(this.Strategy.Session).Created)
                || this.PurchaseInvoiceState.Equals(new PurchaseInvoiceStates(this.Strategy.Session).Cancelled)
                || this.PurchaseInvoiceState.Equals(new PurchaseInvoiceStates(this.Strategy.Session).Rejected))
            && !this.ExistSalesInvoiceWherePurchaseInvoice;

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public InvoiceItem[] InvoiceItems => this.PurchaseInvoiceItems;

        public Task[] OpenTasks => this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();

        public bool NeedsApproval
        {
            get
            {
                if (this.PurchaseOrders.Count > 0)
                {
                    var orderTotal = this.PurchaseInvoiceItems.SelectMany(v => v.OrderItemBillingsWhereInvoiceItem).Select(o => o.OrderItem).Sum(i => i.TotalExVat);
                    if (this.TotalExVat > this.ActualInvoiceAmount)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseInvoiceState)
            {
                this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Created;
            }

            if (!this.ExistInvoiceDate)
            {
                this.InvoiceDate = this.Session().Now();
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = this.Session().Now();
            }
        }

        public void BaseOnInit(ObjectOnInit method)
        {
            this.DerivedCurrency = this.BilledTo?.PreferredCurrency;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistBilledTo && internalOrganisations.Count() == 1)
            {
                this.BilledTo = internalOrganisations.First();
            }
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;
            var internalOrganisation = this.Strategy.Session.GetSingleton();

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.AddDependency(this, internalOrganisation);
                iteration.Mark(internalOrganisation);

                iteration.AddDependency(this, this.BilledFrom);
                iteration.Mark(this.BilledFrom);
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistBilledTo && internalOrganisations.Count() == 1)
            {
                this.BilledTo = internalOrganisations.First();
            }

            if (!this.ExistInvoiceNumber)
            {
                var year = this.InvoiceDate.Year;
                this.InvoiceNumber = this.BilledTo.NextPurchaseInvoiceNumber(year);

                var fiscalYearInternalOrganisationSequenceNumbers = this.BilledTo.FiscalYearsInternalOrganisationSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);
                var prefix = this.BilledTo.InvoiceSequence.IsEnforcedSequence ? this.BilledTo.PurchaseInvoiceNumberPrefix : fiscalYearInternalOrganisationSequenceNumbers.PurchaseInvoiceNumberPrefix;
                this.SortableInvoiceNumber = this.Session().GetSingleton().SortableNumber(prefix, this.InvoiceNumber, year.ToString());
            }

            if (this.BilledFrom is Organisation supplier)
            {
                if (!this.BilledTo.ActiveSuppliers.Contains(supplier))
                {
                    derivation.Validation.AddError(this, this.Meta.BilledFrom, ErrorMessages.PartyIsNotASupplier);
                }
            }

            if (this.PurchaseInvoiceState.IsCreated)
            {
                this.DerivedVatRegime = this.AssignedVatRegime;
                this.DerivedIrpfRegime = this.AssignedIrpfRegime;
                this.DerivedCurrency = this.AssignedCurrency ?? this.BilledTo?.PreferredCurrency;

                if (this.ExistInvoiceDate)
                {
                    this.DerivedVatRate = this.DerivedVatRegime?.VatRates.First(v => v.FromDate <= this.InvoiceDate && (!v.ExistThroughDate || v.ThroughDate >= this.InvoiceDate));
                    this.DerivedIrpfRate = this.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= this.InvoiceDate && (!v.ExistThroughDate || v.ThroughDate >= this.InvoiceDate));
                }
            }

            if (this.ExistInvoiceDate
                && this.ExistBilledTo
                && this.DerivedCurrency != this.BilledTo.PreferredCurrency)
            {
                var exchangeRate = this.DerivedCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= this.InvoiceDate.Date && v.ToCurrency.Equals(this.BilledTo.PreferredCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();

                if (exchangeRate == null)
                {
                    exchangeRate = this.BilledTo.PreferredCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= this.InvoiceDate.Date && v.ToCurrency.Equals(this.DerivedCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();
                }

                if (exchangeRate == null)
                {
                    derivation.Validation.AddError(this, M.Quote.AssignedCurrency, ErrorMessages.CurrencyNotAllowed);
                }
            }

            this.PurchaseOrders = this.InvoiceItems.SelectMany(v => v.OrderItemBillingsWhereInvoiceItem).Select(v => v.OrderItem.OrderWhereValidOrderItem).ToArray();

            var validInvoiceItems = this.PurchaseInvoiceItems.Where(v => v.IsValid).ToArray();
            this.ValidInvoiceItems = validInvoiceItems;

            var purchaseInvoiceStates = new PurchaseInvoiceStates(this.Strategy.Session);
            var purchaseInvoiceItemStates = new PurchaseInvoiceItemStates(this.Strategy.Session);

            this.AmountPaid = this.PaymentApplicationsWhereInvoice.Sum(v => v.AmountApplied);

            //// Perhaps payments are recorded at the item level.
            if (this.AmountPaid == 0)
            {
                this.AmountPaid = this.InvoiceItems.Sum(v => v.AmountPaid);
            }

            foreach (var invoiceItem in validInvoiceItems)
            {
                if (invoiceItem.PurchaseInvoiceWherePurchaseInvoiceItem.PurchaseInvoiceState.IsRevising)
                {
                    invoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.Revising;
                }
                else if (invoiceItem.PurchaseInvoiceWherePurchaseInvoiceItem.PurchaseInvoiceState.IsCreated)
                {
                    invoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.Created;
                }
                else if (invoiceItem.PurchaseInvoiceWherePurchaseInvoiceItem.PurchaseInvoiceState.IsAwaitingApproval)
                {
                    invoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.AwaitingApproval;
                }
                else if (invoiceItem.AmountPaid == 0)
                {
                    invoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.NotPaid;
                }
                else if (invoiceItem.ExistAmountPaid && invoiceItem.AmountPaid >= invoiceItem.GrandTotal)
                {
                    invoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.Paid;
                }
                else
                {
                    invoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.PartiallyPaid;
                }
            }

            if (validInvoiceItems.Any())
            {
                if (!this.PurchaseInvoiceState.IsRevising
                    && !this.PurchaseInvoiceState.IsCreated
                    && !this.PurchaseInvoiceState.IsAwaitingApproval)
                {
                    if (this.PurchaseInvoiceItems.All(v => v.PurchaseInvoiceItemState.IsPaid))
                    {
                        this.PurchaseInvoiceState = purchaseInvoiceStates.Paid;
                    }
                    else if (this.PurchaseInvoiceItems.All(v => v.PurchaseInvoiceItemState.IsNotPaid))
                    {
                        this.PurchaseInvoiceState = purchaseInvoiceStates.NotPaid;
                    }
                    else
                    {
                        this.PurchaseInvoiceState = purchaseInvoiceStates.PartiallyPaid;
                    }
                }
            }

            // If disbursements are not matched at invoice level
            if (!this.PurchaseInvoiceState.IsRevising
                && this.AmountPaid != 0)
            {
                if (this.AmountPaid >= decimal.Round(this.GrandTotal, 2))
                {
                    this.PurchaseInvoiceState = purchaseInvoiceStates.Paid;
                }
                else
                {
                    this.PurchaseInvoiceState = purchaseInvoiceStates.PartiallyPaid;
                }

                foreach (var invoiceItem in validInvoiceItems)
                {
                    if (this.AmountPaid >= decimal.Round(this.GrandTotal, 2))
                    {
                        invoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.Paid;
                    }
                    else
                    {
                        invoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.PartiallyPaid;
                    }
                }
            }

            if (!this.PurchaseInvoiceState.IsRevising
                && !this.PurchaseInvoiceState.IsCreated
                && !this.PurchaseInvoiceState.IsAwaitingApproval)
            {
                foreach (var invoiceItem in validInvoiceItems)
                {
                    if (invoiceItem.ExistSerialisedItem
                        && this.BilledTo.SerialisedItemSoldOns.Contains(new SerialisedItemSoldOns(this.Session()).PurchaseInvoiceConfirm))
                    {
                        if ((this.BilledFrom as InternalOrganisation)?.IsInternalOrganisation == false)
                        {
                            invoiceItem.SerialisedItem.Buyer = this.BilledTo;
                        }

                        //TODO: Martien , remove check on date and place it in custom derive
                        // who comes first?
                        // Item you purchased can be on sold via sales invoice even before purchase invoice is created and confirmed!!

                        if (this.InvoiceDate > new DateTime(2021, 07, 04)
                             && !invoiceItem.SerialisedItem.SalesInvoiceItemsWhereSerialisedItem.Any(v => (v.SalesInvoiceWhereSalesInvoiceItem.BillToCustomer as Organisation)?.IsInternalOrganisation == false
                                                                                                            && v.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceType.Equals(new SalesInvoiceTypes(this.Session()).SalesInvoice)
                                                                                                            && !v.SalesInvoiceWhereSalesInvoiceItem.ExistSalesInvoiceWhereCreditedFromInvoice))
                        {
                            invoiceItem.SerialisedItem.OwnedBy = this.BilledTo;
                            invoiceItem.SerialisedItem.Ownership = new Ownerships(this.Session()).Own;
                        }
                    }
                }
            }

            this.BaseOnDeriveInvoiceItems(derivation);
            this.BaseOnDeriveInvoiceTotals();

            this.DeriveWorkflow();

            var singleton = this.Session().GetSingleton();

            this.AddSecurityToken(new SecurityTokens(this.Session()).DefaultSecurityToken);

            this.Sync(this.Session());

            this.ResetPrintDocument();
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var deletePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Delete, Operations.Execute);
            if (this.IsDeletable)
            {
                this.RemoveDeniedPermission(deletePermission);
            }
            else
            {
                this.AddDeniedPermission(deletePermission);
            }

            var revisePermission = new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.Revise, Operations.Execute);
            if (this.BilledTo.DoAccounting)
            {
                this.AddDeniedPermission(revisePermission);
            }

            if (!this.ExistSalesInvoiceWherePurchaseInvoice
                && (this.BilledFrom as Organisation)?.IsInternalOrganisation == true
                && (this.PurchaseInvoiceState.IsPaid || this.PurchaseInvoiceState.IsPartiallyPaid || this.PurchaseInvoiceState.IsNotPaid))
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.CreateSalesInvoice, Operations.Execute));
            }
            else
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.CreateSalesInvoice, Operations.Execute));
            }
        }

        public void BasePrint(PrintablePrint method)
        {
            if (!method.IsPrinted)
            {
                var singleton = this.Strategy.Session.GetSingleton();
                var logo = this.BilledTo?.ExistLogoImage == true ?
                    this.BilledTo.LogoImage.MediaContent.Data :
                    singleton.LogoImage.MediaContent.Data;

                var images = new Dictionary<string, byte[]>
                {
                    { "Logo", logo },
                };

                if (this.ExistInvoiceNumber)
                {
                    var session = this.Strategy.Session;
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(this.InvoiceNumber, BarcodeType.CODE_128, 320, 80, pure: true);
                    images.Add("Barcode", barcode);
                }

                var model = new Print.PurchaseInvoiceModel.Model(this);
                this.RenderPrintDocument(this.BilledTo?.PurchaseInvoiceTemplate, model, images);

                this.PrintDocument.Media.InFileName = $"{this.InvoiceNumber}.odt";
            }
        }

        public void BaseOnDeriveInvoiceTotals()
        {
            this.TotalBasePrice = 0;
            this.TotalDiscount = 0;
            this.TotalSurcharge = 0;
            this.TotalVat = 0;
            this.TotalExVat = 0;
            this.TotalIncVat = 0;
            this.TotalIrpf = 0;
            this.TotalExtraCharge = 0;
            this.GrandTotal = 0;

            foreach (PurchaseInvoiceItem item in this.PurchaseInvoiceItems)
            {
                this.TotalBasePrice += item.TotalBasePrice;
                this.TotalSurcharge += item.TotalSurcharge;
                this.TotalIrpf += item.TotalIrpf;
                this.TotalVat += item.TotalVat;
                this.TotalExVat += item.TotalExVat;
                this.TotalIncVat += item.TotalIncVat;
                this.GrandTotal += item.GrandTotal;
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

            if (this.ExistInvoiceDate && this.ExistDerivedCurrency && this.ExistBilledTo)
            {
                this.TotalBasePriceInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalBasePrice, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalDiscountInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalDiscount, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalSurchargeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalSurcharge, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalExtraChargeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalExtraCharge, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalFeeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalFee, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalShippingAndHandlingInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalShippingAndHandling, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalExVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalExVat, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalVat, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalIncVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalIncVat, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.TotalIrpfInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalIrpf, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
                this.GrandTotalInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(grandTotal, this.InvoiceDate, this.DerivedCurrency, this.BilledTo.PreferredCurrency), 2);
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
        }

        public void BaseConfirm(PurchaseInvoiceConfirm method) => this.PurchaseInvoiceState = this.NeedsApproval ? new PurchaseInvoiceStates(this.Strategy.Session).AwaitingApproval : new PurchaseInvoiceStates(this.Strategy.Session).NotPaid;

        public void BaseCancel(PurchaseInvoiceCancel method)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Cancelled;
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                purchaseInvoiceItem.CancelFromInvoice();
            }
        }

        public void BaseReject(PurchaseInvoiceReject method)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Rejected;
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                purchaseInvoiceItem.Reject();
            }
        }

        public void BaseReopen(PurchaseInvoiceReopen method)
        {
            this.PurchaseInvoiceState = this.PreviousPurchaseInvoiceState;
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.InvoiceItems)
            {
                purchaseInvoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItem.PreviousPurchaseInvoiceItemState;
            }
        }

        public void BaseApprove(PurchaseInvoiceApprove method)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).NotPaid;

            var openTasks = this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();

            if (openTasks.OfType<PurchaseInvoiceApproval>().Any())
            {
                openTasks.First().DateClosed = this.Session().Now();
            }

            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                if (purchaseInvoiceItem.ExistPart)
                {
                    var previousOffering = purchaseInvoiceItem.Part.SupplierOfferingsWherePart.FirstOrDefault(v =>
                        v.Supplier.Equals(this.BilledFrom) && v.FromDate <= this.Session().Now() &&
                        (!v.ExistThroughDate || v.ThroughDate >= this.Session().Now()));

                    if (previousOffering != null)
                    {
                        if (purchaseInvoiceItem.UnitBasePrice != previousOffering.Price)
                        {
                            previousOffering.ThroughDate = this.Session().Now();

                            var newOffering = new SupplierOfferingBuilder(this.Session())
                                .WithSupplier(this.BilledFrom)
                                .WithPart(purchaseInvoiceItem.Part)
                                .WithPrice(purchaseInvoiceItem.UnitBasePrice)
                                .WithFromDate(this.Session().Now())
                                .WithComment(previousOffering.Comment)
                                .WithMinimalOrderQuantity(previousOffering.MinimalOrderQuantity)
                                .WithPreference(previousOffering.Preference)
                                .WithQuantityIncrements(previousOffering.QuantityIncrements)
                                .WithRating(previousOffering.Rating)
                                .WithStandardLeadTime(previousOffering.StandardLeadTime)
                                .WithSupplierProductId(previousOffering.SupplierProductId)
                                .WithSupplierProductName(previousOffering.SupplierProductName)
                                .WithUnitOfMeasure(previousOffering.UnitOfMeasure)
                                .Build();

                            newOffering.LocalisedComments = previousOffering.LocalisedComments;
                        }
                    }
                    else
                    {
                        new SupplierOfferingBuilder(this.Session())
                            .WithSupplier(this.BilledFrom)
                            .WithPart(purchaseInvoiceItem.Part)
                            .WithUnitOfMeasure(purchaseInvoiceItem.Part?.UnitOfMeasure)
                            .WithPrice(purchaseInvoiceItem.UnitBasePrice)
                            .WithFromDate(this.Session().Now())
                            .Build();
                    }

                    foreach(OrderItemBilling orderItemBilling in purchaseInvoiceItem.OrderItemBillingsWhereInvoiceItem)
                    {
                        foreach (ShipmentReceipt receipt in orderItemBilling.OrderItem.ShipmentReceiptsWhereOrderItem)
                        {
                            receipt.ShipmentItem.InventoryItemTransactionWhereShipmentItem.Cost = purchaseInvoiceItem.UnitBasePrice;
                        }
                    }
                }
            }
        }

        public void BaseRevise(PurchaseInvoiceRevise method)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Revising;
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                purchaseInvoiceItem.Revise();
            }
        }

        public void BaseFinishRevising(PurchaseInvoiceFinishRevising method)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Created;
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                purchaseInvoiceItem.FinishRevising();
            }
        }

        public void BaseDelete(DeletableDelete method)
        {
            if (this.IsDeletable)
            {
                foreach (OrderAdjustment orderAdjustment in this.OrderAdjustments)
                {
                    orderAdjustment.Delete();
                }

                foreach (PurchaseInvoiceItem invoiceItem in this.PurchaseInvoiceItems)
                {
                    invoiceItem.Delete();
                }

                foreach (SalesTerm salesTerm in this.SalesTerms)
                {
                    salesTerm.Delete();
                }
            }
        }

        public void BaseCreateSalesInvoice(PurchaseInvoiceCreateSalesInvoice method)
        {
            var salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                .WithPurchaseInvoice(this)
                .WithBilledFrom(this.BilledTo)
                .WithBilledFromContactPerson(this.BilledToContactPerson)
                .WithBillToCustomer(this.BillToEndCustomer)
                .WithAssignedBillToContactMechanism(this.DerivedBillToEndCustomerContactMechanism)
                .WithBillToContactPerson(this.BillToEndCustomerContactPerson)
                .WithShipToCustomer(this.ShipToEndCustomer)
                .WithAssignedShipToAddress(this.DerivedShipToEndCustomerAddress)
                .WithShipToContactPerson(this.ShipToEndCustomerContactPerson)
                .WithDescription(this.Description)
                .WithInvoiceDate(this.Session().Now())
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                .WithCustomerReference(this.CustomerReference)
                .WithAssignedPaymentMethod(this.DerivedBillToCustomerPaymentMethod)
                .WithComment(this.Comment)
                .WithInternalComment(this.InternalComment)
                .Build();

            foreach (OrderAdjustment orderAdjustment in this.OrderAdjustments)
            {
                OrderAdjustment newAdjustment = null;
                if (orderAdjustment.GetType().Name.Equals(typeof(DiscountAdjustment).Name))
                {
                    newAdjustment = new DiscountAdjustmentBuilder(this.Session()).Build();
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(SurchargeAdjustment).Name))
                {
                    newAdjustment = new SurchargeAdjustmentBuilder(this.Session()).Build();
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(Fee).Name))
                {
                    newAdjustment = new FeeBuilder(this.Session()).Build();
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(ShippingAndHandlingCharge).Name))
                {
                    newAdjustment = new ShippingAndHandlingChargeBuilder(this.Session()).Build();
                }

                if (orderAdjustment.GetType().Name.Equals(typeof(MiscellaneousCharge).Name))
                {
                    newAdjustment = new MiscellaneousChargeBuilder(this.Session()).Build();
                }

                newAdjustment.Amount ??= orderAdjustment.Amount;
                newAdjustment.Percentage ??= orderAdjustment.Percentage;
                salesInvoice.AddOrderAdjustment(newAdjustment);
            }

            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.PurchaseInvoiceItems)
            {
                var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                    .WithInvoiceItemType(purchaseInvoiceItem.InvoiceItemType)
                    .WithAssignedUnitPrice(purchaseInvoiceItem.AssignedUnitPrice)
                    .WithProduct(purchaseInvoiceItem.Part as UnifiedGood)
                    .WithSerialisedItem(purchaseInvoiceItem.SerialisedItem)
                    .WithNextSerialisedItemAvailability(new SerialisedItemAvailabilities(this.Session()).Sold)
                    .WithQuantity(purchaseInvoiceItem.Quantity)
                    .WithComment(purchaseInvoiceItem.Comment)
                    .WithInternalComment(purchaseInvoiceItem.InternalComment)
                    .Build();

                salesInvoice.AddSalesInvoiceItem(invoiceItem);
            }

            var internalOrganisation = (InternalOrganisation)salesInvoice.BilledFrom;
            if (!internalOrganisation.ActiveCustomers.Contains(salesInvoice.BillToCustomer))
            {
                new CustomerRelationshipBuilder(this.Strategy.Session)
                    .WithCustomer(salesInvoice.BillToCustomer)
                    .WithInternalOrganisation(internalOrganisation)
                    .Build();
            }

            this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.ObjectType, this.Meta.CreateSalesInvoice, Operations.Execute));
        }

        public void BaseOnDeriveInvoiceItems(IDerivation derivation)
        {
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                if (purchaseInvoiceItem.PurchaseInvoiceItemState.IsNotPaid
                    && purchaseInvoiceItem.ExistSerialisedItem
                    && purchaseInvoiceItem.InvoiceItemType.IsProductItem)
                {
                    var serialisedItem = purchaseInvoiceItem.SerialisedItem;
                    var deriveRoles = (SerialisedItemDerivedRoles)purchaseInvoiceItem.SerialisedItem;

                    serialisedItem.RemoveAssignedPurchasePrice();
                    deriveRoles.PurchasePrice = purchaseInvoiceItem.TotalExVat;
                }

                purchaseInvoiceItem.BaseOnDerivePrices();
            }
        }

        private void Sync(ISession session)
        {
            // session.Prefetch(this.SyncPrefetch, this);
            foreach (PurchaseInvoiceItem invoiceItem in this.PurchaseInvoiceItems)
            {
                invoiceItem.Sync(this);
            }
        }

        private void DeriveWorkflow()
        {
            this.WorkItemDescription = $"PurchaseInvoice: {this.InvoiceNumber} [{this.BilledFrom?.PartyName}]";

            if (this.PurchaseInvoiceState.IsAwaitingApproval)
            {
                if (!this.OpenTasks.OfType<PurchaseInvoiceApproval>().Any())
                {
                    var approval = new PurchaseInvoiceApprovalBuilder(this.Session()).WithPurchaseInvoice(this).Build();
                    approval.DerivedRoles.WorkItem = approval.PurchaseInvoice;
                }
            }
        }
    }
}
