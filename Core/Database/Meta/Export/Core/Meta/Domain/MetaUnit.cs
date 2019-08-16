//-------------------------------------------------------------------------------------------------
// <copyright file="MetaUnit.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    public abstract partial class MetaUnit
    {
        public abstract Unit Unit { get; }

        public Unit ObjectType => this.Unit;

        public static implicit operator ObjectType(MetaUnit metaUnit) => metaUnit.Unit;
    }
}
