// <copyright file="DerivationErrorRequired.cs" company="Allors bv">
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

    public class DerivationErrorRequired : DerivationError
    {
        public DerivationErrorRequired(IValidation validation, DerivationRelation relation)
            : base(validation, new[] { relation }, DomainErrors.DerivationErrorRequired)
        {
        }

        public DerivationErrorRequired(IValidation validation, IObject association, RoleType roleType) :
            this(validation, new DerivationRelation(association, roleType))
        {
        }

        public DerivationErrorRequired(IValidation validation, IObject role, AssociationType associationType) :
            this(validation, new DerivationRelation(role, associationType))
        {
        }
    }
}
