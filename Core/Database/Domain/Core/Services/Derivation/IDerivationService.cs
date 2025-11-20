// <copyright file="IDerivationService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using Allors.Domain;
    using Domain.Derivations;

    public interface IDerivationService
    {
        IDerivation CreateDerivation(ISession session);
    }
}
