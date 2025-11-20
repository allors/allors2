// <copyright file="DomainObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Workspace.Meta
{
    public abstract partial class DomainObject : MetaObjectBase
    {
        protected DomainObject(MetaPopulation metaPopulation)
            : base(metaPopulation)
        {
        }
    }
}
