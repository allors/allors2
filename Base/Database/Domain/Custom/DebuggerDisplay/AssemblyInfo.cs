// <copyright file="AssemblyInfo.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics;
using Allors.Domain;
using static DebuggerDisplayConstants;

// General
[assembly: DebuggerDisplay("[Key={Key}, Value={Value}]", Target = typeof(KeyValuePair<,>))]

// Allors
[assembly: DebuggerDisplay("{DebuggerDisplay}" + id, Target = typeof(AccessControl))]
[assembly: DebuggerDisplay(name, Target = typeof(Facility))]
[assembly: DebuggerDisplay(name, Target = typeof(NonSerialisedInventoryItem))]
[assembly: DebuggerDisplay(name, Target = typeof(NonUnifiedPart))]
[assembly: DebuggerDisplay(name, Target = typeof(Organisation))]
[assembly: DebuggerDisplay("{PartyName}" + id, Target = typeof(Person))]
[assembly: DebuggerDisplay("{QuoteNumber ?? Description}" + id, Target = typeof(ProductQuote))]
[assembly: DebuggerDisplay("{InvoiceNumber ?? Description}" + id, Target = typeof(SalesInvoice))]
[assembly: DebuggerDisplay("{OrderNumber ?? Description}" + id, Target = typeof(SalesOrder))]
[assembly: DebuggerDisplay("{Part} of {SalesOrderWhereSalesOrderItem?.OrderNumber} with quantity {QuantityOrdered}" + id, Target = typeof(SalesOrderItem))]
[assembly: DebuggerDisplay("{SalesOrderItem} <-> {InventoryItem}" + id, Target = typeof(SalesOrderItemInventoryAssignment))]
[assembly: DebuggerDisplay(name, Target = typeof(SalesInvoiceItemState))]
[assembly: DebuggerDisplay(name, Target = typeof(SalesOrderState))]
[assembly: DebuggerDisplay(name, Target = typeof(SalesOrderInvoiceState))]
[assembly: DebuggerDisplay(name, Target = typeof(SalesOrderShipmentState))]
[assembly: DebuggerDisplay(name, Target = typeof(SalesOrderPaymentState))]
[assembly: DebuggerDisplay("{DebuggerDisplay}" + id, Target = typeof(SecurityToken))]
[assembly: DebuggerDisplay(name, Target = typeof(TimeFrequency))]
[assembly: DebuggerDisplay(name, Target = typeof(UnifiedGood))]
[assembly: DebuggerDisplay("{WorkEffortNumber ?? Name}" + id, Target = typeof(WorkTask))]
