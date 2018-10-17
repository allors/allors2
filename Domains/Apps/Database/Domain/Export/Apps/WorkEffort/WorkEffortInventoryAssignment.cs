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
            
            foreach (InventoryTransactionReason createReason in state.InventoryTransactionReasonsToCreate)
            {
                var transactionQuantity = 0;
                var matchingTransactions = transactions.Where(t => t.Reason.Equals(createReason));

                if (matchingTransactions.Count() > 0)
                {
                    transactionQuantity = matchingTransactions.Sum(t => t.Quantity);
                }

                if (this.Quantity != transactionQuantity)
                {
                    this.AddInventoryItemTransaction(new InventoryItemTransactionBuilder(this.strategy.Session)
                        .WithPart(this.Part)
                        .WithQuantity(this.Quantity - transactionQuantity)
                        .WithReason(createReason)
                        .Build());
                }
            }

            foreach (InventoryTransactionReason cancelReason in state.InventoryTransactionReasonsToCancel)
            {
                var transactionQuantity = 0;
                var matchingTransactions = transactions.Where(t => t.Reason.Equals(cancelReason));

                if (matchingTransactions.Count() > 0)
                {
                    transactionQuantity = matchingTransactions.Sum(t => t.Quantity);
                }

                if (transactionQuantity != 0)
                {
                    this.AddInventoryItemTransaction(new InventoryItemTransactionBuilder(this.strategy.Session)
                        .WithPart(this.Part)
                        .WithQuantity(0 - transactionQuantity)
                        .WithReason(cancelReason)
                        .Build());
                }
            }
        }
    }
}