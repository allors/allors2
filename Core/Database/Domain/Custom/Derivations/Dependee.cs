
// <copyright file="Dependee.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Dependee
    {
        public void CustomOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCounter)
            {
                this.Counter = 0;
            }

            if (!this.ExistSubcounter)
            {
                this.Subcounter = 0;
            }

            if (!this.ExistDeleteDependent)
            {
                this.DeleteDependent = false;
            }
        }

        public void CustomOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;
            if (this.ExistDependentWhereDependee)
            {
                derivation.AddDependency(this.DependentWhereDependee, this);
            }
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            this.Counter = this.Counter + 1;

            if (this.ExistSubdependee)
            {
                this.Subcounter = this.Subdependee.Subcounter;
            }

            if (this.DeleteDependent.HasValue && this.DeleteDependent.Value)
            {
                this.DependentWhereDependee.Delete();
            }
        }
    }
}
