// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Path.cs" company="Allors bvba">
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

    public static class PathExtensions
    {
        public static object Get(this Path path, IObject allorsObject, IAccessControlListFactory aclFactory)
        {
            var acl = aclFactory.Create(allorsObject);
            if (acl.CanRead(path.PropertyType))
            {
                if (path.ExistNext)
                {
                    var currentValue = path.PropertyType.Get(allorsObject.Strategy);

                    if (currentValue != null)
                    {
                        if (currentValue is IObject)
                        {
                            return path.Step.Get((IObject)currentValue, aclFactory);
                        }

                        var results = new HashSet<object>();
                        foreach (var item in (IEnumerable)currentValue)
                        {
                            var nextValueResult = path.Step.Get((IObject)item, aclFactory);
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

                if (path.ExistPropertyType)
                {
                    return path.PropertyType.Get(allorsObject.Strategy);
                }
            }

            return null;
        }

        public static bool Set(this Path path, IObject allorsObject, IAccessControlListFactory aclFactory, object value)
        {
            var acl = aclFactory.Create(allorsObject);
            if (path.ExistNext)
            {
                if (acl.CanRead(path.PropertyType))
                {
                    var property = path.PropertyType.Get(allorsObject.Strategy) as IObject;
                    if (property != null)
                    {
                        path.Step.Set(property, aclFactory, value);
                        return true;
                    }
                }

                return false;
            }

            var roleType = path.PropertyType as RoleType;
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

        public static void Ensure(this Path path, IObject allorsObject, IAccessControlListFactory aclFactory)
        {
            var acl = aclFactory.Create(allorsObject);

            var roleType = path.PropertyType as RoleType;
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

                        if (path.ExistNext)
                        {
                            var next = role as IObject;
                            if (next != null)
                            {
                                path.Step.Ensure(next, aclFactory);
                            }
                        }
                    }
                }
            }
            else
            {
                var associationType = (AssociationType)path.PropertyType;
                if (associationType.IsMany)
                {
                    throw new NotSupportedException("AssociationType with muliplicity many");
                }

                if (acl.CanRead(associationType))
                {
                    var association = associationType.Get(allorsObject.Strategy) as IObject;
                    if (association != null)
                    {
                        if (path.ExistNext)
                        {
                            path.Step.Ensure(association, aclFactory);
                        }
                    }
                }
            }
        }
    }
}