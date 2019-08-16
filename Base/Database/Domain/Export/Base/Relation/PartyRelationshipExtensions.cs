
// <copyright file="PartyRelationshipExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public static partial class PartyRelationshipExtensions
    {
        public static void BaseOnBuild(this PartyRelationship @this, ObjectOnBuild method)
        {
            if (!@this.ExistFromDate)
            {
                @this.FromDate = @this.Strategy.Session.Now();
            }
        }
    }
}
