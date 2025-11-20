// <copyright file="DerivationErrorAtMostOne.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Errors
{
    using Resources;

    public class DerivationErrorAtMostOne : DerivationError
    {
        public DerivationErrorAtMostOne(IValidation validation, DerivationRelation[] relations)
            : base(validation, relations, DomainErrors.DerivationErrorAtMostOne)
        {
        }
    }
}
