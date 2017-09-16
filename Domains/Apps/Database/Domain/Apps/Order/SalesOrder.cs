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
    using Meta;
    using Resources;

    public partial class SalesOrder
    {
        ObjectState Transitional.CurrentObjectState => this.CurrentObjectState;

        public PaymentMethod GetPaymentMethod
        {
            get
            {
                if (this.ExistBillToCustomer && this.BillToCustomer.ExistDefaultPaymentMethod)
                {
                    return this.BillToCustomer.DefaultPaymentMethod;
                }

                return this.Store.DefaultPaymentMethod;
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

        public void SetActualDiscountAmount(decimal amount)
        {
            if (!this.ExistDiscountAdjustment)
            {
                this.DiscountAdjustment = new DiscountAdjustmentBuilder(this.Strategy.Session).Build();
            }

            this.DiscountAdjustment.Amount = amount;
            this.DiscountAdjustment.RemovePercentage();
        }

        public void SetActualDiscountPercentage(decimal percentage)
        {
            if (!this.ExistDiscountAdjustment)
            {
                this.DiscountAdjustment = new DiscountAdjustmentBuilder(this.Strategy.Session).Build();
            }

            this.DiscountAdjustment.Percentage = percentage;
            this.DiscountAdjustment.RemoveAmount();
        }

        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCurrentObjectState)
            {
                this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).Provisional;
            }

            if (!this.ExistPartiallyShip)
            {
                this.PartiallyShip = true;
            }

            if (!this.ExistOrderDate)
            {
                this.OrderDate = DateTime.UtcNow;
            }

            if (!this.ExistEntryDate)
            {
                this.EntryDate = DateTime.UtcNow;
            }

            if (!this.ExistTakenByInternalOrganisation)
            {
                this.TakenByInternalOrganisation = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }

            if (!this.ExistStore && this.ExistTakenByInternalOrganisation)
            {
                if (this.TakenByInternalOrganisation.StoresWhereOwner.Count == 1)
                {
                    this.Store = this.TakenByInternalOrganisation.StoresWhereOwner.First;
                }
            }

            if (!this.ExistVatRegime && this.ExistBillToCustomer)
            {
                this.VatRegime = this.BillToCustomer.VatRegime;
            }

            if (!this.ExistPaymentMethod)
            {
                if (this.ShipToCustomer != null)
                {
                    this.PaymentMethod = this.ShipToCustomer.DefaultPaymentMethod;
                }

                if (!this.ExistPaymentMethod && this.ExistStore)
                {
                    this.PaymentMethod = this.Store.DefaultPaymentMethod;
                }
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

            if (!this.ExistShipmentMethod)
            {
                if (this.ShipToCustomer != null)
                {
                    this.ShipmentMethod = this.ShipToCustomer.DefaultShipmentMethod;
                }

                if (this.ShipmentMethod == null && this.ExistStore)
                {
                    this.ShipmentMethod = this.Store.DefaultShipmentMethod;
                }
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistBillToCustomer)
            {
                var customerRelationships = this.BillToCustomer.CustomerRelationshipsWhereCustomer;
                customerRelationships.Filter.AddEquals(M.CustomerRelationship.InternalOrganisation, this.TakenByInternalOrganisation);

                foreach (CustomerRelationship customerRelationship in customerRelationships)
                {
                    if (customerRelationship.FromDate <= DateTime.UtcNow && (!customerRelationship.ExistThroughDate || customerRelationship.ThroughDate >= DateTime.UtcNow))
                    {
                        derivation.AddDependency(customerRelationship, this);
                    }
                }
            }

            if (this.ExistShipToCustomer)
            {
                var customerRelationships = this.ShipToCustomer.CustomerRelationshipsWhereCustomer;
                customerRelationships.Filter.AddEquals(M.CustomerRelationship.InternalOrganisation, this.TakenByInternalOrganisation);

                foreach (CustomerRelationship customerRelationship in customerRelationships)
                {
                    if (customerRelationship.FromDate <= DateTime.UtcNow && (!customerRelationship.ExistThroughDate || customerRelationship.ThroughDate >= DateTime.UtcNow))
                    {
                        derivation.AddDependency(customerRelationship, this);
                    }
                }
            }

            foreach (var orderItem in this.OrderItems)
            {
                derivation.AddDependency(this, orderItem);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

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

            if (!this.ExistShipmentMethod && this.ExistShipToCustomer)
            {
                this.ShipmentMethod = this.ShipToCustomer.DefaultShipmentMethod ?? this.Store.DefaultShipmentMethod;
            }

            if (!this.ExistBillToContactMechanism && this.ExistBillToCustomer)
            {
                this.BillToContactMechanism = this.BillToCustomer.BillingAddress;
            }

            if (!this.ExistBillFromContactMechanism && this.ExistTakenByInternalOrganisation)
            {
                this.BillFromContactMechanism = this.TakenByInternalOrganisation.BillingAddress;
            }

            if (!this.ExistTakenByContactMechanism && this.ExistTakenByInternalOrganisation)
            {
                this.TakenByContactMechanism = this.TakenByInternalOrganisation.OrderAddress;
            }

            if (!this.ExistCustomerCurrency)
            {
                if (this.ExistBillToCustomer &&
                    (this.BillToCustomer.ExistPreferredCurrency || this.BillToCustomer.ExistLocale))
                {
                    this.CustomerCurrency = this.BillToCustomer.ExistPreferredCurrency ? this.BillToCustomer.PreferredCurrency : this.BillToCustomer.Locale.Country.Currency;
                }
                else
                {
                    if (this.ExistTakenByInternalOrganisation)
                    {
                        this.CustomerCurrency = this.TakenByInternalOrganisation.ExistPreferredCurrency ? this.TakenByInternalOrganisation.PreferredCurrency : this.TakenByInternalOrganisation.Locale.Country.Currency;
                    }
                }
            }

            if (this.ExistBillToCustomer && this.ExistTakenByInternalOrganisation)
            {
                if (!this.TakenByInternalOrganisation.Equals(this.BillToCustomer.InternalOrganisationWhereCustomer))
                {
                    derivation.Validation.AddError(this, M.ISalesOrder.BillToCustomer, ErrorMessages.PartyIsNotACustomer);
                }
            }

            if (this.ExistShipToCustomer && this.ExistTakenByInternalOrganisation)
            {
                if (!this.TakenByInternalOrganisation.Equals(this.ShipToCustomer.InternalOrganisationWhereCustomer))
                {
                    derivation.Validation.AddError(this, M.ISalesOrder.ShipToCustomer, ErrorMessages.PartyIsNotACustomer);
                }
            }

            if (!this.ExistVatRegime && this.ExistBillToCustomer)
            {
                this.VatRegime = this.BillToCustomer.VatRegime;
            }

            if (!this.ExistPaymentMethod && this.ExistBillToCustomer)
            {
                this.PaymentMethod = this.BillToCustomer.DefaultPaymentMethod ?? this.Store.DefaultPaymentMethod;
            }

            this.AppsOnDeriveOrderItems(derivation);

            this.AppsOnDeriveCurrentShipmentStatus(derivation);
            this.AppsOnDeriveCurrentOrderStatus(derivation);
            this.AppsOnDeriveLocale(derivation);
            this.AppsOnDeriveOrderTotals(derivation);
            this.AppsOnDeriveCustomers(derivation);
            this.AppsOnDeriveSalesReps(derivation);
            this.AppsOnDeriveCurrentPaymentStatus(derivation);

            if (Equals(this.Store.ProcessFlow, new ProcessFlows(this.strategy.Session).ShipFirst))
            {
                this.AppsTryShip(derivation);
            }

            this.PreviousBillToCustomer = this.BillToCustomer;
            this.PreviousShipToCustomer = this.ShipToCustomer;
        }

        public void AppsOnPostDerive(ObjectOnPostDerive method)
        {
            var isNewVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.InternalComment, this.CurrentVersion.InternalComment);

            var isNewStateVersion =
                !this.ExistCurrentVersion ||
                !object.Equals(this.CurrentObjectState, this.CurrentVersion.CurrentObjectState);

            if (isNewVersion)
            {
                this.PreviousVersion = this.CurrentVersion;
                this.CurrentVersion = new SalesOrderVersionBuilder(this.Strategy.Session).WithSalesOrder(this).Build();
                this.AddAllVersion(this.CurrentVersion);
            }

            if (isNewStateVersion)
            {
                this.CurrentStateVersion = CurrentVersion;
                this.AddAllStateVersion(this.CurrentStateVersion);
            }
        }

        public void AppsCancel(OrderCancel method)
        {
            this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).Cancelled;
        }

        public void AppsConfirm(OrderConfirm method)
        {
            var orderThreshold = this.Store.OrderThreshold;

            var customerRelationships = this.BillToCustomer.CustomerRelationshipsWhereCustomer;
            customerRelationships.Filter.AddEquals(M.CustomerRelationship.InternalOrganisation, this.TakenByInternalOrganisation);

            decimal amountOverDue = 0;
            decimal creditLimit = 0;
            foreach (CustomerRelationship customerRelationship in customerRelationships)
            {
                if (customerRelationship.FromDate <= DateTime.UtcNow && (!customerRelationship.ExistThroughDate || customerRelationship.ThroughDate >= DateTime.UtcNow))
                {
                    creditLimit = customerRelationship.CreditLimit ?? (this.Store.ExistCreditLimit ? this.Store.CreditLimit : 0);
                    amountOverDue = customerRelationship.AmountOverDue;
                }
            }

            if (amountOverDue > creditLimit || this.TotalExVat < orderThreshold)
            {
                this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).RequestsApproval;
            }
            else
            {
                this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).InProcess;
            }
        }

        public void AppsReject(OrderReject method)
        {
            this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).Rejected;
        }

        public void AppsHold(OrderHold method)
        {
            this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).OnHold;
        }

        public void AppsApprove(OrderApprove method)
        {
            this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).InProcess;
        }

        public void AppsContinue(OrderContinue method)
        {
            this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).InProcess;
        }

        public void AppsComplete(OrderComplete method)
        {
            this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).Completed;

            if (Equals(this.Store.ProcessFlow, new ProcessFlows(this.strategy.Session).PayFirst))
            {
                this.AppsInvoice();
            }
        }

        public void AppsFinish(OrderFinish method)
        {
            this.CurrentObjectState = new SalesOrderObjectStates(this.Strategy.Session).Finished;
        }

        public void AppsOnDeriveCurrentPaymentStatus(IDerivation derivation)
        {
            var itemsPaid = false;
            var itemsPartiallyPaid = false;
            var itemsUnpaid = false;

            foreach (SalesOrderItem orderItem in this.ValidOrderItems)
            {
                if (orderItem.ExistCurrentPaymentStateVersion)
                {
                    if (orderItem.CurrentPaymentStateVersion.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).PartiallyPaid))
                    {
                        itemsPartiallyPaid = true;
                    }

                    if (orderItem.CurrentPaymentStateVersion.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Paid))
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
                (!this.ExistCurrentPaymentStateVersion || !this.CurrentPaymentStateVersion.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).Paid)))
            {
                var newVersion = new SalesOrderVersionBuilder(this.Strategy.Session)
                    .WithCurrentObjectState(new SalesOrderObjectStates(this.Strategy.Session).Paid)
                    .WithSalesOrder(this)
                    .Build();
                this.CurrentPaymentStateVersion = newVersion;
                this.AddAllPaymentStateVersion(newVersion);
            }

            if ((itemsPartiallyPaid || (itemsPaid && itemsUnpaid)) &&
                (!this.ExistCurrentPaymentStateVersion || !this.CurrentPaymentStateVersion.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).PartiallyPaid)))
            {
                var newVersion = new SalesOrderVersionBuilder(this.Strategy.Session)
                    .WithCurrentObjectState(new SalesOrderObjectStates(this.Strategy.Session).PartiallyPaid)
                    .WithSalesOrder(this)
                    .Build();
                this.CurrentPaymentStateVersion = newVersion;
                this.AddAllPaymentStateVersion(newVersion);
            }

            this.AppsOnDeriveCurrentOrderStatus(derivation);
        }

        public void AppsOnDeriveCurrentShipmentStatus(IDerivation derivation)
        {
            var itemsShipped = false;
            var itemsPartiallyShipped = false;
            var itemsNotShipped = false;

            foreach (SalesOrderItem orderItem in this.ValidOrderItems)
            {
                if (orderItem.ExistCurrentShipmentStateVersion)
                {
                    if (orderItem.CurrentShipmentStateVersion.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).PartiallyShipped))
                    {
                        itemsPartiallyShipped = true;
                    }

                    if (orderItem.CurrentShipmentStateVersion.CurrentObjectState.Equals(new SalesOrderItemObjectStates(this.Strategy.Session).Shipped))
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
                (!this.ExistCurrentShipmentStateVersion || !this.CurrentShipmentStateVersion.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).Shipped)))
            {
                var newVersion = new SalesOrderVersionBuilder(this.Strategy.Session)
                    .WithCurrentObjectState(new SalesOrderObjectStates(this.Strategy.Session).Shipped)
                    .WithSalesOrder(this)
                    .Build();
                this.CurrentShipmentStateVersion = newVersion;
                this.AddAllShipmentStateVersion(newVersion);
            }

            if ((itemsPartiallyShipped || (itemsShipped && itemsNotShipped)) &&
                (!this.ExistCurrentShipmentStateVersion || !this.CurrentShipmentStateVersion.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).PartiallyShipped)))
            {
                var newVersion = new SalesOrderVersionBuilder(this.Strategy.Session)
                    .WithCurrentObjectState(new SalesOrderObjectStates(this.Strategy.Session).PartiallyShipped)
                    .WithSalesOrder(this)
                    .Build();
                this.CurrentShipmentStateVersion = newVersion;
                this.AddAllShipmentStateVersion(newVersion);
            }

            this.AppsOnDeriveCurrentOrderStatus(derivation);
        }

        public void AppsOnDeriveCurrentOrderStatus(IDerivation derivation)
        {
            if (this.ExistCurrentShipmentStateVersion && this.CurrentShipmentStateVersion.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).Shipped))
            {
                this.Complete();
            }

            if (this.ExistCurrentPaymentStateVersion && this.CurrentPaymentStateVersion.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).Paid))
            {
                this.Finish();
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
                    if (!item.ExistISalesOrderItemWhereOrderedWithFeature)
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
                this.Locale = this.ExistTakenByInternalOrganisation ? this.TakenByInternalOrganisation.Locale : Singleton.Instance(this.Strategy.Session).DefaultLocale;
            }
        }

        public void AppsOnDeriveCustomers(IDerivation derivation)
        {
            this.RemoveCustomers();

            this.AddCustomer(this.BillToCustomer);
            this.AddCustomer(this.ShipToCustomer);
            this.AddCustomer(this.PlacingCustomer);
        }

        public void AppsTryShip(IDerivation derivation)
        {
            if (this.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).InProcess))
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

                if (this.CurrentObjectState.Equals(new SalesOrderObjectStates(this.Strategy.Session).InProcess) &&
                    ((!this.PartiallyShip && allItemsAvailable) || somethingToShip))
                {
                    this.AppsShip(derivation);
                }
            }
        }

        private List<Shipment> AppsShip(IDerivation derivation)
        {
            var addresses = this.ShipToAddresses();
            var shipments = new List<Shipment>();
            if (addresses.Count > 0)
            {
                foreach (var address in addresses)
                {
                    shipments.Add(this.AppsShip(derivation, address));
                }
            }

            return shipments;
        }

        private CustomerShipment AppsShip(IDerivation derivation, KeyValuePair<PostalAddress, Party> address)
        {
            var pendingShipment = address.Value.AppsGetPendingCustomerShipmentForStore(address.Key, this.Store, this.ShipmentMethod);

            if (pendingShipment == null)
            {
                pendingShipment = new CustomerShipmentBuilder(this.Strategy.Session)
                    .WithBillFromInternalOrganisation(this.TakenByInternalOrganisation)
                    .WithShipFromParty(this.TakenByInternalOrganisation)
                    .WithShipFromAddress(this.TakenByInternalOrganisation.ShippingAddress)
                    .WithBillToParty(this.BillToCustomer)
                    .WithBillToContactMechanism(this.BillToContactMechanism)
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
                    orderShipmentsWhereShipmentItem.Filter.AddEquals(M.OrderShipment.SalesOrderItem, orderItem);

                    if (orderShipmentsWhereShipmentItem.First == null)
                    {
                        new OrderShipmentBuilder(this.Strategy.Session)
                            .WithSalesOrderItem(orderItem)
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

        private void AppsInvoice()
        {
            var salesInvoice = new SalesInvoiceBuilder(this.Strategy.Session)
                .WithStore(this.Store)
                .WithInvoiceDate(DateTime.UtcNow)
                .WithSalesChannel(this.SalesChannel)
                .WithSalesInvoiceType(new SalesInvoiceTypes(this.Strategy.Session).SalesInvoice)
                .WithVatRegime(this.VatRegime)
                .WithBilledFromContactMechanism(this.BillFromContactMechanism)
                .WithBillToContactMechanism(this.BillToContactMechanism)
                .WithBillToCustomer(this.BillToCustomer)
                .WithShipToCustomer(this.ShipToCustomer)
                .WithShipToAddress(this.ShipToAddress)
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
                var invoiceItem = new SalesInvoiceItemBuilder(this.Strategy.Session)
                    .WithSalesInvoiceItemType(new SalesInvoiceItemTypes(this.Strategy.Session).ProductItem)
                    .WithProduct(orderItem.Product)
                    .WithQuantity(orderItem.QuantityOrdered)
                    .WithComment(orderItem.Comment)
                    .WithInternalComment(orderItem.InternalComment)
                    .Build();

                salesInvoice.AddSalesInvoiceItem(invoiceItem);
            }
        }

        public void AppsOnDeriveOrderItems(IDerivation derivation)
        {
            var quantityOrderedByProduct = new Dictionary<Product, decimal>();
            var totalBasePriceByProduct = new Dictionary<Product, decimal>();

            foreach (SalesOrderItem salesOrderItem in this.ValidOrderItems)
            {
                foreach (SalesOrderItem featureItem in salesOrderItem.OrderedWithFeatures)
                {
                    featureItem.AppsOnDerivePrices(derivation, 0, 0);
                }

                salesOrderItem.AppsOnDeriveDeliveryDate(derivation);
                salesOrderItem.AppsOnDeriveShipTo(derivation);
                salesOrderItem.AppsOnDeriveSalesRep(derivation);
                salesOrderItem.AppsCalculatePurchasePrice(derivation);
                salesOrderItem.AppsCalculateUnitPrice(derivation);
                salesOrderItem.AppsOnDerivePrices(derivation, 0, 0);
                salesOrderItem.AppsOnDeriveCurrentShipmentStatus(derivation);
                salesOrderItem.AppsOnDeriveVatRegime(derivation);
                salesOrderItem.AppsOnDeriveCurrentPaymentStatus(derivation);

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
