// <copyright file="SurchargeComponent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class SurchargeComponent
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.SurchargeComponent.Price, M.SurchargeComponent.Percentage);
            derivation.Validation.AssertExistsAtMostOne(this, M.SurchargeComponent.Price, M.SurchargeComponent.Percentage);

            if (this.ExistPrice)
            {
                if (!this.ExistCurrency)
                {
                    this.Currency = this.PricedBy.PreferredCurrency;
                }

                derivation.Validation.AssertExists(this, M.BasePrice.Currency);
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
