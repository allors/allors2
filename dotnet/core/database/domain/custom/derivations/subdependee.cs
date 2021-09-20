// <copyright file="Subdependee.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Subdependee
    {
        public void CustomOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSubcounter)
            {
                this.Subcounter = 0;
            }
        }

        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (this.ExistDependeeWhereSubdependee)
            {
                iteration.AddDependency(this.DependeeWhereSubdependee, this);
                iteration.Mark(this, this.DependeeWhereSubdependee);
            }
        }

        public void CustomOnDerive(ObjectOnDerive method) => this.Subcounter += 1;
    }
}
