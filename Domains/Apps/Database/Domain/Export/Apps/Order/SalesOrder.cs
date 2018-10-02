// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesOrder.cs" company="Allors bvba">
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
namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Domain.NonLogging;
    using Allors.Services;

    using Meta;

    using Microsoft.Extensions.DependencyInjection;

    using Resources;

    public partial class SalesOrder
    {
        public static readonly TransitionalConfiguration[] StaticTransitionalConfigurations =
            {
                new TransitionalConfiguration(M.SalesOrder, M.SalesOrder.SalesOrderState),
                new TransitionalConfiguration(M.SalesOrder, M.SalesOrder.SalesOrderShipmentState),
                new TransitionalConfiguration(M.SalesOrder, M.SalesOrder.SalesOrderInvoiceState),
                new TransitionalConfiguration(M.SalesOrder, M.SalesOrder.SalesOrderPaymentState),
            };

        public TransitionalConfiguration[] TransitionalConfigurations => StaticTransitionalConfigurations;

        public int PaymentNetDays
        {
            get
            {
                if (this.ExistSalesTerms)
                {
                    foreach (AgreementTerm term in this.SalesTerms)
                    {
                        if (term.TermType.Equals(new InvoiceTermTypes(this.Strategy.Session).PaymentNetDays))
                        {
                            if (int.TryParse(term.TermValue, out var netDays))
                            {
                                return netDays;
                            }
                        }
                    }
                }

                if (this.ExistBillToCustomer && this.BillToCustomer.PaymentNetDays() != null)
                {
                    return this.BillToCustomer.PaymentNetDays().Value;
                }

                if (this.ExistStore && this.Store.ExistPaymentNetDays)
                {
                    return this.Store.PaymentNetDays;
                }

                return 0;
            }
        }

        public DateTime? DueDate
        {
            get
            {
                if (this.ExistOrderDate)
                {
                    return this.OrderDate.AddDays(this.PaymentNetDays);
                }

                return null;
            }
        }

        public OrderItem[] OrderItems => this.SalesOrderItems;

        public bool ScheduledManually
        {
            get
            {
                if (this.ExistOrderKind && this.OrderKind.ScheduleManually)
                {
                    return true;
                }

                return false;
            }
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSalesOrderState)
            {
                this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Provisional;
            }

            if (!this.ExistOrderDate)
            {
                this.OrderDate = DateTime.UtcNow;
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = DateTime.UtcNow;
            }

            if (!this.ExistPartiallyShip)
            {
                this.PartiallyShip = true;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            derivation.AddDependency(this.BillToCustomer, this);
            derivation.AddDependency(this.ShipToCustomer, this);

            foreach (var orderItem in this.OrderItems)
            {
                derivation.AddDependency(this, orderItem);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var internalOrganisations = new Organisations(this.strategy.Session).Extent().Where(v => Equals(v.IsInternalOrganisation, true)).ToArray();

            if (!this.ExistTakenBy && internalOrganisations.Count() == 1)
            {
                this.TakenBy = internalOrganisations.First();
            }

            if (!this.ExistStore && this.ExistTakenBy)
            {
                var store = new Stores(this.strategy.Session).Extent().FirstOrDefault(v => Equals(v.InternalOrganisation, this.TakenBy));
                if (store != null)
                {
                    this.Store = store;
                }
            }

            if (!this.ExistOrderNumber && this.ExistStore)
            {
                this.OrderNumber = this.Store.DeriveNextSalesOrderNumber();
            }

            if (!this.ExistBillToCustomer && this.ExistShipToCustomer)
            {
                this.BillToCustomer = this.ShipToCustomer;
            }

            if (!this.ExistShipToCustomer && this.ExistBillToCustomer)
            {
                this.ShipToCustomer = this.BillToCustomer;
            }

            if (!this.ExistShipToAddress && this.ExistShipToCustomer)
            {
                this.ShipToAddress = this.ShipToCustomer.ShippingAddress;
            }

            if (!this.ExistVatRegime && this.ExistBillToCustomer)
            {
                this.VatRegime = this.BillToCustomer.VatRegime;
            }

            if (!this.ExistShipmentMethod && this.ExistShipToCustomer)
            {
                this.ShipmentMethod = this.ShipToCustomer.DefaultShipmentMethod ?? this.Store.DefaultShipmentMethod;
            }

            if (!this.ExistTakenByContactMechanism)
            {
                this.TakenByContactMechanism = this.TakenBy.ExistOrderAddress ? this.TakenBy.OrderAddress : this.TakenBy.ExistBillingAddress ? this.TakenBy.BillingAddress : this.TakenBy.GeneralCorrespondence;
            }

            if (!this.ExistBillToContactMechanism && this.ExistBillToCustomer)
            {
                this.BillToContactMechanism = this.BillToCustomer.ExistBillingAddress ?
                    this.BillToCustomer.BillingAddress : this.BillToCustomer.ExistShippingAddress ? this.BillToCustomer.ShippingAddress : this.BillToCustomer.GeneralCorrespondence;
            }

            if (!this.ExistBillToEndCustomerContactMechanism && this.ExistBillToEndCustomer)
            {
                this.BillToEndCustomerContactMechanism = this.BillToEndCustomer.ExistBillingAddress ?
                    this.BillToEndCustomer.BillingAddress : this.BillToEndCustomer.ExistShippingAddress ? this.BillToEndCustomer.ShippingAddress : this.BillToCustomer.GeneralCorrespondence;
            }

            if (!this.ExistShipToEndCustomerAddress && this.ExistShipToEndCustomer)
            {
                this.ShipToEndCustomerAddress = this.ShipToEndCustomer.ExistShippingAddress ? this.ShipToEndCustomer.ShippingAddress : this.ShipToCustomer.GeneralCorrespondence;
            }

            if (!this.ExistCurrency)
            {
                if (this.ExistBillToCustomer &&
                    (this.BillToCustomer.ExistPreferredCurrency || this.BillToCustomer.ExistLocale))
                {
                    this.Currency = this.BillToCustomer.ExistPreferredCurrency ? this.BillToCustomer.PreferredCurrency : this.BillToCustomer.Locale.Country.Currency;
                }
                else
                {
                    this.Currency = this.TakenBy.PreferredCurrency;
                }
            }

            if (this.ExistBillToCustomer)
            {
                if (!this.BillToCustomer.AppsIsActiveCustomer(this.TakenBy, this.OrderDate))
                {
                    derivation.Validation.AddError(this, M.SalesOrder.BillToCustomer, ErrorMessages.PartyIsNotACustomer);
                }
            }

            if (this.ExistShipToCustomer)
            {
                if (!this.ShipToCustomer.AppsIsActiveCustomer(this.TakenBy, this.OrderDate))
                {
                    derivation.Validation.AddError(this, M.SalesOrder.ShipToCustomer, ErrorMessages.PartyIsNotACustomer);
                }
            }

            if (!this.ExistVatRegime && this.ExistBillToCustomer)
            {
                this.VatRegime = this.BillToCustomer.VatRegime;
            }

            if (!this.ExistPaymentMethod)
            {
                var partyFinancial = this.ShipToCustomer.PartyFinancialRelationshipsWhereParty.FirstOrDefault(v => Equals(v.InternalOrganisation, this.TakenBy));

                if (!this.ExistShipToCustomer && partyFinancial != null)
                {
                    this.PaymentMethod= partyFinancial.DefaultPaymentMethod;
                }

                if (!this.ExistPaymentMethod && this.ExistStore)
                {
                    this.PaymentMethod= this.Store.DefaultCollectionMethod;
                }
            }

            if (!this.ExistPaymentMethod && this.ExistBillToCustomer)
            {
                var partyFinancial = this.BillToCustomer.PartyFinancialRelationshipsWhereParty.FirstOrDefault(v => Equals(v.InternalOrganisation, this.TakenBy));

                if (partyFinancial != null)
                {
                    this.PaymentMethod = partyFinancial.DefaultPaymentMethod ?? this.Store.DefaultCollectionMethod;
                }
            }

            if (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).InProcess))
            {
                derivation.Validation.AssertExists(this, this.Meta.ShipToAddress);
                derivation.Validation.AssertExists(this, this.Meta.BillToContactMechanism);
            }

            this.AppsOnDeriveOrderItems(derivation);

            this.AppsOnDeriveOrderShipmentState(derivation);
            this.AppsOnDeriveOrderInvoiceState(derivation);
            this.AppsOnDeriveOrderState(derivation);
            this.AppsOnDeriveLocale(derivation);
            this.AppsOnDeriveOrderTotals(derivation);
            this.AppsOnDeriveCustomers(derivation);
            this.AppsOnDeriveSalesReps(derivation);
            this.AppsOnDeriveOrderPaymentState(derivation);
            this.AppsDeriveCanShip(derivation);
            this.AppsDeriveCanInvoice(derivation);

            if (this.CanShip)
            {
                this.AppsShipThis(derivation);
            }

            this.PreviousBillToCustomer = this.BillToCustomer;
            this.PreviousShipToCustomer = this.ShipToCustomer;

            // TODO: DocumentService
            //var templateService = this.strategy.Session.ServiceProvider.GetRequiredService<ITemplateService>();

            //var model = new PrintSalesOrder
            //{
            //    SalesOrder = this
            //};

            //this.HtmlContent = templateService.Render("Templates/SalesOrder.cshtml", model).Result;
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            if (!CanShip)
            {
                this.AddDeniedPermission(new Permissions(this.strategy.Session).Get(this.Meta.Class, this.Meta.Ship, Operations.Execute));
            }

            if (!CanInvoice)
            {
                this.AddDeniedPermission(new Permissions(this.strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }

            if (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).InProcess) &&
                Equals(this.Store.BillingProcess, new BillingProcesses(this.strategy.Session).BillingForShipmentItems))
            {
                this.RemoveDeniedPermission(new Permissions(this.strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }
        }

        public void AppsCancel(OrderCancel method)
        {
            this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Cancelled;
        }

        public void AppsConfirm(OrderConfirm method)
        {
            var orderThreshold = this.Store.OrderThreshold;
            var partyFinancial = this.BillToCustomer.PartyFinancialRelationshipsWhereParty.FirstOrDefault(v => Equals(v.InternalOrganisation, this.TakenBy));

            decimal amountOverDue = partyFinancial.AmountOverDue;
            decimal creditLimit = partyFinancial.CreditLimit ?? (this.Store.ExistCreditLimit ? this.Store.CreditLimit : 0);

            if (amountOverDue > creditLimit || this.TotalExVat < orderThreshold)
            {
                this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).RequestsApproval;
            }
            else
            {
                this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).InProcess;
            }
        }

        public void AppsReject(OrderReject method)
        {
            this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Rejected;
        }

        public void AppsHold(OrderHold method)
        {
            this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).OnHold;
        }

        public void AppsApprove(OrderApprove method)
        {
            this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).InProcess;
        }

        public void AppsContinue(OrderContinue method)
        {
            this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).InProcess;
        }

        public void AppsComplete(OrderComplete method)
        {
            this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Completed;
        }

        public void AppsOnDeriveOrderPaymentState(IDerivation derivation)
        {
            var itemsPaid = false;
            var itemsPartiallyPaid = false;
            var itemsUnpaid = false;

            foreach (SalesOrderItem orderItem in this.ValidOrderItems)
            {
                if (orderItem.ExistSalesOrderItemPaymentState)
                {
                    if (orderItem.SalesOrderItemPaymentState.Equals(new SalesOrderItemPaymentStates(this.Strategy.Session).PartiallyPaid))
                    {
                        itemsPartiallyPaid = true;
                    }

                    if (orderItem.SalesOrderItemPaymentState.Equals(new SalesOrderItemPaymentStates(this.Strategy.Session).Paid))
                    {
                        itemsPaid = true;
                    }
                }
                else
                {
                    itemsUnpaid = true;
                }
            }

            if (itemsPaid && !itemsUnpaid && !itemsPartiallyPaid &&
                (!this.ExistSalesOrderPaymentState || !this.SalesOrderPaymentState.Equals(new SalesOrderPaymentStates(this.Strategy.Session).Paid)))
            {
                this.SalesOrderPaymentState = new SalesOrderPaymentStates(this.Strategy.Session).Paid;
            }

            if ((itemsPartiallyPaid || (itemsPaid && itemsUnpaid)) &&
                (!this.ExistSalesOrderPaymentState || !this.SalesOrderPaymentState.Equals(new SalesOrderPaymentStates(this.Strategy.Session).PartiallyPaid)))
            {
                this.SalesOrderPaymentState = new SalesOrderPaymentStates(this.Strategy.Session).PartiallyPaid;
            }

            this.AppsOnDeriveOrderState(derivation);
        }

        public void AppsOnDeriveOrderShipmentState(IDerivation derivation)
        {
            var itemsShipped = false;
            var itemsPartiallyShipped = false;
            var itemsNotShipped = false;

            foreach (SalesOrderItem orderItem in this.ValidOrderItems)
            {
                if (orderItem.ExistSalesOrderItemShipmentState)
                {
                    if (orderItem.SalesOrderItemShipmentState.Equals(new SalesOrderItemShipmentStates(this.Strategy.Session).PartiallyShipped))
                    {
                        itemsPartiallyShipped = true;
                    }

                    if (orderItem.SalesOrderItemShipmentState.Equals(new SalesOrderItemShipmentStates(this.Strategy.Session).Shipped))
                    {
                        itemsShipped = true;
                    }
                }
                else
                {
                    itemsNotShipped = true;
                }
            }

            if (itemsShipped && !itemsNotShipped && !itemsPartiallyShipped &&
                (!this.ExistSalesOrderShipmentState || !this.SalesOrderShipmentState.Equals(new SalesOrderShipmentStates(this.Strategy.Session).Shipped)))
            {
                this.SalesOrderShipmentState = new SalesOrderShipmentStates(this.Strategy.Session).Shipped;
            }

            if ((itemsPartiallyShipped || (itemsShipped && itemsNotShipped)) &&
                (!this.ExistSalesOrderShipmentState || !this.SalesOrderShipmentState.Equals(new SalesOrderShipmentStates(this.Strategy.Session).PartiallyShipped)))
            {
                this.SalesOrderShipmentState = new SalesOrderShipmentStates(this.Strategy.Session).PartiallyShipped;
            }

            this.AppsOnDeriveOrderState(derivation);
        }

        public void AppsOnDeriveOrderInvoiceState(IDerivation derivation)
        {
            var itemsInvoiced = false;
            var itemsPartiallyInvoiced = false;
            var itemsNotInvoiced = false;

            foreach (SalesOrderItem orderItem in this.ValidOrderItems)
            {
                if (orderItem.ExistSalesOrderItemInvoiceState)
                {
                    if (orderItem.SalesOrderItemInvoiceState.Equals(new SalesOrderItemInvoiceStates(this.Strategy.Session).PartiallyInvoiced))
                    {
                        itemsPartiallyInvoiced = true;
                    }

                    if (orderItem.SalesOrderItemInvoiceState.Equals(new SalesOrderItemInvoiceStates(this.Strategy.Session).Invoiced))
                    {
                        itemsInvoiced = true;
                    }
                }
                else
                {
                    itemsNotInvoiced = true;
                }
            }

            if (itemsInvoiced && !itemsNotInvoiced && !itemsPartiallyInvoiced &&
                (!this.ExistSalesOrderInvoiceState || !this.SalesOrderInvoiceState.Equals(new SalesOrderInvoiceStates(this.Strategy.Session).Invoiced)))
            {
                this.SalesOrderInvoiceState = new SalesOrderInvoiceStates(this.Strategy.Session).Invoiced;
            }

            if ((itemsPartiallyInvoiced || (itemsInvoiced && itemsNotInvoiced)) &&
                (!this.ExistSalesOrderInvoiceState || !this.SalesOrderInvoiceState.Equals(new SalesOrderInvoiceStates(this.Strategy.Session).PartiallyInvoiced)))
            {
                this.SalesOrderInvoiceState = new SalesOrderInvoiceStates(this.Strategy.Session).PartiallyInvoiced;
            }

            this.AppsOnDeriveOrderState(derivation);
        }

        public void AppsOnDeriveOrderState(IDerivation derivation)
        {
            if (this.ExistSalesOrderShipmentState && this.SalesOrderShipmentState.Equals(new SalesOrderShipmentStates(this.Strategy.Session).Shipped) &&
                this.ExistSalesOrderInvoiceState && this.SalesOrderInvoiceState.Equals(new SalesOrderInvoiceStates(this.Strategy.Session).Invoiced))
            {
                this.Complete();
            }

            if (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).Completed) &&
                this.ExistSalesOrderPaymentState && this.SalesOrderPaymentState.Equals(new SalesOrderPaymentStates(this.Strategy.Session).Paid))
            {
                this.SalesOrderState = new SalesOrderStates(this.strategy.Session).Finished;
            }
        }

        public void AppsOnDeriveSalesReps(IDerivation derivation)
        {
            this.RemoveSalesReps();
            foreach (SalesOrderItem item in this.ValidOrderItems)
            {
                this.AddSalesRep(item.SalesRep);
            }
        }

        public void AppsOnDeriveOrderTotals(IDerivation derivation)
        {
            if (this.ExistValidOrderItems)
            {
                this.TotalBasePrice = 0;
                this.TotalDiscount = 0;
                this.TotalSurcharge = 0;
                this.TotalExVat = 0;
                this.TotalFee = 0;
                this.TotalShippingAndHandling = 0;
                this.TotalVat = 0;
                this.TotalIncVat = 0;
                this.TotalPurchasePrice = 0;
                this.TotalListPrice = 0;
                this.MaintainedMarkupPercentage = 0;
                this.InitialMarkupPercentage = 0;
                this.MaintainedProfitMargin = 0;
                this.InitialProfitMargin = 0;

                foreach (SalesOrderItem item in this.ValidOrderItems)
                {
                    if (!item.ExistSalesOrderItemWhereOrderedWithFeature)
                    {
                        this.TotalBasePrice += item.TotalBasePrice;
                        this.TotalDiscount += item.TotalDiscount;
                        this.TotalSurcharge += item.TotalSurcharge;
                        this.TotalExVat += item.TotalExVat;
                        this.TotalVat += item.TotalVat;
                        this.TotalIncVat += item.TotalIncVat;
                        this.TotalPurchasePrice += item.UnitPurchasePrice;
                        this.TotalListPrice += item.CalculatedUnitPrice;
                    }
                }

                this.AppsOnDeriveOrderAdjustments();
                this.AppsOnDeriveTotalFee();
                this.AppsOnDeriveTotalShippingAndHandling();
                this.AppsOnDeriveMarkupAndProfitMargin(derivation);
            }
        }

        public void AppsOnDeriveOrderAdjustments()
        {
            if (this.ExistDiscountAdjustment)
            {
                decimal discount = this.DiscountAdjustment.Percentage.HasValue ?
                    Math.Round((this.TotalExVat * this.DiscountAdjustment.Percentage.Value) / 100, 2) :
                    this.DiscountAdjustment.Amount ?? 0;

                this.TotalDiscount += discount;
                this.TotalExVat -= discount;

                if (this.ExistVatRegime)
                {
                    decimal vat = Math.Round((discount * this.VatRegime.VatRate.Rate) / 100, 2);

                    this.TotalVat -= vat;
                    this.TotalIncVat -= discount + vat;
                }
            }

            if (this.ExistSurchargeAdjustment)
            {
                decimal surcharge = this.SurchargeAdjustment.Percentage.HasValue ?
                    Math.Round((this.TotalExVat * this.SurchargeAdjustment.Percentage.Value) / 100, 2) :
                    this.SurchargeAdjustment.Amount ?? 0;

                this.TotalSurcharge += surcharge;
                this.TotalExVat += surcharge;

                if (this.ExistVatRegime)
                {
                    decimal vat = Math.Round((surcharge * this.VatRegime.VatRate.Rate) / 100, 2);
                    this.TotalVat += vat;
                    this.TotalIncVat += surcharge + vat;
                }
            }
        }

        public void AppsOnDeriveTotalFee()
        {
            if (this.ExistFee)
            {
                decimal fee = this.Fee.Percentage.HasValue ?
                    Math.Round((this.TotalExVat * this.Fee.Percentage.Value) / 100, 2) :
                    this.Fee.Amount ?? 0;

                this.TotalFee += fee;
                this.TotalExVat += fee;

                if (this.Fee.ExistVatRate)
                {
                    decimal vat = Math.Round((fee * this.Fee.VatRate.Rate) / 100, 2);
                    this.TotalVat += vat;
                    this.TotalIncVat += fee + vat;
                }
            }
        }

        public void AppsOnDeriveTotalShippingAndHandling()
        {
            if (this.ExistShippingAndHandlingCharge)
            {
                decimal shipping = this.ShippingAndHandlingCharge.Percentage.HasValue ?
                    Math.Round((this.TotalExVat * this.ShippingAndHandlingCharge.Percentage.Value) / 100, 2) :
                    this.ShippingAndHandlingCharge.Amount ?? 0;

                this.TotalShippingAndHandling += shipping;
                this.TotalExVat += shipping;

                if (this.ShippingAndHandlingCharge.ExistVatRate)
                {
                    decimal vat = Math.Round((shipping * this.ShippingAndHandlingCharge.VatRate.Rate) / 100, 2);
                    this.TotalVat += vat;
                    this.TotalIncVat += shipping + vat;
                }
            }
        }

        public void AppsOnDeriveMarkupAndProfitMargin(IDerivation derivation)
        {
            //// Only take into account items for which there is data at the item level.
            //// Skip negative sales.
            decimal totalPurchasePrice = 0;
            decimal totalUnitBasePrice = 0;
            decimal totalListPrice = 0;

            foreach (SalesOrderItem item in this.ValidOrderItems)
            {
                if (item.TotalExVat > 0)
                {
                    totalPurchasePrice += item.UnitPurchasePrice;
                    totalUnitBasePrice += item.UnitBasePrice;
                    totalListPrice += item.CalculatedUnitPrice;
                }
            }

            if (totalPurchasePrice != 0 && totalListPrice != 0 && totalUnitBasePrice != 0)
            {
                this.InitialMarkupPercentage = Math.Round(((totalUnitBasePrice / totalPurchasePrice) - 1) * 100, 2);
                this.MaintainedMarkupPercentage = Math.Round(((totalListPrice / totalPurchasePrice) - 1) * 100, 2);

                this.InitialProfitMargin = Math.Round(((totalUnitBasePrice - totalPurchasePrice) / totalUnitBasePrice) * 100, 2);
                this.MaintainedProfitMargin = Math.Round(((totalListPrice - totalPurchasePrice) / totalListPrice) * 100, 2);
            }
        }

        public void AppsOnDeriveLocale(IDerivation derivation)
        {
            if (this.ExistBillToCustomer && this.BillToCustomer.ExistLocale)
            {
                this.Locale = this.BillToCustomer.Locale;
            }
            else
            {
                this.Locale = this.strategy.Session.GetSingleton().DefaultLocale;
            }
        }

        public void AppsOnDeriveCustomers(IDerivation derivation)
        {
            this.RemoveCustomers();

            this.AddCustomer(this.BillToCustomer);
            this.AddCustomer(this.ShipToCustomer);
            this.AddCustomer(this.PlacingCustomer);
        }

        public void AppsDeriveCanShip(IDerivation derivation)
        {
            if (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).InProcess))
            {
                var somethingToShip = false;
                var allItemsAvailable = true;

                foreach (SalesOrderItem salesOrderItem in this.ValidOrderItems)
                {
                    if (!this.PartiallyShip && salesOrderItem.QuantityRequestsShipping != salesOrderItem.QuantityOrdered)
                    {
                        allItemsAvailable = false;
                        break;
                    }

                    if (this.PartiallyShip && salesOrderItem.QuantityRequestsShipping > 0)
                    {
                        somethingToShip = true;
                    }
                }

                if ((!this.PartiallyShip && allItemsAvailable) || somethingToShip)
                {
                    this.CanShip = true;
                    return;
                }
            }

            this.CanShip = false;
        }

        public void AppsShip(SalesOrderShip method)
        {
            var derivation = new Derivation(this.Strategy.Session);

            if (this.CanShip)
            {
                this.AppsShipThis(derivation);
            }
        }

        private List<Shipment> AppsShipThis(IDerivation derivation)
        {
            var addresses = this.ShipToAddresses();
            var shipments = new List<Shipment>();
            if (addresses.Count > 0)
            {
                foreach (var address in addresses)
                {
                    shipments.Add(this.AppsShipToAddress(derivation, address));
                }
            }

            return shipments;
        }

        private CustomerShipment AppsShipToAddress(IDerivation derivation, KeyValuePair<PostalAddress, Party> address)
        {
            var pendingShipment = address.Value.AppsGetPendingCustomerShipmentForStore(address.Key, this.Store, this.ShipmentMethod);

            if (pendingShipment == null)
            {
                pendingShipment = new CustomerShipmentBuilder(this.Strategy.Session)
                    .WithShipFromAddress(this.TakenBy.ShippingAddress)
                    .WithBillToParty(this.BillToCustomer)
                    .WithBillToContactMechanism(this.BillToEndCustomerContactMechanism)
                    .WithShipToAddress(address.Key)
                    .WithShipToParty(address.Value)
                    .WithShipmentPackage(new ShipmentPackageBuilder(this.Strategy.Session).Build())
                    .WithStore(this.Store)
                    .WithShipmentMethod(this.ShipmentMethod)
                    .WithPaymentMethod(this.PaymentMethod)
                    .Build();
            }

            foreach (SalesOrderItem orderItem in this.ValidOrderItems)
            {
                if (orderItem.ExistProduct && orderItem.ShipToAddress.Equals(address.Key) && orderItem.QuantityRequestsShipping > 0)
                {
                    var good = orderItem.Product as Good;

                    ShipmentItem shipmentItem = null;
                    foreach (ShipmentItem item in pendingShipment.ShipmentItems)
                    {
                        if (item.Good.Equals(good))
                        {
                            shipmentItem = item;
                            break;
                        }
                    }

                    if (shipmentItem != null)
                    {
                        shipmentItem.Quantity += orderItem.QuantityRequestsShipping;
                        shipmentItem.ContentsDescription = $"{shipmentItem.Quantity} * {good}";
                    }
                    else
                    {
                        shipmentItem = new ShipmentItemBuilder(this.Strategy.Session)
                            .WithGood(good)
                            .WithQuantity(orderItem.QuantityRequestsShipping)
                            .WithContentsDescription($"{orderItem.QuantityRequestsShipping} * {good}")
                            .Build();

                        pendingShipment.AddShipmentItem(shipmentItem);
                    }

                    foreach (SalesOrderItem featureItem in orderItem.OrderedWithFeatures)
                    {
                        shipmentItem.AddProductFeature(featureItem.ProductFeature);
                    }

                    var orderShipmentsWhereShipmentItem = shipmentItem.OrderShipmentsWhereShipmentItem;
                    orderShipmentsWhereShipmentItem.Filter.AddEquals(M.OrderShipment.OrderItem, orderItem);

                    if (orderShipmentsWhereShipmentItem.First == null)
                    {
                        new OrderShipmentBuilder(this.Strategy.Session)
                            .WithOrderItem(orderItem)
                            .WithShipmentItem(shipmentItem)
                            .WithQuantity(orderItem.QuantityRequestsShipping)
                            .Build();
                    }
                    else
                    {
                        orderShipmentsWhereShipmentItem.First.Quantity += orderItem.QuantityRequestsShipping;
                    }

                    orderItem.AppsOnDeriveOnShip(derivation);
                }
            }

            // TODO: Check

            if (this.Store.IsAutomaticallyShipped)
            {
                pendingShipment.Ship();
            }

            pendingShipment.OnDerive(x => x.WithDerivation(derivation));
            return pendingShipment;
        }

        private Dictionary<PostalAddress, Party> ShipToAddresses()
        {
            var addresses = new Dictionary<PostalAddress, Party>();
            foreach (SalesOrderItem item in this.ValidOrderItems)
            {
                if (item.QuantityRequestsShipping > 0)
                {
                    if (!addresses.ContainsKey(item.ShipToAddress))
                    {
                        addresses.Add(item.ShipToAddress, item.ShipToParty);
                    }
                }
            }

            return addresses;
        }

        private void AppsDeriveCanInvoice(IDerivation derivation)
        {
            if (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).InProcess) &&
                Equals(this.Store.BillingProcess, new BillingProcesses(this.strategy.Session).BillingForOrderItems))
            {
                this.CanInvoice = false;

                foreach (SalesOrderItem orderItem in this.ValidOrderItems)
                {
                    var amountAlreadyInvoiced = orderItem.OrderItemBillingsWhereOrderItem.Sum(v => v.Amount);

                    var leftToInvoice = (orderItem.QuantityOrdered * orderItem.ActualUnitPrice) - amountAlreadyInvoiced;

                    if (leftToInvoice > 0)
                    {
                        this.CanInvoice = true;
                    }
                }
            }
            else
            {
                this.CanInvoice = false;
            }
        }

        public void AppsInvoice(SalesOrderInvoice method)
        {
            var derivation = new Derivation(this.Strategy.Session);

            this.AppsDeriveCanInvoice(derivation);

            if (this.CanInvoice)
            {
                this.AppsInvoiceThis(derivation);
            }
        }

        private void AppsInvoiceThis(IDerivation derivation)
        {
            var salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                .WithSalesOrder(this)
                .WithBilledFrom(this.TakenBy)
                .WithBilledFromContactMechanism(this.TakenByContactMechanism)
                .WithBilledFromContactPerson(this.TakenByContactPerson)
                .WithBillToCustomer(this.BillToCustomer)
                .WithBillToContactMechanism(this.BillToContactMechanism)
                .WithBillToContactPerson(this.BillToContactPerson)
                .WithBillToEndCustomer(this.BillToEndCustomer)
                .WithBillToEndCustomerContactMechanism(this.BillToEndCustomerContactMechanism)
                .WithBillToEndCustomerContactPerson(this.BillToEndCustomerContactPerson)
                .WithShipToCustomer(this.ShipToCustomer)
                .WithShipToAddress(this.ShipToAddress)
                .WithShipToContactPerson(this.ShipToContactPerson)
                .WithShipToEndCustomer(this.ShipToEndCustomer)
                .WithShipToEndCustomerAddress(this.ShipToEndCustomerAddress)
                .WithShipToEndCustomerContactPerson(this.ShipToEndCustomerContactPerson)
                .WithDescription(this.Description)
                .WithStore(this.Store)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithSalesChannel(this.SalesChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                .WithVatRegime(this.VatRegime)
                .WithDiscountAdjustment(this.DiscountAdjustment)
                .WithSurchargeAdjustment(this.SurchargeAdjustment)
                .WithShippingAndHandlingCharge(this.ShippingAndHandlingCharge)
                .WithFee(this.Fee)
                .WithCustomerReference(this.CustomerReference)
                .WithPaymentMethod(this.PaymentMethod)
                .WithComment(this.Comment)
                .WithInternalComment(this.InternalComment)
                .Build();

            foreach (SalesOrderItem orderItem in this.ValidOrderItems)
            {
                var amountAlreadyInvoiced = orderItem.OrderItemBillingsWhereOrderItem.Sum(v => v.Amount);

                var leftToInvoice = (orderItem.QuantityOrdered * orderItem.ActualUnitPrice) - amountAlreadyInvoiced;

                if (leftToInvoice > 0)
                { 
                    var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                        .WithInvoiceItemType(orderItem.InvoiceItemType)
                        .WithActualUnitPrice(orderItem.ActualUnitPrice)
                        .WithProduct(orderItem.Product)
                        .WithQuantity(orderItem.QuantityOrdered)
                        .WithComment(orderItem.Comment)
                        .WithDetails(orderItem.Details)
                        .WithDescription(orderItem.Description)
                        .WithInternalComment(orderItem.InternalComment)
                        .WithMessage(orderItem.Message)
                        .Build();

                    salesInvoice.AddSalesInvoiceItem(invoiceItem);

                    new OrderItemBillingBuilder(this.strategy.Session)
                        .WithQuantity(orderItem.QuantityOrdered)
                        .WithAmount(leftToInvoice)
                        .WithOrderItem(orderItem)
                        .WithInvoiceItem(invoiceItem)
                        .Build();
                }
            }

            foreach (SalesTerm salesTerm in this.SalesTerms)
            {
                if (salesTerm.GetType().Name == typeof(IncoTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new IncoTermBuilder(this.strategy.Session)
                                                .WithTermType(salesTerm.TermType)
                                                .WithTermValue(salesTerm.TermValue)
                                                .WithDescription(salesTerm.Description)
                                                .Build());
                }

                if (salesTerm.GetType().Name == typeof(InvoiceTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new InvoiceTermBuilder(this.strategy.Session)
                        .WithTermType(salesTerm.TermType)
                        .WithTermValue(salesTerm.TermValue)
                        .WithDescription(salesTerm.Description)
                        .Build());
                }

                if (salesTerm.GetType().Name == typeof(OrderTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new OrderTermBuilder(this.strategy.Session)
                        .WithTermType(salesTerm.TermType)
                        .WithTermValue(salesTerm.TermValue)
                        .WithDescription(salesTerm.Description)
                        .Build());
                }
            }
        }

        public void AppsOnDeriveOrderItems(IDerivation derivation)
        {
            var quantityOrderedByProduct = new Dictionary<Product, decimal>();
            var totalBasePriceByProduct = new Dictionary<Product, decimal>();

            foreach (SalesOrderItem salesOrderItem in this.OrderItems)
            {
                salesOrderItem.AppsOnDeriveShipTo(derivation);
                salesOrderItem.AppsOnDeriveIsValidOrderItem(derivation);
            }

            foreach (SalesOrderItem salesOrderItem in this.ValidOrderItems)
            {
                foreach (SalesOrderItem featureItem in salesOrderItem.OrderedWithFeatures)
                {
                    featureItem.AppsOnDerivePrices(derivation, 0, 0);
                }

                salesOrderItem.AppsOnDeriveDeliveryDate(derivation);
                salesOrderItem.AppsOnDeriveSalesRep(derivation);
                salesOrderItem.AppsOnDeriveVatRegime(derivation);
                salesOrderItem.AppsCalculatePurchasePrice(derivation);
                salesOrderItem.AppsCalculateUnitPrice(derivation);
                salesOrderItem.AppsOnDerivePrices(derivation, 0, 0);
                salesOrderItem.AppsOnDeriveInvoiceState(derivation);
                salesOrderItem.AppsOnDeriveShipmentState(derivation);
                salesOrderItem.AppsOnDerivePaymentState(derivation);

                // for the second time, because unitbaseprice might not be set
                salesOrderItem.AppsOnDeriveIsValidOrderItem(derivation);

                if (salesOrderItem.ExistProduct)
                {
                    if (!quantityOrderedByProduct.ContainsKey(salesOrderItem.Product))
                    {
                        quantityOrderedByProduct.Add(salesOrderItem.Product, salesOrderItem.QuantityOrdered);
                        totalBasePriceByProduct.Add(salesOrderItem.Product, salesOrderItem.TotalBasePrice);
                    }
                    else
                    {
                        quantityOrderedByProduct[salesOrderItem.Product] += salesOrderItem.QuantityOrdered;
                        totalBasePriceByProduct[salesOrderItem.Product] += salesOrderItem.TotalBasePrice;
                    }
                }
            }

            foreach (SalesOrderItem salesOrderItem in this.ValidOrderItems)
            {
                decimal quantityOrdered = 0;
                decimal totalBasePrice = 0;

                if (salesOrderItem.ExistProduct)
                {
                    quantityOrderedByProduct.TryGetValue(salesOrderItem.Product, out quantityOrdered);
                    totalBasePriceByProduct.TryGetValue(salesOrderItem.Product, out totalBasePrice);
                }

                foreach (SalesOrderItem featureItem in salesOrderItem.OrderedWithFeatures)
                {
                    featureItem.AppsOnDerivePrices(derivation, quantityOrdered, totalBasePrice);
                }

                salesOrderItem.AppsOnDerivePrices(derivation, quantityOrdered, totalBasePrice);
            }
        }
    }
}
