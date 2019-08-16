//-------------------------------------------------------------------------------------------------
// <copyright file="DomainObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    public abstract partial class DomainObject : MetaObjectBase
    {
        protected DomainObject(MetaPopulation metaPopulation)
            : base(metaPopulation)
        {
        }
    }
}
