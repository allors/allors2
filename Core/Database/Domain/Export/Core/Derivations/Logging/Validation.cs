// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Validation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.Logging
{
    public sealed partial class Validation : ValidationBase
    {
        private readonly IDerivationLog derivationLog;

        public Validation(IDerivation derivation, IDerivationLog derivationLog) : base(derivation)
        {
            this.derivationLog = derivationLog;
        }

        protected override void OnAddedError(IDerivationError derivationError)
        {
            this.derivationLog.AddedError(derivationError);
        }
    }
}
