// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscountAdjustment.cs" company="Allors bvba">
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
    using Meta;

    public partial class DiscountAdjustment
    {
        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (derivation.HasChangedRoles(this))
            {
                if (this.ExistPriceableWhereDiscountAdjustment)
                {
                    var salesInvoiceItem = this.PriceableWhereDiscountAdjustment as SalesInvoiceItem;
                    var salesOrderItem = this.PriceableWhereDiscountAdjustment as SalesOrderItem;

                    if (salesInvoiceItem != null)
                    {
                        derivation.AddDependency(this, salesInvoiceItem);
                    }

                    if (salesOrderItem != null)
                    {
                        derivation.AddDependency(this, salesOrderItem);
                    }
                }

                if (this.ExistOrderWhereDiscountAdjustment)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereDiscountAdjustment;
                    derivation.AddDependency(this, salesOrder);
                }

                if (this.ExistInvoiceWhereDiscountAdjustment)
                {
                    var salesInvoice = (SalesInvoice)this.InvoiceWhereDiscountAdjustment;
                    derivation.AddDependency(this, salesInvoice);
                }
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!this.ExistCurrentVersion)
            {
                derivation.Validation.AssertAtLeastOne(
                    this,
                    M.DiscountAdjustment.Amount,
                    M.DiscountAdjustment.Percentage);
                derivation.Validation.AssertExistsAtMostOne(
                    this,
                    M.DiscountAdjustment.Amount,
                    M.DiscountAdjustment.Percentage);
            }
            else
            {
                if (this.ExistAmount && this.ExistPercentage)
                {
                    var version = CurrentVersion;
                    if (version.ExistAmount)
                    {
                        this.RemoveAmount();
                    }
                    else
                    {
                        this.RemovePercentage();
                    }
                }
            }
        }
    }
}
