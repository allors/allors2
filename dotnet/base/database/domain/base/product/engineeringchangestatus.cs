// <copyright file="EngineeringChangeStatus.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class EngineeringChangeStatus
    {
        public void BaseOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistStartDateTime)
            {
                this.StartDateTime = this.Session().Now();
            }
        }
    }
}
