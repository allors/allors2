
// <copyright file="SalesOrder.cs" company="Allors bvba">
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

                var now = this.Session().Now();
                var customerRelationship = this.BillToCustomer.CustomerRelationshipsWhereCustomer
                    .FirstOrDefault(v => v.InternalOrganisation == this.TakenBy
                      && v.FromDate <= now
                      && (!v.ExistThroughDate || v.ThroughDate >= now));

                if (customerRelationship?.PaymentNetDays().HasValue == true)
                {
                    return customerRelationship.PaymentNetDays().Value;
                }

                if (this.ExistStore && this.Store.ExistPaymentNetDays)
                {
                    return this.Store.PaymentNetDays;
                }

                return 0;
            }
        }

        private bool IsDeletable =>
            (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).Provisional)
                || this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).ReadyForPosting)
                || this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).Cancelled)
                || this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).Rejected))
            && !this.ExistQuote
            && !this.ExistSalesInvoicesWhereSalesOrder
            && this.SalesOrderItems.All(v => v.IsDeletable);

        public void BaseOnBuild(ObjectOnBuild method)
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
                this.OrderDate = this.Session().Now();
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = this.Session().Now();
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
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                iteration.AddDependency(this.BillToCustomer, this);
                iteration.Mark(this.BillToCustomer);

                iteration.AddDependency(this.ShipToCustomer, this);
                iteration.Mark(this.ShipToCustomer);

                foreach (SalesOrderItem orderItem in this.SalesOrderItems)
                {
                    iteration.AddDependency(this, orderItem);
                    iteration.Mark(orderItem);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var session = this.Session();

            // SalesOrder Derivations and Validations
            if (this.SalesOrderState.IsProvisional)
            {
                this.BillToCustomer ??= this.ShipToCustomer;
                this.ShipToCustomer ??= this.BillToCustomer;
                this.Customers = new[] { this.BillToCustomer, this.ShipToCustomer, this.PlacingCustomer };
                this.DerivedLocale = this.Locale ?? this.BillToCustomer?.Locale ?? this.TakenBy?.Locale;
                this.DerivedVatRegime = this.AssignedVatRegime;
                this.DerivedIrpfRegime = this.AssignedIrpfRegime;
                this.DerivedCurrency = this.AssignedCurrency ?? this.BillToCustomer?.PreferredCurrency ?? this.BillToCustomer?.Locale?.Country?.Currency ?? this.TakenBy?.PreferredCurrency;
                this.DerivedTakenByContactMechanism = this.AssignedTakenByContactMechanism ?? this.TakenBy?.OrderAddress ?? this.TakenBy?.BillingAddress ?? this.TakenBy?.GeneralCorrespondence;
                this.DerivedBillToContactMechanism = this.AssignedBillToContactMechanism ?? this.BillToCustomer?.BillingAddress ?? this.BillToCustomer?.ShippingAddress ?? this.BillToCustomer?.GeneralCorrespondence;
                this.DerivedBillToEndCustomerContactMechanism = this.AssignedBillToEndCustomerContactMechanism ?? this.BillToEndCustomer?.BillingAddress ?? this.BillToEndCustomer?.ShippingAddress ?? this.BillToEndCustomer?.GeneralCorrespondence;
                this.DerivedShipToEndCustomerAddress = this.AssignedShipToEndCustomerAddress ?? this.ShipToEndCustomer?.ShippingAddress ?? this.ShipToEndCustomer?.GeneralCorrespondence as PostalAddress;
                this.DerivedShipFromAddress = this.AssignedShipFromAddress ?? this.TakenBy?.ShippingAddress;
                this.DerivedShipToAddress = this.AssignedShipToAddress ?? this.ShipToCustomer?.ShippingAddress;
                this.DerivedShipmentMethod = this.AssignedShipmentMethod ?? this.ShipToCustomer?.DefaultShipmentMethod ?? this.Store?.DefaultShipmentMethod;
                this.DerivedPaymentMethod = this.AssignedPaymentMethod ?? this.TakenBy?.DefaultPaymentMethod ?? this.Store?.DefaultCollectionMethod;

                if (this.ExistOrderDate)
                {
                    this.DerivedVatRate = this.DerivedVatRegime?.VatRates.First(v => v.FromDate <= this.OrderDate && (!v.ExistThroughDate || v.ThroughDate >= this.OrderDate));
                    this.DerivedIrpfRate = this.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= this.OrderDate && (!v.ExistThroughDate || v.ThroughDate >= this.OrderDate));
                }
            }

            if (!this.ExistOrderNumber && this.ExistStore)
            {
                var year = this.OrderDate.Year;
                this.OrderNumber = this.Store.NextSalesOrderNumber(year);

                var fiscalYearStoreSequenceNumbers = this.Store.FiscalYearsStoreSequenceNumbers.FirstOrDefault(v => v.FiscalYear == year);
                var prefix = this.TakenBy.InvoiceSequence.IsEnforcedSequence ? this.Store.SalesOrderNumberPrefix : fiscalYearStoreSequenceNumbers.SalesOrderNumberPrefix;
                this.SortableOrderNumber = this.Session().GetSingleton().SortableNumber(prefix, this.OrderNumber, year.ToString());
            }

            if (this.BillToCustomer?.BaseIsActiveCustomer(this.TakenBy, this.OrderDate) == false)
            {
                derivation.Validation.AddError(this, M.SalesOrder.BillToCustomer, ErrorMessages.PartyIsNotACustomer);
            }

            if (this.ShipToCustomer?.BaseIsActiveCustomer(this.TakenBy, this.OrderDate) == false)
            {
                derivation.Validation.AddError(this, M.SalesOrder.ShipToCustomer, ErrorMessages.PartyIsNotACustomer);
            }

            if (this.SalesOrderState.IsInProcess)
            {
                derivation.Validation.AssertExists(this, this.Meta.DerivedShipToAddress);
                derivation.Validation.AssertExists(this, this.Meta.DerivedBillToContactMechanism);
            }

            if (this.ExistOrderDate
                && this.ExistTakenBy
                && this.DerivedCurrency != this.TakenBy.PreferredCurrency)
            {
                var exchangeRate = this.DerivedCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= this.OrderDate.Date && v.ToCurrency.Equals(this.TakenBy.PreferredCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();

                if (exchangeRate == null)
                {
                    exchangeRate = this.TakenBy.PreferredCurrency.ExchangeRatesWhereFromCurrency.Where(v => v.ValidFrom.Date <= this.OrderDate.Date && v.ToCurrency.Equals(this.DerivedCurrency)).OrderByDescending(v => v.ValidFrom).FirstOrDefault();
                }

                if (exchangeRate == null)
                {
                    derivation.Validation.AddError(this, M.Quote.AssignedCurrency, ErrorMessages.CurrencyNotAllowed);
                }
            }

            // SalesOrderItem Derivations and Validations
            foreach (SalesOrderItem salesOrderItem in this.SalesOrderItems)
            {
                var salesOrderItemDerivedRoles = (SalesOrderItemDerivedRoles)salesOrderItem;

                if (salesOrderItem.SalesOrderItemState.IsProvisional)
                {
                    salesOrderItemDerivedRoles.DerivedShipFromAddress = salesOrderItem.AssignedShipFromAddress ?? this.DerivedShipFromAddress;
                    salesOrderItemDerivedRoles.DerivedShipToAddress = salesOrderItem.AssignedShipToAddress ?? salesOrderItem.AssignedShipToParty?.ShippingAddress ?? this.DerivedShipToAddress;
                    salesOrderItemDerivedRoles.DerivedShipToParty = salesOrderItem.AssignedShipToParty ?? this.ShipToCustomer;
                    salesOrderItemDerivedRoles.DerivedDeliveryDate = salesOrderItem.AssignedDeliveryDate ?? this.DeliveryDate;
                    salesOrderItemDerivedRoles.DerivedVatRegime = salesOrderItem.AssignedVatRegime ?? this.DerivedVatRegime;
                    salesOrderItemDerivedRoles.VatRate = salesOrderItem.DerivedVatRegime?.VatRates.First(v => v.FromDate <= this.OrderDate && (!v.ExistThroughDate || v.ThroughDate >= this.OrderDate));
                    salesOrderItemDerivedRoles.DerivedIrpfRegime = salesOrderItem.AssignedIrpfRegime ?? this.DerivedIrpfRegime;
                    salesOrderItemDerivedRoles.IrpfRate = salesOrderItem.DerivedIrpfRegime?.IrpfRates.First(v => v.FromDate <= this.OrderDate && (!v.ExistThroughDate || v.ThroughDate >= this.OrderDate));
                }

                // TODO: Use versioning
                if (salesOrderItem.ExistPreviousProduct && !salesOrderItem.PreviousProduct.Equals(salesOrderItem.Product))
                {
                    derivation.Validation.AddError(salesOrderItem, M.SalesOrderItem.Product, ErrorMessages.SalesOrderItemProductChangeNotAllowed);
                }
                else
                {
                    salesOrderItemDerivedRoles.PreviousProduct = salesOrderItem.Product;
                }

                if (salesOrderItem.ExistSalesOrderItemWhereOrderedWithFeature)
                {
                    derivation.Validation.AssertExists(salesOrderItem, M.SalesOrderItem.ProductFeature);
                    derivation.Validation.AssertNotExists(salesOrderItem, M.SalesOrderItem.Product);
                }
                else
                {
                    derivation.Validation.AssertNotExists(salesOrderItem, M.SalesOrderItem.ProductFeature);
                }

                if (salesOrderItem.ExistProduct && salesOrderItem.ExistQuantityOrdered && salesOrderItem.QuantityOrdered < salesOrderItem.QuantityShipped)
                {
                    derivation.Validation.AddError(salesOrderItem, M.SalesOrderItem.QuantityOrdered, ErrorMessages.SalesOrderItemLessThanAlreadeyShipped);
                }

                var isSubTotalItem = salesOrderItem.ExistInvoiceItemType && (salesOrderItem.InvoiceItemType.IsProductItem || salesOrderItem.InvoiceItemType.IsPartItem);
                if (isSubTotalItem)
                {
                    if (salesOrderItem.QuantityOrdered == 0)
                    {
                        derivation.Validation.AddError(salesOrderItem, M.SalesOrderItem.QuantityOrdered, "QuantityOrdered is Required");
                    }
                }
                else
                {
                    if (salesOrderItem.AssignedUnitPrice == 0)
                    {
                        derivation.Validation.AddError(salesOrderItem, M.SalesOrderItem.AssignedUnitPrice, "Price is Required");
                    }
                }

                derivation.Validation.AssertExistsAtMostOne(salesOrderItem, M.SalesOrderItem.Product, M.SalesOrderItem.ProductFeature);
                derivation.Validation.AssertExistsAtMostOne(salesOrderItem, M.SalesOrderItem.SerialisedItem, M.SalesOrderItem.ProductFeature);
                derivation.Validation.AssertExistsAtMostOne(salesOrderItem, M.SalesOrderItem.ReservedFromSerialisedInventoryItem, M.SalesOrderItem.ReservedFromNonSerialisedInventoryItem);
                derivation.Validation.AssertExistsAtMostOne(salesOrderItem, M.SalesOrderItem.AssignedUnitPrice, M.SalesOrderItem.DiscountAdjustments, M.SalesOrderItem.SurchargeAdjustments);
            }

            var validOrderItems = this.SalesOrderItems.Where(v => v.IsValid).ToArray();
            this.ValidOrderItems = validOrderItems;

            this.DeriveVatClause();

            var salesOrderShipmentStates = new SalesOrderShipmentStates(this.Strategy.Session);
            var salesOrderPaymentStates = new SalesOrderPaymentStates(this.Strategy.Session);
            var salesOrderInvoiceStates = new SalesOrderInvoiceStates(this.Strategy.Session);

            var salesOrderItemShipmentStates = new SalesOrderItemShipmentStates(derivation.Session);
            var salesOrderItemPaymentStates = new SalesOrderItemPaymentStates(derivation.Session);
            var salesOrderItemInvoiceStates = new SalesOrderItemInvoiceStates(derivation.Session);

            // SalesOrder Shipment State
            if (validOrderItems.Any())
            {
                if (validOrderItems.All(v => v.SalesOrderItemShipmentState.Shipped))
                {
                    this.SalesOrderShipmentState = salesOrderShipmentStates.Shipped;
                }
                else if (validOrderItems.All(v => v.SalesOrderItemShipmentState.NotShipped))
                {
                    this.SalesOrderShipmentState = salesOrderShipmentStates.NotShipped;
                }
                else if (validOrderItems.Any(v => v.SalesOrderItemShipmentState.InProgress))
                {
                    this.SalesOrderShipmentState = salesOrderShipmentStates.InProgress;
                }
                else
                {
                    this.SalesOrderShipmentState = salesOrderShipmentStates.PartiallyShipped;
                }

                // SalesOrder Payment State
                if (validOrderItems.All(v => v.SalesOrderItemPaymentState.Paid))
                {
                    this.SalesOrderPaymentState = salesOrderPaymentStates.Paid;
                }
                else if (validOrderItems.All(v => v.SalesOrderItemPaymentState.NotPaid))
                {
                    this.SalesOrderPaymentState = salesOrderPaymentStates.NotPaid;
                }
                else
                {
                    this.SalesOrderPaymentState = salesOrderPaymentStates.PartiallyPaid;
                }

                // SalesOrder Invoice State
                if (validOrderItems.All(v => v.SalesOrderItemInvoiceState.Invoiced))
                {
                    this.SalesOrderInvoiceState = salesOrderInvoiceStates.Invoiced;
                }
                else if (validOrderItems.All(v => v.SalesOrderItemInvoiceState.NotInvoiced))
                {
                    this.SalesOrderInvoiceState = salesOrderInvoiceStates.NotInvoiced;
                }
                else
                {
                    this.SalesOrderInvoiceState = salesOrderInvoiceStates.PartiallyInvoiced;
                }

                // SalesOrder OrderState
                if (this.SalesOrderShipmentState.Shipped && this.SalesOrderInvoiceState.Invoiced)
                {
                    this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Completed;
                }

                if (this.SalesOrderState.IsCompleted && this.SalesOrderPaymentState.Paid)
                {
                    this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Finished;
                }
            }

            // TODO: Move to versioning
            this.PreviousBillToCustomer = this.BillToCustomer;
            this.PreviousShipToCustomer = this.ShipToCustomer;

            var singleton = session.GetSingleton();

            this.AddSecurityToken(new SecurityTokens(session).DefaultSecurityToken);

            this.ResetPrintDocument();

            // CanShip
            if (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).InProcess))
            {
                var somethingToShip = false;
                var allItemsAvailable = true;

                foreach (var salesOrderItem1 in validOrderItems)
                {
                    if (!this.PartiallyShip && salesOrderItem1.QuantityRequestsShipping != salesOrderItem1.QuantityOrdered)
                    {
                        allItemsAvailable = false;
                        break;
                    }

                    if (this.PartiallyShip && salesOrderItem1.QuantityRequestsShipping > 0)
                    {
                        somethingToShip = true;
                    }
                }

                this.CanShip = (!this.PartiallyShip && allItemsAvailable) || somethingToShip;
            }
            else
            {
                this.CanShip = false;
            }

            // CanInvoice
            if (this.SalesOrderState.IsInProcess && object.Equals(this.Store.BillingProcess, new BillingProcesses(this.Strategy.Session).BillingForOrderItems))
            {
                this.CanInvoice = false;

                foreach (var orderItem2 in validOrderItems)
                {
                    var amountAlreadyInvoiced1 = orderItem2.OrderItemBillingsWhereOrderItem.Sum(v => v.Amount);

                    var leftToInvoice1 = (orderItem2.QuantityOrdered * orderItem2.UnitPrice) - amountAlreadyInvoiced1;

                    if (leftToInvoice1 > 0)
                    {
                        this.CanInvoice = true;
                    }
                }
            }
            else
            {
                this.CanInvoice = false;
            }

            if (this.SalesOrderState.Equals(new SalesOrderStates(this.Strategy.Session).InProcess) &&
                Equals(this.Store.BillingProcess, new BillingProcesses(this.Strategy.Session).BillingForShipmentItems))
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }

            if (this.CanShip && this.Store.AutoGenerateCustomerShipment)
            {
                this.Ship();
            }

            if (this.SalesOrderState.IsInProcess
                && (!this.ExistLastSalesOrderState || !this.LastSalesOrderState.IsInProcess)
                && this.TakenBy.SerialisedItemSoldOns.Contains(new SerialisedItemSoldOns(this.Session()).SalesOrderAccept))
            {
                foreach (SalesOrderItem item in this.ValidOrderItems.Where(v => ((SalesOrderItem)v).ExistSerialisedItem))
                {
                    if (item.ExistNextSerialisedItemAvailability)
                    {
                        item.SerialisedItem.SerialisedItemAvailability = item.NextSerialisedItemAvailability;

                        if (item.NextSerialisedItemAvailability.Equals(new SerialisedItemAvailabilities(this.Session()).Sold))
                        {
                            item.SerialisedItem.OwnedBy = this.ShipToCustomer;
                            item.SerialisedItem.Ownership = new Ownerships(this.Session()).ThirdParty;
                            item.SerialisedItem.AvailableForSale = false;
                        }

                        if (item.NextSerialisedItemAvailability.Equals(new SerialisedItemAvailabilities(this.Session()).InRent))
                        {
                            item.SerialisedItem.RentedBy = this.ShipToCustomer;
                        }
                    }
                }
            }

            this.Sync(derivation, validOrderItems);
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            if (this.CanShip)
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Ship, Operations.Execute));
            }
            else
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Ship, Operations.Execute));
            }

            if (this.CanInvoice)
            {
                this.RemoveDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }
            else
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Invoice, Operations.Execute));
            }

            if (!this.SalesOrderInvoiceState.NotInvoiced || !this.SalesOrderShipmentState.NotShipped)
            {
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.SetReadyForPosting, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Post, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Reopen, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Approve, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Hold, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Continue, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Accept, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Revise, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Complete, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Reject, Operations.Execute));
                this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Cancel, Operations.Execute));

                var deniablePermissionByOperandTypeId = new Dictionary<Guid, Permission>();

                foreach (Permission permission in this.Session().Extent<Permission>())
                {
                    if (permission.ConcreteClassPointer == this.strategy.Class.Id && permission.Operation == Operations.Write)
                    {
                        deniablePermissionByOperandTypeId.Add(permission.OperandTypePointer, permission);
                    }
                }

                foreach (var keyValuePair in deniablePermissionByOperandTypeId)
                {
                    this.AddDeniedPermission(keyValuePair.Value);
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

            if (this.HasChangedStates())
            {
                derivation.Mark(this);
            }
        }

        public void BaseDelete(SalesOrderDelete method)
        {
            if (this.IsDeletable)
            {
                foreach(OrderAdjustment orderAdjustment in this.OrderAdjustments)
                {
                    orderAdjustment.Delete();
                }

                foreach (SalesOrderItem item in this.SalesOrderItems)
                {
                    item.Delete();
                }

                foreach (SalesTerm salesTerm in this.SalesTerms)
                {
                    salesTerm.Delete();
                }
            }
        }

        public void BaseCancel(OrderCancel method) => this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Cancelled;

        public void BaseSetReadyForPosting(SalesOrderSetReadyForPosting  method)
        {
            var orderThreshold = this.Store.OrderThreshold;
            var partyFinancial = this.BillToCustomer.PartyFinancialRelationshipsWhereParty.FirstOrDefault(v => Equals(v.InternalOrganisation, this.TakenBy));

            var amountOverDue = partyFinancial.AmountOverDue;
            var creditLimit = partyFinancial.CreditLimit ?? (this.Store.ExistCreditLimit ? this.Store.CreditLimit : 0);

            if (amountOverDue > creditLimit || this.TotalExVat < orderThreshold)
            {
                this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).RequestsApproval;
            }
            else
            {
                this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).ReadyForPosting;
            }
        }

        public void BasePost(SalesOrderPost method) => this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).AwaitingAcceptance;

        public void BaseReject(OrderReject method) => this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Rejected;

        public void BaseReopen(OrderReopen method) => this.SalesOrderState = this.PreviousSalesOrderState;

        public void BaseHold(OrderHold method) => this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).OnHold;

        public void BaseApprove(OrderApprove method) => this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).ReadyForPosting;

        public void BaseAccept(SalesOrderAccept method) => this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).InProcess;

        public void BaseRevise(OrderRevise method) => this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Provisional;

        public void BaseContinue(OrderContinue method) => this.SalesOrderState = this.PreviousSalesOrderState;

        public void BaseComplete(OrderComplete method) => this.SalesOrderState = new SalesOrderStates(this.Strategy.Session).Completed;

        public void BaseShip(SalesOrderShip method)
        {
            if (this.CanShip)
            {
                var addresses = this.ShipToAddresses();
                var shipments = new List<Shipment>();
                if (addresses.Count > 0)
                {
                    foreach (var address in addresses)
                    {
                        var pendingShipment = address.Value.BaseGetPendingCustomerShipmentForStore(address.Key, this.Store, this.DerivedShipmentMethod);

                        if (pendingShipment == null)
                        {
                            pendingShipment = new CustomerShipmentBuilder(this.Strategy.Session)
                                .WithShipFromParty(this.TakenBy)
                                .WithShipFromAddress(this.DerivedShipFromAddress)
                                .WithShipToAddress(address.Key)
                                .WithShipToParty(address.Value)
                                .WithStore(this.Store)
                                .WithShipmentMethod(this.DerivedShipmentMethod)
                                .WithPaymentMethod(this.DerivedPaymentMethod)
                                .Build();

                            if (this.Store.AutoGenerateShipmentPackage)
                            {
                                pendingShipment.AddShipmentPackage(new ShipmentPackageBuilder(this.Strategy.Session).Build());
                            }
                        }

                        foreach (SalesOrderItem orderItem in this.ValidOrderItems)
                        {
                            var orderItemDerivedRoles = (SalesOrderItemDerivedRoles)orderItem;

                            if (orderItem.ExistProduct && orderItem.DerivedShipToAddress.Equals(address.Key) && orderItem.QuantityRequestsShipping > 0)
                            {
                                var good = orderItem.Product as Good;
                                var nonUnifiedGood = orderItem.Product as NonUnifiedGood;
                                var unifiedGood = orderItem.Product as UnifiedGood;
                                var inventoryItemKind = unifiedGood?.InventoryItemKind ?? nonUnifiedGood?.Part.InventoryItemKind;
                                var part = unifiedGood ?? nonUnifiedGood?.Part;

                                ShipmentItem shipmentItem = null;
                                foreach (ShipmentItem item in pendingShipment.ShipmentItems)
                                {
                                    if (inventoryItemKind != null
                                        && inventoryItemKind.Equals(new InventoryItemKinds(this.Session()).NonSerialised)
                                        && item.Good.Equals(good)
                                        && !item.ItemIssuancesWhereShipmentItem.Any(v => v.PickListItem.PickListWherePickListItem.PickListState.Equals(new PickListStates(this.Session()).Picked)))
                                    {
                                        shipmentItem = item;
                                        break;
                                    }
                                }

                                if (shipmentItem != null)
                                {
                                    shipmentItem.ContentsDescription = $"{shipmentItem.Quantity} * {good.Name}";
                                }
                                else
                                {
                                    shipmentItem = new ShipmentItemBuilder(this.Strategy.Session)
                                        .WithGood(good)
                                        .WithContentsDescription($"{orderItem.QuantityRequestsShipping} * {good}")
                                        .Build();

                                    if (orderItem.ExistSerialisedItem)
                                    {
                                        shipmentItem.SerialisedItem = orderItem.SerialisedItem;
                                    }

                                    if (orderItem.ExistNextSerialisedItemAvailability)
                                    {
                                        shipmentItem.NextSerialisedItemAvailability = orderItem.NextSerialisedItemAvailability;
                                    }

                                    if (orderItem.ExistReservedFromNonSerialisedInventoryItem)
                                    {
                                        shipmentItem.AddReservedFromInventoryItem(orderItem.ReservedFromNonSerialisedInventoryItem);
                                    }

                                    if (orderItem.ExistReservedFromSerialisedInventoryItem)
                                    {
                                        shipmentItem.AddReservedFromInventoryItem(orderItem.ReservedFromSerialisedInventoryItem);
                                    }

                                    pendingShipment.AddShipmentItem(shipmentItem);
                                }

                                foreach (SalesOrderItem featureItem in orderItem.OrderedWithFeatures)
                                {
                                    shipmentItem.AddProductFeature(featureItem.ProductFeature);
                                }

                                new OrderShipmentBuilder(this.Strategy.Session)
                                    .WithOrderItem(orderItem)
                                    .WithShipmentItem(shipmentItem)
                                    .WithQuantity(orderItem.QuantityRequestsShipping)
                                    .Build();

                                shipmentItem.Quantity = shipmentItem.OrderShipmentsWhereShipmentItem.Sum(v => v.Quantity);

                                orderItemDerivedRoles.QuantityRequestsShipping = 0;

                                orderItemDerivedRoles.CostOfGoodsSold = orderItem.QuantityOrdered * part.PartWeightedAverage.AverageCost;
                            }
                        }

                        shipments.Add(pendingShipment);
                        this.AddDeniedPermission(new Permissions(this.Strategy.Session).Get(this.Meta.Class, this.Meta.Ship, Operations.Execute));
                    }
                }
            }
        }

        public void BaseInvoice(SalesOrderInvoice method)
        {
            if (this.CanInvoice)
            {
                var salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                    .WithBilledFrom(this.TakenBy)
                    .WithAssignedBilledFromContactMechanism(this.DerivedTakenByContactMechanism)
                    .WithBilledFromContactPerson(this.TakenByContactPerson)
                    .WithBillToCustomer(this.BillToCustomer)
                    .WithAssignedBillToContactMechanism(this.DerivedBillToContactMechanism)
                    .WithBillToContactPerson(this.BillToContactPerson)
                    .WithBillToEndCustomer(this.BillToEndCustomer)
                    .WithAssignedBillToEndCustomerContactMechanism(this.DerivedBillToEndCustomerContactMechanism)
                    .WithBillToEndCustomerContactPerson(this.BillToEndCustomerContactPerson)
                    .WithShipToCustomer(this.ShipToCustomer)
                    .WithAssignedShipToAddress(this.DerivedShipToAddress)
                    .WithShipToContactPerson(this.ShipToContactPerson)
                    .WithShipToEndCustomer(this.ShipToEndCustomer)
                    .WithAssignedShipToEndCustomerAddress(this.DerivedShipToEndCustomerAddress)
                    .WithShipToEndCustomerContactPerson(this.ShipToEndCustomerContactPerson)
                    .WithDescription(this.Description)
                    .WithStore(this.Store)
                    .WithInvoiceDate(this.Session().Now())
                    .WithSalesChannel(this.SalesChannel)
                    .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                    .WithAssignedVatRegime(this.DerivedVatRegime)
                    .WithAssignedIrpfRegime(this.DerivedIrpfRegime)
                    .WithAssignedVatClause(this.DerivedVatClause)
                    .WithCustomerReference(this.CustomerReference)
                    .WithAssignedPaymentMethod(this.DerivedPaymentMethod)
                    .WithAssignedCurrency(this.DerivedCurrency)
                    .WithLocale(this.DerivedLocale)
                    .Build();

                foreach(OrderAdjustment orderAdjustment in this.OrderAdjustments)
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

                foreach (SalesOrderItem orderItem in this.ValidOrderItems)
                {
                    var amountAlreadyInvoiced = orderItem.OrderItemBillingsWhereOrderItem.Sum(v => v.Amount);

                    var leftToInvoice = (orderItem.QuantityOrdered * orderItem.UnitPrice) - amountAlreadyInvoiced;

                    if (leftToInvoice != 0)
                    {
                        var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                            .WithInvoiceItemType(orderItem.InvoiceItemType)
                            .WithAssignedUnitPrice(orderItem.UnitPrice)
                            .WithProduct(orderItem.Product)
                            .WithQuantity(orderItem.QuantityOrdered)
                            .WithCostOfGoodsSold(orderItem.CostOfGoodsSold)
                            .WithAssignedVatRegime(orderItem.DerivedVatRegime)
                            .WithAssignedIrpfRegime(orderItem.DerivedIrpfRegime)
                            .WithDescription(orderItem.Description)
                            .WithInternalComment(orderItem.InternalComment)
                            .WithMessage(orderItem.Message)
                            .Build();

                        if (orderItem.ExistSerialisedItem)
                        {
                            invoiceItem.SerialisedItem = orderItem.SerialisedItem;
                            invoiceItem.NextSerialisedItemAvailability = orderItem.NextSerialisedItemAvailability;
                        }

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
        }

        public void BasePrint(PrintablePrint method)
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
                    var barcode = barcodeService.Generate(this.OrderNumber, BarcodeType.CODE_128, 320, 80, pure: true);
                    images.Add("Barcode", barcode);
                }

                var model = new Print.SalesOrderModel.Model(this);
                this.RenderPrintDocument(this.TakenBy?.SalesOrderTemplate, model, images);

                this.PrintDocument.Media.InFileName = $"{this.OrderNumber}.odt";
            }
        }

        public void BaseDeriveVatClause(SalesOrderDeriveVatClause method)
        {
            if (!method.Result.HasValue)
            {
                if (this.ExistAssignedVatClause)
                {
                    this.DerivedVatClause = this.AssignedVatClause;
                }
                else if (this.ExistDerivedVatRegime && this.DerivedVatRegime.ExistVatClause)
                {
                    this.DerivedVatClause = this.DerivedVatRegime.VatClause;
                }
                else
                {
                    this.RemoveDerivedVatClause();
                }

                method.Result = true;
            }
        }

        private Dictionary<PostalAddress, Party> ShipToAddresses()
        {
            var addresses = new Dictionary<PostalAddress, Party>();
            foreach (SalesOrderItem item in this.ValidOrderItems)
            {
                if (item.QuantityRequestsShipping > 0)
                {
                    if (!addresses.ContainsKey(item.DerivedShipToAddress))
                    {
                        addresses.Add(item.DerivedShipToAddress, item.DerivedShipToParty);
                    }
                }
            }

            return addresses;
        }

        private void Sync(IDerivation derivation, SalesOrderItem[] validOrderItems)
        {
            // Second run to calculate price (because of order value break)
            foreach (var salesOrderItem in validOrderItems)
            {
                foreach (SalesOrderItem featureItem in salesOrderItem.OrderedWithFeatures)
                {
                    featureItem.SyncPrices(derivation, this);
                }

                salesOrderItem.Sync(this);
                salesOrderItem.SyncPrices(derivation, this);
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

            foreach (var item in validOrderItems)
            {
                if (!item.ExistSalesOrderItemWhereOrderedWithFeature)
                {
                    this.TotalBasePrice += item.TotalBasePrice;
                    this.TotalDiscount += item.TotalDiscount;
                    this.TotalSurcharge += item.TotalSurcharge;
                    this.TotalExVat += item.TotalExVat;
                    this.TotalVat += item.TotalVat;
                    this.TotalIrpf += item.TotalIrpf;
                    this.TotalIncVat += item.TotalIncVat;
                    this.TotalListPrice += item.TotalExVat;
                    this.GrandTotal += item.GrandTotal;
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

            if (this.ExistOrderDate && this.ExistDerivedCurrency && this.ExistTakenBy)
            {
                this.TotalBasePriceInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalBasePrice, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalDiscountInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalDiscount, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalSurchargeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalSurcharge, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalExtraChargeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalExtraCharge, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalFeeInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalFee, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalShippingAndHandlingInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalShippingAndHandling, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalExVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalExVat, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalVat, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalIncVatInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalIncVat, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.TotalIrpfInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(totalIrpf, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
                this.GrandTotalInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(grandTotal, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
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

            foreach (var item1 in validOrderItems)
            {
                if (item1.TotalExVat > 0)
                {
                    totalUnitBasePrice += item1.UnitBasePrice;
                    totalListPrice += item1.UnitPrice;
                }
            }

            if (this.ExistOrderDate && this.ExistDerivedCurrency && this.ExistTakenBy)
            {
                this.TotalListPriceInPreferredCurrency = Rounder.RoundDecimal(Currencies.ConvertCurrency(this.TotalListPrice, this.OrderDate, this.DerivedCurrency, this.TakenBy.PreferredCurrency), 2);
            }

            this.TotalListPrice = Rounder.RoundDecimal(this.TotalListPrice, 2);
        }
    }
}
