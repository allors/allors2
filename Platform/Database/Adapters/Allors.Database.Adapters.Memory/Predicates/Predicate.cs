// <copyright file="Predicate.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    internal abstract class Predicate
    {
        internal virtual bool Include => true;

        internal abstract ThreeValuedLogic Evaluate(Strategy strategy);
    }
}
