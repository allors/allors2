// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscountComponent.cs" company="Allors bvba">
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

    public partial class DiscountComponent
    {

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.DiscountComponent.Price, M.DiscountComponent.Percentage);
            derivation.Validation.AssertExistsAtMostOne(this, M.DiscountComponent.Price, M.DiscountComponent.Percentage);

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
