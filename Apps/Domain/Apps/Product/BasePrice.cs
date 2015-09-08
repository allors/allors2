// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasePrice.cs" company="Allors bvba">
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
    using Resources;

    public partial class BasePrice
    {
        public void AppsDelete(DeletableDelete method)
        {
            this.Product.RemoveFromBasePrices(this);
            this.ProductFeature.RemoveFromBasePrices(this);
        }
        
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSpecifiedFor)
            {
                this.SpecifiedFor = Domain.Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Log.AssertAtLeastOne(this, BasePrices.Meta.Product, BasePrices.Meta.ProductFeature);

            if (this.ExistOrderQuantityBreak)
            {
                derivation.Log.AddError(this, BasePrices.Meta.OrderQuantityBreak, ErrorMessages.BasePriceOrderQuantityBreakNotAllowed);
            }

            if (this.ExistOrderValue)
            {
                derivation.Log.AddError(this, BasePrices.Meta.OrderValue, ErrorMessages.BasePriceOrderValueNotAllowed);
            }

            if (this.ExistRevenueQuantityBreak)
            {
                derivation.Log.AddError(this, BasePrices.Meta.RevenueQuantityBreak, ErrorMessages.BasePriceRevenueQuantityBreakNotAllowed);
            }

            if (this.ExistRevenueValueBreak)
            {
                derivation.Log.AddError(this, BasePrices.Meta.RevenueValueBreak, ErrorMessages.BasePriceRevenueValueBreakNotAllowed);
            }


            if (this.ExistPrice)
            {
                if (!this.ExistCurrency)
                {
                    this.Currency = this.SpecifiedFor.PreferredCurrency;
                }

                derivation.Log.AssertExists(this, BasePrices.Meta.Currency);
            }

            if (this.ExistProduct && !this.ExistProductFeature)
            {
                this.Product.AddToBasePrice(this);
            }

            if (this.ExistProductFeature)
            {
                this.ProductFeature.AddToBasePrice(this);
            }
            
            this.DeriveVirtualProductPriceComponent();
        }

        public void AppsOnDeriveVirtualProductPriceComponent()
        {
            if (this.ExistProduct)
            {
                this.Product.DeriveVirtualProductPriceComponent();
            }
        }
    }
}