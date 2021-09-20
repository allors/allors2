// <copyright file="AgreementProductApplicability.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class AgreementProductApplicability
    {
        public void BaseOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExistsAtMostOne(this, this.Meta.Agreement, this.Meta.AgreementItem);
            derivation.Validation.AssertAtLeastOne(this, this.Meta.Agreement, this.Meta.AgreementItem);
        }
    }
}
