// <copyright file="WorkEffortInventoryAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Linq;
    using Allors.Meta;
    using Resources;

    public partial class WorkEffortInventoryAssignment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                if (this.ExistAssignment)
                {
                    iteration.AddDependency(this.Assignment, this);
                    iteration.Mark(this.Assignment);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var state = this.Assignment.WorkEffortState;
            var inventoryItemChanged = this.ExistCurrentVersion &&
                                       (!Equals(this.CurrentVersion.InventoryItem, this.InventoryItem));

            if (inventoryItemChanged)
            {
                // CurrentVersion is Previous Version until PostDerive
                var previousInventoryItem = this.CurrentVersion.InventoryItem;
                var previousQuantity = this.CurrentVersion.Quantity;
                state = this.CurrentVersion.Assignment.PreviousWorkEffortState ??
                        this.CurrentVersion.Assignment.WorkEffortState;

                foreach (InventoryTransactionReason createReason in state.InventoryTransactionReasonsToCreate)
                {
                    this.SyncInventoryTransactions(derivation, previousInventoryItem, previousQuantity, createReason, true);
                }

                foreach (InventoryTransactionReason cancelReason in state.InventoryTransactionReasonsToCancel)
                {
                    this.SyncInventoryTransactions(derivation, previousInventoryItem, previousQuantity, cancelReason, true);
                }
            }

            this.CalculatePurchasePrice();
            this.CalculateSellingPrice();
            this.CalculateBillableQuantity();

            if (this.ExistAssignment)
            {
                this.Assignment.ResetPrintDocument();
            }
        }

        public void BaseDelete(DeletableDelete method)
        {
            var session = this.strategy.Session;
            var derivation = new Derivations.Default.Derivation(session);
            this.SyncInventoryTransactions(derivation, this.InventoryItem, this.Quantity, new InventoryTransactionReasons(session).Consumption, true);
        }

        public void BaseCalculateBillableQuantity(WorkEffortInventoryAssignmentCalculateBillableQuantity method)
        {
            if (!method.Result.HasValue)
            {
                this.DerivedBillableQuantity = this.AssignedBillableQuantity ?? this.Quantity;

                method.Result = true;
            }
        }

        public void BaseCalculatePurchasePrice(WorkEffortInventoryAssignmentCalculatePurchasePrice method)
        {
            if (!method.Result.HasValue)
            {
                this.CostOfGoodsSold = this.Quantity * this.InventoryItem.Part.PartWeightedAverage.AverageCost;

                method.Result = true;
            }
        }

        public void BaseCalculateSellingPrice(WorkEffortInventoryAssignmentCalculateSellingPrice method)
        {
            if (!method.Result.HasValue)
            {
                if (this.AssignedUnitSellingPrice.HasValue)
                {
                    this.UnitSellingPrice = this.AssignedUnitSellingPrice.Value;
                }
                else
                {
                    var part = this.InventoryItem.Part;
                    var currentPriceComponents = new PriceComponents(this.Strategy.Session).CurrentPriceComponents(this.Assignment.ScheduledStart);
                    var currentPartPriceComponents = part.GetPriceComponents(currentPriceComponents);

                    var price = currentPartPriceComponents.OfType<BasePrice>().Max(v => v.Price);
                    this.UnitSellingPrice = price ?? 0M;
                }

                method.Result = true;
            }
        }

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            if (method.SecurityTokens == null)
            {
                method.SecurityTokens = this.Assignment?.SecurityTokens.ToArray();
            }

            if (method.Restrictions == null)
            {
                method.Restrictions = this.Assignment?.Restrictions.ToArray();
            }
        }

        public void SyncInventoryTransactions(IDerivation derivation, InventoryItem inventoryItem, decimal initialQuantity, InventoryTransactionReason reason, bool isCancellation)
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

                if (inventoryItem is NonSerialisedInventoryItem nonserialisedInventoryItem && nonserialisedInventoryItem.QuantityOnHand < adjustmentQuantity)
                {
                    derivation.Validation.AddError(this, M.NonSerialisedInventoryItem.QuantityOnHand, ErrorMessages.InsufficientStock);
                }
            }

            if (adjustmentQuantity != 0)
            {
                this.AddInventoryItemTransaction(new InventoryItemTransactionBuilder(this.Session())
                    .WithPart(inventoryItem.Part)
                    .WithFacility(inventoryItem.Facility)
                    .WithQuantity(adjustmentQuantity)
                    .WithCost(inventoryItem.Part.PartWeightedAverage.AverageCost)
                    .WithReason(reason)
                    .Build());
            }
        }
    }
}
