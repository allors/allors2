// <copyright file="BasePrice.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
