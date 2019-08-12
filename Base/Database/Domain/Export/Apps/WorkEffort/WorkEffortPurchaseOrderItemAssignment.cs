// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkEffortPurchaseOrderItemAssignment.cs" company="Allors bvba">
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

using System.Linq;

namespace Allors.Domain
{
    public partial class WorkEffortPurchaseOrderItemAssignment
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistPurchaseOrderItem)
            {
                this.PurchaseOrder = this.PurchaseOrderItem.PurchaseOrderWherePurchaseOrderItem;
                this.UnitPurchasePrice = this.PurchaseOrderItem.UnitPrice;
            }

            var unitSellingPrice = this.CalculateSellingPrice().Result.HasValue ? this.CalculateSellingPrice().Result.Value : 0M;
            this.UnitSellingPrice = AssignedUnitSellingPrice ?? unitSellingPrice;

            if (this.ExistAssignment)
            {
                this.Assignment.ResetPrintDocument();
            }
        }

        public void BaseDelegateAccess(DelegatedAccessControlledObjectDelegateAccess method)
        {
            method.SecurityTokens = this.Assignment?.SecurityTokens.ToArray();
            method.DeniedPermissions = this.Assignment?.DeniedPermissions.ToArray();
        }

        public void BaseCalculateSellingPrice(WorkEffortPurchaseOrderItemAssignmentCalculateSellingPrice method)
        {
            if (!method.Result.HasValue)
            {
                var part = this.PurchaseOrderItem.Part;
                var currentPriceComponents = new PriceComponents(this.Strategy.Session).CurrentPriceComponents(this.Assignment.ScheduledStart);
                var currentPartPriceComponents = part.GetPriceComponents(currentPriceComponents);

                method.Result = currentPartPriceComponents.OfType<BasePrice>().Max(v => v.Price);
            }
        }
    }
}