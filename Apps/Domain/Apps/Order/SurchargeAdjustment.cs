// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SurchargeAdjustment.cs" company="Allors bvba">
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
    public partial class SurchargeAdjustment
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistOrderItemWhereSurchargeAdjustment)
                {
                    var salesOrderItem = (SalesOrderItem)this.OrderItemWhereSurchargeAdjustment;
                    derivation.AddDependency(this, salesOrderItem);
                }

                if (this.ExistOrderWhereSurchargeAdjustment)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereSurchargeAdjustment;
                    derivation.AddDependency(this, salesOrder);
                }

                if (this.ExistInvoiceItemWhereSurchargeAdjustment)
                {
                    var salesInvoiceItem = (Allors.Domain.SalesInvoiceItem)this.InvoiceItemWhereSurchargeAdjustment;
                    derivation.AddDependency(this, salesInvoiceItem);
                }

                if (this.ExistInvoiceWhereSurchargeAdjustment)
                {
                    var salesInvoice = (Allors.Domain.SalesInvoice)this.InvoiceWhereSurchargeAdjustment;
                    derivation.AddDependency(this, salesInvoice);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Log.AssertAtLeastOne(this, SurchargeAdjustments.Meta.Amount, SurchargeAdjustments.Meta.Percentage);
            derivation.Log.AssertExistsAtMostOne(this, SurchargeAdjustments.Meta.Amount, SurchargeAdjustments.Meta.Percentage);
        }
    }
}