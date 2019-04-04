// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialsUsage.cs" company="Allors bvba">
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

using System;
using System.Collections.Generic;
using Allors.Meta;

namespace Allors.Domain
{
    using System.Linq;

    public partial class WorkEffortInventoryAssignment
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistAssignment)
            {
                derivation.AddDependency(this.Assignment, this);
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
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
            this.UnitSellingPrice = AssignedUnitSellingPrice ?? unitSellingPrice;

            #endregion
        }

        public void AppsDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            method.SecurityTokens = this.Assignment?.SecurityTokens.ToArray();
            method.DeniedPermissions = this.Assignment?.DeniedPermissions.ToArray();
        }

        private void SyncInventoryTransactions(InventoryItem inventoryItem, decimal initialQuantity,
            InventoryTransactionReason reason, bool isCancellation)
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

        public void AppsCalculateSellingPrice(WorkEffortInventoryAssignmentCalculateSellingPrice method)
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