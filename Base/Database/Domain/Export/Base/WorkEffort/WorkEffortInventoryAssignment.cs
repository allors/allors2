// <copyright file="WorkEffortInventoryAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using Allors.Meta;

namespace Allors.Domain
{
    using System.Linq;

    public partial class WorkEffortInventoryAssignment
    {
        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            method.SecurityTokens = this.Assignment?.SecurityTokens.ToArray();
            method.DeniedPermissions = this.Assignment?.DeniedPermissions.ToArray();
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var state = this.Assignment.WorkEffortState;
            var inventoryItemChanged = this.ExistCurrentVersion &&
                                       (!Equals(this.CurrentVersion.InventoryItem, this.InventoryItem));

            foreach (InventoryTransactionReason createReason in state.InventoryTransactionReasonsToCreate)
            {
                this.SyncInventoryTransactions(this.InventoryItem, this.Quantity, createReason, false);
            }

            foreach (InventoryTransactionReason cancelReason in state.InventoryTransactionReasonsToCancel)
            {
                this.SyncInventoryTransactions(this.InventoryItem, this.Quantity, cancelReason, true);
            }

            if (inventoryItemChanged)
            {
                // CurrentVersion is Previous Version until PostDerive
                var previousInventoryItem = this.CurrentVersion.InventoryItem;
                var previousQuantity = this.CurrentVersion.Quantity;
                state = this.CurrentVersion.Assignment.PreviousWorkEffortState ??
                        this.CurrentVersion.Assignment.WorkEffortState;

                foreach (InventoryTransactionReason createReason in state.InventoryTransactionReasonsToCreate)
                {
                    this.SyncInventoryTransactions(previousInventoryItem, previousQuantity, createReason, true);
                }

                foreach (InventoryTransactionReason cancelReason in state.InventoryTransactionReasonsToCancel)
                {
                    this.SyncInventoryTransactions(previousInventoryItem, previousQuantity, cancelReason, true);
                }
            }

            var date = this.Assignment.ScheduledStart;

            #region Pricing

            try
            {
                this.UnitPurchasePrice = this.InventoryItem.Part.SupplierOfferingsWherePart
                    .Where(v => v.FromDate <= date && (!v.ExistThroughDate || v.ThroughDate >= date)).Max(v => v.Price);
            }
            catch (Exception e)
            {
                this.UnitPurchasePrice = 0M;
            }

            var unitSellingPrice = this.CalculateSellingPrice().Result.HasValue ? this.CalculateSellingPrice().Result.Value : 0M;
            this.UnitSellingPrice = this.AssignedUnitSellingPrice ?? unitSellingPrice;

            #endregion

            if (this.ExistAssignment)
            {
                this.Assignment.ResetPrintDocument();
            }
        }

        public void BaseOnPostDerive(ObjectOnPostDerive method)
        {
        }

        private void SyncInventoryTransactions(InventoryItem inventoryItem, decimal initialQuantity, InventoryTransactionReason reason, bool isCancellation)
        {
            var adjustmentQuantity = 0M;
            var existingQuantity = 0M;
            var matchingTransactions = this.InventoryItemTransactions
                .Where(t => t.Reason.Equals(reason) && t.Part.Equals(inventoryItem.Part)).ToArray();

            if (matchingTransactions.Length > 0)
            {
                existingQuantity = matchingTransactions.Sum(t => t.Quantity);
            }

            if (isCancellation)
            {
                adjustmentQuantity = 0 - existingQuantity;
            }
            else
            {
                adjustmentQuantity = initialQuantity - existingQuantity;
            }

            if (adjustmentQuantity != 0)
            {
                this.AddInventoryItemTransaction(new InventoryItemTransactionBuilder(this.strategy.Session)
                    .WithPart(inventoryItem.Part)
                    .WithQuantity(adjustmentQuantity)
                    .WithReason(reason)
                    .Build());
            }
        }

        public void BaseCalculateSellingPrice(WorkEffortInventoryAssignmentCalculateSellingPrice method)
        {
            if (!method.Result.HasValue)
            {
                var part = this.InventoryItem.Part;
                var currentPriceComponents = new PriceComponents(this.Strategy.Session).CurrentPriceComponents(this.Assignment.ScheduledStart);
                var currentPartPriceComponents = part.GetPriceComponents(currentPriceComponents);

                method.Result = currentPartPriceComponents.OfType<BasePrice>().Max(v => v.Price);
            }
        }
    }
}
