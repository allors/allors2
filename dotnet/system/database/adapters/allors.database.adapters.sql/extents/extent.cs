// <copyright file="Extent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    internal abstract class Extent : Allors.Database.Extent
    {
        internal abstract SqlExtent ContainedInExtent { get; }
    }
}
