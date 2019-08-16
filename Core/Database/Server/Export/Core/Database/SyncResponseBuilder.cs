// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncResponseBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Server
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Remote.Sync;

    public class SyncResponseBuilder
    {
        private static readonly object[][] EmptyRoles = { };
        private static readonly string[][] EmptyMethods = { };

        private readonly ISession session;
        private readonly SyncRequest syncRequest;
        private readonly User user;

        public SyncResponseBuilder(ISession session, User user, SyncRequest syncRequest)
        {
            this.session = session;
            this.user = user;
            this.syncRequest = syncRequest;
        }

        public SyncResponse Build()
        {
            var objects = this.session.Instantiate(this.syncRequest.Objects);

            // Prefetch
            var objectByClass = objects.GroupBy(v => v.Strategy.Class, v => v);
            foreach (var groupBy in objectByClass)
            {
                var prefetchClass = (Class)groupBy.Key;
                var prefetchObjects = groupBy.ToArray();

                var prefetchPolicyBuilder = new PrefetchPolicyBuilder();
                prefetchPolicyBuilder.WithWorkspaceRules(prefetchClass);
                prefetchPolicyBuilder.WithSecurityRules(prefetchClass);
                var prefetcher = prefetchPolicyBuilder.Build();

                this.session.Prefetch(prefetcher, prefetchObjects);
            }

            var accessControlLists = new AccessControlLists(objects, this.user);

            return new SyncResponse
            {
                UserSecurityHash = this.user.SecurityHash(),
                Objects = objects.Select(x => new SyncResponseObject
                {
                    I = x.Id.ToString(),
                    V = x.Strategy.ObjectVersion.ToString(),
                    T = x.Strategy.Class.Name,
                    Roles = this.GetRoles(x, accessControlLists),
                    Methods = this.GetMethods(x, accessControlLists),
                }).ToArray()
            };
        }

        private object[][] GetRoles(IObject obj, AccessControlLists accessControlLists)
        {
            var composite = (Composite)obj.Strategy.Class;

            IList<RoleType> roleTypes = composite.WorkspaceRoleTypes.ToArray();
            if (roleTypes.Count > 0)
            {
                var acl = accessControlLists[obj];

                var roles = new List<object[]>();
                foreach (var roleType in roleTypes)
                {
                    var propertyName = roleType.PropertyName;

                    var canRead = acl == null || acl.CanRead(roleType);
                    var canWrite = acl != null && acl.CanWrite(roleType);
                    var access = (canRead ? "r" : string.Empty) + (canWrite ? "w" : string.Empty);

                    if (canRead)
                    {
                        if (roleType.ObjectType.IsUnit)
                        {
                            var role = obj.Strategy.GetUnitRole(roleType.RelationType);
                            if (role != null)
                            {
                                roles.Add(new[] { propertyName, access, role });
                            }
                            else
                            {
                                roles.Add(new object[] { propertyName, access });
                            }
                        }
                        else
                        {
                            if (roleType.IsOne)
                            {
                                var role = obj.Strategy.GetCompositeRole(roleType.RelationType);
                                if (role != null)
                                {
                                    roles.Add(new object[] { propertyName, access, role.Id.ToString() });
                                }
                                else
                                {
                                    roles.Add(new object[] { propertyName, access });
                                }
                            }
                            else
                            {
                                var role = obj.Strategy.GetCompositeRoles(roleType.RelationType);
                                if (role.Count != 0)
                                {
                                    var ids = role.Cast<IObject>().Select(roleObject => roleObject.Id.ToString()).ToList();
                                    roles.Add(new object[] { propertyName, access, ids });
                                }
                                else
                                {
                                    roles.Add(new object[] { propertyName, access });
                                }
                            }
                        }
                    }
                    else
                    {
                        roles.Add(new object[] { propertyName, access });
                    }
                }

                return roles.ToArray();
            }

            return EmptyRoles;
        }

        private string[][] GetMethods(IObject obj, AccessControlLists accessControlLists)
        {
            var composite = (Composite)obj.Strategy.Class;

            // TODO: remove .ToArray()
            IList<MethodType> methodTypes = composite.WorkspaceMethodTypes.ToArray();
            if (methodTypes.Count > 0)
            {
                var acl = accessControlLists[obj];

                var methods = new List<string[]>();
                foreach (var methodType in methodTypes)
                {
                    var methodName = methodType.Name;

                    var canExecute = acl == null || acl.CanExecute(methodType);
                    var access = canExecute ? "x" : string.Empty;

                    methods.Add(new[] { methodName, access });
                }

                return methods.ToArray();
            }

            return EmptyMethods;
        }
    }
}
