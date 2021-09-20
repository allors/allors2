// <copyright file="IPropertyTypeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Workspace.Data
{
    using Allors.Workspace.Meta;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class IPropertyTypeExtensions
    {
        public static Node Node<T>(this T @this) where T : IPropertyType => new Node(@this);

        public static Node Node<T>(this T @this, Func<T, Node> child) where T : IPropertyType => new Node(@this, new[] { child(@this) });

        public static Node Node<T>(this T @this, params Func<T, Node>[] children) where T : IPropertyType => new Node(@this, children.Select(v => v(@this)));

        public static Node Node<T>(this T @this, Func<T, IEnumerable<Node>> children) where T : IPropertyType => new Node(@this, children(@this));

        public static object Get<T>(this T @this, IStrategy strategy, IComposite ofType = null) where T : IPropertyType
        {
            if (@this is IRoleType roleType)
            {
                if (roleType.IsOne)
                {
                    var association = strategy.GetCompositeRole<IObject>(roleType);

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return !ofType.IsAssignableFrom(association.Strategy.Class) ? null : association;
                }
                else
                {
                    var association = strategy.GetCompositesRole<IObject>(roleType);

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return association.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
                }
            }

            if (@this is IAssociationType associationType)
            {
                if (associationType.IsOne)
                {
                    var association = strategy.GetCompositeAssociation<IObject>(associationType);

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return !ofType.IsAssignableFrom(association.Strategy.Class) ? null : association;
                }
                else
                {
                    var association = strategy.GetCompositesAssociation<IObject>(associationType);

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return association.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
                }
            }

            throw new ArgumentException("Get only supports RoleType or AssociationType");
        }
    }
}
