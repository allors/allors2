// <copyright file="IClassBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    using System.Collections.Generic;

    public partial interface IDomainBase : IMetaIdentifiableObjectBase, IDomain
    {
        IEnumerable<IDomainBase> Superdomains { get; }
    }
}
