// <copyright file="IDomain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;

    public interface IDomain : IMetaIdentifiableObject, IComparable
    {
        IEnumerable<IDomain> DirectSuperdomains { get; }

        string Name { get; }
    }
}
