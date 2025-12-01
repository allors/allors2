// <copyright file="IRelationType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A relation type defines the state and behavior for
    /// a set of association types and role types.
    /// </summary>
    public interface IRelationType : IMetaObject, ITagged, IComparable
    {
        IAssociationType AssociationType { get; }

        IRoleType RoleType { get; }

        Multiplicity Multiplicity { get; }

        bool ExistExclusiveClasses { get; }

        bool IsDerived { get; }

        bool IsSynced { get; }

        bool IsIndexed { get; }
    }
}
