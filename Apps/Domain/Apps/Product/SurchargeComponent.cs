// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SurchargeComponent.cs" company="Allors bvba">
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
    public partial class SurchargeComponent
    {
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

            derivation.Log.AssertAtLeastOne(this, SurchargeComponents.Meta.Price, SurchargeComponents.Meta.Percentage);
            derivation.Log.AssertExistsAtMostOne(this, SurchargeComponents.Meta.Price, SurchargeComponents.Meta.Percentage);

            if (this.ExistPrice)
            {
                if (!this.ExistCurrency)
                {
                    this.Currency = this.SpecifiedFor.PreferredCurrency;
                }

                derivation.Log.AssertExists(this, BasePrices.Meta.Currency);
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