// <copyright file="Middle.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Middle
    {
        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            iteration.AddDependency(this.LeftWhereMiddle, this);
            iteration.AddDependency(this, this.Right);

            iteration.Mark(this, this.LeftWhereMiddle, this.Right);
        }
    }
}
