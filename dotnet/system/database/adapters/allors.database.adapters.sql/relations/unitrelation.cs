// <copyright file="UnitRelation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    public class UnitRelation
    {
        public readonly long Association;
        public readonly object Role;

        public UnitRelation(long association, object role)
        {
            this.Association = association;
            this.Role = role;
        }
    }
}
