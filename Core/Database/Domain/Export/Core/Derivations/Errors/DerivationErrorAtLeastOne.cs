
// <copyright file="DerivationErrorAtLeastOne.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Resources;

    public class DerivationErrorAtLeastOne : DerivationError
    {
        public DerivationErrorAtLeastOne(IValidation validation, DerivationRelation[] derivationRelations)
            : base(validation, derivationRelations, DomainErrors.DerivationErrorAtLeastOne)
        {
        }
    }
}
