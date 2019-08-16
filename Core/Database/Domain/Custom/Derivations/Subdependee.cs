// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Subdependee.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
            var derivation = method.Derivation;
            if (this.ExistDependeeWhereSubdependee)
            {
                derivation.AddDependency(this.DependeeWhereSubdependee, this);
            }
        }

        public void CustomOnDerive(ObjectOnDerive method) => this.Subcounter = this.Subcounter + 1;
    }
}
