// <copyright file="DerivationErrorConflict.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Errors
{
    using Allors;
    using Allors.Meta;
    using Resources;

    public class DerivationErrorConflict : DerivationError
    {
        public DerivationErrorConflict(IValidation validation, DerivationRelation relation)
            : base(validation, new[] { relation }, DomainErrors.DerivationErrorConflict)
        {
        }

        public DerivationErrorConflict(IValidation validation, IObject association, RoleType roleType) :
            this(validation, new DerivationRelation(association, roleType))
        {
        }
    }
}
