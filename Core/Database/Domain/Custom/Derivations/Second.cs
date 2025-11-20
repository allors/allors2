// <copyright file="Second.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class Second
    {
        public void CustomOnDerive(ObjectOnDerive method)
        {
            this.Third = new ThirdBuilder(this.Strategy.Session).Build();

            this.IsDerived = true;
        }
    }
}
