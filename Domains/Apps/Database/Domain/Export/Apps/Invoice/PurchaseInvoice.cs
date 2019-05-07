// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseInvoice.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Allors.Domain.NonLogging;
using Allors.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Allors.Domain
{
    using System;
    using Meta;
    using Resources;

    public partial class PurchaseInvoice
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.PurchaseInvoice, M.PurchaseInvoice.PurchaseInvoiceState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public InvoiceItem[] InvoiceItems => this.PurchaseInvoiceItems;

        public bool NeedsApproval
        {
            get
            {
                if (this.PurchaseOrders.Any())
                {
                    var orderTotal = this.PurchaseInvoiceItems.SelectMany(v => v.OrderItemBillingsWhereInvoiceItem).Select(o => o.OrderItem).Sum(i => i.TotalExVat);
                    if (this.TotalExVat != orderTotal)
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

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistPurchaseInvoiceState)
            {
                this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Created;
            }

            if (!this.ExistInvoiceDate)
            {
                this.InvoiceDate = this.strategy.Session.Now();
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = this.strategy.Session.Now();
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            var internalOrganisation = this.Strategy.Session.GetSingleton();

            // TODO:
            if (derivation.HasChangedRoles(this))
            {
                derivation.AddDependency(this, internalOrganisation);
            }

            derivation.AddDependency(this, this.BilledFrom);
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.Strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistBilledTo && internalOrganisations.Count() == 1)
            {
                this.BilledTo = internalOrganisations.First();
            }

            if (!this.ExistInvoiceNumber)
            {
                this.InvoiceNumber = this.BilledTo.NextPurchaseInvoiceNumber(this.InvoiceDate.Year);
            }

            Organisation supplier = this.BilledFrom as Organisation;
            if (supplier != null)
            {
                if (!this.BilledTo.ActiveSuppliers.Contains(supplier))
                {
                    derivation.Validation.AddError(this, this.Meta.BilledFrom, ErrorMessages.PartyIsNotASupplier);
                }
            }

            var validInvoiceItems = this.PurchaseInvoiceItems.Where(v => v.IsValid).ToArray();
            this.ValidInvoiceItems = validInvoiceItems;

            #region States
            var purchaseInvoiceStates = new PurchaseInvoiceStates(this.Strategy.Session);
            var purchaseInvoiceItemStates = new PurchaseInvoiceItemStates(this.Strategy.Session);

            foreach (PurchaseInvoiceItem purchaseInvoiceItem in ValidInvoiceItems)
            {
                if (this.PurchaseInvoiceState.IsCreated)
                {
                    if (purchaseInvoiceItem.PurchaseInvoiceItemState.IsCancelledByInvoice)
                    {
                        purchaseInvoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.Created;
                    }
                }

                if (this.PurchaseInvoiceState.IsInProcess && purchaseInvoiceItem.PurchaseInvoiceItemState.IsCreated)
                {
                    purchaseInvoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.InProcess;
                }

                if (this.PurchaseInvoiceState.IsPaid && purchaseInvoiceItem.PurchaseInvoiceItemState.IsInProcess)
                {
                    purchaseInvoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.Paid;
                }

                if (this.PurchaseInvoiceState.IsCancelled)
                {
                    purchaseInvoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.Cancelled;
                }

                if (this.PurchaseInvoiceState.IsRejected)
                {
                    purchaseInvoiceItem.PurchaseInvoiceItemState = purchaseInvoiceItemStates.Rejected;
                }
            }
            #endregion

            this.AppsOnDeriveInvoiceItems(derivation);
            this.AppsOnDeriveInvoiceTotals();


            this.DeriveWorkflow();
            this.ResetPrintDocument();
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            if (this.ExistSalesInvoiceWherePurchaseInvoice)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.CreateSalesInvoice, Operations.Execute));
            }
        }

        private void DeriveWorkflow()
        {
            this.WorkItemDescription = $"PurchaseInvoice: {this.InvoiceNumber} [{this.BilledFrom?.PartyName}]";

            var openTasks = this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();

            if (this.PurchaseInvoiceState.IsAwaitingApproval)
            {
                if (!openTasks.OfType<PurchaseInvoiceApproval>().Any())
                {
                    new PurchaseInvoiceApprovalBuilder(this.strategy.Session).WithPurchaseInvoice(this).Build();
                }
            }
        }

        public void AppsPrint(PrintablePrint method)
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
                    var barcode = barcodeService.Generate(this.InvoiceNumber, BarcodeType.CODE_128, 320, 80);
                    images.Add("Barcode", barcode);
                }

                var model = new Print.PurchaseInvoiceModel.Model(this);
                this.RenderPrintDocument(this.BilledTo?.PurchaseInvoiceTemplate, model, images);

                this.PrintDocument.Media.FileName = $"{this.InvoiceNumber}.odt";
            }
        }

        public void AppsCreateSalesInvoice(PurchaseInvoiceCreateSalesInvoice method)
        {
            var derivation = new Derivation(this.Strategy.Session);

            var salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                .WithPurchaseInvoice(this)
                .WithBilledFrom(this.BilledTo)
                .WithBilledFromContactPerson(this.BilledToContactPerson)
                .WithBillToCustomer(this.BillToEndCustomer)
                .WithBillToContactMechanism(this.BillToEndCustomerContactMechanism)
                .WithBillToContactPerson(this.BillToEndCustomerContactPerson)
                .WithShipToCustomer(this.ShipToEndCustomer)
                .WithShipToAddress(this.ShipToEndCustomerAddress)
                .WithShipToContactPerson(this.ShipToEndCustomerContactPerson)
                .WithDescription(this.Description)
                .WithInvoiceDate(this.strategy.Session.Now())
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                .WithVatRegime(this.VatRegime)
                .WithDiscountAdjustment(this.DiscountAdjustment)
                .WithSurchargeAdjustment(this.SurchargeAdjustment)
                .WithShippingAndHandlingCharge(this.ShippingAndHandlingCharge)
                .WithFee(this.Fee)
                .WithCustomerReference(this.CustomerReference)
                .WithPaymentMethod(this.BillToCustomerPaymentMethod)
                .WithComment(this.Comment)
                .WithInternalComment(this.InternalComment)
                .Build();

            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.PurchaseInvoiceItems)
            {
                var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                    .WithInvoiceItemType(purchaseInvoiceItem.InvoiceItemType)
                    .WithAssignedUnitPrice(purchaseInvoiceItem.AssignedUnitPrice)
                    .WithProduct(purchaseInvoiceItem.Product)
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
        }

        public void AppsOnDeriveInvoiceTotals()
        {
            if (this.ExistPurchaseInvoiceItems)
            {
                this.TotalBasePrice = 0;
                this.TotalDiscount = 0;
                this.TotalSurcharge = 0;
                this.TotalVat = 0;
                this.TotalExVat = 0;
                this.TotalIncVat = 0;

                foreach (PurchaseInvoiceItem item in this.PurchaseInvoiceItems)
                {
                    this.TotalBasePrice += item.TotalBasePrice;
                    this.TotalSurcharge += item.TotalSurcharge;
                    this.TotalSurcharge += item.TotalSurcharge;
                    this.TotalVat += item.TotalVat;
                    this.TotalExVat += item.TotalExVat;
                    this.TotalIncVat += item.TotalIncVat;
                }
            }
        }

        public void AppsConfirm(PurchaseInvoiceConfirm method)
        {
            this.PurchaseInvoiceState = this.NeedsApproval ? new PurchaseInvoiceStates(this.Strategy.Session).AwaitingApproval : new PurchaseInvoiceStates(this.Strategy.Session).InProcess;
        }

        public void AppsCancel(PurchaseInvoiceCancel method)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Cancelled;
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                purchaseInvoiceItem.CancelFromInvoice();
            }
        }

        public void AppsReject(PurchaseInvoiceReject method)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).Rejected;
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                purchaseInvoiceItem.Reject();
            }
        }

        public void AppsApprove(PurchaseInvoiceApprove method)
        {
            this.PurchaseInvoiceState = new PurchaseInvoiceStates(this.Strategy.Session).InProcess;

            var openTasks = this.TasksWhereWorkItem.Where(v => !v.ExistDateClosed).ToArray();

            if (openTasks.OfType<PurchaseInvoiceApproval>().Any())
            {
                openTasks.First().DateClosed = this.strategy.Session.Now();
            }

            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                if (purchaseInvoiceItem.ExistPart)
                {
                    var previousOffering = purchaseInvoiceItem.Part.SupplierOfferingsWherePart.Single(v =>
                        v.Supplier.Equals(this.BilledFrom) && v.FromDate <= this.strategy.Session.Now() &&
                        (!v.ExistThroughDate || v.ThroughDate >= this.strategy.Session.Now()));

                    if (previousOffering != null)
                    {
                        if (purchaseInvoiceItem.UnitBasePrice != previousOffering.Price)
                        {
                            previousOffering.ThroughDate = this.strategy.Session.Now();

                            var newOffering = new SupplierOfferingBuilder(this.strategy.Session)
                                .WithSupplier(this.BilledFrom)
                                .WithPart(purchaseInvoiceItem.Part)
                                .WithPrice(purchaseInvoiceItem.UnitBasePrice)
                                .WithFromDate(this.strategy.Session.Now())
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
                        new SupplierOfferingBuilder(this.strategy.Session)
                            .WithSupplier(this.BilledFrom)
                            .WithPart(purchaseInvoiceItem.Part)
                            .WithPrice(purchaseInvoiceItem.UnitBasePrice)
                            .WithFromDate(this.strategy.Session.Now())
                            .Build();
                    }
                }
            }
        }

        public void AppsOnDeriveInvoiceItems(IDerivation derivation)
        {
            foreach (PurchaseInvoiceItem purchaseInvoiceItem in this.ValidInvoiceItems)
            {
                purchaseInvoiceItem.AppsOnDerivePrices();
            }
        }
    }
}
