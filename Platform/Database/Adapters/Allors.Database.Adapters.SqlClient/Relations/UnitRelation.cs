// <copyright file="UnitRelation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.SqlClient
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
