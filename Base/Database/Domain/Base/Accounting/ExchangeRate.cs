// <copyright file="ExchangeRate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ExchangeRate
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistFactor)
            {
                this.Factor = 1;
            }
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistFromCurrency && this.ExistToCurrency && this.FromCurrency.Equals(this.ToCurrency))
            {
                derivation.Validation.AddError(this, this.Meta.Factor, "Currencies can not be same");
            }

            if (this.Factor == 0)
            {
                derivation.Validation.AddError(this, this.Meta.Factor, "Factor not valid");
            }
        }
    }
}
