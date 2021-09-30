// <copyright file="ChangedRoles.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Meta;

    internal class MemberExpressionsVisitor : ExpressionVisitor
    {
        public MemberExpressionsVisitor() => this.MemberExpressions = new List<MemberExpression>();

        public IList<MemberExpression> MemberExpressions { get; }

        protected override Expression VisitMember(MemberExpression node)
        {
            this.MemberExpressions.Insert(0, node);
            return base.VisitMember(node);
        }
    }

    public static partial class ExpressionExtensions
    {
        public static Node Node<T>(this Expression<Func<T, IPropertyType>> @this, IMetaPopulation metaPopulation) where T : IComposite
        {
            var visitor = new MemberExpressionsVisitor();
            visitor.Visit(@this);

            return Node<T>(metaPopulation, visitor);
        }

        public static Node Node<T>(this Expression<Func<T, IComposite>> @this, IMetaPopulation metaPopulation) where T : IComposite
        {
            var visitor = new MemberExpressionsVisitor();
            visitor.Visit(@this);

            return Node<T>(metaPopulation, visitor);
        }

        private static Node Node<T>(IMetaPopulation metaPopulation, MemberExpressionsVisitor visitor) where T : IComposite
        {
            Node path = null;
            Node currentPath = null;

            void AddPath(IPropertyType propertyType)
            {
                var newNode = new Node(propertyType);

                if (path == null)
                {
                    currentPath = newNode;
                    path = currentPath;
                }
                else
                {
                    currentPath.Add(newNode);
                    currentPath = newNode;
                }
            }

            var root = visitor.MemberExpressions[0].Member.DeclaringType;
            var composite = (IComposite) metaPopulation.FindDatabaseCompositeByName(root.Name);

            foreach (var memberExpression in visitor.MemberExpressions)
            {
                if (memberExpression.Type.GetInterfaces().Contains(typeof(IComposite)))
                {
                    var propertyInfo = (PropertyInfo) memberExpression.Member;
                    var propertyType = propertyInfo.PropertyType;
                    composite = (IComposite) metaPopulation.FindDatabaseCompositeByName(propertyType.Name);

                    if (currentPath != null && !currentPath.PropertyType.ObjectType.Equals(composite))
                    {
                        currentPath.OfType = composite;
                    }
                }

                if (memberExpression.Type.GetInterfaces().Contains(typeof(IRoleType)))
                {
                    var name = memberExpression.Member.Name;
                    var propertyType = composite.DatabaseRoleTypes.First(v => v.Name.Equals(name));
                    AddPath(propertyType);
                }

                if (memberExpression.Type.GetInterfaces().Contains(typeof(IAssociationType)))
                {
                    var name = memberExpression.Member.Name;
                    var propertyType = composite.DatabaseAssociationTypes.First(v => v.Name.Equals(name));
                    AddPath(propertyType);
                }
            }

            return path;
        }
    }
}
