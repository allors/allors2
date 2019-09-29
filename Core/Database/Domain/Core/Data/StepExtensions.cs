// <copyright file="StepExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Data;
    using Allors.Meta;

    public static class StepExtensions
    {
        public static object Get(this Step step, IObject @object, IAccessControlLists acls)
        {
            var acl = acls[@object];
            if (acl.CanRead(step.PropertyType))
            {
                if (step.ExistNext)
                {
                    var currentValue = step.PropertyType.Get(@object.Strategy);

                    if (currentValue != null)
                    {
                        if (currentValue is IObject)
                        {
                            return step.Next.Get((IObject)currentValue, acls);
                        }

                        var results = new HashSet<object>();
                        foreach (var item in (IEnumerable)currentValue)
                        {
                            var nextValueResult = step.Next.Get((IObject)item, acls);
                            if (nextValueResult is HashSet<object>)
                            {
                                results.UnionWith((HashSet<object>)nextValueResult);
                            }
                            else
                            {
                                results.Add(nextValueResult);
                            }
                        }

                        return results;
                    }
                }

                return step.PropertyType.Get(@object.Strategy);
            }

            return null;
        }

        public static bool Set(this Step step, IObject @object, IAccessControlLists acls, object value)
        {
            var acl = acls[@object];
            if (step.ExistNext)
            {
                if (acl.CanRead(step.PropertyType))
                {
                    if (step.PropertyType.Get(@object.Strategy) is IObject property)
                    {
                        step.Next.Set(property, acls, value);
                        return true;
                    }
                }

                return false;
            }

            if (step.PropertyType is RoleType roleType)
            {
                if (acl.CanWrite(roleType))
                {
                    roleType.Set(@object.Strategy, value);
                    return true;
                }
            }

            return false;
        }

        public static void Ensure(this Step step, IObject @object, IAccessControlLists acls)
        {
            var acl = acls[@object];

            if (step.PropertyType is RoleType roleType)
            {
                if (roleType.IsMany)
                {
                    throw new NotSupportedException("RoleType with muliplicity many");
                }

                if (roleType.ObjectType.IsComposite)
                {
                    if (acl.CanRead(roleType))
                    {
                        var role = roleType.Get(@object.Strategy);
                        if (role == null)
                        {
                            if (acl.CanWrite(roleType))
                            {
                                role = @object.Strategy.Session.Create((Class)roleType.ObjectType);
                                roleType.Set(@object.Strategy, role);
                            }
                        }

                        if (step.ExistNext)
                        {
                            if (role is IObject next)
                            {
                                step.Next.Ensure(next, acls);
                            }
                        }
                    }
                }
            }
            else
            {
                var associationType = (AssociationType)step.PropertyType;
                if (associationType.IsMany)
                {
                    throw new NotSupportedException("AssociationType with muliplicity many");
                }

                if (acl.CanRead(associationType))
                {
                    if (associationType.Get(@object.Strategy) is IObject association)
                    {
                        if (step.ExistNext)
                        {
                            step.Next.Ensure(association, acls);
                        }
                    }
                }
            }
        }
    }
}
