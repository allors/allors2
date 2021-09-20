// <copyright file="Predicate.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    internal abstract class Predicate
    {
        internal virtual bool Include => true;

        internal virtual bool IsNotFilter => false;

        internal abstract bool BuildWhere(ExtentStatement statement, string alias);

        internal abstract void Setup(ExtentStatement statement);
    }
}
