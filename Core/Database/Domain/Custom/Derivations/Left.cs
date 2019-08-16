// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Left.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class Left
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            derivation.AddDependency(this, this.Middle);
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
