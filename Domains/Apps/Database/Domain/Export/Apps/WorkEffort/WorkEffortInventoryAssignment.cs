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
namespace Allors.Domain
{
    using System.Linq;

    public partial class WorkEffortInventoryAssignment
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            var state = this.Assignment.WorkEffortState;
            var transactions = this.InventoryItemTransactions.ToArray();
            var partChanged = this.ExistCurrentVersion && (this.CurrentVersion.Part != this.Part);

            foreach (InventoryTransactionReason createReason in state.InventoryTransactionReasonsToCreate)
            {
                this.SyncInventoryTransactions(this.Part, this.Quantity, createReason, false);
            }

            foreach (InventoryTransactionReason cancelReason in state.InventoryTransactionReasonsToCancel)
            {
                this.SyncInventoryTransactions(this.Part, this.Quantity, cancelReason, true);
            }

            if (partChanged)
            {
                // CurrentVersion is Previous Version until PostDerive
                var previousPart = this.CurrentVersion.Part;
                var previousQuantity = this.CurrentVersion.Quantity;
                state = this.CurrentVersion.Assignment.PreviousWorkEffortState ?? this.CurrentVersion.Assignment.WorkEffortState;

                foreach (InventoryTransactionReason createReason in state.InventoryTransactionReasonsToCreate)
                {
                    this.SyncInventoryTransactions(previousPart, previousQuantity, createReason, true);
                }

                foreach (InventoryTransactionReason cancelReason in state.InventoryTransactionReasonsToCancel)
                {
                    this.SyncInventoryTransactions(previousPart, previousQuantity, cancelReason, true);
                }
            }
        }

        private void SyncInventoryTransactions(Part part, int initialQuantity, InventoryTransactionReason reason, bool isCancellation)
        {
            var adjustmentQuantity = 0;
            var existingQuantity = 0;
            var matchingTransactions = this.InventoryItemTransactions.Where(t => t.Reason.Equals(reason) && t.Part.Equals(part));

            if (matchingTransactions.Count() > 0)
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
                this.AddInventoryItemTransaction(new InventoryItemTransactionBuilder(this.Strategy.Session)
                    .WithPart(part)
                    .WithQuantity(adjustmentQuantity)
                    .WithReason(reason)
                    .Build());
            }
        }
    }
}