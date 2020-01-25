// <copyright file="ObjectOnPreDerive.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;

    public abstract partial class ObjectOnPreDerive
    {
        public IIteration Iteration { get; set; }

        public ObjectOnPreDerive WithIteration(IIteration iteration)
        {
            this.Iteration = iteration;
            return this;
        }

        public void Deconstruct(out IIteration iteration, out IAccumulatedChangeSet changeSet, out ISet<Object> derivedObjects)
        {
            changeSet = this.Iteration.ChangeSet;
            iteration = this.Iteration;
            derivedObjects = this.Iteration.Cycle.Derivation.DerivedObjects;
        }
    }
}
