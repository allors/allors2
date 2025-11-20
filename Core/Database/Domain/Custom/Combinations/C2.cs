// <copyright file="C2.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Collections.Generic;

    using Allors;

    public partial class C2
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            foreach (Object dependency in this.Dependencies)
            {
                iteration.AddDependency(this, dependency);
                iteration.Mark(dependency);
            }
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            var sequence = (IList<IObject>)derivation["sequence"];
            if (sequence != null)
            {
                sequence.Add(this);
            }
        }

        public override string ToString() => this.Name;
    }
}
