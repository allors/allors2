
// <copyright file="WorkEffortPurchaseOrderItemAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
