// <copyright file="Facilities.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Facilities
    {
        protected override void CorePrepare(Setup setup) => setup.AddDependency(this.ObjectType, M.FacilityType);
    }
}
