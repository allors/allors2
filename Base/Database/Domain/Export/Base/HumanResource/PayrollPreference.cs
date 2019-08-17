// <copyright file="PayrollPreference.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class PayrollPreference
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, M.PayrollPreference.Amount, M.PayrollPreference.Percentage);
            derivation.Validation.AssertExistsAtMostOne(this, M.PayrollPreference.Amount, M.PayrollPreference.Percentage);
        }
    }
}
