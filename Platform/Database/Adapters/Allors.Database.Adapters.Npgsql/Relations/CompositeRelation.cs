// <copyright file="CompositeRelation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Npgsql
{
    internal class CompositeRelation
    {
        internal readonly long Association;
        internal readonly long Role;

        internal CompositeRelation(long association, long role)
        {
            this.Association = association;
            this.Role = role;
        }
    }
}
