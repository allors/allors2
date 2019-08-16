
// <copyright file="Dependent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Dependent
    {
        public void OnBuild(ObjectOnBuild method)
        {
            if (!this.ExistCounter)
            {
                this.Counter = 0;
            }

            if (!this.ExistSubcounter)
            {
                this.Subcounter = 0;
            }
        }

        public void CustomOnDerive(ObjectOnDerive method)
        {
            if (this.ExistDependee)
            {
                this.Counter = this.Dependee.Counter;
                this.Subcounter = this.Dependee.Subcounter;
            }
        }
    }
}
