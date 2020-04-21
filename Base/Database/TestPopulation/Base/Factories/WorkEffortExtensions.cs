// <copyright file="WorkEffortExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.TestPopulation
{
    using System;
    using Allors.Domain;

    public static class WorkEffortExtensions
    {
        public static WorkEffortInventoryAssignment CreateInventoryAssignment(this WorkEffort @this, Part part, int quantity)
        {
            new InventoryItemTransactionBuilder(@this.Session())
                .WithPart(part)
                .WithReason(new InventoryTransactionReasons(@this.Session()).IncomingShipment)
                .WithQuantity(quantity)
                .Build();

            @this.Session().Derive();

            return new WorkEffortInventoryAssignmentBuilder(@this.Session())
                .WithAssignment(@this)
                .WithInventoryItem(part.InventoryItemsWherePart.First)
                .WithQuantity(quantity)
                .Build();
        }

        public static TimeEntry CreateTimeEntry(this WorkEffort @this, DateTime fromDate, DateTime throughDate, TimeFrequency frequency, RateType rateType) =>
            new TimeEntryBuilder(@this.Session())
                .WithRateType(rateType)
                .WithFromDate(fromDate)
                .WithThroughDate(throughDate)
                .WithTimeFrequency(frequency)
                .WithWorkEffort(@this)
                .Build();
    }
}
