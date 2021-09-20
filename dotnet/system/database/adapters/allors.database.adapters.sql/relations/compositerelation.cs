// <copyright file="CompositeRelation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    public class CompositeRelation
    {
        public readonly long Association;
        public readonly long Role;

        public CompositeRelation(long association, long role)
        {
            this.Association = association;
            this.Role = role;
        }
    }
}
