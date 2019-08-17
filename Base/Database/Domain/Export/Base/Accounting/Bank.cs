// <copyright file="Bank.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Text.RegularExpressions;

    using Allors.Meta;

    using Resources;

    public partial class Bank
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (!string.IsNullOrEmpty(this.Bic))
            {
                if (!Regex.IsMatch(this.Bic, "^([a-zA-Z]){4}([a-zA-Z]){2}([0-9a-zA-Z]){2}([0-9a-zA-Z]{3})?$"))
                {
                    derivation.Validation.AddError(this, this.Meta.Bic, ErrorMessages.NotAValidBic);
                }

                var country = new Countries(this.Strategy.Session).FindBy(M.Country.IsoCode, this.Bic.Substring(4, 2));
                if (country == null)
                {
                    derivation.Validation.AddError(this, this.Meta.Bic, ErrorMessages.NotAValidBic);
                }
            }
        }
    }
}
