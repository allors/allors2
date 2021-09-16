// <copyright file="WorkEffortPurchaseOrderItemAssignment.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Linq;

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

            this.CalculateSellingPrice();

            if (this.ExistAssignment)
            {
                this.Assignment.ResetPrintDocument();
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

        public void BaseCalculateSellingPrice(WorkEffortPurchaseOrderItemAssignmentCalculateSellingPrice method)
        {
            if (!method.Result.HasValue)
            {
                if (this.AssignedUnitSellingPrice.HasValue)
                {
                    this.UnitSellingPrice = this.AssignedUnitSellingPrice.Value;
                }
                else
                {
                    var part = this.PurchaseOrderItem.Part;
                    var currentPriceComponents = new PriceComponents(this.Strategy.Session).CurrentPriceComponents(this.Assignment.ScheduledStart);
                    var currentPartPriceComponents = part.GetPriceComponents(currentPriceComponents);

                    var price = currentPartPriceComponents.OfType<BasePrice>().Max(v => v.Price);
                    this.UnitSellingPrice = price ?? 0M;
                }

                method.Result = true;
            }
        }
    }
}
