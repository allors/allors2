// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscountAdjustment.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class DiscountAdjustment
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistOrderItemWhereDiscountAdjustment)
                {
                    var salesOrderItem = (SalesOrderItem)this.OrderItemWhereDiscountAdjustment;
                    derivation.AddDependency(this, salesOrderItem);
                }

                if (this.ExistOrderWhereDiscountAdjustment)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereDiscountAdjustment;
                    derivation.AddDependency(this, salesOrder);
                }

                if (this.ExistInvoiceItemWhereDiscountAdjustment)
                {
                    var salesInvoiceItem = (Allors.Domain.SalesInvoiceItem)this.InvoiceItemWhereDiscountAdjustment;
                    derivation.AddDependency(this, salesInvoiceItem);
                }

                if (this.ExistInvoiceWhereDiscountAdjustment)
                {
                    var salesInvoice = (Allors.Domain.SalesInvoice)this.InvoiceWhereDiscountAdjustment;
                    derivation.AddDependency(this, salesInvoice);
                }
            }        
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Log.AssertAtLeastOne(this, DiscountAdjustments.Meta.Amount, DiscountAdjustments.Meta.Percentage);
            derivation.Log.AssertExistsAtMostOne(this, DiscountAdjustments.Meta.Amount, DiscountAdjustments.Meta.Percentage);
        }
    }
}