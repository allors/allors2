// <copyright file="ObjectExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;

    public static partial class ObjectExtensions
    {
        public static void CoreOnPostBuild(this Domain.Object @this, ObjectOnPostBuild method)
        {
            // TODO: Optimize
            foreach (var concreteRoleType in ((Class)@this.Strategy.Class).ConcreteRoleTypes)
            {
                if (concreteRoleType.IsRequired)
                {
                    var relationType = concreteRoleType.RelationType;
                    if (relationType.RoleType.ObjectType is IUnit unit && !@this.Strategy.ExistRole(relationType))
                    {
                        switch (unit.UnitTag)
                        {
                            case UnitTags.Boolean:
                                @this.Strategy.SetUnitRole(relationType, false);
                                break;

                            case UnitTags.Decimal:
                                @this.Strategy.SetUnitRole(relationType, 0m);
                                break;

                            case UnitTags.Float:
                                @this.Strategy.SetUnitRole(relationType, 0d);
                                break;

                            case UnitTags.Integer:
                                @this.Strategy.SetUnitRole(relationType, 0);
                                break;

                            case UnitTags.Unique:
                                @this.Strategy.SetUnitRole(relationType, Guid.NewGuid());
                                break;

                            case UnitTags.DateTime:
                                @this.Strategy.SetUnitRole(relationType, @this.Strategy.Session.Now());
                                break;
                        }
                    }
                }
            }
        }


        public static T Clone<T>(this T @this, params IRoleType[] deepClone) where T: IObject
        {
            var strategy = @this.Strategy;
            var session = strategy.Session;
            var @class = strategy.Class;

            var clone = (T)ObjectBuilder.Build(session, @class);

            foreach (var roleType in @class.RoleTypes.Where(v => !(v.RelationType.IsDerived || v.RelationType.IsSynced) && !deepClone.Contains(v) && (v.ObjectType.IsUnit || v.AssociationType.IsMany)))
            {
                var relationType = roleType.RelationType;
                if (!clone.Strategy.ExistRole(relationType))
                {
                    var role = @this.Strategy.GetRole(relationType);
                    clone.Strategy.SetRole(relationType, role);
                }
            }

            foreach(var roleType in deepClone)
            {
                var relationType = roleType.RelationType;
                if (roleType.IsOne)
                {
                    var role = strategy.GetCompositeRole(relationType);
                    if (role != null)
                    {
                        clone.Strategy.SetCompositeRole(relationType, role.Clone());
                    }
                }
                else
                {
                    foreach(IObject role in strategy.GetCompositeRoles(relationType))
                    {
                        clone.Strategy.AddCompositeRole(relationType, role.Clone());
                    }
                }
            }

            return clone;
        }
    }
}
