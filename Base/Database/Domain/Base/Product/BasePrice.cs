// <copyright file="BasePrice.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using Resources;

    public partial class BasePrice
    {
        public void BaseDelete(DeletableDelete method)
        {
            // HACK: DerivedRoles
            ((ProductDerivedRoles)this.Product).RemoveBasePrice(this);

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
                // HACK: DerivedRoles
                ((ProductDerivedRoles)this.Product).AddBasePrice(this);
            }

            if (this.ExistProductFeature)
            {
                this.ProductFeature.AddToBasePrice(this);
            }

            if (this.ExistProduct)
            {
                this.Product.BaseOnDeriveVirtualProductPriceComponent();
            }
        }
    }
}
