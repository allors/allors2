// <copyright file="IDerivationError.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using Derivations;

    public interface IDerivationError
    {
        DerivationRelation[] Relations { get; }

        RoleType[] RoleTypes { get; }

        string Message { get; }
    }
}
