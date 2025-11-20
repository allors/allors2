// <copyright file="Left.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Left
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            iteration.AddDependency(this, this.Middle);
            iteration.Mark(this, this.Middle);
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            if (this.CreateMiddle && !this.ExistMiddle)
            {
                this.Middle = new MiddleBuilder(this.strategy.Session).Build();
            }
        }
    }
}
