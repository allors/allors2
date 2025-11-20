// <copyright file="DerivationErrorNotAllowed.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//
// </summary>

namespace Allors.Domain.Derivations.Errors
{
    using Allors;
    using Allors.Meta;
    using Resources;

    public class DerivationErrorNotAllowed : DerivationError
    {
        public DerivationErrorNotAllowed(IValidation validation, DerivationRelation relation)
            : base(validation, new[] { relation }, DomainErrors.DerivationErrorNotAllowed)
        {
        }

        public DerivationErrorNotAllowed(IValidation validation, IObject association, RoleType roleType) :
            this(validation, new DerivationRelation(association, roleType))
        {
        }
    }
}
