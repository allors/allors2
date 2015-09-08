// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fee.cs" company="Allors bvba">
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
    public partial class Fee
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (derivation.ChangeSet.Associations.Contains(this.Id))
            {
                if (this.ExistOrderWhereFee)
                {
                    var salesOrder = (SalesOrder)this.OrderWhereFee;
                    derivation.AddDependency(this, salesOrder);
                }

                if (this.ExistInvoiceWhereFee)
                {
                    var salesInvoice = (Allors.Domain.SalesInvoice)this.InvoiceWhereFee;
                    derivation.AddDependency(this, salesInvoice);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Log.AssertAtLeastOne(this, Fees.Meta.Amount, Fees.Meta.Percentage);
            derivation.Log.AssertExistsAtMostOne(this, Fees.Meta.Amount, Fees.Meta.Percentage);
        }
    }
}