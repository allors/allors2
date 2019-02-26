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

            if (!this.ExistSalesOrderShipmentState)
            {
                this.SalesOrderShipmentState = new SalesOrderShipmentStates(this.Strategy.Session).NotShipped;
            }

            if (!this.ExistSalesOrderInvoiceState)
            {
                this.SalesOrderInvoiceState = new SalesOrderInvoiceStates(this.Strategy.Session).NotInvoiced;
            }

            if (!this.ExistSalesOrderPaymentState)
            {
                this.SalesOrderPaymentState = new SalesOrderPaymentStates(this.Strategy.Session).NotPaid;
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

            if (!this.ExistTakenBy)
            {
                var internalOrganisations = new Organisations(this.Strategy.Session).InternalOrganisations();
                if (internalOrganisations.Count() == 1)
                {
                    this.TakenBy = internalOrganisations.First();
                }
            }

            if (!this.ExistStore && this.ExistTakenBy)
            {
                var stores = new Stores(this.Strategy.Session).Extent();
                stores.Filter.AddEquals(M.Store.InternalOrganisation, this.TakenBy);

                if (stores.Any())
                {
                    this.Store = stores.First;
                }
            }

            if (!this.ExistOriginFacility)
            {
                this.OriginFacility = this.ExistStore ? this.Store.DefaultFacility : this.Strategy.Session.GetSingleton().Settings.DefaultFacility;
            }

            if (!this.ExistOrderNumber && this.ExistStore)
            {
                this.OrderNumber = this.Store.DeriveNextSalesOrderNumber();
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.IsModified(this))
            {
                derivation.MarkAsModified(this.BillToCustomer, M.SalesOrder.BillToCustomer);
                derivation.AddDependency(this.BillToCustomer, this);

                derivation.MarkAsModified(this.ShipToCustomer, M.SalesOrder.ShipToCustomer);
                derivation.AddDependency(this.ShipToCustomer, this);

                foreach (SalesOrderItem orderItem in this.SalesOrderItems)
                {
                    derivation.MarkAsModified(orderItem);
                    derivation.AddDependency(this, orderItem);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            this.AppsOnDeriveShipment(derivation);


            this.AppsOnDeriveOrderPaymentState(derivation);
            this.AppsOnDeriveOrderInvoiceState(derivation);
            this.AppsOnDeriveOrderState(derivation);

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
                    this.PaymentMethod = partyFinancial.DefaultPaymentMethod;
                }

                if (!this.ExistPaymentMethod && this.ExistStore)
                {
                    this.PaymentMethod = this.Store.DefaultCollectionMethod;
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

            var quantityOrderedByProduct = new Dictionary<Product, decimal>();
            var totalBasePriceByProduct = new Dictionary<Product, decimal>();

            this.ValidOrderItems = this.SalesOrderItems.Where(v => v.IsValid).ToArray();

            foreach (SalesOrderItem salesOrderItem in this.SalesOrderItems)
            {
                salesOrderItem.ShipToAddress = salesOrderItem.ExistAssignedShipToAddress ? salesOrderItem.AssignedShipToAddress : salesOrderItem.SalesOrderWhereSalesOrderItem.ShipToAddress;
                salesOrderItem.ShipToParty = salesOrderItem.ExistAssignedShipToParty ? salesOrderItem.AssignedShipToParty : salesOrderItem.SalesOrderWhereSalesOrderItem.ShipToCustomer;

                if (salesOrderItem.AssignedDeliveryDate.HasValue)
                {
                    salesOrderItem.DeliveryDate = salesOrderItem.AssignedDeliveryDate.Value;
                }
                else if (this.ExistDeliveryDate)
                {
                    //TODO: check if item can have its own deliverydate
                    salesOrderItem.DeliveryDate = this.DeliveryDate;
                }

                if (salesOrderItem.ExistProduct)
                {
                    salesOrderItem.SalesReps = salesOrderItem.Product.ProductCategoriesWhereAllProduct
                        .Select(v => SalesRepRelationships.SalesRep(salesOrderItem.ShipToParty, v, salesOrderItem.SalesOrderWhereSalesOrderItem.OrderDate))
                        .ToArray();
                }
                else
                {
                    salesOrderItem.RemoveSalesReps();
                }

                salesOrderItem.VatRegime = salesOrderItem.ExistAssignedVatRegime ? salesOrderItem.AssignedVatRegime : this.VatRegime;


                if (salesOrderItem.ExistVatRegime && salesOrderItem.VatRegime.ExistVatRate)
                {
                    salesOrderItem.DerivedVatRate = salesOrderItem.VatRegime.VatRate;
                }

                if (!salesOrderItem.ExistDerivedVatRate && (salesOrderItem.ExistProduct || salesOrderItem.ExistProductFeature))
                {
                    salesOrderItem.DerivedVatRate = salesOrderItem.ExistProduct ? salesOrderItem.Product.VatRate : salesOrderItem.ProductFeature.VatRate;
                }

                // TODO
                if (!salesOrderItem.SalesOrderItemShipmentState.Shipped)
                {

                }

                //foreach (OrderShipment orderShipment in salesOrderItem.OrderShipmentsWhereOrderItem)
                //{
                //    if ((!salesOrderItem.ExistSalesOrderItemShipmentState || !salesOrderItem.SalesOrderItemShipmentState.Equals(new SalesOrderItemShipmentStates(salesOrderItem.Strategy.Session).Shipped)))
                //    {
                //        decimal quantity = orderShipment.Quantity;
                //        salesOrderItem.QuantityPicked -= quantity;
                //        salesOrderItem.QuantityPendingShipment -= quantity;
                //        salesOrderItem.QuantityShipped += quantity;
                //    }
                //}

                #region State

                var salesOrderItemStates = new SalesOrderItemStates(salesOrderItem.Strategy.Session);

                if (salesOrderItem.SalesOrderItemShipmentState.Shipped && salesOrderItem.SalesOrderItemInvoiceState.Invoiced)
                {
                    salesOrderItem.SalesOrderItemState = salesOrderItemStates.Completed;
                }

                if (salesOrderItem.SalesOrderItemState.Completed && salesOrderItem.SalesOrderItemPaymentState.Paid)
                {
                    salesOrderItem.SalesOrderItemState = salesOrderItemStates.Finished;
                }

                if (salesOrderItem.ExistOrderWhereValidOrderItem)
                {
                    var order = salesOrderItem.SalesOrderWhereSalesOrderItem;

                    if (order.SalesOrderState.InProcess)
                    {
                        if (salesOrderItem.SalesOrderItemState.Created)
                        {
                            salesOrderItem.Confirm();
                        }
                    }

                    if (order.SalesOrderState.Finished)
                    {
                        salesOrderItem.SalesOrderItemState = salesOrderItemStates.Finished;
                    }

                    if (order.SalesOrderState.Cancelled)
                    {
                        salesOrderItem.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Cancelled;
                    }

                    if (order.SalesOrderState.Rejected)
                    {
                        salesOrderItem.SalesOrderItemState = new SalesOrderItemStates(this.Strategy.Session).Rejected;
                    }
                }

                if (salesOrderItem.SalesOrderItemState.Equals(salesOrderItemStates.InProcess)
                    && !salesOrderItem.SalesOrderItemShipmentState.Equals(new SalesOrderItemShipmentStates(salesOrderItem.Strategy.Session).Shipped))
                {
                    if (salesOrderItem.ExistReservedFromNonSerialisedInventoryItem)
                    {
                        salesOrderItem.AppsOnDeriveQuantities(derivation);

                        salesOrderItem.PreviousQuantity = salesOrderItem.QuantityOrdered;
                        salesOrderItem.PreviousReservedFromNonSerialisedInventoryItem = salesOrderItem.ReservedFromNonSerialisedInventoryItem;
                        salesOrderItem.PreviousProduct = salesOrderItem.Product;
                    }

                    if (salesOrderItem.ExistReservedFromSerialisedInventoryItem)
                    {
                        salesOrderItem.ReservedFromSerialisedInventoryItem.SerialisedInventoryItemState = new SerialisedInventoryItemStates(salesOrderItem.Strategy.Session).Assigned;
                        salesOrderItem.QuantityReserved = 1;
                        salesOrderItem.QuantityRequestsShipping = 1;
                    }
                }

                if (salesOrderItem.SalesOrderItemState.Equals(salesOrderItemStates.Cancelled) ||
                    salesOrderItem.SalesOrderItemState.Equals(salesOrderItemStates.Rejected))
                {
                    if (salesOrderItem.ExistReservedFromNonSerialisedInventoryItem)
                    {
                        salesOrderItem.AppsOnDeriveQuantities(derivation);
                    }

                    if (salesOrderItem.ExistReservedFromSerialisedInventoryItem)
                    {
                        salesOrderItem.ReservedFromSerialisedInventoryItem.SerialisedInventoryItemState = new SerialisedInventoryItemStates(salesOrderItem.Strategy.Session).Available;
                    }
                }

                if (salesOrderItem.QuantityShipped > 0)
                {
                    if (salesOrderItem.QuantityShipped < salesOrderItem.QuantityOrdered)
                    {
                        if (!salesOrderItem.SalesOrderItemShipmentState.Equals(new SalesOrderItemShipmentStates(salesOrderItem.Strategy.Session).PartiallyShipped))
                        {
                            salesOrderItem.SalesOrderItemShipmentState = new SalesOrderItemShipmentStates(salesOrderItem.Strategy.Session).PartiallyShipped;
                        }
                    }
                    else
                    {
                        if (!salesOrderItem.SalesOrderItemShipmentState.Equals(new SalesOrderItemShipmentStates(salesOrderItem.Strategy.Session).Shipped))
                        {
                            salesOrderItem.SalesOrderItemShipmentState = new SalesOrderItemShipmentStates(salesOrderItem.Strategy.Session).Shipped;
                        }
                    }
                }

                SalesOrderItemPaymentState state = null;

                foreach (OrderItemBilling orderItemBilling in salesOrderItem.OrderItemBillingsWhereOrderItem)
                {
                    if (orderItemBilling.InvoiceItem is SalesInvoiceItem salesInvoiceItem)
                    {
                        if (salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Equals(new SalesInvoiceStates(salesOrderItem.Strategy.Session).Paid))
                        {
                            state = new SalesOrderItemPaymentStates(salesOrderItem.Strategy.Session).Paid;
                        }

                        if (salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Equals(new SalesInvoiceStates(salesOrderItem.Strategy.Session).PartiallyPaid))
                        {
                            state = new SalesOrderItemPaymentStates(salesOrderItem.Strategy.Session).PartiallyPaid;
                        }
                    }
                }

                foreach (OrderShipment orderShipment in salesOrderItem.OrderShipmentsWhereOrderItem)
                {
                    foreach (ShipmentItemBilling shipmentItemBilling in orderShipment.ShipmentItem.ShipmentItemBillingsWhereShipmentItem)
                    {
                        if (shipmentItemBilling.InvoiceItem is SalesInvoiceItem salesInvoiceItem)
                        {
                            if (salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Equals(new SalesInvoiceStates(salesOrderItem.Strategy.Session).Paid))
                            {
                                state = new SalesOrderItemPaymentStates(salesOrderItem.Strategy.Session).Paid;
                            }

                            if (salesInvoiceItem.SalesInvoiceWhereSalesInvoiceItem.SalesInvoiceState.Equals(new SalesInvoiceStates(salesOrderItem.Strategy.Session).PartiallyPaid))
                            {
                                state = new SalesOrderItemPaymentStates(salesOrderItem.Strategy.Session).PartiallyPaid;
                            }
                        }
                    }
                }

                if (state != null)
                {
                    if (!salesOrderItem.SalesOrderItemPaymentState.Equals(state))
                    {
                        salesOrderItem.SalesOrderItemPaymentState = state;
                    }
                }

                #endregion

            }


            foreach (SalesOrderItem salesOrderItem in this.SalesOrderItems)
            {
                foreach (SalesOrderItem featureItem in salesOrderItem.OrderedWithFeatures)
                {
                    this.CalculatePrices(featureItem, 0, 0);
                }

                salesOrderItem.UnitPurchasePrice = 0;

                if (salesOrderItem.Part != null &&
                    salesOrderItem.Part.ExistSupplierOfferingsWherePart &&
                    salesOrderItem.Part.SupplierOfferingsWherePart.Select(v => v.Supplier).Distinct().Count() == 1)
                {
                    decimal price = 0;
                    UnitOfMeasure uom = null;

                    foreach (SupplierOffering supplierOffering in salesOrderItem.Part.SupplierOfferingsWherePart)
                    {
                        if (supplierOffering.FromDate <= this.OrderDate &&
                            (!supplierOffering.ExistThroughDate || supplierOffering.ThroughDate >= this.OrderDate))
                        {
                            price = supplierOffering.Price;
                            uom = supplierOffering.UnitOfMeasure;
                        }
                    }

                    if (price != 0)
                    {
                        salesOrderItem.UnitPurchasePrice = price;
                        if (uom != null && !uom.Equals(salesOrderItem.Product.UnitOfMeasure))
                        {
                            foreach (UnitOfMeasureConversion unitOfMeasureConversion in uom.UnitOfMeasureConversions)
                            {
                                if (unitOfMeasureConversion.ToUnitOfMeasure.Equals(salesOrderItem.Product.UnitOfMeasure))
                                {
                                    salesOrderItem.UnitPurchasePrice = Math.Round(salesOrderItem.UnitPurchasePrice * (1 / unitOfMeasureConversion.ConversionFactor), 2);
                                }
                            }
                        }
                    }
                }

                if (salesOrderItem.RequiredMarkupPercentage.HasValue && salesOrderItem.UnitPurchasePrice > 0)
                {
                    salesOrderItem.ActualUnitPrice = Math.Round((1 + (salesOrderItem.RequiredMarkupPercentage.Value / 100)) * salesOrderItem.UnitPurchasePrice, 2);
                }

                if (salesOrderItem.RequiredProfitMargin.HasValue && salesOrderItem.UnitPurchasePrice > 0)
                {
                    salesOrderItem.ActualUnitPrice = Math.Round(salesOrderItem.UnitPurchasePrice / (1 - (salesOrderItem.RequiredProfitMargin.Value / 100)), 2);
                }

                this.CalculatePrices(salesOrderItem, 0, 0);

                var amountAlreadyInvoiced = salesOrderItem.OrderItemBillingsWhereOrderItem?.Sum(v => v.Amount);
                if (amountAlreadyInvoiced == 0)
                {
                    amountAlreadyInvoiced = salesOrderItem.OrderShipmentsWhereOrderItem
                        .SelectMany(orderShipment => orderShipment.ShipmentItem.ShipmentItemBillingsWhereShipmentItem)
                        .Aggregate(amountAlreadyInvoiced, (current, shipmentItemBilling) => current + shipmentItemBilling.Amount);
                }

                var leftToInvoice = (salesOrderItem.QuantityOrdered * salesOrderItem.ActualUnitPrice) - amountAlreadyInvoiced;

                if (amountAlreadyInvoiced > 0 && leftToInvoice > 0)
                {
                    salesOrderItem.SalesOrderItemInvoiceState = new SalesOrderItemInvoiceStates(salesOrderItem.Strategy.Session).PartiallyInvoiced;
                }

                if (amountAlreadyInvoiced > 0 && leftToInvoice <= 0)
                {
                    salesOrderItem.SalesOrderItemInvoiceState = new SalesOrderItemInvoiceStates(salesOrderItem.Strategy.Session).Invoiced;
                }

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

            // for the second time, because unitbaseprice might not be set
            this.ValidOrderItems = this.SalesOrderItems.Where(v => v.IsValid).ToArray();

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
                    this.CalculatePrices(featureItem, quantityOrdered, totalBasePrice);
                }

                this.CalculatePrices(salesOrderItem, quantityOrdered, totalBasePrice);
            }

            this.AppsOnDeriveLocale(derivation);
            this.AppsOnDeriveOrderTotals(derivation);
            this.AppsOnDeriveCustomers(derivation);

            this.SalesReps = this.ValidOrderItems
                .OfType<SalesOrderItem>()
                .SelectMany(v => v.SalesReps)
                .Distinct()
                .ToArray();
            this.AppsDeriveCanShip(derivation);
            this.AppsDeriveCanInvoice(derivation);

            if (this.CanShip)
            {
                this.AppsShipThis(derivation);
            }

            this.PreviousBillToCustomer = this.BillToCustomer;
            this.PreviousShipToCustomer = this.ShipToCustomer;

            this.ResetPrintDocument();
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            if (!this.CanShip)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Ship, Operations.Execute));
            }

            if (!this.CanInvoice)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }

            if (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).InProcess) &&
                Equals(this.Store.BillingProcess, new BillingProcesses(this.Strategy.Session).BillingForShipmentItems))
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
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
        }


        public void AppsOnDeriveShipment(IDerivation derivation)
        {
            foreach (SalesOrderItem salesOrderItem in this.SalesOrderItems)
            {
                salesOrderItem.QuantityPicked = salesOrderItem.OrderShipmentsWhereOrderItem.Sum(v => v.QuantityPicked);
                salesOrderItem.QuantityShipped = salesOrderItem.OrderShipmentsWhereOrderItem.Sum(v => v.Quantity);
            }

            var validSalesOrderItems = this.ValidOrderItems.Cast<SalesOrderItem>().ToArray();

            // true if any item has been shipped
            var itemsShipped = validSalesOrderItems.Any(v => v.SalesOrderItemShipmentState.Shipped);

            // true if any item has been Partially Shipped
            var itemsPartiallyShipped = validSalesOrderItems.Any(v => v.SalesOrderItemShipmentState.PartiallyShipped);

            // true if any item has no shipmentstate
            var itemsNotShipped = validSalesOrderItems.Any(v => !v.ExistSalesOrderItemShipmentState);

            if (itemsShipped && !itemsNotShipped
                             && !itemsPartiallyShipped
                             && !this.SalesOrderShipmentState.Shipped)
            {
                this.SalesOrderShipmentState = new SalesOrderShipmentStates(this.Strategy.Session).Shipped;
            }

            if ((itemsPartiallyShipped
                 || itemsShipped && itemsNotShipped)
                    && !this.SalesOrderShipmentState.PartiallyShipped)
            {
                this.SalesOrderShipmentState = new SalesOrderShipmentStates(this.Strategy.Session).PartiallyShipped;
            }
        }

        public void AppsOnDeriveOrderInvoiceState(IDerivation derivation)
        {
            var validSalesOrderItems = this.ValidOrderItems.Cast<SalesOrderItem>().ToArray();

            var itemsInvoiced = validSalesOrderItems.Any(v => v.SalesOrderItemInvoiceState.PartiallyInvoiced);

            var itemsPartiallyInvoiced = validSalesOrderItems.Any(v => v.SalesOrderItemInvoiceState.PartiallyInvoiced);

            var itemsNotInvoiced = false;

            foreach (SalesOrderItem orderItem in this.ValidOrderItems)
            {
                if (orderItem.ExistSalesOrderItemInvoiceState)
                {
                    if (orderItem.SalesOrderItemInvoiceState.PartiallyInvoiced)
                    {
                        itemsPartiallyInvoiced = true;
                    }

                    if (orderItem.SalesOrderItemInvoiceState.Invoiced)
                    {
                        itemsInvoiced = true;
                    }

                    if (orderItem.SalesOrderItemInvoiceState.NotInvoiced)
                    {
                        itemsNotInvoiced = true;
                    }
                }
            }

            if (itemsInvoiced && !itemsNotInvoiced && !itemsPartiallyInvoiced && !this.SalesOrderInvoiceState.Invoiced)
            {
                this.SalesOrderInvoiceState = new SalesOrderInvoiceStates(this.Strategy.Session).Invoiced;
            }

            if ((itemsPartiallyInvoiced || itemsInvoiced && itemsNotInvoiced) && !this.SalesOrderInvoiceState.PartiallyInvoiced)
            {
                this.SalesOrderInvoiceState = new SalesOrderInvoiceStates(this.Strategy.Session).PartiallyInvoiced;
            }
        }

        /// <summary>
        /// Depends on AppsOnDeriveShipment, AppsOnDeriveOrderPaymentState, AppsOnDeriveOrderInvoiceState
        /// </summary>
        /// <param name="derivation"></param>
        public void AppsOnDeriveOrderState(IDerivation derivation)
        {
            if (this.SalesOrderShipmentState.Shipped && this.SalesOrderInvoiceState.Invoiced)
            {
                this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Completed;
            }

            if (this.SalesOrderState.Completed && this.SalesOrderPaymentState.Paid)
            {
                this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Finished;
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
                this.Locale = this.Strategy.Session.GetSingleton().DefaultLocale;
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
                Equals(this.Store.BillingProcess, new BillingProcesses(this.Strategy.Session).BillingForOrderItems))
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
                        .WithDetails(orderItem.Details)
                        .WithDescription(orderItem.Description)
                        .WithInternalComment(orderItem.InternalComment)
                        .WithMessage(orderItem.Message)
                        .Build();

                    salesInvoice.AddSalesInvoiceItem(invoiceItem);

                    new OrderItemBillingBuilder(this.Strategy.Session)
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
                    salesInvoice.AddSalesTerm(new IncoTermBuilder(this.Strategy.Session)
                                                .WithTermType(salesTerm.TermType)
                                                .WithTermValue(salesTerm.TermValue)
                                                .WithDescription(salesTerm.Description)
                                                .Build());
                }

                if (salesTerm.GetType().Name == typeof(InvoiceTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new InvoiceTermBuilder(this.Strategy.Session)
                        .WithTermType(salesTerm.TermType)
                        .WithTermValue(salesTerm.TermValue)
                        .WithDescription(salesTerm.Description)
                        .Build());
                }

                if (salesTerm.GetType().Name == typeof(OrderTerm).Name)
                {
                    salesInvoice.AddSalesTerm(new OrderTermBuilder(this.Strategy.Session)
                        .WithTermType(salesTerm.TermType)
                        .WithTermValue(salesTerm.TermValue)
                        .WithDescription(salesTerm.Description)
                        .Build());
                }
            }
        }

        public void AppsPrint(PrintablePrint method)
        {
            if (!method.IsPrinted)
            {
                var singleton = this.Strategy.Session.GetSingleton();
                var logo = this.TakenBy?.ExistLogoImage == true ?
                               this.TakenBy.LogoImage.MediaContent.Data :
                               singleton.LogoImage.MediaContent.Data;

                var images = new Dictionary<string, byte[]>
                                 {
                                     { "Logo", logo },
                                 };

                if (this.ExistOrderNumber)
                {
                    var session = this.Strategy.Session;
                    var barcodeService = session.ServiceProvider.GetRequiredService<IBarcodeService>();
                    var barcode = barcodeService.Generate(this.OrderNumber, BarcodeType.CODE_128, 320, 80);
                    images.Add("Barcode", barcode);
                }

                var model = new Print.SalesOrderModel.Model(this);
                this.RenderPrintDocument(this.TakenBy?.SalesOrderTemplate, model, images);

                this.PrintDocument.Media.FileName = $"{this.OrderNumber}.odt";
            }
        }

        public void CalculatePrices(SalesOrderItem salesOrderItem, decimal quantityOrdered, decimal totalBasePrice)
        {
            salesOrderItem.RemoveCurrentPriceComponents();

            salesOrderItem.UnitBasePrice = 0;
            salesOrderItem.UnitDiscount = 0;
            salesOrderItem.UnitSurcharge = 0;
            salesOrderItem.CalculatedUnitPrice = 0;
            decimal discountAdjustmentAmount = 0;
            decimal surchargeAdjustmentAmount = 0;

            var customer = this.BillToCustomer;

            var priceComponents = salesOrderItem.GetPriceComponents();

            foreach (var priceComponent in priceComponents)
            {
                if (priceComponent.Strategy.Class.Equals(M.BasePrice.ObjectType))
                {
                    if (PriceComponents.AppsIsEligible(new PriceComponents.IsEligibleParams
                    {
                        PriceComponent = priceComponent,
                        Customer = customer,
                        Product = salesOrderItem.Product,
                        SalesOrder = this,
                        QuantityOrdered = quantityOrdered,
                        ValueOrdered = totalBasePrice,
                    }))
                    {
                        if (priceComponent.ExistPrice)
                        {
                            if (salesOrderItem.UnitBasePrice == 0 || priceComponent.Price < salesOrderItem.UnitBasePrice)
                            {
                                salesOrderItem.UnitBasePrice = priceComponent.Price ?? 0;

                                salesOrderItem.RemoveCurrentPriceComponents();
                                salesOrderItem.AddCurrentPriceComponent(priceComponent);
                            }
                        }
                    }
                }
            }

            ////SafeGuard
            if (salesOrderItem.ExistProduct && !salesOrderItem.ExistActualUnitPrice)
            {
                var invalid = true;
                foreach (BasePrice basePrice in salesOrderItem.CurrentPriceComponents)
                {
                    if (basePrice.Price > 0)
                    {
                        invalid = false;
                    }
                }

                if (invalid)
                {
                    salesOrderItem.QuantityOrdered = 0;
                }
            }

            if (!salesOrderItem.ExistActualUnitPrice)
            {
                var revenueBreakDiscount = 0M;
                var revenueBreakSurcharge = 0M;

                foreach (var priceComponent in priceComponents)
                {
                    if (priceComponent.Strategy.Class.Equals(M.DiscountComponent.ObjectType) || priceComponent.Strategy.Class.Equals(M.SurchargeComponent.ObjectType))
                    {
                        if (PriceComponents.AppsIsEligible(new PriceComponents.IsEligibleParams
                        {
                            PriceComponent = priceComponent,
                            Customer = customer,
                            Product = salesOrderItem.Product,
                            SalesOrder = this,
                            QuantityOrdered = quantityOrdered,
                            ValueOrdered = totalBasePrice,
                        }))
                        {
                            salesOrderItem.AddCurrentPriceComponent(priceComponent);

                            revenueBreakDiscount = salesOrderItem.SetUnitDiscount(priceComponent, revenueBreakDiscount);
                            revenueBreakSurcharge = salesOrderItem.SetUnitSurcharge(priceComponent, revenueBreakSurcharge);
                        }
                    }
                }

                var adjustmentBase = salesOrderItem.UnitBasePrice - salesOrderItem.UnitDiscount + salesOrderItem.UnitSurcharge;

                if (salesOrderItem.ExistDiscountAdjustment)
                {
                    if (salesOrderItem.DiscountAdjustment.Percentage.HasValue)
                    {
                        discountAdjustmentAmount = Math.Round((adjustmentBase * salesOrderItem.DiscountAdjustment.Percentage.Value) / 100, 2);
                    }
                    else
                    {
                        discountAdjustmentAmount = salesOrderItem.DiscountAdjustment.Amount ?? 0;
                    }

                    salesOrderItem.UnitDiscount += discountAdjustmentAmount;
                }

                if (salesOrderItem.ExistSurchargeAdjustment)
                {
                    if (salesOrderItem.SurchargeAdjustment.Percentage.HasValue)
                    {
                        surchargeAdjustmentAmount = Math.Round((adjustmentBase * salesOrderItem.SurchargeAdjustment.Percentage.Value) / 100, 2);
                    }
                    else
                    {
                        surchargeAdjustmentAmount = salesOrderItem.SurchargeAdjustment.Amount ?? 0;
                    }

                    salesOrderItem.UnitSurcharge += surchargeAdjustmentAmount;
                }
            }

            var price = salesOrderItem.ActualUnitPrice ?? salesOrderItem.UnitBasePrice;

            decimal vat = 0;
            if (salesOrderItem.ExistDerivedVatRate)
            {
                var vatRate = salesOrderItem.DerivedVatRate.Rate;
                var vatBase = price - salesOrderItem.UnitDiscount + salesOrderItem.UnitSurcharge;
                vat = Math.Round((vatBase * vatRate) / 100, 2);
            }

            salesOrderItem.UnitVat = vat;
            salesOrderItem.TotalBasePrice = price * salesOrderItem.QuantityOrdered;
            salesOrderItem.TotalDiscount = salesOrderItem.UnitDiscount * salesOrderItem.QuantityOrdered;
            salesOrderItem.TotalSurcharge = salesOrderItem.UnitSurcharge * salesOrderItem.QuantityOrdered;
            salesOrderItem.TotalOrderAdjustment = (0 - discountAdjustmentAmount + surchargeAdjustmentAmount) * salesOrderItem.QuantityOrdered;

            if (salesOrderItem.TotalBasePrice > 0)
            {
                salesOrderItem.TotalDiscountAsPercentage = Math.Round((salesOrderItem.TotalDiscount / salesOrderItem.TotalBasePrice) * 100, 2);
                salesOrderItem.TotalSurchargeAsPercentage = Math.Round((salesOrderItem.TotalSurcharge / salesOrderItem.TotalBasePrice) * 100, 2);
            }

            if (salesOrderItem.ActualUnitPrice.HasValue)
            {
                salesOrderItem.CalculatedUnitPrice = salesOrderItem.ActualUnitPrice.Value;
            }
            else
            {
                salesOrderItem.CalculatedUnitPrice = salesOrderItem.UnitBasePrice - salesOrderItem.UnitDiscount + salesOrderItem.UnitSurcharge;
            }

            salesOrderItem.TotalExVat = salesOrderItem.CalculatedUnitPrice * salesOrderItem.QuantityOrdered;
            salesOrderItem.TotalVat = salesOrderItem.UnitVat * salesOrderItem.QuantityOrdered;
            salesOrderItem.TotalIncVat = salesOrderItem.TotalExVat + salesOrderItem.TotalVat;

            foreach (SalesOrderItem featureItem in salesOrderItem.OrderedWithFeatures)
            {
                salesOrderItem.CalculatedUnitPrice += salesOrderItem.CalculatedUnitPrice;
                salesOrderItem.TotalBasePrice += salesOrderItem.TotalBasePrice;
                salesOrderItem.TotalDiscount += salesOrderItem.TotalDiscount;
                salesOrderItem.TotalSurcharge += salesOrderItem.TotalSurcharge;
                salesOrderItem.TotalExVat += salesOrderItem.TotalExVat;
                salesOrderItem.TotalVat += salesOrderItem.TotalVat;
                salesOrderItem.TotalIncVat += salesOrderItem.TotalIncVat;
            }

            var toCurrency = this.Currency;
            var fromCurrency = this.TakenBy?.PreferredCurrency;

            if (fromCurrency != null && toCurrency != null)
            {
                if (fromCurrency != null && toCurrency != null && fromCurrency.Equals(toCurrency))
                {
                    salesOrderItem.TotalBasePriceCustomerCurrency = salesOrderItem.TotalBasePrice;
                    salesOrderItem.TotalDiscountCustomerCurrency = salesOrderItem.TotalDiscount;
                    salesOrderItem.TotalSurchargeCustomerCurrency = salesOrderItem.TotalSurcharge;
                    salesOrderItem.TotalExVatCustomerCurrency = salesOrderItem.TotalExVat;
                    salesOrderItem.TotalVatCustomerCurrency = salesOrderItem.TotalVat;
                    salesOrderItem.TotalIncVatCustomerCurrency = salesOrderItem.TotalIncVat;
                }
                else
                {
                    salesOrderItem.TotalBasePriceCustomerCurrency =
                        Currencies.ConvertCurrency(salesOrderItem.TotalBasePrice, fromCurrency, toCurrency);
                    salesOrderItem.TotalDiscountCustomerCurrency =
                        Currencies.ConvertCurrency(salesOrderItem.TotalDiscount, fromCurrency, toCurrency);
                    salesOrderItem.TotalSurchargeCustomerCurrency =
                        Currencies.ConvertCurrency(salesOrderItem.TotalSurcharge, fromCurrency, toCurrency);
                    salesOrderItem.TotalExVatCustomerCurrency =
                        Currencies.ConvertCurrency(salesOrderItem.TotalExVat, fromCurrency, toCurrency);
                    salesOrderItem.TotalVatCustomerCurrency = Currencies.ConvertCurrency(salesOrderItem.TotalVat, fromCurrency, toCurrency);
                    salesOrderItem.TotalIncVatCustomerCurrency =
                        Currencies.ConvertCurrency(salesOrderItem.TotalIncVat, fromCurrency, toCurrency);
                }
            }

            salesOrderItem.InitialMarkupPercentage = 0;
            salesOrderItem.MaintainedMarkupPercentage = 0;
            salesOrderItem.InitialProfitMargin = 0;
            salesOrderItem.MaintainedProfitMargin = 0;

            ////internet wiki page on markup business
            if (salesOrderItem.ExistUnitPurchasePrice && salesOrderItem.UnitPurchasePrice != 0 && salesOrderItem.CalculatedUnitPrice != 0 && salesOrderItem.UnitBasePrice != 0)
            {
                salesOrderItem.InitialMarkupPercentage = Math.Round(((salesOrderItem.UnitBasePrice / salesOrderItem.UnitPurchasePrice) - 1) * 100, 2);
                salesOrderItem.MaintainedMarkupPercentage = Math.Round(((salesOrderItem.CalculatedUnitPrice / salesOrderItem.UnitPurchasePrice) - 1) * 100, 2);

                salesOrderItem.InitialProfitMargin = Math.Round(((salesOrderItem.UnitBasePrice - salesOrderItem.UnitPurchasePrice) / salesOrderItem.UnitBasePrice) * 100, 2);
                salesOrderItem.MaintainedProfitMargin = Math.Round(((salesOrderItem.CalculatedUnitPrice - salesOrderItem.UnitPurchasePrice) / salesOrderItem.CalculatedUnitPrice) * 100, 2);
            }
        }
    }
}
