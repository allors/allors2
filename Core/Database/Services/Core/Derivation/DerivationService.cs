// <copyright file="DerivationService.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Services
{
    using System;
    using Allors.Domain;
    using Domain.Derivations;

    public class DerivationService : IDerivationService
    {
        public Func<ISession, IDerivation> Factory { get; set; }

        public IDerivation CreateDerivation(ISession session) => this.Factory != null ? this.Factory(session) : new Domain.Derivations.Default.Derivation(session);
    }
}
