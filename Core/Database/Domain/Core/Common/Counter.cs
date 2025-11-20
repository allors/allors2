// <copyright file="Counter.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Counter
    {
        public void CoreOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistValue)
            {
                this.Value = 0;
            }
        }

        public int NextValue() => ++this.Value;
    }
}
