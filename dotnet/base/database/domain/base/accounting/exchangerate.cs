// <copyright file="ExchangeRate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class ExchangeRate
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.ExistFromCurrency && this.ExistToCurrency && this.FromCurrency.Equals(this.ToCurrency))
            {
                derivation.Validation.AddError(this, this.Meta.FromCurrency, "Currencies can not be same");
            }
        }
    }
}
