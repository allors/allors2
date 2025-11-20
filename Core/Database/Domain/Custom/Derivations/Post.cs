// <copyright file="Dependee.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Post
    {
        public void CustomOnDerive(ObjectOnDerive method) => this.Counter += 1;

        public void CustomOnPostDerive(ObjectOnPostDerive method)
        {
            if (this.Counter % 2 == 0)
            {
                method.Derivation.Mark(this);
            }
        }
    }
}
