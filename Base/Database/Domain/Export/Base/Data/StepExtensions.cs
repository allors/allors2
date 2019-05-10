// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StepExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Allors.Data;
    using Allors.Meta;

    public static class StepExtensions
    {
        public static object Get(this Step step, IObject allorsObject, IAccessControlListFactory aclFactory)
        {
            var acl = aclFactory.Create(allorsObject);
            if (acl.CanRead(step.PropertyType))
            {
                if (step.ExistNext)
                {
                    var currentValue = step.PropertyType.Get(allorsObject.Strategy);

                    if (currentValue != null)
                    {
                        if (currentValue is IObject)
                        {
                            return step.Next.Get((IObject)currentValue, aclFactory);
                        }

                        var results = new HashSet<object>();
                        foreach (var item in (IEnumerable)currentValue)
                        {
                            var nextValueResult = step.Next.Get((IObject)item, aclFactory);
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

                return step.PropertyType.Get(allorsObject.Strategy);
            }

            return null;
        }

        public static bool Set(this Step step, IObject allorsObject, IAccessControlListFactory aclFactory, object value)
        {
            var acl = aclFactory.Create(allorsObject);
            if (step.ExistNext)
            {
                if (acl.CanRead(step.PropertyType))
                {
                    var property = step.PropertyType.Get(allorsObject.Strategy) as IObject;
                    if (property != null)
                    {
                        step.Next.Set(property, aclFactory, value);
                        return true;
                    }
                }

                return false;
            }

            var roleType = step.PropertyType as RoleType;
            if (roleType != null)
            {
                if (acl.CanWrite(roleType))
                {
                    roleType.Set(allorsObject.Strategy, value);
                    return true;
                }
            }

            return false;
        }

        public static void Ensure(this Step step, IObject allorsObject, IAccessControlListFactory aclFactory)
        {
            var acl = aclFactory.Create(allorsObject);

            var roleType = step.PropertyType as RoleType;
            if (roleType != null)
            {
                if (roleType.IsMany)
                {
                    throw new NotSupportedException("RoleType with muliplicity many");
                }

                if (roleType.ObjectType.IsComposite)
                {
                    if (acl.CanRead(roleType))
                    {
                        var role = roleType.Get(allorsObject.Strategy);
                        if (role == null)
                        {
                            if (acl.CanWrite(roleType))
                            {
                                role = allorsObject.Strategy.Session.Create((Class)roleType.ObjectType);
                                roleType.Set(allorsObject.Strategy, role);
                            }
                        }

                        if (step.ExistNext)
                        {
                            var next = role as IObject;
                            if (next != null)
                            {
                                step.Next.Ensure(next, aclFactory);
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
                    var association = associationType.Get(allorsObject.Strategy) as IObject;
                    if (association != null)
                    {
                        if (step.ExistNext)
                        {
                            step.Next.Ensure(association, aclFactory);
                        }
                    }
                }
            }
        }
    }
}