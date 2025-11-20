// <copyright file="DerivationErrorUnique.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Errors
{
    using Allors;
    using Allors.Meta;
    using Resources;

    public class DerivationErrorUnique : DerivationError
    {
        public DerivationErrorUnique(IValidation validation, DerivationRelation relation)
            : base(validation, new[] { relation }, DomainErrors.DerivationErrorUnique)
        {
        }

        public DerivationErrorUnique(IValidation validation, IObject association, RoleType roleType) :
            this(validation, new DerivationRelation(association, roleType))
        {
        }
    }
}
