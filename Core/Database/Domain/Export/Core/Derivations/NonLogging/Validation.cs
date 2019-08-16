// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Validation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain.NonLogging
{
    public sealed partial class Validation : ValidationBase
    {
        public Validation(IDerivation derivation)
            : base(derivation)
        {
        }

        protected override void OnAddedError(IDerivationError derivationError)
        {
        }
    }
}
