// <copyright file="UnitRelation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.Npgsql
{
    internal class UnitRelation
    {
        internal readonly long Association;
        internal readonly object Role;

        internal UnitRelation(long association, object role)
        {
            this.Association = association;
            this.Role = role;
        }
    }
}
