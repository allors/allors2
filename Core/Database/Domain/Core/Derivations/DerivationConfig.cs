// <copyright file="Constraints.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations
{
    public class DerivationConfig
    {
        public int MaxCycles { get; set; } = 0;

        public int MaxIterations { get; set; } = 0;

        public int MaxPreparations { get; set; } = 0;
    }
}
