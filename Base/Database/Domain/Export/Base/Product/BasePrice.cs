// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasePrice.cs" company="Allors bvba">
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
    using Meta;
    using Resources;

    public partial class BasePrice
    {
        public void BaseDelete(DeletableDelete method)
        {
            this.Product.RemoveFromBasePrices(this);
            this.ProductFeature.RemoveFromBasePrices(this);
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.BasePrice.Part, M.BasePrice.Product, M.BasePrice.ProductFeature);

            if (this.ExistOrderQuantityBreak)
            {
                derivation.Validation.AddError(this, M.BasePrice.OrderQuantityBreak, ErrorMessages.BasePriceOrderQuantityBreakNotAllowed);
            }

            if (this.ExistOrderValue)
            {
                derivation.Validation.AddError(this, M.BasePrice.OrderValue, ErrorMessages.BasePriceOrderValueNotAllowed);
            }

            if (this.ExistPrice)
            {
                if (!this.ExistCurrency)
                {
                    this.Currency = this.PricedBy.PreferredCurrency;
                }

                derivation.Validation.AssertExists(this, M.BasePrice.Currency);
            }

            if (this.ExistProduct && !this.ExistProductFeature)
            {
                this.Product.AddToBasePrice(this);
            }

            if (this.ExistProductFeature)
            {
                this.ProductFeature.AddToBasePrice(this);
            }

            this.BaseOnDeriveVirtualProductPriceComponent();
        }

        public void BaseOnDeriveVirtualProductPriceComponent()
        {
            if (this.ExistProduct)
            {
                this.Product.BaseOnDeriveVirtualProductPriceComponent();
            }
        }
    }
}
